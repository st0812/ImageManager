using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApplication.Images
{
    public class ImageRegisterCommand
    {
        public string FilePath { get; set; }
        public IEnumerable<string> TagIDs { get; set; }

    }

   
    public class ImageUpdateCommand
    {
        public string ID { get; set; }
        public IEnumerable<string> TagIDs { get; set; }
    }

    public class ImageSearchCommand
    {
        public IEnumerable<string> TagIDs { get; set; }
    }

}
