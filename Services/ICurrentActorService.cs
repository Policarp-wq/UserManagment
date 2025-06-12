namespace UserManagment.Services
{
    public interface ICurrentActorService
    {
        string GetLogin();
        bool IsAdmin();
    }
}