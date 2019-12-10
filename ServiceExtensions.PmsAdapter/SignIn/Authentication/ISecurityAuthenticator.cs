namespace ServiceExtensions.PmsAdapter.SignIn.Authentication
{
    public interface ISecurityAuthenticator
    {
        bool Validate(string username, string password, string hotelId);
    }
}
