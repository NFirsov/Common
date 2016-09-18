using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DrawToolsLib
{
	/// <summary>
	/// Base class for all tools which create new graphic object
	/// </summary>
	abstract class ObjectTool : Tool
	{
		protected ObjectTool(MainViewModel vm) : base(vm)
		{
		}

		private Cursor toolCursor;

		/// <summary>
		/// Tool cursor.
		/// </summary>
		protected Cursor ToolCursor
		{
			get
			{
				return toolCursor;
			}
			set
			{
				toolCursor = value;
			}
		}

		/// <summary>
		/// Left mouse is released.
		/// New object is created and resized.
		/// </summary>
		public override void OnMouseUp(RectBaseViewModel diagramViewModel, MouseButtonEventArgs e)
		{
			/*if (diagramViewModel.Count > 0)
			{
				diagramViewModel.Last.Normalize();
			}

			diagramViewModel.Tool = ToolType.Pointer;
			diagramViewModel.Cursor = AuxiliaryFunctions.DefaultCursor;
			diagramViewModel.ReleaseMouseCapture();*/
		}

		/// <summary>
		/// Set cursor
		/// </summary>
		public override void SetCursor(RectBaseViewModel diagramViewModel)
		{
			//diagramViewModel.Cursor = toolCursor;
		}
	}
}
