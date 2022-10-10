using ImageApplication.Images;
using System.Collections.Generic;
using System.IO;
using WpfViewModel;

namespace ImageViewer.ViewModels
{
    public class ImageViewModel:NotificationObject
    {
        public ImageViewModel(ImageData model)
        {
            Model = model;
            BitmapImageSource = new BitmapImageSource(Model.ImagePath, 100);
        }

        public ImageData Model { get; }

        public string ID { get => Model.ImageID; }
        public string FilePath { get => Model.ImagePath; }
        public BitmapImageSource BitmapImageSource { get; }
       
       
        public IEnumerable<string> AttachedTagIDs { get => Model.AttachedTagIDs; }

        public void CopyTo(string targetDir)
        {
            var filePath = Path.Combine(targetDir, Path.GetFileName(FilePath));
            File.Copy(FilePath, filePath);
        }
    }
}
