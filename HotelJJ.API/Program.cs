using System.Text;
using System.Text.Json;
using Asp.Versioning;
using HotelJJ.API.Extensions;
using HotelJJ.API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Services.AddHealthChecks();

var allowedCorsOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>()?
    .Where(origin => !string.IsNullOrWhiteSpace(origin))
    .Select(origin => origin.Trim().TrimEnd('/'))
    .Distinct(StringComparer.OrdinalIgnoreCase)
    .ToArray();

if (allowedCorsOrigins is null || allowedCorsOrigins.Length == 0)
{
    allowedCorsOrigins =
    [
        "http://localhost:5173",
        "http://127.0.0.1:5173",
        "http://localhost:5174",
        "http://127.0.0.1:5174",
        "http://localhost:3000",
        "http://127.0.0.1:3000",
        "http://localhost:4173",
        "http://127.0.0.1:4173"
    ];
}

builder.Services.AddCors(options => options.AddPolicy("FrontendCors", policy =>
    policy
        .WithOrigins(allowedCorsOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod()));

builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}).AddMvc().AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddHotelJJIntegrationServices(builder.Configuration);
AddJwtAuthentication(builder.Services, builder.Configuration, builder.Environment);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Middleware HotelJJ API",
        Version = "v1",
        Description = "Middleware bus para orquestar integraciones entre la aplicacion HotelJJ y los microservicios."
    });
    options.OperationFilter<AccommodationDetailSwaggerCleanupFilter>();
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT con el formato: Bearer {token}"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();
app.Logger.LogInformation("Iniciando {Service} en ambiente {Environment}", "Middleware.HotelJJ", app.Environment.EnvironmentName);
app.Logger.LogInformation("Swagger habilitado en /swagger y /swagger/v1/swagger.json");

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Middleware HotelJJ API v1"));

if (app.Configuration.GetValue("HttpsRedirection:Enabled", !app.Environment.IsDevelopment()))
{
    app.UseHttpsRedirection();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("FrontendCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "Middleware.HotelJJ" }));
app.MapGet("/health/downstreams", async (IConfiguration configuration, IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory) =>
{
    var logger = loggerFactory.CreateLogger("DownstreamHealth");
    var httpClient = httpClientFactory.CreateClient();
    httpClient.Timeout = TimeSpan.FromSeconds(10);

    var checks = new List<DownstreamHealthCheck>();
    foreach (var serviceSection in configuration.GetSection("Microservicios").GetChildren())
    {
        var serviceName = serviceSection.Key;
        var baseUrl = serviceSection["BaseUrl"];
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            checks.Add(new DownstreamHealthCheck(serviceName, "missing-base-url", null, baseUrl, null));
            continue;
        }

        var healthUrl = new Uri(new Uri(baseUrl.TrimEnd('/') + "/"), "health");
        try
        {
            using var response = await httpClient.GetAsync(healthUrl);
            checks.Add(new DownstreamHealthCheck(
                serviceName,
                response.IsSuccessStatusCode ? "ok" : "unhealthy",
                (int)response.StatusCode,
                baseUrl,
                null));
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "No se pudo validar health del microservicio {Service}", serviceName);
            checks.Add(new DownstreamHealthCheck(serviceName, "error", null, baseUrl, ex.Message));
        }
    }

    var allHealthy = checks.All(check => string.Equals(check.Status, "ok", StringComparison.OrdinalIgnoreCase));

    return allHealthy
        ? Results.Ok(new { status = "ok", service = "Middleware.HotelJJ", downstreams = checks })
        : Results.Json(new { status = "degraded", service = "Middleware.HotelJJ", downstreams = checks }, statusCode: StatusCodes.Status503ServiceUnavailable);
});
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Logger.LogInformation("{Service} listo para recibir solicitudes", "Middleware.HotelJJ");
app.Run();

static void AddJwtAuthentication(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
{
    var secret = configuration["Jwt:Secret"] ?? throw new InvalidOperationException("La configuracion 'Jwt:Secret' es obligatoria.");
    var issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("La configuracion 'Jwt:Issuer' es obligatoria.");
    var audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("La configuracion 'Jwt:Audience' es obligatoria.");

    if (string.IsNullOrWhiteSpace(secret) || secret.Length < 32)
        throw new InvalidOperationException("La configuracion 'Jwt:Secret' debe tener al menos 32 caracteres.");

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = !environment.IsDevelopment();
        options.SaveToken = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();

                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        statusCode = StatusCodes.Status401Unauthorized,
                        message = "No autorizado. Debe iniciar sesión para consultar la reserva."
                    }));
                }

                return Task.CompletedTask;
            },
            OnForbidden = context =>
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        statusCode = StatusCodes.Status403Forbidden,
                        message = "Esta reserva no está permitida para el cliente indicado."
                    }));
                }

                return Task.CompletedTask;
            }
        };
    });
}

sealed record DownstreamHealthCheck(
    string Service,
    string Status,
    int? StatusCode,
    string? BaseUrl,
    string? Error);

sealed class AccommodationDetailSwaggerCleanupFilter : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
{
    private static readonly HashSet<string> PathsWithoutQueryApiVersion = new(StringComparer.OrdinalIgnoreCase)
    {
        "api/v{version}/accommodations/{sucursalGuid}",
        "api/v1/accommodations/{sucursalGuid}",
        "api/v{version}/accommodations/search",
        "api/v1/accommodations/search",
        "api/v{version}/accommodations/{sucursalGuid}/reviews",
        "api/v1/accommodations/{sucursalGuid}/reviews"
    };

    public void Apply(OpenApiOperation operation, Swashbuckle.AspNetCore.SwaggerGen.OperationFilterContext context)
    {
        var relativePath = context.ApiDescription.RelativePath?.TrimEnd('/');
        if (relativePath is null || !PathsWithoutQueryApiVersion.Contains(relativePath))
            return;

        if (operation.Parameters is null)
            return;

        for (var index = operation.Parameters.Count - 1; index >= 0; index--)
        {
            var parameter = operation.Parameters[index];
            if (string.Equals(parameter.Name, "api-version", StringComparison.OrdinalIgnoreCase)
                && parameter.In == ParameterLocation.Query)
            {
                operation.Parameters.RemoveAt(index);
            }
        }
    }
}
