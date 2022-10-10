using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageDomain.Models.Images
{
    public interface IImageRepository
    {
        void Save(Image image);
        Image Find(ImageID id);
        IReadOnlyList<Image> FindAll();
    }
}
