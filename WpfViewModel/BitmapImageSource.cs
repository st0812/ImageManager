using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;

namespace WpfViewModel
{
    public class BitmapImageSource
    {
        public string FilePath { get; }
        public int Width { get; }
        public BitmapImageSource(string filePath, int thumbnailSize)
        {
            FilePath = filePath;
            Width = thumbnailSize;
        }

        public BitmapSource BitmapSource
        {
            get => BitmapToBitmapSource(CreateBitmapImage(FilePath));
        }

        private BitmapSource _bitmapSource;
        public BitmapSource Thumbnail
        {
            get
            {
                if (_bitmapSource != null) return _bitmapSource;
                _bitmapSource = BitmapToBitmapSource(GetThumbnail(CreateBitmapImage(FilePath), Width));
                return _bitmapSource;

            }
        }

        private static Bitmap CreateBitmapImage(string filename)
        {
            FileStream fs = new FileStream(
                filename,
                FileMode.Open,
                FileAccess.Read);
            Image img = Image.FromStream(fs);
            fs.Close();
            return new Bitmap(img);
        }

        private static Bitmap GetThumbnail(Image img, int w)
        {
            Bitmap canvas = new Bitmap(w, w);
            Graphics g = Graphics.FromImage(canvas);
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, w, w);

            float fh = w;
            float fw = w;
            int n = Math.Max(img.Width, img.Height);
            if (n > 0)
            {
                fw = (float)img.Width / n * w;
                fh = (float)img.Height / n * w;
            }
            g.DrawImage(img, (w - fw) / 2, (w - fh) / 2, fw, fh);
            g.Dispose();
            return canvas;
        }


        private static BitmapSource BitmapToBitmapSource(Bitmap bitmap)
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                return
                     BitmapFrame.Create(
                         ms,
                         BitmapCreateOptions.None,
                         BitmapCacheOption.OnLoad
                     );
            }
        }
    }
}
