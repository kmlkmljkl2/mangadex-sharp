using System.Collections.Generic;

namespace MangaDexSharp.Api.Data
{
    public class ChapterReadingMetadata
    {
        /// <summary>
        /// Contains Urls to full sized images of Chapter
        /// </summary>
        public IEnumerable<string> Data { get; }

        /// <summary>
        /// Contains Urls to compressed images of Chapter
        /// </summary>
        public IEnumerable<string> DataSaver { get; }

        /// <summary>
        /// Hash for reading via api
        /// </summary>
        public string Hash { get; }

        public ChapterReadingMetadata(
            IEnumerable<string> data,
            IEnumerable<string> dataSaver,
            string hash)
        {
            Data = data;
            DataSaver = dataSaver;
            Hash = hash;
        }
    }
}