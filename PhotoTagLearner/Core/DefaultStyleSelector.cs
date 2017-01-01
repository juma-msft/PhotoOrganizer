using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PhotoTagLearner.Core
{
    public class DefaultStyleSelector : StyleSelector
    {
        public static DefaultStyleSelector Instance
        {
            get
            {
                if (_selector == null)
                {
                    _selector = new DefaultStyleSelector();
                }

                return _selector;
            }
        }

        private static DefaultStyleSelector _selector;

        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            Style result = null;

            if (container is PhotoView)
            {
                result = SelectPhotoViewStyles(item as FrameworkElement, container as PhotoView);
            }
            else if (container is PhotoViewDisplay)
            {
                result = SelectPhotoViewDisplayStyles(item as FrameworkElement, container as PhotoViewDisplay);
            }

            if (result == null)
            {
                result = base.SelectStyleCore(item, container);
            }

            return result;
        }

        private Style SelectPhotoViewDisplayStyles(FrameworkElement item, PhotoViewDisplay container)
        {
            Style result = null;
            switch (item.Name)
            {
                case "PART_PhotoView":
                    if (item is GridView)
                    {
                        result = ResourceLookup.GenericXaml["PhotoView_DefaultGridView"] as Style;
                    }
                    break;
                case "PART_TagList":
                    if (item is ListView)
                    {
                        result = ResourceLookup.GenericXaml["PhotoView_DefaultHorizontalList"] as Style;
                    }
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        private Style SelectPhotoViewStyles(FrameworkElement item, PhotoView container)
        {
            Style result = null;
            switch(item.Name)
            {
                case "PART_PhotoViewContainer":
                    result = ResourceLookup.GenericXaml["PhotoView_DefaultViewContainer"] as Style;
                    break;
                case "PART_SourceList":
                    if (item is ListView)
                    {
                        result = ResourceLookup.GenericXaml["PhotoView_DefaultHorizontalList"] as Style;
                    }
                    break;
                case "PART_PhotoViewDisplay":
                    if (item is PhotoViewDisplay)
                    {
                        var photoViewDisplay = item as PhotoViewDisplay;
                        switch(photoViewDisplay.TagListLocation)
                        {
                            default:
                                result = ResourceLookup.GenericXaml["PhotoViewDisplay_TopTagList"] as Style;
                                break;
                        }
                    }
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }
    }
}
