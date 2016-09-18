using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace DrawToolsLib
{
	public class SelectionRectangleViewModel : RectangleViewModel
	{
		private readonly RectBaseViewModel parentDiagramViewModel;

		public SelectionRectangleViewModel(MainViewModel vm): base(vm)
		{
		}

		public SelectionRectangleViewModel(MainViewModel vm, RectBaseViewModel parentDiagramVM,double top, double left, double width, double height)
			: base(vm)
		{
			BindingOperations.SetBinding(this, TopProperty, new Binding() {Source = parentDiagramVM, Path = new PropertyPath("Top"), Mode = BindingMode.TwoWay});
			BindingOperations.SetBinding(this, LeftProperty, new Binding() { Source = parentDiagramVM, Path = new PropertyPath("Left"), Mode = BindingMode.TwoWay });
			BindingOperations.SetBinding(this, WidthProperty, new Binding() { Source = parentDiagramVM, Path = new PropertyPath("Width"), Mode = BindingMode.TwoWay });
			BindingOperations.SetBinding(this, HeightProperty, new Binding() { Source = parentDiagramVM, Path = new PropertyPath("Height"), Mode = BindingMode.TwoWay });

			parentDiagramViewModel = parentDiagramVM;
		}

		public int LastSelectionIndex { get; set; }

		public override void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			var pointTool = sender as PointerTool;
			if (pointTool == null)
				return;

			AuxiliaryFunctions.UnselectAll(ParentViewModel, parentDiagramViewModel);

			pointTool.SelectMode = PointerTool.SelectionMode.Size;
			pointTool.ResizingObject = parentDiagramViewModel;
			pointTool.SelectionRectIndex = LastSelectionIndex;
		}
	}
}