namespace Breeze.Models.GenericResponses;

public class GenericResponse<T>
{
    public int StatusCode { get; set; }
    public T? Payload { get; set; }
    public string? Message { get; set; }
    public bool Status { get; set; }

    public static GenericResponse<T> Success(T? payload, string message, short statusCode)
    {
        return new GenericResponse<T>()
        {
            Payload = payload,
            Status = true,
            Message = message,
            StatusCode = statusCode
        };
    }

    public static GenericResponse<T> Success(string message, short statusCode)
    {
        return new GenericResponse<T>()
        {
            Status = true,
            Message = message,
            StatusCode = statusCode
        };
    }

    public static GenericResponse<T> Failure(T? payload, string message, short statusCode)
    {
        return new GenericResponse<T>()
        {
            Payload = payload,
            Status = false,
            Message = message,
            StatusCode = statusCode
        };
    }

    public static GenericResponse<T> Failure(string message, short statusCode)
    {
        return new GenericResponse<T>()
        {
            Status = false,
            Message = message,
            StatusCode = statusCode
        };
    }
}

public class GenericResponse
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public bool Status { get; set; }

    public static GenericResponse Success(string message, short statusCode)
    {
        return new GenericResponse()
        {
            Status = true,
            Message = message,
            StatusCode = statusCode
        };
    }

    public static GenericResponse Failure(string message, short statusCode)
    {
        return new GenericResponse()
        {
            Status = false,
            Message = message,
            StatusCode = statusCode
        };
    }
}