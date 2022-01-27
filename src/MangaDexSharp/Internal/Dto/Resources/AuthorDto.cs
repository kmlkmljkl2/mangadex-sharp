#nullable disable

using MangaDexSharp.Constants;
using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Internal.Dto.ResourceAttributes;
using MangaDexSharp.Resources;
using System.Collections.Generic;

namespace MangaDexSharp.Internal.Dto.Resources
{
    [HasAttributesOtType(typeof(AuthorAttributes))]
    [DtoFor(typeof(Author))]
    internal class AuthorDto : ResourceDto
    {
        public AuthorAttributes Attributes => GetAttributes<AuthorAttributes>();

        [Relationship(RelationshipNames.Manga)]
        public IEnumerable<MangaDto> MangaRelations { get; set; }
    }
}