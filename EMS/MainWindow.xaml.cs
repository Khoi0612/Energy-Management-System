using System.Windows;
using System.Windows.Input;

namespace EMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool isFullscreen = false;
        private Rect previousWindowRect;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void CloseAppClick(object sender, RoutedEventArgs e)
        {
            Close();

        }

        private void MaximiseAppClick(object sender, RoutedEventArgs e)
        {
            if (!isFullscreen)
            {
                // Save the current window position and size
                previousWindowRect = new Rect(Left, Top, Width, Height);

                // Set fullscreen
                WindowStyle = WindowStyle.None;
                ResizeMode = ResizeMode.NoResize;

                Left = 0;
                Top = 0;
                Width = SystemParameters.PrimaryScreenWidth;
                Height = SystemParameters.PrimaryScreenHeight;

                isFullscreen = true;
            }
            else
            {
                // Restore the previous window position and size
                Left = previousWindowRect.Left;
                Top = previousWindowRect.Top;
                Width = previousWindowRect.Width;
                Height = previousWindowRect.Height;

                WindowStyle = WindowStyle.None;
                ResizeMode = ResizeMode.CanResize;

                isFullscreen = false;
            }
        }

        private void MinimizeAppClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;

        }
    }
}
