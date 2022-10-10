using ImageDomain.Models.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageDomain.Models.Images
{
    public class Image
    {
        public ImageID ID { get; }
        private ImagePath _imagePath;
        public ImagePath ImagePath {
            get
            {
                return _imagePath;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _imagePath = value;
            }
        }
        private List<TagID> _attachedTags { get; set; }

        public IReadOnlyList<TagID> AttachedTags
        {
            get
            {
                return _attachedTags.AsReadOnly();
            }
        }

      
        
        public Image(ImagePath imagePath,IEnumerable<TagID> tags):this(new ImageID(Guid.NewGuid().ToString()),imagePath,tags)
        {
            
        }

        public Image(ImageID imageID,ImagePath imagePath, IEnumerable<TagID> tags)
        {
            ID = imageID;
            ImagePath = imagePath;
            _attachedTags = tags.ToList();
        }

        public Image(ImageID imageID, ImagePath imagePath):this(imageID,imagePath,new List<TagID>())
        {

        }
        public void AttachTag(TagID tagID)
        {
            if (tagID == null) throw new ArgumentNullException(nameof(tagID));
            if (!_attachedTags.Contains(tagID)) _attachedTags.Add(tagID);
        }

        public void DetachAllTags()
        {
            _attachedTags = new List<TagID>();
        }
       
    }
}
