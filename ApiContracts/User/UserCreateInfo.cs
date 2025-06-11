namespace UserManagment.ApiContracts.User
{
    public record UserCreateInfo(string Login, string Pasword, string Name, int Gender, DateTime? Birthday, bool IsAdmin) //isAdmin extract to another?
    { } 
}
