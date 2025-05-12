using EMS.MVVM.ViewModel;
using System.Windows.Controls;

namespace EMS.MVVM.View
{
    /// <summary>
    /// Interaction logic for PowerView.xaml
    /// </summary>
    public partial class PowerView : UserControl
    {
        public PowerView()
        {
            InitializeComponent();
            DataContext = PowerVM.Instance;
        }
    }
}
