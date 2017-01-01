using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace PhotoTagLearner.Core
{
    public interface IPhotoStore
    {
        void StartPopulation(IEnumerable<string> sourceList);
        IEnumerable<PhotoItem> GetItems(string tag);
        IEnumerable<string> GetAvailableTags(string source);
        event TypedEventHandler<IPhotoStore, string> SourcePopulationComplete;
    }

    class DefaultPhotoStore : IPhotoStore
    {
        public DefaultPhotoStore()
        {
        }

        public static IPhotoStore Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DefaultPhotoStore();
                }

                return instance;
            }
        }

        private static IPhotoStore instance;

        public void StartPopulation(IEnumerable<string> sourceList)
        {
#pragma warning disable CS4014
            PopulateAsync(sourceList);
#pragma warning restore CS4014
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

        public IEnumerable<PhotoItem> GetItems(string tag)
        {
            return from photo in items
                   select photo;
        }

        public IEnumerable<string> GetAvailableTags(string source)
        {
            return new string[] { "All" };
        }

        private async void PopulateFromPhotoPictureLibrary()
        {
            var myPictures = await KnownFolders.PicturesLibrary.GetFilesAsync();

            foreach (var image in myPictures)
            {
                var photoItem = await PhotoItem.CreateFromStorageItem(image);
                items.Add(photoItem);
            }

            SourcePopulationComplete?.Invoke(this, BuiltInPhotoItemSource.PictureLibrary);
        }

        public event TypedEventHandler<IPhotoStore, string> SourcePopulationComplete;

        private ConcurrentBag<PhotoItem> items = new ConcurrentBag<PhotoItem>();
    }
}
