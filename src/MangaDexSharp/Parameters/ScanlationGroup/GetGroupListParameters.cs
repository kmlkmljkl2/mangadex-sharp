using MangaDexSharp.Internal.Attributes;
using System;
using System.Collections.Generic;

namespace MangaDexSharp.Parameters.ScanlationGroup
{
    public sealed class GetGroupListParameters : ListQueryParameters, ICanQueryByIdCollection
    {
        [QueryParameterName("focusedLanguage")]
        public string? FocusedLanguage { get; set; }

        [QueryParameterName("ids")]
        public ICollection<Guid>? GroupIds { get; set; }

        [QueryParameterName("name")]
        public string? GroupName { get; set; }

        ICollection<Guid> ICanQueryByIdCollection.Ids
        {
            get => GroupIds ??= new HashSet<Guid>();
            set => GroupIds = value;
        }

        public GetGroupListParameters() : base()
        {
        }
    }
}