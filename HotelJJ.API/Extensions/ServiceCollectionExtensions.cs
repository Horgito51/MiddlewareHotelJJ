using HotelJJ.API.Models.Settings;
using HotelJJ.API.Infrastructure.Proxy;
using HotelJJ.Business.Interfaces.Alojamiento;
using HotelJJ.Business.Interfaces.Auth;
using HotelJJ.Business.Interfaces.Facturacion;
using HotelJJ.Business.Interfaces.Flujos;
using HotelJJ.Business.Interfaces.Hospedaje;
using HotelJJ.Business.Interfaces.Reservas;
using HotelJJ.Business.Services.Alojamiento;
using HotelJJ.Business.Services.Auth;
using HotelJJ.Business.Services.Facturacion;
using HotelJJ.Business.Services.Flujos;
using HotelJJ.Business.Services.Hospedaje;
using HotelJJ.Business.Services.Reservas;
using HotelJJ.DataAccess.Http.Clients;
using HotelJJ.DataAccess.Http.Interfaces;
using HotelJJ.DataManagement.Alojamiento.Interfaces;
using HotelJJ.DataManagement.Alojamiento.Services;
using HotelJJ.DataManagement.Auth.Interfaces;
using HotelJJ.DataManagement.Auth.Services;
using HotelJJ.DataManagement.Facturacion.Interfaces;
using HotelJJ.DataManagement.Facturacion.Services;
using HotelJJ.DataManagement.Hospedaje.Interfaces;
using HotelJJ.DataManagement.Hospedaje.Services;
using HotelJJ.DataManagement.Reservas.Interfaces;
using HotelJJ.DataManagement.Reservas.Services;
using HotelJJ.DataManagement.Common.Identifiers;

namespace HotelJJ.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHotelJJIntegrationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MicroserviciosSettings>(configuration.GetSection("Microservicios"));
        services.AddScoped<ProxyRouteResolver>();
        services.AddScoped<ProxyResponseWriter>();
        services.AddScoped<IMicroserviceProxy, MicroserviceProxy>();

        services.AddScoped<ISecurityDataService, SecurityDataService>();
        services.AddScoped<ISecurityOrchestrationService, SecurityOrchestrationService>();
        services.AddScoped<IAlojamientoDataService, AlojamientoDataService>();
        services.AddScoped<IAlojamientoOrchestrationService, AlojamientoOrchestrationService>();
        services.AddScoped<IReservasDataService, ReservasDataService>();
        services.AddScoped<IReservationOrchestrationService, ReservationOrchestrationService>();
        services.AddScoped<IHospedajeDataService, HospedajeDataService>();
        services.AddScoped<IHospedajeOrchestrationService, HospedajeOrchestrationService>();
        services.AddScoped<IFacturacionDataService, FacturacionDataService>();
        services.AddScoped<IFacturacionOrchestrationService, FacturacionOrchestrationService>();
        services.AddScoped<IIdentifierResolverService, IdentifierResolverService>();
        services.AddScoped<IIntegratedFlowOrchestrationService, IntegratedFlowOrchestrationService>();

        services.AddHttpClient<ISeguridadHttpClient, SeguridadHttpClient>(client =>
        {
            var baseUrl = configuration["Microservicios:Seguridad:BaseUrl"];
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new InvalidOperationException("Debe configurar Microservicios:Seguridad:BaseUrl.");
            }

            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(15);
        });

        services.AddHttpClient<IAlojamientoHttpClient, AlojamientoHttpClient>(client =>
        {
            var baseUrl = configuration["Microservicios:Alojamiento:BaseUrl"];
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new InvalidOperationException("Debe configurar Microservicios:Alojamiento:BaseUrl.");
            }

            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(15);
        });

        services.AddHttpClient<IReservasHttpClient, ReservasHttpClient>(client =>
        {
            var baseUrl = configuration["Microservicios:Reservas:BaseUrl"];
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new InvalidOperationException("Debe configurar Microservicios:Reservas:BaseUrl.");
            }

            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(15);
        });

        services.AddHttpClient<IHospedajeHttpClient, HospedajeHttpClient>(client =>
        {
            var baseUrl = configuration["Microservicios:Hospedaje:BaseUrl"];
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new InvalidOperationException("Debe configurar Microservicios:Hospedaje:BaseUrl.");
            }

            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(15);
        });

        services.AddHttpClient<IFacturacionHttpClient, FacturacionHttpClient>(client =>
        {
            var baseUrl = configuration["Microservicios:Facturacion:BaseUrl"];
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new InvalidOperationException("Debe configurar Microservicios:Facturacion:BaseUrl.");
            }

            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(15);
        });

        return services;
    }
}
