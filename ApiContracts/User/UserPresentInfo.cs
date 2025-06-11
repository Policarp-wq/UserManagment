namespace UserManagment.ApiContracts.User
{
    public record UserPresentInfo(string Name, int Gender, DateTime? Birthday, bool IsActive)
    {
    }
}
