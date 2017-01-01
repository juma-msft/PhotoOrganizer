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
    enum PhotoItemSource
    {
        PictureLibrary
    }

    class PhotoItem : INotifyPropertyChanged
    {
        public PhotoItemSource Source
        {
            get { return this.source; }
            set { if (this.source != value) { this.source = value; NotifyPropertyChanged(); } }
        }

        private PhotoItemSource source;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal static PhotoItem CreateFromStorageItem(PhotoItemSource source, IStorageItem item)
        {
            PhotoItem result = new PhotoItem();
            result.source = source;

            return result;
        }
    }
}
