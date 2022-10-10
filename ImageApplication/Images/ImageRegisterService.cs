using ImageApplication.Tags;
using ImageDomain.Models.Images;
using ImageDomain.Models.Tags;
using System;
using System.Linq;

namespace ImageApplication.Images
{
    public class ImageRegisterService
    {
        private IImageRepository ImageRepository { get; }
        private TagGetService TagGetService { get; }

        public ImageRegisterService(IImageRepository imageRepository,  TagGetService tagGetService)
        {
            if (imageRepository == null) throw new ArgumentNullException(nameof(ImageRepository));
            if (tagGetService == null) throw new ArgumentNullException(nameof(tagGetService));
            ImageRepository = imageRepository;
            TagGetService = tagGetService;
        }

        public void Register(ImageRegisterCommand command)
        {
            foreach(var tagID in command.TagIDs)
            {
                if(TagGetService.Get(new TagGetCommand() {ID=tagID })==null)
                {
                    throw new Exception();
                }
            }
            var tags=command.TagIDs.Select(tagID => new TagID(tagID));
            var Image = new Image(new ImagePath(command.FilePath),tags);
            ImageRepository.Save(Image);
        }
    }
}
