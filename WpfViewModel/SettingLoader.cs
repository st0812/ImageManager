using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WpfViewModel
{
    public class SettingLoader
    {
        public string ConnectionString { get; }
        public string ImageFolderPath { get; }
        public string TempImageFolderPath { get; }
        public SettingLoader(string filepath)
        {
            XElement xml = XElement.Load(filepath);
            ConnectionString=xml.Element("DBPath").Value;
            ImageFolderPath = xml.Element("ImageFolderPath").Value;
            TempImageFolderPath = xml.Element("TempImageFolderPath").Value;
        }
    }
}
