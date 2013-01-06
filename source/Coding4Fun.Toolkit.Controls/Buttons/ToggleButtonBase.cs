﻿#if WINDOWS_STORE

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

#elif WINDOWS_PHONE

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#endif

namespace Coding4Fun.Toolkit.Controls
{
	public abstract class ToggleButtonBase : CheckBox, IButtonBase, IImageSourceButton, IAppBarButton
	{
		protected ImageBrush OpacityImageBrush;
		protected ImageBrush DisabledOpacityImageBrush;


#if WINDOWS_STORE
		protected override void OnApplyTemplate()
#elif WINDOWS_PHONE
		public override void OnApplyTemplate()
#endif
		{
			base.OnApplyTemplate();
			
			OpacityImageBrush = GetTemplateChild(ButtonBaseConstants.OpacityImageBrushName) as ImageBrush;
			DisabledOpacityImageBrush = GetTemplateChild(ButtonBaseConstants.DisabledOpacityImageBrushName) as ImageBrush;
			var contentBody = GetTemplateChild(ButtonBaseConstants.ContentBodyName) as ContentControl;

			ButtonBaseHelper.ApplyTemplate(this, OpacityImageBrush, contentBody, Stretch, ImageSourceProperty);
			ButtonBaseHelper.ApplyTemplate(this, DisabledOpacityImageBrush, contentBody, Stretch, ImageSourceProperty);
        }

        #region dependency properties

		public object Title
		{
			get { return GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(object), typeof(ToggleButtonBase), new PropertyMetadata(new object()));

		public Brush CheckedBrush
		{
			get { return (Brush)GetValue(CheckedBrushProperty); }
			set { SetValue(CheckedBrushProperty, value); }
		}

		// Using a DependencyProperty as the backing store for CheckedBrush.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CheckedBrushProperty =
			DependencyProperty.Register("CheckedBrush", typeof(Brush), typeof(ToggleButtonBase), new PropertyMetadata(new SolidColorBrush()));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ToggleButtonBase), new PropertyMetadata(Orientation.Vertical));

		public Stretch Stretch
		{
			get { return (Stretch)GetValue(StretchProperty); }
			set { SetValue(StretchProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Stretch.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StretchProperty =
			DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ToggleButtonBase), new PropertyMetadata(Stretch.Fill, OnStretch));

		public ImageSource ImageSource
        {
			get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

		// Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
			DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ToggleButtonBase),
            new PropertyMetadata(null, OnImageSource));

		public double ButtonWidth
		{
			get { return (double)GetValue(ButtonWidthProperty); }
			set { SetValue(ButtonWidthProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ButtonWidth.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ButtonWidthProperty =
			DependencyProperty.Register("ButtonWidth", typeof(double), typeof(ToggleButtonBase), new PropertyMetadata(72d));

		public double ButtonHeight
		{
			get { return (double)GetValue(ButtonHeightProperty); }
			set { SetValue(ButtonHeightProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ButtonHeight.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ButtonHeightProperty =
			DependencyProperty.Register("ButtonHeight", typeof(double), typeof(ToggleButtonBase), new PropertyMetadata(72d));
        #endregion

        #region dp onchange callbacks
        private static void OnImageSource(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
			var sender = o as ToggleButtonBase;

			if (sender == null)
				return;

			ButtonBaseHelper.OnImageChange(e, sender.OpacityImageBrush);
        }

		private static void OnStretch(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var sender = o as ToggleButtonBase;

			if (sender == null)
				return;

			ButtonBaseHelper.OnStretch(e, sender.OpacityImageBrush);
		}
        #endregion
	}
}
