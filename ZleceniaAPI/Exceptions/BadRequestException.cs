namespace ZleceniaAPI.Exceptions
{
    public class BadRequestException : Exception
    {
        private readonly string message;
        private readonly string fieldName;
        public BadRequestException(string message) : base(message)
        {
            
        }

        public BadRequestException(string message, string fieldName) : base(message)
        {
            fieldName = fieldName;
        }
    }
}
