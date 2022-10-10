using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageDomain.Models.Tags
{
    public class TagID:IEquatable<TagID>
    {
        public string Value { get; }
        public  TagID(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException(nameof(value));
            Value = value;
        }

        public bool Equals(TagID other)
        {
            return Equals(Value, other.Value);
        }
    }
}
