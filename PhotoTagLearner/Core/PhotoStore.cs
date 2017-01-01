using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace PhotoTagLearner.Core
{
    class PhotoStore
    {
        public PhotoStore()
        {
        }

        public void StartPopulation(IEnumerable<PhotoItemSource> sourceList)
        {
            PopulateAsync(sourceList);
        }

        private IAsyncAction PopulateAsync(IEnumerable<PhotoItemSource> sourceList)
        {
            return Task.Run(() =>
            {
                Parallel.ForEach(sourceList, (source) =>
                {
                    switch(source)
                    {
                        case PhotoItemSource.PictureLibrary:
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
                    var photoItem = PhotoItem.CreateFromStorageItem(PhotoItemSource.PictureLibrary, item);
                    photoItems.Add(photoItem);
                }
            }

            SourcePopulationComplete?.Invoke(this, PhotoItemSource.PictureLibrary);
        }

        public event TypedEventHandler<PhotoStore, PhotoItemSource> SourcePopulationComplete;
    }
}
