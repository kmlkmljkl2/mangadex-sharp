﻿using System;
using System.Collections.Generic;

using MangaDexSharp.Constants;
using MangaDexSharp.Enums;
using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Parameters.Order.Manga;

namespace MangaDexSharp.Parameters.Manga
{
    public sealed class GetMangaFeedParameters : ListQueryParameters
    {
        [QueryParameterName("contentRating")]
        public ICollection<ContentRating>? ContentRatings { get; set; }

        [QueryParameterName("createdAtSince")]
        public DateTime? CreatedAtSince { get; set; }

        [QueryParameterName("excludedOriginalLanguage")]
        public ICollection<string>? ExcludedOriginalLanguages { get; set; }

        [QueryParameterName("includeFutureUpdates")]
        public bool IncludeFutureUpdates { get; set; } = true;

        [QueryParameterName("originalLanguage")]
        public ICollection<string>? OriginalLanguages { get; set; }

        [QueryParameterName("publishAtSince")]
        public DateTime? PublishAtSince { get; set; }

        [QueryParameterName("translatedLanguage")]
        public ICollection<string>? TranslatedLanguages { get; set; }

        [QueryParameterName("updatedAtSince")]
        public DateTime? UpdatedAtSince { get; set; }

        public GetMangaFeedParameters() : base(ListQueryRestrictions.MangaFeedMaximumAmount)
        {
            base.Amount = ListQueryRestrictions.MangaFeedDefaultAmount;
            Order =  new GetMangaFeedOrderParameters();
        }

        public GetMangaFeedParameters(GetMangaFeedOrderParameters parameters) : base(ListQueryRestrictions.MangaFeedMaximumAmount)
        {
            base.Amount = ListQueryRestrictions.MangaFeedDefaultAmount;
            Order = parameters;
        }
    }
}
