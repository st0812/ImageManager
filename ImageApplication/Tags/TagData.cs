using ImageDomain.Models.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApplication.Tags
{
    public class TagData
    {
        public string TagID { get; }
        public string TagName { get; }
        public TagData(Tag tag)
        {
            this.TagID = tag.TagID.Value;
            this.TagName = tag.TagName.Value;
        }
    }
}
