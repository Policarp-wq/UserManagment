namespace UserManagment.Exceptions
{
    public class DatabaseException : ServerException
    {
        public DatabaseException(string message, int code = StatusCodes.Status400BadRequest) : base(message, code) { }
    }
}
