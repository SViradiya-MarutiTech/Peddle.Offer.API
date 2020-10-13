using Newtonsoft.Json;

namespace Domain.Dtos
{
    public class ErrorResponse<T>
    {
        public ErrorResponse()
        {
        }
        public ErrorResponse(string code, string message,T data )
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
        
        public string Code { get; set; }
        [JsonIgnore]
        public T Data { get; set; }
    }
}
