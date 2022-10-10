using ImageDomain.Models.Tags;
using System;

namespace ImageApplication.Tags
{
    public class TagRegisterService
    {
        private ITagRepository TagRepository { get; set; }

        public TagRegisterService(ITagRepository tagRepository)
        {
            if (tagRepository == null) throw new ArgumentNullException(nameof(tagRepository));
            TagRepository = tagRepository;
        }

        public void Register(TagRegisterCommand command)
        {
            if (TagRepository.Find(new TagName(command.TagName)) != null) throw new Exception();
            var tag = new Tag(new TagName(command.TagName));
            TagRepository.Save(tag);
        }
    }
}
