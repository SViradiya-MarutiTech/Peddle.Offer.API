using Newtonsoft.Json;
using System.Collections.Generic;

namespace Domain.Dtos
{
    public class ErrorResponse<T>
    {
        public ErrorResponse()
        {
        }
        public ErrorResponse(T data, string code, string message = null)
        {
            Message = message;
            Data = data;
            Code = code;
        }
        public ErrorResponse(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }
        public T Data { get; set; }
    }
}
