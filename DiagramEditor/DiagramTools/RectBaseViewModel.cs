using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DrawToolsLib
{
	public class RectBaseViewModel : DependencyObject, IBaseGliph
	{
		public readonly MainViewModel ParentViewModel;

		protected RectBaseViewModel(MainViewModel vm)
		{
			ParentViewModel = vm;
		}

		static RectBaseViewModel()
		{
			var metaData = new PropertyMetadata(1.0, TopChanged);
			TopProperty = DependencyProperty.Register("Top", typeof(double), typeof(RectBaseViewModel), metaData);

			metaData = new PropertyMetadata(1.0, LeftChanged);
			LeftProperty = DependencyProperty.Register("Left", typeof(double), typeof(RectBaseViewModel), metaData);

			metaData = new PropertyMetadata(1.0, HeightChanged);
			HeightProperty = DependencyProperty.Register("Height", typeof(double), typeof(RectBaseViewModel), metaData);

			metaData = new PropertyMetadata(1.0, WidthChanged);
			WidthProperty = DependencyProperty.Register("Width", typeof(double), typeof(RectBaseViewModel), metaData);

			metaData = new PropertyMetadata(1.0);
			LineWidthProperty = DependencyProperty.Register("LineWidth", typeof(double), typeof(RectBaseViewModel), metaData);

			metaData = new PropertyMetadata(Colors.BlueViolet, ColorChanged);
			ObjectColorProperty = DependencyProperty.Register("ObjectColor", typeof(Color), typeof(RectBaseViewModel), metaData);

			metaData = new PropertyMetadata(false, IsSelectedChanged);
			IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(RectBaseViewModel), metaData);
		}

		public static readonly DependencyProperty LineWidthProperty;
		public static readonly DependencyProperty ObjectColorProperty;
		public static readonly DependencyProperty IsSelectedProperty;
		public static readonly DependencyProperty TopProperty;
		public static readonly DependencyProperty LeftProperty;
		public static readonly DependencyProperty HeightProperty;
		public static readonly DependencyProperty WidthProperty;

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

		public int Id { get; set; }

		public byte A
		{
			get { return ObjectColor.A; }
		}

		public byte R
		{
			get { return ObjectColor.R; }
		}

		public byte G
		{
			get { return ObjectColor.G; }
		}

		public byte B
		{
			get { return ObjectColor.B; }
		}

		public Color ObjectColor
		{
			get
			{
				return (Color)GetValue(ObjectColorProperty);
			}
			set
			{
				SetValue(ObjectColorProperty, value);
				OnPropertyChanged();
			}
		}

		private SelectionRectangleViewModel selectionRectangle;

		public bool IsSelected
		{
			get
			{
				return (bool)GetValue(IsSelectedProperty);
			}
			set
			{
				if (this is SelectionRectangleViewModel)
					return;

				SetValue(IsSelectedProperty, value);

				if (value)
				{
					if (selectionRectangle == null)
					{
						selectionRectangle = new SelectionRectangleViewModel(ParentViewModel, this, Top, Left, Width, Height);
						ParentViewModel.Diagrams.Add(selectionRectangle);
					}
				}
				else
				{
					if (selectionRectangle != null)
					{
						ParentViewModel.Diagrams.Remove(selectionRectangle);
						selectionRectangle = null;
					}
				}
			}
		}

		public double Top
		{
			get
			{
				return (double)GetValue(TopProperty);
			}
			set
			{
				SetValue(TopProperty, value);
				//OnPropertyChanged();
			}
		}

		public double Left
		{
			get
			{
				return (double)GetValue(LeftProperty);
			}
			set
			{
				SetValue(LeftProperty, value);
				//OnPropertyChanged();
			}
		}

		public double Height
		{
			get
			{
				return (double)GetValue(HeightProperty);
			}
			set
			{
				SetValue(HeightProperty, value);
				//OnPropertyChanged();
			}
		}

		public double Width
		{
			get
			{
				return (double)GetValue(WidthProperty);
			}
			set
			{
				SetValue(WidthProperty, value);
				//OnPropertyChanged();
			}
		}

		static void ColorChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
		{
			//var d = property as RectBaseViewModel;

			//AuxiliaryFunctions.ApplyLineWidth(d, d.LineWidth);
		}

		static void TopChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
		{
			//var d = property as DiagramViewModel;

			//AuxiliaryFunctions.ApplyLineWidth(d, d.LineWidth);
		}

		static void LeftChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
		{
			//var d = property as DiagramViewModel;

			//AuxiliaryFunctions.ApplyLineWidth(d, d.LineWidth);
		}

		static void HeightChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
		{
			//var d = property as DiagramViewModel;

			//AuxiliaryFunctions.ApplyLineWidth(d, d.LineWidth);
		}

		static void WidthChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
		{
			//var d = property as DiagramViewModel;

			//AuxiliaryFunctions.ApplyLineWidth(d, d.LineWidth);
		}

		static void IsSelectedChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
		{
			var d = property as RectBaseViewModel;

			AuxiliaryFunctions.UpdateSelection(d.ParentViewModel);
		}

		public virtual void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			var pointTool = sender as PointerTool;
			if(pointTool == null)
				return;

			// Unselect all if Ctrl is not pressed
			if (Keyboard.Modifiers != ModifierKeys.Control && !IsSelected)
			{
				AuxiliaryFunctions.UnselectAll(ParentViewModel);
			}

			IsSelected = true;

			pointTool.SelectMode = PointerTool.SelectionMode.Move;
		}

		public void OnPropertyChanged()
		{
			ParentViewModel.OnPropertyChanged(this);
		}

		public void Move(double deltaX, double deltaY)
		{
			Left += deltaX;
			Top += deltaY;
		}

		public void MoveHandleTo(double deltaX, double deltaY, int handleIndex)
		{
			switch (handleIndex)
			{
				case 0:
					Left += deltaX;
					Top += deltaY;
					Width -= deltaX;
					Height -= deltaY;
					break;
				case 1:
					Top += deltaY;
					Height -= deltaY;
					break;
				case 2:
					Top += deltaY;
					Height -= deltaY;
					Width += deltaX;
					break;
				case 3:
					Width += deltaX;
					break;
				case 4:
					Width += deltaX;
					Height += deltaY;
					break;
				case 5:
					Height += deltaY;
					break;
				case 6:
					Left += deltaX;
					Height += deltaY;
					Width -= deltaX;
					break;
				case 7:
					Left += deltaX;
					Width -= deltaX;
					break;
			}
		}

	}
}