using ImageApplication.Tags;
using System.Collections.Generic;
using System.Linq;
namespace WpfViewModel
{
    public class TagCollectionViewModel : NotificationObject
    {

        public IReadOnlyCollection<TagViewModel> AvailableTags { get; }
        public TagCollectionViewModel(IEnumerable<TagData> tagDatas)
        {
            var newAvailableTags = tagDatas.Select(tag => new TagViewModel(tag)).ToList();
            AvailableTags = newAvailableTags.AsReadOnly();
        }

        public TagCollectionViewModel(IEnumerable<TagData> tagDatas, TagCollectionViewModel oldViewModel)
        {
            var newAvailableTags = tagDatas.Select(tag => new TagViewModel(tag)).ToList();
            foreach (var tag in newAvailableTags)
            {
                var oldTag = oldViewModel.AvailableTags.FirstOrDefault(tag2 => tag.ID == tag2.ID);
                if (oldTag != null) tag.IsSelected = oldTag.IsSelected;
                else tag.IsSelected = true;
            }

            AvailableTags = newAvailableTags.AsReadOnly();
        }

        public void DisableAllTags()
        {
            foreach (var tag in AvailableTags) tag.IsSelected = false;
        }

        public IEnumerable<string> GetSelectedTagIDs()
        {
            return AvailableTags.Where(tag => tag.IsSelected).Select(tag => tag.ID);
        }

    }
}
