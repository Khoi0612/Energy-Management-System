using EMS.MVVM.ViewModel;
using System;
using System.Windows.Controls;

namespace EMS.MVVM.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {

        public MainView()
        {
            InitializeComponent();
            DataContext = MainVM.Instance;
        }

        private Boolean AutoScroll = true;

        private void scrollviewer_Messages_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer sv = sender as ScrollViewer;
            if (sv.ScrollableHeight - sv.VerticalOffset < 20)
            {
                sv.ScrollToEnd();
            }
        }
    }
}
