namespace SistemaGym.Core.CustomEntities
{
    public class ErrorResponse
    {
        public int Status { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public object? Errors { get; set; }

        public string TraceId { get; set; }
    }
}