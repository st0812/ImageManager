using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageDomain.Models.Tags
{
    public class TagName : IEquatable<TagName>
    {
        public string Value { get; }

        public TagName(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException(nameof(value));
            if (value.Contains(' ') || value.Contains('　')) throw new ArgumentException(nameof(value));
            Value = value;
        }
        public bool Equals(TagName other)
        {
            return Equals(Value, other.Value);
        }

    }
}
