using System.Net;
using Grpc.Core;
using HotelJJ.DataAccess.Http.Common;

namespace HotelJJ.DataAccess.Grpc.Common;

public static class GrpcStatusMapper
{
    public static DownstreamApiException ToDownstream(string service, RpcException ex)
    {
        return new DownstreamApiException(
            service,
            ToHttpStatusCode(ex.StatusCode),
            ex.Status.Detail,
            ex.ToString());
    }

    private static HttpStatusCode ToHttpStatusCode(StatusCode code)
    {
        return code switch
        {
            StatusCode.InvalidArgument => HttpStatusCode.BadRequest,
            StatusCode.NotFound => HttpStatusCode.NotFound,
            StatusCode.AlreadyExists => HttpStatusCode.Conflict,
            StatusCode.Aborted => HttpStatusCode.Conflict,
            StatusCode.PermissionDenied => HttpStatusCode.Forbidden,
            StatusCode.Unauthenticated => HttpStatusCode.Unauthorized,
            StatusCode.DeadlineExceeded => HttpStatusCode.GatewayTimeout,
            StatusCode.Unavailable => HttpStatusCode.ServiceUnavailable,
            _ => HttpStatusCode.BadGateway
        };
    }
}
