using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageDomain.Models.Images
{
    public class ImagePath
    {
        public ImagePath(string path)
        {
            FilePath = path;
        }

        private string _path;
        public string FilePath
        {
            get
            {
                return _path;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException(nameof(value));
                _path = value;
            }
        }

        public string Extension
        {
                get => Path.GetExtension(FilePath);
        }
    }
}
