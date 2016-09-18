using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;

namespace DrawToolsLib
{
	/// <summary>
	/// Helper class which contains general helper functions and properties.
	/// </summary>
	static class AuxiliaryFunctions
	{
		/// <summary>
		/// Default cursor
		/// </summary>
		public static Cursor DefaultCursor
		{
			get
			{
				return Cursors.Arrow;
			}
		}

		public static int GetNewId(MainViewModel diagramViewModel)
		{
			var h = new HashSet<int>();

			foreach (var vm in diagramViewModel.Diagrams.Where(vm => !h.Contains(vm.Id)))
				h.Add(vm.Id);

			for (int i = 0; i < int.MaxValue; i++)
			{
				if (!h.Contains(i))
					return i;
			}

			return 0;
		}

		/// <summary>
		/// Select all graphic objects
		/// </summary>
		public static void SelectAll(MainViewModel diagramViewModel)
		{
			var list = diagramViewModel.Diagrams.Where(d => !(d is SelectionRectangleViewModel)).ToList();
			list.ForEach(d => d.IsSelected = true);

			UpdateSelection(diagramViewModel);
		}

		/// <summary>
		/// Unselect all graphic objects
		/// </summary>
		public static void UnselectAll(MainViewModel diagramViewModel, RectBaseViewModel except = null)
		{
			var list = diagramViewModel.Diagrams.Where(d => !(d == except) && !(d is SelectionRectangleViewModel)).ToList();
			list.ForEach(d => d.IsSelected = false);
		}

		/// <summary>
		/// Update dependency properties of view model
		/// </summary>
		public static void UpdateSelection(MainViewModel diagramViewModel)
		{
			var selection = diagramViewModel.Selection.ToList();

			var colors = selection.Select(g => g.ObjectColor).Distinct().ToList();
			if (colors.Count == 1)
			{
				diagramViewModel.ObjectColor = colors[0];
			}

			var widths = selection.Where(g => (g is RectBaseViewModel) && !(g is TextViewModel)).Select(g => g.LineWidth).Distinct().ToList();
			if (widths.Count == 1)
			{
				diagramViewModel.LineWidth = widths[0];
			}

			var fonts = selection.OfType<TextViewModel>().Select(g => new
			{
				TextFontFamilyName = g.TextFontFamilyName,
				TextFontSize = g.TextFontSize,
				TextFontStretch = g.TextFontStretch,
				TextFontStyle = g.TextFontStyle,
				TextFontWeight = g.TextFontWeight
			}
			).Distinct().ToList();
			if (fonts.Count == 1)
			{
				diagramViewModel.TextFontFamilyName = fonts[0].TextFontFamilyName;
				diagramViewModel.TextFontSize = fonts[0].TextFontSize;
				diagramViewModel.TextFontStretch = fonts[0].TextFontStretch;
				diagramViewModel.TextFontStyle = fonts[0].TextFontStyle;
				diagramViewModel.TextFontWeight = fonts[0].TextFontWeight;
			}
		}

		/// <summary>
		/// Delete selected graphic objects
		/// </summary>
		public static void DeleteSelection(MainViewModel diagramViewModel)
		{
			foreach (var g in diagramViewModel.Reverse.Where(g => g.IsSelected))
			{
				diagramViewModel.Diagrams.Remove(g);
			}
		}

		/// <summary>
		/// Delete all graphic objects
		/// </summary>
		public static void DeleteAll(MainViewModel diagramViewModel)
		{
			if (diagramViewModel.Diagrams.Count > 0 )
			{
				diagramViewModel.Diagrams.Clear();
			}

		}

		/// <summary>
		/// Move selection to front
		/// </summary>
		public static void MoveSelectionToFront(MainViewModel diagramViewModel)
		{
			// Moving to front of z-order means moving
			// to the end of VisualCollection.

			// Read GraphicsList in the reverse order, and move every selected object
			// to temporary list.

			var list = new List<RectBaseViewModel>();

			foreach (var g in diagramViewModel.Reverse.Where(g => g.IsSelected))
			{
				list.Insert(0, g);
				diagramViewModel.Diagrams.Remove(g);
			}

			// Add all items from temporary list to the end of GraphicsList
			foreach(var g in list)
			{
				diagramViewModel.Diagrams.Add(g);
			}
		}

		/// <summary>
		/// Move selection to back
		/// </summary>
		public static void MoveSelectionToBack(MainViewModel diagramViewModel)
		{
			// Moving to back of z-order means moving
			// to the beginning of VisualCollection.

			// Read GraphicsList in the reverse order, and move every selected object
			// to temporary list.

			var list = new List<RectBaseViewModel>();

			foreach (var g in diagramViewModel.Reverse.Where(g => g.IsSelected))
			{
				list.Add(g);
				diagramViewModel.Diagrams.Remove(g);
			}

			// Add all items from temporary list to the beginning of GraphicsList
			foreach (var g in list)
			{
				diagramViewModel.Diagrams.Insert(0, g);
			}
		}

		/// <summary>
		/// Apply new line width
		/// </summary>
		public static bool ApplyLineWidth(MainViewModel diagramViewModel, double value)
		{
			var wasChange = false;

			foreach (var g in diagramViewModel.Selection.Where(g => g is RectangleViewModel || g is EllipseViewModel).Where(g => g.LineWidth != value))
			{
				g.LineWidth = value;
				wasChange = true;
			}

			return wasChange;
		}

		/// <summary>
		/// Apply new color
		/// </summary>
		public static bool ApplyColor(MainViewModel diagramViewModel, Color value)
		{
			var wasChange = false;

			foreach (var g in diagramViewModel.Selection.Where(g => g.ObjectColor != value))
			{
				g.ObjectColor = value;
				wasChange = true;
			}

			return wasChange;
		}

		/// <summary>
		/// Apply new font family
		/// </summary>
		public static bool ApplyFontFamily(MainViewModel diagramViewModel, string value)
		{
			var wasChange = false;

			foreach (var gt in diagramViewModel.Selection.OfType<TextViewModel>().Where(gt => gt.TextFontFamilyName != value))
			{
				gt.TextFontFamilyName = value;
				wasChange = true;
			}

			return wasChange;
		}

		/// <summary>
		/// Apply new font style
		/// </summary>
		public static bool ApplyFontStyle(MainViewModel diagramViewModel, FontStyle value)
		{
			var wasChange = false;

			foreach (var gt in diagramViewModel.Selection.OfType<TextViewModel>().Where(gt => gt.TextFontStyle != value))
			{
				gt.TextFontStyle = value;
				wasChange = true;
			}

			return wasChange;
		}

		/// <summary>
		/// Apply new font weight
		/// </summary>
		public static bool ApplyFontWeight(MainViewModel diagramViewModel, FontWeight value)
		{
			var wasChange = false;

			foreach (var gt in diagramViewModel.Selection.OfType<TextViewModel>().Where(gt => gt.TextFontWeight != value))
			{
				gt.TextFontWeight = value;
				wasChange = true;
			}

			return wasChange;
		}

		/// <summary>
		/// Apply new font stretch
		/// </summary>
		public static bool ApplyFontStretch(MainViewModel diagramViewModel, FontStretch value)
		{
			var wasChange = false;

			foreach (var gt in diagramViewModel.Selection.OfType<TextViewModel>().Where(gt => gt.TextFontStretch != value))
			{
				gt.TextFontStretch = value;
				wasChange = true;
			}

			return wasChange;
		}

		/// <summary>
		/// Apply new font size
		/// </summary>
		public static bool ApplyFontSize(MainViewModel diagramViewModel, double value)
		{
			var wasChange = false;

			foreach (var gt in diagramViewModel.Selection.OfType<TextViewModel>().Where(gt => gt.TextFontSize != value))
			{
				gt.TextFontSize = value;
				wasChange = true;
			}

			return wasChange;
		}

		/// <summary>
		/// Return true if currently active properties (line width, color etc.)
		/// can be applied to selected items.
		/// 
		/// If at least one selected object has property different from currently
		/// active property value, properties can be applied.
		/// </summary>
		public static bool CanApplyProperties(MainViewModel diagramViewModel)
		{
			foreach (var g in diagramViewModel.Selection)
			{
				// ObjectColor - used in all graphics objects
				if ( g.ObjectColor != diagramViewModel.ObjectColor )
				{
					return true;
				}

				var text = g as TextViewModel;
				if ( text == null )
				{
					// LineWidth - used in all objects except of Text
					if ( g.LineWidth != diagramViewModel.LineWidth )
					{
						return true;
					}
				}
				else
				{
					// Font - for Text
					if (text.TextFontFamilyName != diagramViewModel.TextFontFamilyName ||
						text.TextFontSize != diagramViewModel.TextFontSize ||
						text.TextFontStretch != diagramViewModel.TextFontStretch ||
						text.TextFontStyle != diagramViewModel.TextFontStyle ||
						text.TextFontWeight != diagramViewModel.TextFontWeight)
					{
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Apply currently active properties to selected objects
		/// </summary>
		public static bool ApplyProperties(MainViewModel diagramViewModel)
		{
			// Apply every property.
			// Call every Apply* function with addToHistory = false.

			var wasChange = false;

			// Line Width
			if (ApplyLineWidth(diagramViewModel, diagramViewModel.LineWidth))
			{
				wasChange = true;
			}

			// Color
			if (ApplyColor(diagramViewModel, diagramViewModel.ObjectColor))
			{
				wasChange = true;
			}

			// Font properties
			if (ApplyFontFamily(diagramViewModel, diagramViewModel.TextFontFamilyName))
			{
				wasChange = true;
			}

			if (ApplyFontSize(diagramViewModel, diagramViewModel.TextFontSize))
			{
				wasChange = true;
			}

			if (ApplyFontStretch(diagramViewModel, diagramViewModel.TextFontStretch))
			{
				wasChange = true;
			}

			if (ApplyFontStyle(diagramViewModel, diagramViewModel.TextFontStyle))
			{
				wasChange = true;
			}

			if (ApplyFontWeight(diagramViewModel, diagramViewModel.TextFontWeight))
			{
				wasChange = true;
			}

			return wasChange;
		}
	}
}
