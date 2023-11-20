using System.Net;
using System.Web;
using UserService.Infrastructure.Models;

namespace UserService.Infrastructure.Helpers
{
    public static class ResponseHelper
    {
        public static void SetErrorResponse(HttpResponse response, HttpStatusCode code = HttpStatusCode.OK, string errorMessage = null)
        {
            response.StatusCode = (int)code;
            response.Write(SerializeHelper.Serialize(new GenericResponse(code, errorMessage)));
        }

        public static void SetSuccessResponse<T>(HttpResponse response, T data)
        {
            response.Write(SerializeHelper.Serialize(new GenericResponse(data)));
        }
    }
}