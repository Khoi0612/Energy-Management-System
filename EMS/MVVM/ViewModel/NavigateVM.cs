using EMS.Core;
using System.Windows.Input;

namespace EMS.MVVM.ViewModel
{
    internal class NavigateVM : BaseVM
    {
        private BaseVM _currentView;

        public BaseVM CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand MainCommand { get; set; }
        public ICommand VoltageCommand { get; set; }
        public ICommand CurrentCommand { get; set; }
        public ICommand PowerCommand { get; set; }
        public ICommand SettingCommand { get; set; }

        // Properties
        private void Main(object obj) => CurrentView = MainVM.Instance;
        private void Voltage(object obj) => CurrentView = VoltageVM.Instance;
        private void Current(object obj) => CurrentView = CurrentVM.Instance;
        private void Power(object obj) => CurrentView = PowerVM.Instance;
        private void Setting(object obj) => CurrentView = SettingsVM.Instance;

        public NavigateVM()
        {
            MainCommand = new RelayCommand(Main);
            VoltageCommand = new RelayCommand(Voltage);
            CurrentCommand = new RelayCommand(Current);
            PowerCommand = new RelayCommand(Power);
            SettingCommand = new RelayCommand(Setting);

            // Startup Page
            CurrentView = MainVM.Instance;
        }
    }
}
