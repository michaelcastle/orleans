using ServiceExtensions.PmsAdapter.SignIn.CachedLogin;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ServiceExtensions.PmsAdapter.SignIn
{
    public class SessionItem : ICachedSessionItem
    {
        public string UserName;
        public Guid SessionId;
        public bool IsAuthorised;
        public string HashedPassword;

        public SessionItem(string userName, string password, SessionItem userSessionDto)
        {
            UserName = userName;
            SetPassword(password);

            if (userSessionDto == null)
            {
                IsAuthorised = false;
            }
            else
            {
                IsAuthorised = userSessionDto.IsAuthorised;
                SessionId = userSessionDto.SessionId;
            }
        }

        public SessionItem()
        {
        }

        public bool CredentialsEqual(string username, string password)
        {
            return HashedPassword == HashPassword(password);
        }

        private void SetPassword(string password)
        {
            HashedPassword = HashPassword(password);
        }

        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }

            var sha1 = new SHA1CryptoServiceProvider();
            var bytes = sha1.ComputeHash(Encoding.ASCII.GetBytes(password));
            var hashedPassword = Convert.ToBase64String(bytes);
            return hashedPassword;
        }
    }
}
