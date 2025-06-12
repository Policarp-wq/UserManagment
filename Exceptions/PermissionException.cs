namespace UserManagment.Exceptions
{
    public class PermissionException : ServerException
    {
        public PermissionException(string message) : base(message, StatusCodes.Status403Forbidden)
        {
        }
    }
}
