using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using System.Collections.Generic;
using System.Linq;

namespace DrawToolsLib
{
	/// <summary>
	/// Pointer tool
	/// </summary>
	class PointerTool : Tool
	{
		public PointerTool(MainViewModel vm) : base(vm)
		{
		}

		public enum SelectionMode
		{
			None,
			Move,           // object(s) are moved
			Size,           // object is resized
			GroupSelection
		}

		public SelectionMode SelectMode { get; set; }
		public RectBaseViewModel ResizingObject { get; set; }
		public int SelectionRectIndex { get; set; }
		private Point lastPoint = new Point(0, 0);

		/// <summary>
		/// Handle mouse down.
		/// Start moving, resizing or group selection.
		/// </summary>
		public override void OnMouseDown(RectBaseViewModel diagramViewModel, MouseButtonEventArgs e, Point point)
		{
			SelectMode = SelectionMode.None;
			lastPoint = point;

			// Mouse down on object
			if (diagramViewModel != null)
			{
				diagramViewModel.OnMouseDown(this, e);
				return;
			}

			// On clear area
			if (Keyboard.Modifiers != ModifierKeys.Control)
			{
				AuxiliaryFunctions.UnselectAll(parentViewModel);
			}

			// Capture mouse until MouseUp event is received
			//diagramViewModel.CaptureMouse();
		}

		/// <summary>
		/// Handle mouse move.
		/// Set cursor, move/resize, make group selection.
		/// </summary>
		public override void OnMouseMove(RectBaseViewModel diagramViewModel, MouseEventArgs e, Point point)
		{
			// Exclude all cases except left button on/off.
			if ( e.MiddleButton == MouseButtonState.Pressed ||
					e.RightButton == MouseButtonState.Pressed )
			{
				return;
			}

			// Find difference between previous and current position
			double dx = point.X - lastPoint.X;
			double dy = point.Y - lastPoint.Y;

			lastPoint = point;

			// Resize
			if ( SelectMode == SelectionMode.Size )
			{
				ResizingObject.MoveHandleTo(dx, dy, SelectionRectIndex);
			}

			// Move
			if ( SelectMode == SelectionMode.Move )
			{
				foreach(var o in parentViewModel.Selection)
				{
					o.Move(dx, dy);
				}
			}

			// Group selection
			if ( SelectMode == SelectionMode.GroupSelection )
			{
				//TODO: Put resize selection rectangle logic here
			}
		}

		/// <summary>
		/// Handle mouse up.
		/// Return to normal state.
		/// </summary>
		public override void OnMouseUp(RectBaseViewModel diagramViewModel, MouseButtonEventArgs e)
		{
			if (SelectMode == SelectionMode.Move || SelectMode == SelectionMode.Size)
			{
				// Move or Resize operations performed, update model
				foreach (var o in parentViewModel.Selection)
					o.OnPropertyChanged();
			}

			SelectMode = SelectionMode.None;

			/*if ( ! diagramViewModel.IsMouseCaptured )
			{
				diagramViewModel.Cursor = AuxiliaryFunctions.DefaultCursor;
				selectMode = SelectionMode.None;
				return;
			}*/
		}


		/// <summary>
		/// Set cursor
		/// </summary>
		public override void SetCursor(RectBaseViewModel diagramViewModel)
		{
			//diagramViewModel.Cursor = AuxiliaryFunctions.DefaultCursor;
		}
	}
}
