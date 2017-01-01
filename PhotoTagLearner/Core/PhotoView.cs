using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace PhotoTagLearner.Core
{
    public sealed class PhotoView : Control
    {
        public PhotoView()
        {
            this.DefaultStyleKey = typeof(PhotoView);
            this.photoStore.SourcePopulationComplete += OnPhotoStoreSourcePopulateComplete;
        }

        #region Dependency Properties
        public StyleSelector StyleSelector
        {
            get
            {
                return (StyleSelector)GetValue(ControlStyleSelectorProperty);
            }
            set
            {
                SetValue(ControlStyleSelectorProperty, value);
            }
        }

        public static readonly DependencyProperty ControlStyleSelectorProperty = DependencyProperty.Register("StyleSelector", typeof(StyleSelector),
            typeof(PhotoView), new PropertyMetadata(DefaultStyleSelector.Instance, new PropertyChangedCallback(OnStyleSelectorChanged)));

        private static void OnStyleSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PhotoView pv = d as PhotoView;
            if ((pv != null) && (e.OldValue != e.NewValue))
            {
                pv.ApplyTemplate();
            }
        }

        public DataTemplateSelector DataTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)GetValue(ControlDataTemplateSelectorProperty);
            }
            set
            {
                SetValue(ControlDataTemplateSelectorProperty, value);
            }
        }

        private static void OnPhotoItemSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PhotoView pv = d as PhotoView;
            if ((pv != null) && (e.OldValue != e.NewValue))
            {
                var photoView = pv.GetTemplateChild("PART_PhotoView") as ListViewBase;
                photoView.ItemTemplateSelector = e.NewValue as DataTemplateSelector;
            }
        }

        public static readonly DependencyProperty ControlDataTemplateSelectorProperty = DependencyProperty.Register("ControlDataTemplateSelector", typeof(DataTemplateSelector),
            typeof(PhotoView), new PropertyMetadata(DefaultPhotoItemTemplateSelector.Instance, new PropertyChangedCallback(OnPhotoItemSelectorChanged)));

        public object PhotoItemSources
        {
            get
            {
                return GetValue(PhotoItemSourcesProperty);
            }
            set
            {
                SetValue(PhotoItemSourcesProperty, value);
            }
        }

        public static readonly DependencyProperty PhotoItemSourcesProperty = DependencyProperty.Register("PhotoItemSources", typeof(object),
            typeof(PhotoView), new PropertyMetadata(BuildDefaultPhotoItemSources(), new PropertyChangedCallback(OnPhotoItemSourcesChanged)));

        private static void OnPhotoItemSourcesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PhotoView pv = d as PhotoView;
            if ((pv != null) && (e.OldValue != e.NewValue))
            {
                pv.ApplyTemplate();
            }
        }

        private static IEnumerable<PhotoItemSource> BuildDefaultPhotoItemSources()
        {
            return new PhotoItemSource[] { PhotoItemSource.PictureLibrary };
        }

        #endregion

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var styleSelector = this.StyleSelector;
            var container = GetTemplateChild("PART_PhotoViewContainer") as FrameworkElement;
            container.Style = styleSelector.SelectStyle(container, this);

            var photoView = GetTemplateChild("PART_PhotoView") as ListViewBase;
            photoView.Style = styleSelector.SelectStyle(photoView, this);

            var tagList = GetTemplateChild("PART_TagList") as ListViewBase;
            tagList.Style = styleSelector.SelectStyle(tagList, this);

            var sourceList = GetTemplateChild("Part_SourceList") as ListViewBase;
            sourceList.Style = styleSelector.SelectStyle(sourceList, this);

            InitializeViewer(photoView);

        }

        private void InitializeViewer(ListViewBase view)
        {
            view.ItemsSource = this.photos;
            view.ItemTemplateSelector = this.PhotoItemTemplateSelector;
            var sources = this.PhotoItemSources as IEnumerable<PhotoItemSource>;
            this.photoStore.StartPopulation(sources);
        }

        private void OnPhotoStoreSourcePopulateComplete(PhotoStore sender, PhotoItemSource args)
        {
            // The dispatch call is purposefully not awaited.

#pragma warning disable CS4014
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
            });
#pragma warning restore CS4014
        }

        private List<string> photoTags = new List<string>();
        private List<PhotoItem> photos = new List<PhotoItem>();
        private PhotoStore photoStore = new PhotoStore();
    }
}
