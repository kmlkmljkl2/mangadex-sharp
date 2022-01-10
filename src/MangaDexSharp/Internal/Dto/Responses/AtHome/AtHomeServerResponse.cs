#nullable disable
using System;
using System.Collections.Generic;

namespace MangaDexSharp.Internal.Dto.Responses.AtHome
{
    internal class AtHomeServerResponse : MangaDexResponse
    {
        public string BaseUrl { get; set; }

        public ChapterAtHomeData Chapter { get; set; }
    }
}
