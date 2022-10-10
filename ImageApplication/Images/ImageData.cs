using ImageDomain.Models.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApplication.Images
{
    public class ImageData
    {
        public string ImageID { get; }
        public string ImagePath { get; }

        public IEnumerable<string> AttachedTagIDs { get; }
       
        public ImageData(Image image)
        {
            this.ImageID = image.ID.Value;
            this.ImagePath = image.ImagePath.FilePath;
            this.AttachedTagIDs = image.AttachedTags.Select(tag=>tag.Value);
        }

    }
}
