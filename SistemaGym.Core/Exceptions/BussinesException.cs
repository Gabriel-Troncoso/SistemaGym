using System.Net;

namespace SistemaGym.Core.Exceptions
{
    public class BussinesException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public object? Details { get; }

        public BussinesException()
        {
            StatusCode = HttpStatusCode.BadRequest;
        }

        public BussinesException(string message) : base(message)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }

        public BussinesException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public BussinesException(string message, HttpStatusCode statusCode, object? details) : base(message)
        {
            StatusCode = statusCode;
            Details = details;
        }
    }
}