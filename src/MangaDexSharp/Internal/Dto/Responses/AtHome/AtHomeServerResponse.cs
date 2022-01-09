#nullable disable
using System;
using System.Collections.Generic;

namespace MangaDexSharp.Internal.Dto.Responses.AtHome
{
    internal class AtHomeServerResponse : MangaDexResponse
    {
        public string BaseUrl { get; set; }

        /// <summary>
        /// Contains Urls to full sized images of Chapter
        /// </summary>
        public IEnumerable<Uri> Data { get; set; }

        /// <summary>
        /// Contains Urls to compressed images of Chapter
        /// </summary>
        public IEnumerable<Uri> DataSaver { get; set; }

        public string Hash { get; set; }
    }
}
