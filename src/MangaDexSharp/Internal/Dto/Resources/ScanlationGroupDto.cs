#nullable disable

using MangaDexSharp.Constants;
using MangaDexSharp.Internal.Attributes;
using MangaDexSharp.Internal.Dto.ResourceAttributes;
using MangaDexSharp.Resources;
using System.Collections.Generic;

namespace MangaDexSharp.Internal.Dto.Resources
{
    [HasAttributesOtType(typeof(ScanlationGroupAttributes))]
    [DtoFor(typeof(ScanlationGroup))]
    internal class ScanlationGroupDto : ResourceDto
    {
        public ScanlationGroupAttributes Attributes => GetAttributes<ScanlationGroupAttributes>();

        [Relationship(RelationshipNames.GroupLeader)]
        public IEnumerable<UserDto> LeaderRelations { get; set; }

        [Relationship(RelationshipNames.GroupMember)]
        public IEnumerable<UserDto> MemberRelations { get; set; }
    }
}