﻿#nullable disable

using MangaDexSharp.Constants;
using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Internal.Dto.ResourceAttributes;
using MangaDexSharp.Resources;
using System.Collections.Generic;

namespace MangaDexSharp.Internal.Dto.Resources
{
    [HasAttributesOtType(typeof(CustomListAttributes))]
    [DtoFor(typeof(CustomList))]
    internal class CustomListDto : ResourceDto
    {
        public CustomListAttributes Attributes => GetAttributes<CustomListAttributes>();

        [Relationship(RelationshipNames.Manga)]
        public IEnumerable<MangaDto> MangaRelations { get; set; }

        [Relationship(RelationshipNames.User)]
        public IEnumerable<UserDto> UserRelations { get; set; }
    }
}