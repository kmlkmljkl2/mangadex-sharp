﻿using MangaDexSharp.Constants;
using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Internal.Dto.ResourceAttributes;
using MangaDexSharp.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaDexSharp.Internal.Dto.Resources
{
    [HasAttributesOtType(typeof(CreatorAttribute))]
    [DtoFor(typeof(Manga))]
    internal class CreatorDto : ResourceDto
    {
        public CreatorAttribute Attributes => GetAttributes<CreatorAttribute>();

        [Relationship(RelationshipNames.Manga)]
        public IEnumerable<MangaDto> MangaRelations { get; set; }
    }
}
