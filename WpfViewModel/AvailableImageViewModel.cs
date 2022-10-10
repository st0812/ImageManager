using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using WpfViewModel;

namespace WpfViewModel

{
    public class AvailableImageViewModel : NotificationObject
    {
        public string FilePath { get; }
        public AvailableImageViewModel(string filePath)
        {
            FilePath = filePath;
            BitmapImageSource = new BitmapImageSource(filePath, 100);

        }
        public BitmapImageSource BitmapImageSource { get; }
    }


    public class ImageSearch
    {

        private string path;
        public ImageSearch(string path)
        {
            this.path = path;
        }
        public IEnumerable<AvailableImageViewModel> FetchResult()
        {
            string pattern = @"\.(jp(e)?g|bmp|png|gif|jfif)$";
            return Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).Where(file => Regex.IsMatch(file, pattern, RegexOptions.IgnoreCase)).Select(file => new AvailableImageViewModel(file));

        }
    }
}
