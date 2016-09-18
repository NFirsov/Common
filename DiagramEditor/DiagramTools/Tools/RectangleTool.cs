using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;

namespace DrawToolsLib
{
	/// <summary>
	/// Rectangle tool
	/// </summary>
	class RectangleTool : RectangleBaseTool
	{
		public RectangleTool(MainViewModel vm) : base(vm) { }

		/// <summary>
		/// Create new rectangle
		/// </summary>
		public override void OnMouseDown(RectBaseViewModel diagramViewModel, MouseButtonEventArgs e, Point p)
		{
			try
			{
				parentViewModel.SuppressDiagramChanged();

				var obj = new RectangleViewModel(parentViewModel) { Top = p.Y, Left = p.X, Height = 100, Width = 100, ObjectColor = parentViewModel.ObjectColor };

				parentViewModel.AddNewDiagram(obj);

			}
			finally
			{
				parentViewModel.AllowDiagramChanged();
			}

			parentViewModel.Tool = ToolType.Pointer;
		}
	}
}
