namespace Greggs.Products.Shared
{
    public class GenericResponse<T>
    {
        public T Object { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }

        public GenericResponse(T obj, string message, bool success = true)
        {
            Message = message;
            Object = obj;
            Success = success;
        }

        public GenericResponse(GenericResult<T> res)
        {
            Message = res.Message;
            Object = res.Object;
            Success = res.Success;
        }
    }
}
