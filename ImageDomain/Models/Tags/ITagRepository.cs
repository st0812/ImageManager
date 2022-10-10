using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageDomain.Models.Tags
{
    public interface ITagRepository
    {
        void Save(Tag tag);
        Tag Find(TagID id);

        Tag Find(TagName name);
        IReadOnlyList<Tag> FindAll();
    }
}
