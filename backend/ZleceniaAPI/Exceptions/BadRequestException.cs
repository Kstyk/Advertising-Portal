using System.Net;

namespace ZleceniaAPI.Exceptions
{
    public class BadRequestException : Exception
    {
        public HttpStatusCode StatusCode { get; } // Właściwość przechowująca kod statusu

        public BadRequestException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
        public BadRequestException(string message) : base(message)
        {
            
        }

     
    }
}
