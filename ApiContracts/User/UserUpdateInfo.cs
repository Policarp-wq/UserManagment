namespace UserManagment.ApiContracts.User
{
    public record UserUpdateInfo(string? Name, int? Gender, DateTime? Birthday)
    {
    }
}
