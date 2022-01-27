#nullable disable

using System.Collections.Generic;

namespace MangaDexSharp.Internal.Dto.Responses.AtHome
{
    internal class ChapterAtHomeData
    {
        public IEnumerable<string> Data { get; set; }
        public IEnumerable<string> DataSaver { get; set; }
        public string Hash { get; set; }
    }
}