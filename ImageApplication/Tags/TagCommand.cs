using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApplication.Tags
{
    public class TagRegisterCommand
    {
        public string TagName { get; set; }
    }

    public class TagGetCommand
    {
        public string ID { get; set; }
    }

    public class TagSearchCommand
    {
        public string TagName { get; set; }
    }

}
