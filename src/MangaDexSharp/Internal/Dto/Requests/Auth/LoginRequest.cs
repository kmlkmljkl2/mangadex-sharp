#nullable disable
using System;
using System.Text.Json.Serialization;

namespace MangaDexSharp.Internal.Dto.Requests.Auth
{
    internal class LoginRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("client_id")]
        public string Client_id { get; set; }

        [JsonPropertyName("client_secret")]
        public string Client_secret { get; set; }

        [JsonPropertyName("grant_type")]

        public string Grant_type { get; } = "password";

        public LoginRequest(string username, string password)
        {
            if(username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }
            if (password == null)
            {
                throw new ArgumentNullException(nameof(username));
            }
            this.Username = username;
            this.Password = password;
        }

        public LoginRequest(string username, string password, string clientId, string clientSecret)
        {
            if (username == null)
            {
                throw new ArgumentException("Login request must have username or password");
            }
            if (password == null)
            {
                throw new ArgumentNullException(nameof(username));
            }
            this.Username = username ?? "";
            this.Password = password;
            Client_id = clientId ?? "";
            Client_secret = clientSecret ?? "";
        }
    }
}
