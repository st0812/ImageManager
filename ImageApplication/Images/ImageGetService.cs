using ImageApplication.Tags;
using ImageDomain.Models.Images;
using ImageDomain.Models.Tags;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageApplication.Images
{
    public class ImageGetService
    {
        private IImageRepository ImageRepository { get; set; }

        public ImageGetService(IImageRepository imageRepository)
        {
            if (imageRepository == null) throw new ArgumentNullException(nameof(ImageRepository));
            ImageRepository = imageRepository;
        }

        public IReadOnlyList<ImageData> GetAll()
        {

            return ImageRepository
                .FindAll()
                .Select(image => new ImageData(image))
                .ToList()
                .AsReadOnly();
        }

        public IReadOnlyList<ImageData> SearchImages(ImageSearchCommand command)
        {
            return GetAll()
                .Where(imageData => command.TagIDs.All(searchID => imageData.AttachedTagIDs.Contains(searchID)))
                .ToList()
                .AsReadOnly();

        }
    }
}
