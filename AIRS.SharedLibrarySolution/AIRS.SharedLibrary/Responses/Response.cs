

namespace AIRS.SharedLibrary.Responses;

public class Response<T>
{
    public bool Flag { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }

    public Response(bool flag, string message, T? data = default)
    {
        Flag = flag;
        Message = message;
        Data = data;
    }
}

