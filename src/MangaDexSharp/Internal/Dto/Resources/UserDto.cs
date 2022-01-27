#nullable disable

using MangaDexSharp.Constants;
using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Internal.Dto.ResourceAttributes;
using MangaDexSharp.Resources;
using System.Collections.Generic;

namespace MangaDexSharp.Internal.Dto.Resources
{
    [HasAttributesOtType(typeof(UserAttributes))]
    [DtoFor(typeof(User))]
    internal class UserDto : ResourceDto
    {
        public UserAttributes Attributes => GetAttributes<UserAttributes>();

        [Relationship(RelationshipNames.ScanlationGroup)]
        public IEnumerable<ScanlationGroupDto> ScanlationGroupRelations { get; set; }
    }
}