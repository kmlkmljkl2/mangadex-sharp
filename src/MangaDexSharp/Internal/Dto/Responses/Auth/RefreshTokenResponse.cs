#nullable disable
using MangaDexSharp.Internal.Dto.Responses.Objects;

namespace MangaDexSharp.Internal.Dto.Responses.Auth
{
    internal class RefreshTokenResponse : MangaDexResponse
    {
        public string Access_Token { get; set; }
        public string Refresh_Token { get; set; }
    }
}
