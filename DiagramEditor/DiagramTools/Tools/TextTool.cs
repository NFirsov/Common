using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DrawToolsLib
{
	/// <summary>
	/// Text tool
	/// </summary>
	class TextTool : RectangleBaseTool
	{
		public TextTool(MainViewModel vm) : base(vm) { }

		TextBox textBox;
		string oldText;

		//TextViewModel _editedText;

		public TextBox TextBox
		{
			get { return textBox; }
			set { textBox = value; }
		}

		/// <summary>
		/// Create new text object
		/// </summary>
		public override void OnMouseDown(RectBaseViewModel diagramViewModel, MouseButtonEventArgs e, Point p)
		{
			try
			{
				parentViewModel.SuppressDiagramChanged();

				var obj = new TextViewModel(parentViewModel) 
				{ Top = p.Y, Left = p.X, Height = 100, Width = 100, 
					ObjectColor = parentViewModel.ObjectColor,
					TextFontFamilyName = parentViewModel.TextFontFamilyName,
					TextFontSize = parentViewModel.TextFontSize,
					TextFontStyle = parentViewModel.TextFontStyle,
					TextFontStretch = parentViewModel.TextFontStretch,
					TextFontWeight = parentViewModel.TextFontWeight,
					Text = "SomeText"
				};

				parentViewModel.AddNewDiagram(obj);

			}
			finally
			{
				parentViewModel.AllowDiagramChanged();
			}

			parentViewModel.Tool = ToolType.Pointer;
		}

		/// <summary>
		/// Left mouse is released.
		/// New object is created and resized.
		/// </summary>
		public override void OnMouseUp(RectBaseViewModel diagramViewModel, MouseButtonEventArgs e)
		{
			/*diagramViewModel.Tool = ToolType.Pointer;
			diagramViewModel.Cursor = AuxiliaryFunctions.DefaultCursor;
			diagramViewModel.ReleaseMouseCapture();

			if (diagramViewModel.Count > 0)
			{
				diagramViewModel.Last.Normalize();

				var t = diagramViewModel.Last as TextViewModel;
				if ( t != null )
				{
					// Create textbox for editing of graphics object which is just created
					CreateTextBox(t, diagramViewModel);
				}
			}*/
		}

		/// <summary>
		/// Create textbox for in-place editing
		/// </summary>
		/*public void CreateTextBox(TextViewModel text, DiagramViewModel diagramViewModel)
		{
			text.IsSelected = false;

			oldText = text.Text;
			_editedText = text;

			textBox = new TextBox
			{
				Width = text.Rectangle.Width,
				Height = text.Rectangle.Height,
				FontFamily = new FontFamily(text.TextFontFamilyName),
				FontSize = text.TextFontSize,
				FontStretch = text.TextFontStretch,
				FontStyle = text.TextFontStyle,
				FontWeight = text.TextFontWeight,
				Text = text.Text,
				AcceptsReturn = true,
				TextWrapping = TextWrapping.Wrap
			};

			diagramViewModel.Children.Add(textBox);

			Canvas.SetLeft(textBox, text.Rectangle.Left);
			Canvas.SetTop(textBox, text.Rectangle.Top);
			textBox.Width = textBox.Width;
			textBox.Height = textBox.Height;

			textBox.Focus();

			textBox.LostFocus += textBox_LostFocus;
			textBox.LostKeyboardFocus += textBox_LostKeyboardFocus;
			textBox.PreviewKeyDown += textBox_PreviewKeyDown;
			textBox.ContextMenu = null;

			textBox.Loaded += textBox_Loaded;
		}*/

		/*
		/// <summary>
		/// Correct textbox position. (text should not jump)
		/// </summary>
		void textBox_Loaded(object sender, RoutedEventArgs e)
		{
			double xOffset, yOffset;

			ComputeTextOffset(textBox, out xOffset, out yOffset);

			Canvas.SetLeft(textBox, Canvas.GetLeft(textBox) - xOffset);
			Canvas.SetTop(textBox, Canvas.GetTop(textBox) - yOffset);
			textBox.Width = textBox.Width + xOffset + xOffset;
			textBox.Height = textBox.Height + yOffset + yOffset;
		}*/

		/// <summary>
		/// Compute distance between textbox top-left point and actual
		/// text top-left point inside of textbox.
		/// (code in MSDN WPF Forum)
		/// </summary>
		static void ComputeTextOffset(TextBox tb, out double xOffset, out double yOffset)
		{
			xOffset = 5;
			yOffset = 3;

			try
			{
				var cc = (ContentControl)tb.Template.FindName("PART_ContentHost", tb);

				var tf = ((Visual)cc.Content).TransformToAncestor(tb);
				var offset = tf.Transform(new Point(0, 0));

				xOffset = offset.X;
				yOffset = offset.Y;
			}
			catch(ArgumentException e)
			{
				System.Diagnostics.Trace.WriteLine("ComputeTextOffset: " + e.Message);
			}
			catch (InvalidOperationException e)
			{
				System.Diagnostics.Trace.WriteLine("ComputeTextOffset: " + e.Message);
			}
		}


		// Hide textbox when Enter or Esc is changed
		void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				textBox.Text = oldText;

				//diagramViewModel.HideTextbox(_editedText);

				e.Handled = true;
				return;
			}

			// Enter without modifiers - Shift+Enter should be available.
			if (e.Key == Key.Return && Keyboard.Modifiers == ModifierKeys.None)
			{
				//diagramViewModel.HideTextbox(_editedText);

				e.Handled = true;
			}
		}

		/// <summary>
		/// Hide textbox when it looses focus.
		/// </summary>
		void textBox_LostFocus(object sender, RoutedEventArgs e)
		{
			//diagramViewModel.HideTextbox(_editedText);
		}

		/// <summary>
		/// Hide textbox when it looses keyboard focus.
		/// </summary>
		void textBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			//diagramViewModel.HideTextbox(_editedText);
		}

		/// <summary>
		/// Textbox text value before in-place editing.
		/// </summary>
		public string OldText
		{
			get { return oldText; }
		}

	}
}
