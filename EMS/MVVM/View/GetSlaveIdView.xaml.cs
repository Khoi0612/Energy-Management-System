using EMS.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace EMS.MVVM.View
{
    /// <summary>
    /// Interaction logic for SlaveIdTextBox.xaml
    /// </summary>
    public partial class SlaveIdTextBox : UserControl
    {
        private MosbusMasterVM _masterVM;
        public SlaveIdTextBox()
        {

            _masterVM = new MosbusMasterVM();
            InitializeComponent();
            DataContext = _masterVM;
        }

        private void TextBoxLoaded(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            MosbusMasterVM.AttachNumericOnly(textBox);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _masterVM.CheckConnection();
            _masterVM.SlaveIdString = textBox.Text;

        }
    }
}
