namespace NewsAppNet.Services
{
    // Used for returning result from service layer to controller
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string? Message { get; set; } = null;
    }
}
