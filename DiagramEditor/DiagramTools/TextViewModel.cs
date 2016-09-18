using System.Windows;

namespace DrawToolsLib
{
	public class TextViewModel : RectBaseViewModel, ITextGliph
	{
		public TextViewModel(MainViewModel vm)
			: base(vm)
		{
		}

		static TextViewModel()
		{
			var metaData = new PropertyMetadata("Text");
			TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextViewModel), metaData);

			// TextFontFamilyName
			metaData = new PropertyMetadata("Tahoma", TextFontFamilyNameChanged);
			TextFontFamilyNameProperty = DependencyProperty.Register("TextFontFamilyName", typeof(string), typeof(TextViewModel), metaData);

			// TextFontStyle
			metaData = new PropertyMetadata(FontStyles.Normal, TextFontStyleChanged);
			TextFontStyleProperty = DependencyProperty.Register("TextFontStyle", typeof(FontStyle), typeof(TextViewModel), metaData);

			// TextFontWeight
			metaData = new PropertyMetadata(FontWeights.Normal, TextFontWeightChanged);
			TextFontWeightProperty = DependencyProperty.Register("TextFontWeight", typeof(FontWeight), typeof(TextViewModel), metaData);

			// TextFontStretch
			metaData = new PropertyMetadata(FontStretches.Normal, TextFontStretchChanged);
			TextFontStretchProperty = DependencyProperty.Register("TextFontStretch", typeof(FontStretch), typeof(TextViewModel), metaData);

			// TextFontSize
			metaData = new PropertyMetadata(12.0, TextFontSizeChanged);
			TextFontSizeProperty = DependencyProperty.Register("TextFontSize", typeof(double), typeof(TextViewModel), metaData);

		}

		public static readonly DependencyProperty TextProperty;

		public static readonly DependencyProperty TextFontFamilyNameProperty;
		public static readonly DependencyProperty TextFontStyleProperty;
		public static readonly DependencyProperty TextFontWeightProperty;
		public static readonly DependencyProperty TextFontStretchProperty;
		public static readonly DependencyProperty TextFontSizeProperty;

		public string Text
		{
			get
			{
				return (string)GetValue(TextProperty);
			}
			set
			{
				SetValue(TextProperty, value);
				OnPropertyChanged();
			}
		}

		#region TextFontFamilyName

		/// <summary>
		/// Font Family name of new graphics object.
		/// Setting this property is also applied to current selection.
		/// </summary>
		public string TextFontFamilyName
		{
			get
			{
				return (string)GetValue(TextFontFamilyNameProperty);
			}
			set
			{
				SetValue(TextFontFamilyNameProperty, value);
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Callback function called when TextFontFamilyName dependency property is changed
		/// </summary>
		static void TextFontFamilyNameChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
		{
			//var d = property as MainViewModel;

			//AuxiliaryFunctions.ApplyFontFamily(d, d.TextFontFamilyName);
		}

		#endregion TextFontFamilyName

		#region TextFontStyle

		/// <summary>
		/// Font style of new graphics object.
		/// Setting this property is also applied to current selection.
		/// </summary>
		public FontStyle TextFontStyle
		{
			get
			{
				return (FontStyle)GetValue(TextFontStyleProperty);
			}
			set
			{
				SetValue(TextFontStyleProperty, value);
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Callback function called when TextFontStyle dependency property is changed
		/// </summary>
		static void TextFontStyleChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
		{
			//var d = property as DiagramViewModel;

			//AuxiliaryFunctions.ApplyFontStyle(d, d.TextFontStyle);
		}

		#endregion TextFontStyle

		#region TextFontWeight

		/// <summary>
		/// Font weight of new graphics object.
		/// Setting this property is also applied to current selection.
		/// </summary>
		public FontWeight TextFontWeight
		{
			get
			{
				return (FontWeight)GetValue(TextFontWeightProperty);
			}
			set
			{
				SetValue(TextFontWeightProperty, value);
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Callback function called when TextFontWeight dependency property is changed
		/// </summary>
		static void TextFontWeightChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
		{
			//var d = property as DiagramViewModel;

			//AuxiliaryFunctions.ApplyFontWeight(d, d.TextFontWeight);
		}

		#endregion TextFontWeight

		#region TextFontStretch

		/// <summary>
		/// Font stretch of new graphics object.
		/// Setting this property is also applied to current selection.
		/// </summary>
		public FontStretch TextFontStretch
		{
			get
			{
				return (FontStretch)GetValue(TextFontStretchProperty);
			}
			set
			{
				SetValue(TextFontStretchProperty, value);
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Callback function called when TextFontStretch dependency property is changed
		/// </summary>
		static void TextFontStretchChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
		{
			//var d = property as DiagramViewModel;

			//AuxiliaryFunctions.ApplyFontStretch(d, d.TextFontStretch);
		}

		#endregion TextFontStretch

		#region TextFontSize

		/// <summary>
		/// Font size of new graphics object.
		/// Setting this property is also applied to current selection.
		/// </summary>
		public double TextFontSize
		{
			get
			{
				return (double)GetValue(TextFontSizeProperty);
			}
			set
			{
				SetValue(TextFontSizeProperty, value);
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Callback function called when TextFontSize dependency property is changed
		/// </summary>
		static void TextFontSizeChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
		{
			//var d = property as DiagramViewModel;

			//AuxiliaryFunctions.ApplyFontSize(d, d.TextFontSize);
		}

		#endregion TextFontSize

	}
}