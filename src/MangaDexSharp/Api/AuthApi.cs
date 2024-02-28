using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MangaDexSharp.Internal;
using MangaDexSharp.Internal.Dto.Requests.Auth;
using MangaDexSharp.Internal.Dto.Responses.Auth;
using MangaDexSharp.Internal.Dto.Responses.Objects;

namespace MangaDexSharp.Api
{
    /// <summary>
    /// Api for acessing Auth endpoints
    /// </summary>
    /// <remarks>Learn more at https://api.mangadex.org/docs.html#tag/Auth </remarks>
    public class AuthApi : MangaDexApi
    {
        private TokenContainer? _refreshToken;
        private TokenContainer? _token;

        private DateTime? _lastRefresh;

        /// <summary>
        /// Gets last refresh token
        /// </summary>
        public string? LastRefreshToken => _refreshToken?.Token;

        internal AuthApi(MangaDexClient client) : base(client)
        {
            BaseApiPath = MangaDexApiPath + "/auth";
        }

        internal async Task AddAuthorizationHeaders(HttpRequestMessage message, CancellationToken cancelToken)
        {
            if (_lastRefresh != null && DateTime.Now - _lastRefresh.Value > TimeSpan.FromMinutes(14))
            {
                if (_token == null)
                {
                    await Login(cancelToken);
                }
                else
                {
                    await RefreshToken(cancelToken);
                }
            }

            if(_token == null)
            {
                return;
            }

            message.Headers.Add("Authorization", "Bearer " + _token.Token);
        }

        //TODO: Check endpoint

        /// <summary>
        /// Login with <seealso="UserCredentials">
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public async Task Login(CancellationToken cancelToken = default)
        {
            if(mangaDexClient.Credentials == null)
            {
                throw new InvalidOperationException(nameof(UserCredentials) + " should be initialized for auth requests");
            }

            var loginRequest = new LoginRequest(
                mangaDexClient.Credentials.Value.Username,
                mangaDexClient.Credentials.Value.Password,
                mangaDexClient.Credentials.Value.ClientId,
                mangaDexClient.Credentials.Value.ClientSecret);

            
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://auth.mangadex.org/realms/mangadex/protocol/openid-connect/token");


            //var baka = JsonContent.Create(loginRequest, new MediaTypeHeaderValue("application/x-www-form-urlencoded"), jsonOptions);
            //requestMessage.Content = JsonContent.Create(loginRequest, new MediaTypeHeaderValue("application/x-www-form-urlencoded"), jsonOptions);

            var test = new Dictionary<string, string>()
            {
                {    "grant_type", "password" },
                {    "username", loginRequest.Username },
                {    "password", loginRequest.Password },
                {    "client_id", loginRequest.Client_id },
                {    "client_secret", loginRequest.Client_secret },
            };
            requestMessage.Content = new FormUrlEncodedContent(test);

            HttpResponseMessage response = await httpClient.SendAsync(requestMessage, cancelToken);

            if (response.IsSuccessStatusCode == false)
            {
                throw new HttpRequestException("Request failed with code: " + response.StatusCode);
            }
            
          //  Stream jsonStream = await response.Content.ReadAsStreamAsync(cancelToken);

            string tesst = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<TokenDto>(tesst,  new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            _token = new TokenContainer(loginResponse.Access_Token);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            _refreshToken = new TokenContainer(loginResponse.Refresh_Token);
            _lastRefresh = DateTime.Now;

            mangaDexClient.CurrentUser = await mangaDexClient.User.GetLoggedInUserDetails(cancelToken);
        }
       
        /// <summary>
        /// Login with refresh token
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="HttpRequestException"></exception>

        public async Task LoginWithToken(string refreshToken, CancellationToken cancelToken = default)
        {
            _refreshToken = new TokenContainer(refreshToken);
            await RefreshToken(cancelToken);
            
            mangaDexClient.CurrentUser = await mangaDexClient.User.GetLoggedInUserDetails(cancelToken);
        }

        /// <summary>
        /// Requests token refresh
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="HttpRequestException"></exception>

        public async Task RefreshToken(CancellationToken cancelToken = default)
        {
            if(_refreshToken == null)
            {
                throw new InvalidOperationException("Cannot update token without refresh-token");
            }
            //var refreshTokenRequest = new RefreshTokenRequest(_refreshToken.Token);
          

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://auth.mangadex.org/realms/mangadex/protocol/openid-connect/token");

            var test = new Dictionary<string, string>()
            {
                {    "grant_type", "refresh_token" },
                {    "client_id",  mangaDexClient.Credentials.Value.ClientId },
                {    "client_secret",  mangaDexClient.Credentials.Value.ClientSecret },
                {    "refresh_token", _refreshToken.Token }
            };
            requestMessage.Content = new FormUrlEncodedContent(test);

            // requestMessage.Content = JsonContent.Create(refreshTokenRequest, new MediaTypeHeaderValue("application/json"), jsonOptions);

            HttpResponseMessage response = await httpClient.SendAsync(requestMessage, cancelToken);

            if (response.IsSuccessStatusCode == false)
            {
                throw new HttpRequestException("Request failed with code: " + response.StatusCode);
            }
            string a = await response.Content.ReadAsStringAsync();
            Stream jsonStream = await response.Content.ReadAsStreamAsync(cancelToken);
            var refreshTokenResponse = await JsonSerializer.DeserializeAsync<RefreshTokenResponse>(
                jsonStream,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true },
                cancelToken);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            _token = new TokenContainer(refreshTokenResponse.Access_Token);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            _refreshToken = new TokenContainer(refreshTokenResponse.Refresh_Token);
            _lastRefresh = DateTime.Now;
        }

        /// <summary>
        /// Requests log out for current session
        /// </summary>
        /// <returns></returns>

        public async Task Logout()
        {
            await Logout(default);
        }

        /// <summary>
        /// Requests log out for current session
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task Logout(CancellationToken cancelToken)
        {
            //TODO: checks
            var response = await PostRequest<MangaDexResponse>(BaseApiPath + "/logout", cancelToken);
            if (response.IsOk)
            {
                mangaDexClient.CurrentUser = null;
                _lastRefresh = null;
            }
        }

        private sealed class TokenContainer
        {
            public string Token { get; }

            public TokenContainer(string token)
            {
                Token = token;
            }

            public override string ToString()
            {
                return Token;
            }
        }
    }
}
