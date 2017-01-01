using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PhotoTagLearner.Core
{
    class BuiltInPhotoItemSource
    {
        public const string PictureLibrary = "PictureLibrary";
    }

    class PhotoItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal static PhotoItem CreateFromStorageItem(IStorageItem item)
        {
            PhotoItem result = new PhotoItem();
            return result;
        }
    }
}
