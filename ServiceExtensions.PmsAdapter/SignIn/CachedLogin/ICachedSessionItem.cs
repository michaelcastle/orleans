namespace ServiceExtensions.PmsAdapter.SignIn.CachedLogin
{
    public interface ICachedSessionItem
    {
        bool CredentialsEqual(string username, string password);
    }
}
