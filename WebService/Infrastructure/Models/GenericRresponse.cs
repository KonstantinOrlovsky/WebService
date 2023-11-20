using System.Net;

namespace UserService.Infrastructure.Models
{
    public class GenericResponse
    {
        public GenericResponse()
        {
            Code = HttpStatusCode.OK;
        }

        public GenericResponse(object data)
        {
            Code = HttpStatusCode.OK;
            Data = data;
        }

        public GenericResponse(HttpStatusCode code, string message)
        {
            Code = code;
            Message = message;
        }

        public object Data { get; set; }
        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }
    }
}