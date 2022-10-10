using ImageViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfViewModel;

namespace ImageViewer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void TagList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in TagList.SelectedItems)
            {
                var tag = item as TagViewModel;
                if (tag != null)
                {
                    tag.IsSelected = !tag.IsSelected;
                }
            }
        }
       
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) ((SearchImageViewModel)DataContext).SearchImageCommand.Execute(null);
        }

        private void AddTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) ((SearchImageViewModel)DataContext).RegisterNewTagCommand.Execute(null);
        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            var vm = (SearchImageViewModel)DataContext;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var path = vm.TargetImage.FilePath;
                DataObject dataObject = new DataObject();
                dataObject.SetData(DataFormats.FileDrop, new string[] { path });
                DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Copy);

            }
        }
    }
}
