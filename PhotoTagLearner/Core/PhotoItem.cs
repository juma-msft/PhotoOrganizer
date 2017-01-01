using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace PhotoTagLearner.Core
{
    class BuiltInPhotoItemSource
    {
        public const string PictureLibrary = "PictureLibrary";
    }

    public class PhotoItem : INotifyPropertyChanged
    {
        public IRandomAccessStream ImageStream
        {
            get { return this.imageStream; }
            set { if (this.imageStream != value) { this.imageStream = value; NotifyPropertyChanged(); } }
        }

        private IRandomAccessStream imageStream;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal static async Task<PhotoItem> CreateFromStorageItem(StorageFile item)
        {
            PhotoItem result = new PhotoItem();
            result.imageStream = await item.OpenReadAsync();
            return result;
        }
    }
}
