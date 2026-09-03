using System.Net;

namespace Domain.Responses;

public class Response<T>
{
    public T? Data { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = new();
    public int StatusCode { get; set; }

    public Response(T? data)
    {
        Data = data;
        StatusCode = 200;
    }

    public Response(HttpStatusCode code, string message)
    {
        StatusCode = (int)code;
        if (IsSuccessStatusCode(code))
            Message = message;
        else
            Errors.Add(message);
    }

    public Response(HttpStatusCode code, List<string> message)
    {
        StatusCode = (int)code;
        if (IsSuccessStatusCode(code))
            Message = string.Join(" ", message);
        else
            Errors.AddRange(message);
    }

    private static bool IsSuccessStatusCode(HttpStatusCode code) => (int)code is >= 200 and < 300;
    
    public Response(HttpStatusCode code, List<string> message,T data)
    {
        Data = data;
        StatusCode = (int)code;
        Errors.AddRange(message);
    }
    
    public Response(HttpStatusCode code, string message,T data)
    {
        Data = data;
        StatusCode = (int)code;
        Errors.Add(message);
    }

   
}