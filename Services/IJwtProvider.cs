namespace UserManagment.Services
{
    public interface IJwtProvider
    {
        string GenerateToken(string login, bool isAdmin);
    }
}