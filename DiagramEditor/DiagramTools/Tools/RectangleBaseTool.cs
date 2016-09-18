using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;

namespace DrawToolsLib
{
	/// <summary>
	/// Base class for rectangle-based tools
	/// </summary>
	abstract class RectangleBaseTool : ObjectTool
	{
		protected RectangleBaseTool(MainViewModel vm) : base(vm) {}

		/// <summary>
		/// Set cursor and resize new object.
		/// </summary>
		public override void OnMouseMove(RectBaseViewModel diagramViewModel, MouseEventArgs e, Point p)
		{
			/*diagramViewModel.Cursor = ToolCursor;

			if (e.LeftButton == MouseButtonState.Pressed)
			{
				if (diagramViewModel.IsMouseCaptured)
				{
					if ( diagramViewModel.Count > 0 )
					{
						diagramViewModel.Last.MoveHandleTo(e.GetPosition(diagramViewModel), 5);
					}
				}
			}*/
		}

	}
}
