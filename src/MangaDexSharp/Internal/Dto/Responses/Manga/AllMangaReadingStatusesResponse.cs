#nullable disable

using MangaDexSharp.Enums;
using System;
using System.Collections.Generic;

namespace MangaDexSharp.Internal.Dto.Responses.Manga
{
    internal class AllMangaReadingStatusesResponse : MangaDexResponse
    {
        public IDictionary<Guid, MangaReadingStatus> Statuses { get; set; }
    }
}