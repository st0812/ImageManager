using ImageApplication.Images;
using ImageApplication.Tags;
using SQLiteInfrastructure.Images;
using SQLiteInfrastructure.Tags;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfViewModel;

namespace ImageUploader
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public static ImageGetService ImageGetService { get; private set; }
        public static ImageRegisterService ImageRegisterService { get; private set; }
        public static TagGetService TagGetService { get; private set; }
        public static TagRegisterService TagRegisterService { get; private set; }
        private static string settingFilePath = "./settings.xml";
        public static string TempImageFolderPath = "./temp";

        protected override void OnStartup(StartupEventArgs e)
        {
            var loader = new SettingLoader(settingFilePath);
            ImageGetService = new ImageGetService(new SQLiteImageRepository(loader.ConnectionString, loader.ImageFolderPath));
            TagGetService = new TagGetService(new SQLiteTagRepository(loader.ConnectionString));
            ImageRegisterService = new ImageRegisterService(new SQLiteImageRepository(loader.ConnectionString, loader.ImageFolderPath), TagGetService);
            TagRegisterService = new TagRegisterService(new SQLiteTagRepository(loader.ConnectionString));
            TempImageFolderPath = loader.TempImageFolderPath;
            MainWindow window = new MainWindow();
            window.Show();
        }
    }
}
