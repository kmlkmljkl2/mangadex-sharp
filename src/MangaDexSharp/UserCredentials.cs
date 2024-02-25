using System;

namespace MangaDexSharp
{
    //TODO: Validate username length and password length
    /// <summary>
    /// User credentials for login
    /// </summary>
    public struct UserCredentials
    {
        public string Password {  get; }
        public string Username { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
        public UserCredentials(string username, string password, string clientId, string clientSecret)
        {
            Username = username;
            //TODO: Validate email format
            Password = password;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
    }
}
