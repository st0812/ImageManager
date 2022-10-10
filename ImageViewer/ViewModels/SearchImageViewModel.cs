using ImageApplication.Images;
using ImageApplication.Tags;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using WpfViewModel;

namespace ImageViewer.ViewModels
{
    public class SearchImageViewModel : NotificationObject
    {
        private IReadOnlyCollection<ImageViewModel> _targetImageList;
        public IReadOnlyCollection<ImageViewModel> TargetImageList
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


        private ImageViewModel _targetImage;
        public ImageViewModel TargetImage
        {
            get
            {
                return this._targetImage;
            }
            set
            {
                if (value == null)
                {
                    this._targetImage = null;
                    return;
                }
                this._targetImage = value;
                var assignedTags = _targetImage.AttachedTagIDs;
                foreach (var tag in AvailableTags.AvailableTags)
                {
                    if (assignedTags.Contains(tag.ID))
                    {
                        tag.IsSelected = true;
                    }
                    else
                    {
                        tag.IsSelected = false;
                    }
                }
                RaisePropertyChanged();
            }
        }

        private SearchWordViewModel _searchWord;
        public SearchWordViewModel SearchWord
        {
            get
            {
                return this._searchWord;
            }
            set
            {
                this._searchWord = value;

                RaisePropertyChanged();
            }
        }

        private ReadOnlyCollection<TagViewModel> _searchTags;
        public ReadOnlyCollection<TagViewModel> SearchTags
        {
            get => _searchTags;
            private set
            {
                _searchTags = value;
                RaisePropertyChanged();
            }
        }


        private DelegateCommand _addSearchWordCommand;
        public DelegateCommand AddSearchWordCommand
        {
            get
            {
                return _addSearchWordCommand = _addSearchWordCommand ?? new DelegateCommand(
                    parameter =>
                    {
                        var word = (parameter as TagViewModel).Name;
                        SearchWord.AddWord(word);
                    }
                    );
            }
        }


        private void SearchImage()
        {
            if (SearchWord.IsExist())
            {
                var tags = SearchWord
                    .ToKeywordss()
                    .Select(word => App.TagGetService.Find(new TagSearchCommand() { TagName = word }).TagID);

                var searchCommand = new ImageSearchCommand()
                {
                    TagIDs = tags
                };

                TargetImageList = App.ImageGetService.SearchImages(searchCommand)
                    .Select(image => new ImageViewModel(image))
                    .ToList()
                    .AsReadOnly();
            }
            else
            {
                TargetImageList = App.ImageGetService.GetAll()
                    .Select(image => new ImageViewModel(image))
                    .ToList()
                    .AsReadOnly();
            }

            TargetImage = TargetImageList.FirstOrDefault();
        }


        private DelegateCommand _searchImageCommand;
        public DelegateCommand SearchImageCommand
        {
            get
            {
                return _searchImageCommand = _searchImageCommand ?? new DelegateCommand(_ => SearchImage());
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
            var newTagName = NewTagName.Normalize();
            foreach (var tag in AvailableTags.AvailableTags)
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

            var latestTags = App.TagGetService.GetAll();
            AvailableTags = new TagCollectionViewModel(latestTags,AvailableTags);
            SearchTags = latestTags.Select(tag => new TagViewModel(tag)).ToList().AsReadOnly();
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

        private void UpdateImageTag()
        {
            var id = TargetImage.ID;
            var updateCommand = new ImageUpdateCommand()
            {
                ID = id,
                TagIDs = AvailableTags.GetSelectedTagIDs()
            };
            App.ImageUpdateService.Update(updateCommand);
            SearchImage();
            TargetImage = TargetImageList.FirstOrDefault(image => image.ID == id);
        }


        private DelegateCommand _updateImageTagCommand;
        public DelegateCommand UpdateImageTagCommand
        {
            get
            {
                return _updateImageTagCommand = _updateImageTagCommand ?? new DelegateCommand(_ => UpdateImageTag());
            }
        }

        private void OpenImagesToFolder()
        {
            var targetDir = Path.Combine(App.TempImageFolderPath, Guid.NewGuid().ToString());
            Directory.CreateDirectory(targetDir);
            foreach (var image in TargetImageList) image.CopyTo(targetDir);
            Process.Start("explorer.exe", targetDir);
        }

        private DelegateCommand _openImageToFolderCommand;
        public DelegateCommand OpenImageToFolderCommand
        {
            get
            {
                return _openImageToFolderCommand = _openImageToFolderCommand ?? new DelegateCommand(_ => OpenImagesToFolder());
            }
        }

        public SearchImageViewModel()
        {
            NewTagName = new NewTagNameViewModel();
            SearchWord = new SearchWordViewModel();
            var latestTags = App.TagGetService.GetAll();
            AvailableTags = new TagCollectionViewModel(latestTags);
            SearchTags = latestTags.Select(tag => new TagViewModel(tag)).ToList().AsReadOnly();
            SearchImage();
        }
    }
}
