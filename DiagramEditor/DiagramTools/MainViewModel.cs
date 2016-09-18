using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace DrawToolsLib
{
	public class MainViewModel : DependencyObject
	{
		public MainViewModel()
		{
			//CreateContextMenu();

			// create array of drawing tools
			_tools = new Tool[(int)ToolType.Max];

			_tools[(int)ToolType.Pointer] = new PointerTool(this);
			_tools[(int)ToolType.Rectangle] = new RectangleTool(this);
			_tools[(int)ToolType.Ellipse] = new EllipseTool(this);
			//tools[(int)ToolType.Line] = new ToolLine();
			//tools[(int)ToolType.PolyLine] = new ToolPolyLine();

			_textTool = new TextTool(this);
			_tools[(int)ToolType.Text] = _textTool;   // kept as class member for in-place editing

			_diagrams = new ObservableCollection<RectBaseViewModel>();

			LineWidth = 1;

			var list = new List<double>();
			for (int i = 0; i <= 12; i++)
			{
				list.Add(i);
			}

			_lineWidthValues = new CollectionView(list);
		}

		static MainViewModel()
		{
			// Create dependency properties

			PropertyMetadata metaData;

			// Tool
			metaData = new PropertyMetadata(ToolType.Pointer);
			ToolProperty = DependencyProperty.Register("Tool", typeof(ToolType), typeof(MainViewModel), metaData);

			// LineWidth
			metaData = new PropertyMetadata(1.0, LineWidthChanged);
			LineWidthProperty = DependencyProperty.Register("LineWidth", typeof(double), typeof(MainViewModel), metaData);

			// ObjectColor
			metaData = new PropertyMetadata(Colors.Black, ObjectColorChanged);
			ObjectColorProperty = DependencyProperty.Register("ObjectColor", typeof(Color), typeof(MainViewModel), metaData);

			// TextFontFamilyName
			metaData = new PropertyMetadata("Tahoma", TextFontFamilyNameChanged);
			TextFontFamilyNameProperty = DependencyProperty.Register("TextFontFamilyName", typeof(string), typeof(MainViewModel), metaData);

			// TextFontStyle
			metaData = new PropertyMetadata(FontStyles.Normal, TextFontStyleChanged);
			TextFontStyleProperty = DependencyProperty.Register("TextFontStyle", typeof(FontStyle), typeof(MainViewModel), metaData);

			// TextFontWeight
			metaData = new PropertyMetadata(FontWeights.Normal, TextFontWeightChanged);
			TextFontWeightProperty = DependencyProperty.Register("TextFontWeight", typeof(FontWeight), typeof(MainViewModel), metaData);

			// TextFontStretch
			metaData = new PropertyMetadata(FontStretches.Normal, TextFontStretchChanged);
			TextFontStretchProperty = DependencyProperty.Register("TextFontStretch", typeof(FontStretch), typeof(MainViewModel), metaData);

			// TextFontSize
			metaData = new PropertyMetadata(12.0, TextFontSizeChanged);
			TextFontSizeProperty = DependencyProperty.Register("TextFontSize", typeof(double), typeof(MainViewModel), metaData);
		}

		// Events
		public event Action<IBaseGliph> DiagramCreated;
		public event Action<IBaseGliph> DiagramChanged;
		public event Action<IBaseGliph> DiagramDeleted;

		// Dependency properties
		public static readonly DependencyProperty ToolProperty;
		public static readonly DependencyProperty LineWidthProperty;
		public static readonly DependencyProperty ObjectColorProperty;

		public static readonly DependencyProperty TextFontFamilyNameProperty;
		public static readonly DependencyProperty TextFontStyleProperty;
		public static readonly DependencyProperty TextFontWeightProperty;
		public static readonly DependencyProperty TextFontStretchProperty;
		public static readonly DependencyProperty TextFontSizeProperty;

		private readonly Tool[] _tools; // Array of tools
		private readonly TextTool _textTool;
		private ContextMenu _contextMenu;

		private readonly ObservableCollection<RectBaseViewModel> _diagrams;

		public ObservableCollection<RectBaseViewModel> Diagrams
		{
			get { return _diagrams; }
		}

		#region Tool

		/// <summary>
		/// Currently active drawing tool
		/// </summary>
		public ToolType Tool
		{
			get
			{
				return (ToolType)GetValue(ToolProperty);
			}
			set
			{
				if ((int)value >= 0 && (int)value < (int)ToolType.Max)
				{
					SetValue(ToolProperty, value);

					AuxiliaryFunctions.UnselectAll(this);
					//_tools[(int)Tool].SetCursor(this);
				}
			}
		}

		#endregion Tool

		#region LineWidth

		/// <summary>
		/// Line width of new graphics object.
		/// Setting this property is also applied to current selection.
		/// </summary>
		public double LineWidth
		{
			get
			{
				return (double)GetValue(LineWidthProperty);
			}
			set
			{
				SetValue(LineWidthProperty, value);
			}
		}

		/// <summary>
		/// Callback function called when LineWidth dependency property is changed
		/// </summary>
		static void LineWidthChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
		{
			//var d = property as DiagramViewModel;

			//AuxiliaryFunctions.ApplyLineWidth(d, d.LineWidth);
		}


		private CollectionView _lineWidthValues;

		public CollectionView LineWidthValues
		{
			get { return _lineWidthValues; }
		}

		#endregion LineWidth

		#region ObjectColor

		/// <summary>
		/// Color of new graphics object.
		/// Setting this property is also applied to current selection.
		/// </summary>
		public Color ObjectColor
		{
			get
			{
				return (Color)GetValue(ObjectColorProperty);
			}
			set
			{
				SetValue(ObjectColorProperty, value);
			}
		}

		/// <summary>
		/// Callback function called when ObjectColor dependency property is changed
		/// </summary>
		static void ObjectColorChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
		{
			var d = property as MainViewModel;

			AuxiliaryFunctions.ApplyColor(d, d.ObjectColor);
		}

		#endregion ObjectColor

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

			}
		}

		/// <summary>
		/// Callback function called when TextFontFamilyName dependency property is changed
		/// </summary>
		static void TextFontFamilyNameChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
		{
			var d = property as MainViewModel;

			AuxiliaryFunctions.ApplyFontFamily(d, d.TextFontFamilyName);
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

			}
		}

		/// <summary>
		/// Callback function called when TextFontStyle dependency property is changed
		/// </summary>
		static void TextFontStyleChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
		{
			var d = property as MainViewModel;

			AuxiliaryFunctions.ApplyFontStyle(d, d.TextFontStyle);
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

			}
		}

		/// <summary>
		/// Callback function called when TextFontWeight dependency property is changed
		/// </summary>
		static void TextFontWeightChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
		{
			var d = property as MainViewModel;

			AuxiliaryFunctions.ApplyFontWeight(d, d.TextFontWeight);
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

			}
		}

		/// <summary>
		/// Callback function called when TextFontStretch dependency property is changed
		/// </summary>
		static void TextFontStretchChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
		{
			var d = property as MainViewModel;

			AuxiliaryFunctions.ApplyFontStretch(d, d.TextFontStretch);
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

			}
		}

		/// <summary>
		/// Callback function called when TextFontSize dependency property is changed
		/// </summary>
		static void TextFontSizeChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
		{
			var d = property as MainViewModel;

			AuxiliaryFunctions.ApplyFontSize(d, d.TextFontSize);
		}

		#endregion TextFontSize

		#region Internal Properties
/*
		/// <summary>
		/// Get graphic object by index
		/// </summary>
		internal RectBaseViewModel this[int index]
		{
			get
			{
				if (index >= 0 && index < Count)
				{
					return _diagrams[index];
				}

				return null;
			}
		}

		/// <summary>
		/// Get last graphic object
		/// </summary>
		/*internal GliphViewModel Last
		{
			get { return _diagrams.L as GliphViewModel; }
		}

		/// <summary>
		/// Get number of graphic objects
		/// </summary>
		internal int Count
		{
			get
			{
				return _diagrams.Count;
			}
		}*/

		/// <summary>
		/// Get number of selected graphic objects
		/// </summary>
		internal int SelectionCount
		{
			get
			{
				return _diagrams.Count(g => g.IsSelected);
			}
		}

		/// <summary>
		/// Return list of graphics
		/// </summary>
		/*internal VisualCollection GraphicsList
		{
			get
			{
				return _graphicsList;
			}
		}*/

		/// <summary>
		/// Returns INumerable which may be used for enumeration
		/// of selected objects.
		/// </summary>
		internal IEnumerable<RectBaseViewModel> Selection
		{
			get { return _diagrams.Where(o => o.IsSelected);}
		}

		/// <summary>
		/// Reverse GraphicsList enumerator
		/// </summary>
		internal IEnumerable<RectBaseViewModel> Reverse
		{
			get
			{
				if (_diagrams.Count > 0)
				{
					for (int i = _diagrams.Count - 1; i >= 0; i--)
					{
						yield return _diagrams[i];
					}
				}
			}
		}

		#endregion Internal Properties

		#region Mouse Event Handlers

		/// <summary>
		/// Mouse down.
		/// Left button down event is passed to active tool.
		/// Right button down event is handled in this class.
		/// </summary>
		public void OnMouseDown(object sender, MouseButtonEventArgs e, Point p)
		{
			if (_tools[(int)Tool] == null)
			{
				return;
			}

			//Focus();

			if (e.ChangedButton == MouseButton.Left)
			{
				if (e.ClickCount == 2)
				{
					// TODO: Double click to edit text
					//HandleDoubleClick(e);        // special case for Text
				}
				else
				{
					var vm = sender as RectBaseViewModel;
					_tools[(int)Tool].OnMouseDown(vm, e, p);
				}

				UpdateState();
			}
			else if (e.ChangedButton == MouseButton.Right)
			{
				//ShowContextMenu(e);
			}
		}

		/// <summary>
		/// Mouse move.
		/// Moving without button pressed or with left button pressed
		/// is passed to active tool.
		/// </summary>
		public void OnMouseMove(object sender, MouseEventArgs e, Point p)
		{
			if (_tools[(int)Tool] == null)
			{
				return;
			}

			if (e.MiddleButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
			{
				_tools[(int)Tool].OnMouseMove(null, e, p);

				UpdateState();
			}
		}

		/// <summary>
		/// Mouse up event.
		/// Left button up event is passed to active tool.
		/// </summary>
		public void OnMouseUp(object sender, MouseButtonEventArgs e)
		{
			if (_tools[(int)Tool] == null)
			{
				return;
			}

			if (e.ChangedButton == MouseButton.Left)
			{
				_tools[(int)Tool].OnMouseUp(null, e);

				UpdateState();
			}
		}

		#endregion Mouse Event Handlers

		#region Other Functions

		internal void OnPropertyChanged(IBaseGliph bg)
		{
			if(_suppressDiagramChanged)
				return;

			if (DiagramChanged != null)
				DiagramChanged(bg);
		}

		private bool _suppressDiagramChanged;

		public void SuppressDiagramChanged()
		{
			_suppressDiagramChanged = true;
		}

		public void AllowDiagramChanged()
		{
			_suppressDiagramChanged = false;
		} 

		public void OnModelGliphsLoaded(IEnumerable<IBaseGliph> list)
		{
			try
			{
				_suppressDiagramChanged = true;

				foreach (var gliph in list)
				{
					_diagrams.Add(CreateViewModel(gliph));
				}
			}
			finally
			{
				_suppressDiagramChanged = false;
			}
		}

		public void AddNewDiagram(RectBaseViewModel vm)
		{
			Diagrams.Add(vm);
			vm.Id = AuxiliaryFunctions.GetNewId(this);

			if (DiagramCreated != null)
				DiagramCreated(vm);
		}

		private RectBaseViewModel CreateViewModel(IBaseGliph bg)
		{
			RectBaseViewModel vm;

			var gl = bg as IGliph;
			if (gl != null)
			{
				switch (gl.GliphType)
				{
					case GliphType.Rectangle:
						vm = new RectangleViewModel(this);
						break;
					case GliphType.Ellipse:
						vm = new EllipseViewModel(this);
						break;
					default:
						vm = new RectangleViewModel(this);
						break;
				}

				SetRectBaseProperties(vm, gl);
				return vm;
			}

			var text = bg as ITextGliph;
			if (text != null)
			{
				vm = new TextViewModel(this)
				{
					Text = text.Text,
					TextFontFamilyName = text.TextFontFamilyName,
					TextFontStyle = text.TextFontStyle,
					TextFontWeight = text.TextFontWeight,
					TextFontStretch = text.TextFontStretch,
					TextFontSize = text.TextFontSize,
				};

				SetRectBaseProperties(vm, text);
				return vm;
			}

			/*var image = bg as IImageGliph;
			if (image != null)
			{
				gliph = new ImageGliph()
				{
					Path = image.Path
				};

				SetRectBaseProperties(vm, gl);
				return;
			}*/
			return null;
		}

		private static void SetRectBaseProperties(RectBaseViewModel vm, IRectangleBaseGliph igliph)
		{
			vm.Id = igliph.Id;
			vm.LineWidth = igliph.LineWidth;
			vm.ObjectColor = Color.FromArgb(igliph.A, igliph.R, igliph.G, igliph.B);

			vm.Top = igliph.Top;
			vm.Left = igliph.Left;
			vm.Height = igliph.Height;
			vm.Width = igliph.Width;
		}


		/// <summary>
		/// Update state of Can* dependency properties
		/// used for Edit commands.
		/// This function calls after any change in drawing canvas state,
		/// caused by user commands.
		/// Helps to keep client controls state up-to-date, in the case
		/// if Can* properties are used for binding.
		/// </summary>
		void UpdateState()
		{
			/*bool hasObjects = (Count > 0);
			bool hasSelectedObjects = (SelectionCount > 0);

			CanSelectAll = hasObjects;
			CanUnselectAll = hasObjects;
			CanDelete = hasSelectedObjects;
			CanDeleteAll = hasObjects;
			CanMoveToFront = hasSelectedObjects;
			CanMoveToBack = hasSelectedObjects;

			CanSetProperties = AuxiliaryFunctions.CanApplyProperties(this);*/
		}

		#endregion Other Functions
	}
}
