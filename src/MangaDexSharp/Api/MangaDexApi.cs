using MangaDexSharp.Enums;
using MangaDexSharp.Internal;
using MangaDexSharp.Internal.Dto.Resources;
using MangaDexSharp.Internal.JsonConverters;
using MangaDexSharp.Parameters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MangaDexSharp.Api
{
    /// <summary>
    /// Represents base class for Api endpoins
    /// </summary>
    public abstract class MangaDexApi
    {
        protected static JsonSerializerOptions jsonOptions;

        protected readonly HttpClient httpClient;
        protected readonly MangaDexClient mangaDexClient;

        /// <summary>
        /// Base path of the Api
        /// </summary>
        public string BaseApiPath { get; protected set; }

        public const string MangaDexApiPath = "https://api.mangadex.org";

        static MangaDexApi()
        {
            jsonOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            foreach (var converter in ConverterExtensions.GetMappableEnumConverters())
            {
                jsonOptions.Converters.Add(converter);
            }
            jsonOptions.Converters.Add(new LocalizedStringConverter());
            jsonOptions.Converters.Add(new GenericDictionaryConverter<Guid, IReadOnlyCollection<Guid>>());
            jsonOptions.Converters.Add(new GenericDictionaryConverter<Guid, MangaReadingStatus>());
            jsonOptions.Converters.Add(new MangaDexResourceConverter<AuthorDto>());
            jsonOptions.Converters.Add(new MangaDexResourceConverter<ChapterDto>());
            jsonOptions.Converters.Add(new MangaDexResourceConverter<CoverArtDto>());
            jsonOptions.Converters.Add(new MangaDexResourceConverter<CustomListDto>());
            jsonOptions.Converters.Add(new MangaConverter());
            jsonOptions.Converters.Add(new MangaDexResourceConverter<ScanlationGroupDto>());
            jsonOptions.Converters.Add(new MangaDexResourceConverter<TagDto>());
            jsonOptions.Converters.Add(new MangaDexResourceConverter<UserDto>());
        }

        internal MangaDexApi(MangaDexClient client)
        {
            mangaDexClient = client;
            httpClient = client.HttpClient;
            BaseApiPath = "https://api.mangadex.org/";
        }

        internal async Task<TResponse> DeleteRequest<TResponse>(string url, CancellationToken cancelToken)
            where TResponse : MangaDexResponse
        {
            var response = await SendRequest<TResponse>(
                HttpMethod.Delete,
                url,
                cancelToken,
                null,
                true);

            return response;
        }

        internal async Task<TResponse> DeleteRequestWithBody<TRequest, TResponse>(TRequest requestObject, string url, CancellationToken cancelToken)
            where TResponse : MangaDexResponse
        {
            var response = await SendRequestWithBody<TRequest, TResponse>(
                HttpMethod.Delete,
                requestObject,
                url,
                cancelToken,
                null,
                true);

            return response;
        }

        internal async Task<T> GetRequest<T>(
            string baseUrl,
            IQueryParameters? queryParameters,
            CancellationToken cancelToken,
            bool requireAuth = false)
            where T : MangaDexResponse
        {
            string query = baseUrl.BuildQuery(queryParameters);
            return await GetRequest<T>(query, cancelToken, requireAuth);
        }

        internal async Task<TResponse> GetRequest<TResponse>(string url, CancellationToken cancelToken, bool requireAuth = false)
            where TResponse : MangaDexResponse
        {
            var response = await SendRequest<TResponse>(
                HttpMethod.Get,
                url,
                cancelToken,
                null,
                requireAuth);

            return response;
        }

        internal async Task<CollectionResponse<T>> GetCollectionRequest<T>(
            string baseUrl,
            IQueryParameters? queryParameters,
            CancellationToken cancelToken,
            bool requireAuth = false)
            where T : ResourceDto
        {
            string query = baseUrl.BuildQuery(queryParameters);
            //FollowedList gets grabbed from here
            return await GetCollectionRequest<T>(query, cancelToken, requireAuth);
        }

        internal async Task<CollectionResponse<T>> GetCollectionRequest<T>(string url, CancellationToken cancelToken, bool requireAuth = false)
            where T : ResourceDto
        {
            var response = await SendRequest<CollectionResponse<T>>(
                HttpMethod.Get,
                url,
                cancelToken,
                null,
                requireAuth);

            return response;
        }

        internal async Task<T> GetObjectRequest<T>(
            string baseUrl,
            IQueryParameters? queryParameters,
            CancellationToken cancelToken,
            bool requireAuth = false)
            where T : ResourceDto
        {
            string query = baseUrl.BuildQuery(queryParameters);
            return await GetObjectRequest<T>(query, cancelToken, requireAuth);
        }

        internal async Task<T> GetObjectRequest<T>(string url, CancellationToken cancelToken, bool requireAuth = false)
            where T : ResourceDto
        {
            var response = await SendRequest<ObjectResponse<T>>(
                HttpMethod.Get,
                url,
                cancelToken,
                null,
                requireAuth);
            return response.Data;
        }

        internal async Task<TResponse> PostRequest<TResponse>(
            string url,
            CancellationToken cancelToken,
            IQueryParameters? parameters = null)
            where TResponse : MangaDexResponse
        {
            return await SendRequest<TResponse>(
                HttpMethod.Post,
                url,
                cancelToken,
                parameters,
                true);
        }

        internal async Task<TResponse> PostRequestWithBody<TRequest, TResponse>(
            TRequest requestObject,
            string url,
            CancellationToken cancelToken,
            IQueryParameters? parameters = null)
            where TResponse : MangaDexResponse
        {
            return await SendRequestWithBody<TRequest, TResponse>(
                HttpMethod.Post,
                requestObject,
                url,
                cancelToken,
                parameters,
                true);
        }

        internal async Task<TResponse> SendRequest<TResponse>(
            HttpMethod method,
            string url,
            CancellationToken cancelToken,
            IQueryParameters? parameters = null,
            bool requiresAuth = false)
            where TResponse : MangaDexResponse
        {
            url = url.BuildQuery(parameters);
            var request = new HttpRequestMessage(method, url);
            if (requiresAuth)
            {
                await mangaDexClient.Auth.AddAuthorizationHeaders(request, cancelToken);
            }

            HttpResponseMessage response = await httpClient.SendAsync(request, cancelToken);
            Stream jsonStream = await response.Content.ReadAsStreamAsync(cancelToken);


            //var request2 = new HttpRequestMessage(HttpMethod.Post, "https://api.mangadex.org/settings/template");
            //await mangaDexClient.Auth.AddAuthorizationHeaders(request2, cancelToken);
            ////request2.Content = new StringContent(JsonSerializer.Serialize(new SettingsClass()), System.Text.Encoding.UTF8, "application/json");
            //HttpResponseMessage response2 = await httpClient.SendAsync(request2, cancelToken);
            //var strink = response2.Content.ReadAsStringAsync();

#pragma warning disable CS8600// Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            TResponse deserializedResponse;
            try
            {
                deserializedResponse = await JsonSerializer.DeserializeAsync<TResponse>(jsonStream, jsonOptions, cancelToken);
            }
            catch (JsonException)
            {
                response.EnsureSuccessStatusCode();
                throw;
            }

            if (response.IsSuccessStatusCode == false)
            {
                deserializedResponse.EnsureResponseIsValid();
                response.EnsureSuccessStatusCode();
            }

#pragma warning disable CS8603 // Possible null reference return.
            return deserializedResponse;
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }

        internal async Task<TResponse> SendRequestWithBody<TRequest, TResponse>(
            HttpMethod method,
            TRequest requestObject,
            string url,
            CancellationToken cancelToken,
            IQueryParameters? parameters = null,
            bool requiresAuth = false)
            where TResponse : MangaDexResponse
        {
            url = url.BuildQuery(parameters);
            var request = new HttpRequestMessage(method, url);
            if (requiresAuth)
            {
                await mangaDexClient.Auth.AddAuthorizationHeaders(request, cancelToken);
            }
            request.Content = JsonContent.Create(requestObject, new MediaTypeHeaderValue("application/json"), jsonOptions);

            HttpResponseMessage response = await httpClient.SendAsync(request, cancelToken);
            Stream jsonStream = await response.Content.ReadAsStreamAsync(cancelToken);

#pragma warning disable CS8600// Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            TResponse deserializedResponse;
            try
            {
                deserializedResponse = await JsonSerializer.DeserializeAsync<TResponse>(jsonStream, jsonOptions, cancelToken);
            }
            catch (JsonException)
            {
                response.EnsureSuccessStatusCode();
                throw;
            }

            if (response.IsSuccessStatusCode == false)
            {
                deserializedResponse.EnsureResponseIsValid();
                response.EnsureSuccessStatusCode();
            }

#pragma warning disable CS8603 // Possible null reference return.
            return deserializedResponse;
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }
    }
    public class SettingsClass : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public Action SettingsChanged { get; set; }

        public bool DataSaver
        {
            get => _dataSaver;
            set
            {
                _dataSaver = value;
                OnPropertyChanged();
            }
        }

        public bool DifferentPort
        {
            get => _differentPort;
            set
            {
                _differentPort = value;
                OnPropertyChanged();
            }
        }

        public bool Erotica
        {
            get => _erotica;
            set
            {
                _erotica = value;
                OnPropertyChanged();
            }
        }

        public bool Pornographic
        {
            get => _pornographic;
            set
            {
                _pornographic = value;
                OnPropertyChanged();
            }
        }

        public bool Safe
        {
            get => _safe;
            set
            {
                _safe = value;
                OnPropertyChanged();
            }
        }

        public bool Suggestive
        {
            get => _suggestive;
            set
            {
                _suggestive = value;
                OnPropertyChanged();
            }
        }

        public int ChapterLoadAmount
        {
            get => _chapterAmount;
            set
            {
                if (_chapterAmount != value)
                {
                    _chapterAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _chapterAmount { get; set; } = 32;
        private bool _dataSaver { get; set; } = false;
        private bool _differentPort { get; set; } = false;
        private bool _erotica { get; set; } = true;
        private bool _pornographic { get; set; } = false;
        private bool _safe { get; set; } = true;
        private bool _suggestive { get; set; } = true;

        public void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}