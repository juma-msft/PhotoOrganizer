using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace PhotoTagLearner.Core
{
    interface IPhotoStore
    {
        void StartPopulation(IEnumerable<string> sourceList);
        event TypedEventHandler<PhotoStore, string> SourcePopulationComplete;
    }

    class PhotoStore : IPhotoStore
    {
        public PhotoStore()
        {
        }

        public void StartPopulation(IEnumerable<string> sourceList)
        {
            PopulateAsync(sourceList);
        }

        private IAsyncAction PopulateAsync(IEnumerable<string> sourceList)
        {
            return Task.Run(() =>
            {
                Parallel.ForEach(sourceList, (source) =>
                {
                    switch (source)
                    {
                        case BuiltInPhotoItemSource.PictureLibrary:
                            PopulateFromPhotoPictureLibrary();
                            break;
                    }
                });
            }).AsAsyncAction();

        }

        private async void PopulateFromPhotoPictureLibrary()
        {
            var myPictures = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Pictures);
            var photoItems = new List<PhotoItem>();

            foreach (var folder in myPictures.Folders)
            {
                var items = await folder.GetItemsAsync();
                foreach (var item in items)
                {
                    var photoItem = PhotoItem.CreateFromStorageItem(item);
                    photoItems.Add(photoItem);
                }
            }

            SourcePopulationComplete?.Invoke(this, BuiltInPhotoItemSource.PictureLibrary);
        }

        public event TypedEventHandler<PhotoStore, string> SourcePopulationComplete;
    }
}
