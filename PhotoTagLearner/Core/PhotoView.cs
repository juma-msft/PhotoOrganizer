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
using Windows.Foundation;

namespace PhotoTagLearner.Core
{
    public sealed class PhotoView : Control
    {
        public PhotoView()
        {
            this.DefaultStyleKey = typeof(PhotoView);
        }

        #region Dependency Properties
        public StyleSelector ControlStyleSelector
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

        public static readonly DependencyProperty ControlStyleSelectorProperty = DependencyProperty.Register("ControlStyleSelector", typeof(StyleSelector),
            typeof(PhotoView), new PropertyMetadata(DefaultStyleSelector.Instance, new PropertyChangedCallback(OnStyleSelectorChanged)));

        private static void OnStyleSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PhotoView pv = d as PhotoView;
            if ((pv != null) && (e.OldValue != e.NewValue))
            {
                pv.ApplyTemplate();
            }
        }

        public DataTemplateSelector ControlDataTemplateSelector
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

        private static void OnControlDataTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PhotoView pv = d as PhotoView;
            if ((pv != null) && (e.OldValue != e.NewValue))
            {
                var photoView = pv.GetTemplateChild("PART_PhotoView") as ListViewBase;
                photoView.ItemTemplateSelector = e.NewValue as DataTemplateSelector;
            }
        }

        public static readonly DependencyProperty ControlDataTemplateSelectorProperty = DependencyProperty.Register("ControlDataTemplateSelector", typeof(DataTemplateSelector),
            typeof(PhotoView), new PropertyMetadata(DefaultPhotoItemTemplateSelector.Instance, new PropertyChangedCallback(OnControlDataTemplateSelectorChanged)));

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

        private static IEnumerable<string> BuildDefaultPhotoItemSources()
        {
            return new string[] { BuiltInPhotoItemSource.PictureLibrary };
        }

        public IPhotoStore PhotoStore
        {
            get
            {
                return (IPhotoStore)GetValue(PhotoStoreProperty);
            }
            set
            {
                SetValue(PhotoStoreProperty, value);
            }
        }

        private static void OnPhotoStoreChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PhotoView pv = d as PhotoView;
            if ((pv != null) && (e.OldValue != e.NewValue) && (e.NewValue != null))
            {
                var photoStore = e.NewValue as IPhotoStore;
                pv.displayView.PhotoStore = photoStore;
                photoStore.SourcePopulationComplete += pv.OnPhotoStoreSourcePopulateComplete;
            }
        }

        public static readonly DependencyProperty PhotoStoreProperty = DependencyProperty.Register("PhotoStore", typeof(DataTemplateSelector),
            typeof(PhotoViewDisplay), new PropertyMetadata(DefaultPhotoStore.Instance, OnPhotoStoreChanged));

        #endregion 

        protected override void OnApplyTemplate()
        {
            var styleSelector = this.ControlStyleSelector;
            var photoStore = this.PhotoStore;
            container = GetTemplateChild("PART_PhotoViewContainer") as FrameworkElement;
            container.Style = styleSelector.SelectStyle(container, this);

            sourceList = GetTemplateChild("PART_SourceList") as ListViewBase;
            sourceList.Style = styleSelector.SelectStyle(sourceList, this);

            displayView = GetTemplateChild("PART_PhotoViewDisplay") as PhotoViewDisplay;
            displayView.Style = styleSelector.SelectStyle(displayView, this);

            var sources = this.PhotoItemSources as IEnumerable<string>;
            photoStore.SourcePopulationComplete += OnPhotoStoreSourcePopulateComplete;
            photoStore.StartPopulation(sources);

            base.OnApplyTemplate();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var result = base.MeasureOverride(availableSize);
            return result;
        }

        private void OnPhotoStoreSourcePopulateComplete(IPhotoStore sender, string args)
        {
            // The dispatch call is purposefully not awaited.

#pragma warning disable CS4014
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                displayView.SetFirstTag();
            });
#pragma warning restore CS4014
        }

        private FrameworkElement container;
        private ListViewBase sourceList;
        private PhotoViewDisplay displayView;
    }
}
