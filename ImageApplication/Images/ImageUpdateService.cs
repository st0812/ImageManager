using ImageApplication.Tags;
using ImageDomain.Models.Images;
using ImageDomain.Models.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApplication.Images
{
    public class ImageUpdateService
    {
        private IImageRepository ImageRepository { get; }
        private TagGetService TagGetService { get; }

        public ImageUpdateService(IImageRepository imageRepository, TagGetService tagGetService)
        {
            if (imageRepository == null) throw new ArgumentNullException(nameof(ImageRepository));
            if (tagGetService == null) throw new ArgumentNullException(nameof(tagGetService));
            ImageRepository = imageRepository;
            TagGetService = tagGetService;
        }

        public void Update(ImageUpdateCommand command)
        {
            var Image =ImageRepository.Find(new ImageID(command.ID));
            if (Image == null) throw new Exception();
           
            foreach (var tagID in command.TagIDs)
            {
                if (TagGetService.Get(new TagGetCommand() { ID = tagID }) == null)
                {
                    throw new Exception();
                }
            }
            var tags = command.TagIDs.Select(tagID => new TagID(tagID));
            Image.DetachAllTags();
            foreach(var tag in tags)
            {
                Image.AttachTag(tag);
            }
            ImageRepository.Save(Image);
        }
    }
}
