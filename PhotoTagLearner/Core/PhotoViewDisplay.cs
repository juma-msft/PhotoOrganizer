﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace PhotoTagLearner.Core
{
    public enum Location
    {
        Left,
        Right,
        Top,
        Bottom
    }

    public sealed class PhotoViewDisplay : Control
    {
        public PhotoViewDisplay()
        {
            this.DefaultStyleKey = typeof(PhotoViewDisplay);
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
            typeof(PhotoViewDisplay), new PropertyMetadata(DefaultStyleSelector.Instance, new PropertyChangedCallback(OnStyleSelectorChanged)));

        private static void OnStyleSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pv = d as PhotoViewDisplay;
            if ((pv != null) && (e.OldValue != e.NewValue))
            {
                pv.ApplyTemplate();
            }
        }

        public Location TagListLocation
        {
            get
            {
                return (Location)GetValue(TagListLocationProperty);
            }
            set
            {
                SetValue(TagListLocationProperty, value);
            }
        }

        public static readonly DependencyProperty TagListLocationProperty = DependencyProperty.Register("TagListLocation", typeof(Location),
            typeof(PhotoViewDisplay), new PropertyMetadata(Location.Left, new PropertyChangedCallback(OnTagListLocationChanged)));

        private static void OnTagListLocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pv = d as PhotoViewDisplay;
            if ((pv != null) && (e.OldValue != e.NewValue))
            {
                var styleSelector = pv.ControlStyleSelector;
                styleSelector.SelectStyle(pv, null);
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
            var pv = d as PhotoViewDisplay;
            if ((pv != null) && (e.OldValue != e.NewValue))
            {
                pv.tagList.ItemTemplateSelector = e.NewValue as DataTemplateSelector;
            }
        }

        public static readonly DependencyProperty ControlDataTemplateSelectorProperty = DependencyProperty.Register("ControlDataTemplateSelector", typeof(DataTemplateSelector),
            typeof(PhotoViewDisplay), new PropertyMetadata(DefaultPhotoItemTemplateSelector.Instance, new PropertyChangedCallback(OnControlDataTemplateSelectorChanged)));

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

        public static readonly DependencyProperty PhotoStoreProperty = DependencyProperty.Register("PhotoStore", typeof(DataTemplateSelector),
            typeof(PhotoViewDisplay), new PropertyMetadata(DefaultPhotoStore.Instance, null));

        #endregion

        protected override void OnApplyTemplate()
        {
            var styleSelector = this.ControlStyleSelector;
            var dataTemplateSelector = this.ControlDataTemplateSelector;

            tagList = GetTemplateChild("PART_TagList") as ListViewBase;
            tagList.Style = styleSelector.SelectStyle(tagList, this);
            tagList.ItemsSource = tagListSource;
            tagList.ItemTemplateSelector = dataTemplateSelector;
            tagList.ItemContainerStyleSelector = styleSelector;

            displayView = GetTemplateChild("PART_PhotoView") as ListViewBase;
            displayView.Style = styleSelector.SelectStyle(displayView, this);
            displayView.ItemsSource = displayItemSource;
            displayView.ItemTemplateSelector = dataTemplateSelector;
            displayView.ItemContainerStyleSelector = styleSelector;

            base.OnApplyTemplate();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var result = base.MeasureOverride(availableSize);
            return result;
        }

        public void SetFirstTag()
        {
            var photoStore = this.PhotoStore;

            tagListSource.Clear();
            tagListSource.AddRange(photoStore.GetAvailableTags(null));
            var selectedTag = tagListSource.First();
            displayItemSource.Clear();
            displayItemSource.AddRange(photoStore.GetItems(selectedTag));

            if (!tagListSource.Any())
            {
                throw new IndexOutOfRangeException("No Tags");
            }


            tagList.ItemsSource = tagListSource;
            displayView.ItemsSource = displayItemSource;
            tagList.SelectedItem = selectedTag;
        }

        private List<string> tagListSource = new List<string>();
        private List<PhotoItem> displayItemSource = new List<PhotoItem>();
        private ListViewBase tagList;
        private ListViewBase displayView;
    }
}
