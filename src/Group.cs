using System;

namespace RDP_Portal
{
    public class Group : IComparable<Group>
    {
        public int Id { get; set; }
        public string GroupName { get; set; } = "";

        public int CompareTo(Group? other)
        {
            if (other == null) return 1;
            return string.Compare(GroupName, other.GroupName, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Group other)
            {
                return string.Equals(GroupName, other.GroupName, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return GroupName?.ToLowerInvariant().GetHashCode() ?? 0;
        }
    }
}
