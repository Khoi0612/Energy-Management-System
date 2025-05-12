using EMS.MVVM.ViewModel;
using System.Windows.Controls;

namespace EMS.MVVM.View
{
    /// <summary>
    /// Interaction logic for CurrentView.xaml
    /// </summary>
    public partial class CurrentView : UserControl
    {
        public CurrentView()
        {
            InitializeComponent();
            DataContext = CurrentVM.Instance;
        }

    }
}
