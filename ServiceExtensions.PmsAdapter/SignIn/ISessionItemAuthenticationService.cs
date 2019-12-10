namespace ServiceExtensions.PmsAdapter.SignIn
{
    public interface ISessionItemAuthenticationService
    {
        SessionItem SignIn(string username, string password, string lastAction, string hotelId);
    }
}
