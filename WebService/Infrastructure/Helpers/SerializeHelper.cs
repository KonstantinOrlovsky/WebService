using System;
using System.IO;
using System.Web.Script.Serialization;

namespace UserService.Infrastructure.Helpers
{
    public static class SerializeHelper
    {
        public static T GetDeserializedObjectFromBody<T>(Stream stream)
        {
            JavaScriptSerializer _js = new JavaScriptSerializer();

            var rawRequestBody = new StreamReader(stream).ReadToEnd();

            if (rawRequestBody == null)
            {
                return default;
            }

            return _js.Deserialize<T>(rawRequestBody);
        }

        public static string Serialize<T>(T data)
        {
            JavaScriptSerializer _js = new JavaScriptSerializer();

            return _js.Serialize(data);
        }
    }
}