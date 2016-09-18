using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using DrawToolsLib;
using System.Windows.Interactivity;

namespace DiagramEditor
{
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel = new MainViewModel();
        private Model _model = new Model();

        public MainViewModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; }
        }

        public MainWindow()
        {
            InitializeComponent();

            SubscribeToEvents();
        }
        
        void SubscribeToEvents()
        {
            // Tools buttons
            buttonToolPointer.PreviewMouseDown += ToolButton_PreviewMouseDown;
            buttonToolRectangle.PreviewMouseDown += ToolButton_PreviewMouseDown;
            buttonToolEllipse.PreviewMouseDown += ToolButton_PreviewMouseDown;
            buttonToolText.PreviewMouseDown += ToolButton_PreviewMouseDown;

            _model.GliphsLoaded += _viewModel.OnModelGliphsLoaded;

            _viewModel.DiagramChanged += _model.OnChange;
            _viewModel.DiagramCreated += _model.OnCreate;
            _viewModel.DiagramDeleted += _model.OnDelete;

            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
        }
        
        
        #region Tools Event Handlers
        
        void ToolButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel.Tool = (ToolType)Enum.Parse(typeof(ToolType),
                ((System.Windows.Controls.Primitives.ButtonBase)sender).Tag.ToString());

            e.Handled = true;
        }

        #endregion Tools Event Handlers

        #region Properties Event Handlers

        /// <summary>
        /// Show Font dialog
        /// </summary>
        void PropertiesFont_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Petzold.ChooseFont.FontDialog
            {
                Owner = this,
                Background = SystemColors.ControlBrush,
                Title = "Select Font",
                FaceSize = ViewModel.TextFontSize,
                Typeface = new Typeface(
                    new FontFamily(ViewModel.TextFontFamilyName),
                    ViewModel.TextFontStyle,
                    ViewModel.TextFontWeight,
                    ViewModel.TextFontStretch)
            };

            if (dlg.ShowDialog().GetValueOrDefault() != true)
                return;
            
            ViewModel.TextFontSize = dlg.FaceSize;
            ViewModel.TextFontFamilyName = dlg.Typeface.FontFamily.ToString();
            ViewModel.TextFontStyle = dlg.Typeface.Style;
            ViewModel.TextFontWeight = dlg.Typeface.Weight;
            ViewModel.TextFontStretch = dlg.Typeface.Stretch;
        }

        /// <summary>
        /// Show Color dialog
        /// </summary>
        void PropertiesColor_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new ColorDialog {Owner = this, Color = ViewModel.ObjectColor};

            if (dlg.ShowDialog().GetValueOrDefault() != true)
                return;
            
            ViewModel.ObjectColor = dlg.Color;
        }

        #endregion Properties Event Handlers

        #region Other Event Handlers

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _model.Save();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _model.Load();

            //itemsControl
        }

        private void Border_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var p = e.GetPosition(itemsControl);

            ViewModel.OnMouseDown(sender, e, p);
        }

        private void Border_OnMouseMove(object sender, MouseEventArgs e)
        {
            var p = e.GetPosition(itemsControl);

            ViewModel.OnMouseMove(sender, e, p);
        }

        private void Border_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            ViewModel.OnMouseUp(sender, e);
        }

        private void Presenter_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var presenter = sender as ContentPresenter;
            var p = e.GetPosition(itemsControl);

            var selectRect = presenter.Content as SelectionRectangleViewModel;
            if (selectRect != null)
                selectRect.LastSelectionIndex = lastSelRectIndex;

            ViewModel.OnMouseDown(presenter.Content, e, p);
            
            e.Handled = true;
        }
        
        private int lastSelRectIndex = -1;
        
        private void SelectionRectangle_OnMouseMove(object sender, MouseEventArgs e)
        {
            var rect = sender as Rectangle;
            lastSelRectIndex = int.Parse(rect.Tag as string);
        }
        #endregion

        
    }
}
