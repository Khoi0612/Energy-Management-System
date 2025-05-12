using EMS.MVVM.ViewModel;
using System.Windows.Controls;

namespace EMS.MVVM.View
{
    /// <summary>
    /// Interaction logic for VolatgeView.xaml
    /// </summary>
    public partial class VoltageView : UserControl
    {

        public VoltageView()
        {
            InitializeComponent();
            DataContext = VoltageVM.Instance;
        }
    }
}
