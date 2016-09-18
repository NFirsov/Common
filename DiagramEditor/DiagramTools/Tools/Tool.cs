using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace DrawToolsLib
{
	/// <summary>
	/// Base class for all drawing tools
	/// </summary>
	abstract class Tool
	{
		protected readonly MainViewModel parentViewModel;

		protected Tool(MainViewModel vm)
		{
			parentViewModel = vm;
		}

		public abstract void OnMouseDown(RectBaseViewModel diagramViewModel, MouseButtonEventArgs e, Point p);

		public abstract void OnMouseMove(RectBaseViewModel diagramViewModel, MouseEventArgs e, Point p);

		public abstract void OnMouseUp(RectBaseViewModel diagramViewModel, MouseButtonEventArgs e);

		public abstract void SetCursor(RectBaseViewModel diagramViewModel);
	}
}
