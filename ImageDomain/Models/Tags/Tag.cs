using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageDomain.Models.Tags
{
    public class Tag
    {

        public TagID TagID { get; }
        public TagName TagName { get; }

        public Tag(TagName tagName):this(new TagID(Guid.NewGuid().ToString()),tagName)
        {

        }
        public Tag(TagID tagID, TagName value)
        {
            if (tagID == null) throw new ArgumentNullException(nameof(tagID));
            if (value == null) throw new ArgumentException(nameof(value));
            TagID = tagID;
            TagName = value;
        }
    }
}
