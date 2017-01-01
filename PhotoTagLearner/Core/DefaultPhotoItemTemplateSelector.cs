using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PhotoTagLearner.Core
{
    class DefaultPhotoItemTemplateSelector : DataTemplateSelector
    {
        public static DefaultPhotoItemTemplateSelector Instance
        {
            get
            {
                if (_selector == null)
                {
                    _selector = new DefaultPhotoItemTemplateSelector();
                }

                return _selector;
            }
        }

        private static DefaultPhotoItemTemplateSelector _selector;

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return base.SelectTemplateCore(item);
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return base.SelectTemplateCore(item, container);
        }
    }
}
