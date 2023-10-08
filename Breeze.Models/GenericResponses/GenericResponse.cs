namespace Breeze.Models.GenericResponses;

public class GenericResponse<T>
{
    public int statusCode;
    public T? payload;
    public string? message;
    public bool status;

    public static GenericResponse<T> Success(T? payload, string message, short statusCode)
    {
        return new GenericResponse<T>()
        {
            payload = payload,
            status = true,
            message = message,
            statusCode = statusCode
        };
    }

    public static GenericResponse<T> Success(string message, short statusCode)
    {
        return new GenericResponse<T>()
        {
            status = true,
            message = message,
            statusCode = statusCode
        };
    }

    public static GenericResponse<T> Failure(T? payload, string message, short statusCode)
    {
        return new GenericResponse<T>()
        {
            payload = payload,
            status = false,
            message = message,
            statusCode = statusCode
        };
    }

    public static GenericResponse<T> Failure(string message, short statusCode)
    {
        return new GenericResponse<T>()
        {
            status = false,
            message = message,
            statusCode = statusCode
        };
    }
}

public class GenericResponse
{
    public int statusCode;
    public string? message;
    public bool status;

    public static GenericResponse Success(string message, short statusCode)
    {
        return new GenericResponse()
        {
            status = true,
            message = message,
            statusCode = statusCode
        };
    }

    public static GenericResponse Failure(string message, short statusCode)
    {
        return new GenericResponse()
        {
            status = false,
            message = message,
            statusCode = statusCode
        };
    }
}