using ImageDomain.Models.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApplication.Tags
{
    public class TagGetService
    {
        private ITagRepository TagRepository { get; set; }

        public TagGetService(ITagRepository tagRepository)
        {
            if (tagRepository == null) throw new ArgumentNullException(nameof(tagRepository));
            TagRepository = tagRepository;
        }

        public TagData Get(TagGetCommand command)
        {
            return new TagData(TagRepository.Find(new TagID(command.ID)));
        }

        public IReadOnlyList<TagData> GetAll()
        {
            return TagRepository
                .FindAll()
                .Select(tag=>new TagData(tag))
                .ToList()
                .AsReadOnly();
        }

        public TagData Find(TagSearchCommand command)
        {
            return new TagData(TagRepository.Find(new TagName(command.TagName)));
        }
    }
}
