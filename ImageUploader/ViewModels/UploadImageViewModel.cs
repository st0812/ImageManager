using ImageApplication.Images;
using ImageApplication.Tags;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using WpfViewModel;

namespace ImageUploader
{
    public class UploadImageViewModel: NotificationObject
    {
        private ObservableCollection<AvailableImageViewModel> _targetImageList;
        public ObservableCollection<AvailableImageViewModel> TargetImageList
        {
            get
            {
                return this._targetImageList;
            }
            set
            {
                this._targetImageList = value;
                RaisePropertyChanged();
            }
        }


        private AvailableImageViewModel _targetImage;
        public AvailableImageViewModel TargetImage
        {
            get
            {
                return this._targetImage;
            }
            set
            {
                this._targetImage = value;
                RaisePropertyChanged();
            }
        }

        private DelegateCommand _searchImageCommand;
        public DelegateCommand SearchImageCommand
        {
            get
            {
                return _searchImageCommand = _searchImageCommand ?? new DelegateCommand(
                    _ =>
                    {
                        var browser = new System.Windows.Forms.FolderBrowserDialog();

                        browser.Description = "フォルダーを選択してください";

                        if (browser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            TargetImageList = new ObservableCollection<AvailableImageViewModel>(new ImageSearch(browser.SelectedPath).FetchResult());
                            TargetImage = TargetImageList.FirstOrDefault();
                        }
                    });
            }
        }

        private NewTagNameViewModel _newTagName;

        public NewTagNameViewModel NewTagName
        {
            get
            {
                return this._newTagName;
            }
            set
            {
                this._newTagName = value;
                RaisePropertyChanged();
            }
        }

       

        private TagCollectionViewModel _availableTags;
        public TagCollectionViewModel AvailableTags
        {
            get => _availableTags;
            set
            {
                _availableTags = value;
                RaisePropertyChanged();
            }
        }

        private void RegisterNewTag()
        {
            if (!NewTagName.IsValid()) return;
            var newTagName=NewTagName.Normalize();
            foreach(var tag in AvailableTags.AvailableTags)
            {
                if (tag.Name == newTagName)
                {
                    tag.IsSelected = true;
                    NewTagName.Reset();
                    return;
                }
            }
            var command = new TagRegisterCommand()
            {
                TagName = newTagName
            };
            App.TagRegisterService.Register(command);
            AvailableTags = new TagCollectionViewModel(App.TagGetService.GetAll(), AvailableTags);
            NewTagName.Reset();
        }

        private DelegateCommand _registerNewTagCommand;
        public DelegateCommand RegisterNewTagCommand
        {
            get
            {
                return _registerNewTagCommand = _registerNewTagCommand ?? new DelegateCommand(_ => RegisterNewTag());
            }
        }

        private void RegisterNewImage()
        {
            var command = new ImageRegisterCommand()
            {
                TagIDs= AvailableTags.GetSelectedTagIDs(),
                FilePath=TargetImage.FilePath,
            };
            App.ImageRegisterService.Register(command);

            var nextIndex = TargetImageList.IndexOf(TargetImage);
            TargetImageList.Remove(TargetImage);
            nextIndex = Math.Min(nextIndex, TargetImageList.Count() - 1);
            TargetImage = TargetImageList[nextIndex];
            AvailableTags.DisableAllTags();
        }

        private DelegateCommand _registerNewImageCommand;
        public DelegateCommand RegisterNewImageCommand
        {
            get
            {
                return _registerNewImageCommand = _registerNewImageCommand ?? new DelegateCommand(_ => RegisterNewImage());
            }
        }

        public UploadImageViewModel()
        {
            NewTagName = new NewTagNameViewModel();
            AvailableTags = new TagCollectionViewModel(App.TagGetService.GetAll());
        }
    }
}
