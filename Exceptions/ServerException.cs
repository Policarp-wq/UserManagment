namespace UserManagment.Exceptions
{
    public class ServerException : Exception
    {
        public readonly int StatusCode;
        public ServerException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
