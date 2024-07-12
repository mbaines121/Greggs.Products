namespace Greggs.Products.Shared;

public class GenericResult<T>
{
    public T Object { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }

    public static GenericResult<T> Ok(T returnObject)
    {
        var res = Ok(string.Empty, returnObject);
        return res;
    }

    public static GenericResult<T> Ok(string message, T returnObject)
    {
        var result = new GenericResult<T>
        {
            Success = true,
            Object = returnObject
        };

        if (!string.IsNullOrWhiteSpace(message))
        {
            result.Message = message;
        }

        return result;
    }

    public static GenericResult<T> Failed(string message)
    {
        var result = new GenericResult<T>
        {
            Success = false
        };

        if (!string.IsNullOrWhiteSpace(message))
        {
            result.Message = message;
        }

        return result;
    }
}
