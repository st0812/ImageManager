using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageDomain.Models.Images
{
    public class ImageID:IEquatable<ImageID>
    {
        public string Value { get; }
        public ImageID(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException(nameof(value));
            Value = value;
        }

        public bool Equals(ImageID other)
        {
            return Equals(Value, other.Value);
        }
    }
}
