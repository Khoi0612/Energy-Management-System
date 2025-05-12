using EMS.Core;
using EMS.MVVM.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace EMS.MVVM.ViewModel
{
    internal class MainVM : BaseVM
    {
        // Model fields
        private readonly MainModel _mainModel;
        private readonly CurrentVM _currentVM;
        private readonly VoltageVM _voltageVM;
        private readonly PowerVM _powerVM;

        private static MainVM _instance; // Initiate an instance

        // Warning fields
        private ObservableCollection<WarningModel> _activeWarnings;
        private bool _canAcknowledged;
        private readonly Dispatcher _dispatcher;

        private MainVM()
        {
            _mainModel = new MainModel();

            // Initiated instances
            _currentVM = CurrentVM.Instance;
            _voltageVM = VoltageVM.Instance;
            _powerVM = PowerVM.Instance;

            // Subscribed to Parameters change events
            // Current
            _mainModel.CurrentModel.Current_L1_Changed += OnCurrent_L1_Changed;
            _mainModel.CurrentModel.Current_L2_Changed += OnCurrent_L2_Changed;
            _mainModel.CurrentModel.Current_L3_Changed += OnCurrent_L3_Changed;

            // Phase Voltage
            _mainModel.VoltageModel.PhaseVoltage_L1_Changed += OnPhaseVoltage_L1_Changed;
            _mainModel.VoltageModel.PhaseVoltage_L2_Changed += OnPhaseVoltage_L2_Changed;
            _mainModel.VoltageModel.PhaseVoltage_L3_Changed += OnPhaseVoltage_L3_Changed;

            // Line Voltage
            _mainModel.VoltageModel.LineVoltage_L12_Changed += OnLineVoltage_L12_Changed;
            _mainModel.VoltageModel.LineVoltage_L23_Changed += OnLineVoltage_L23_Changed;
            _mainModel.VoltageModel.LineVoltage_L31_Changed += OnLineVoltage_L31_Changed;

            // Active Power
            _mainModel.PowerModel.ActivePower_L1_Changed += OnActivePower_L1_Changed;
            _mainModel.PowerModel.ActivePower_L2_Changed += OnActivePower_L2_Changed;
            _mainModel.PowerModel.ActivePower_L3_Changed += OnActivePower_L3_Changed;

            // Reactive Power
            _mainModel.PowerModel.ReactivePower_L1_Changed += OnReactivePower_L1_Changed;
            _mainModel.PowerModel.ReactivePower_L2_Changed += OnReactivePower_L2_Changed;
            _mainModel.PowerModel.ReactivePower_L3_Changed += OnReactivePower_L3_Changed;

            // Apparent Power
            _mainModel.PowerModel.ApparentPower_L1_Changed += OnApparentPower_L1_Changed;
            _mainModel.PowerModel.ApparentPower_L2_Changed += OnApparentPower_L2_Changed;
            _mainModel.PowerModel.ApparentPower_L3_Changed += OnApparentPower_L3_Changed;

            // Power Factor
            _mainModel.PowerModel.PowerFactor_L1_Changed += OnPowerFactor_L1_Changed;
            _mainModel.PowerModel.PowerFactor_L2_Changed += OnPowerFactor_L2_Changed;
            _mainModel.PowerModel.PowerFactor_L3_Changed += OnPowerFactor_L3_Changed;

            // Initiated commands
            AcknowledgeWarningCommand = new RelayCommand(param => AcknowledgeWarning((WarningModel)param));
            AcknowledgeAllWarningCommand = new RelayCommand(AcknowledgeAllWarning);

            // Initiated warning fields
            _dispatcher = Application.Current.Dispatcher;
            ActiveWarnings = new ObservableCollection<WarningModel>();
            _canAcknowledged = false;

            // Initiated warning events
            // Current
            _currentVM.Current_L1_WarningTriggered += AddWarning;
            _currentVM.Current_L2_WarningTriggered += AddWarning;
            _currentVM.Current_L3_WarningTriggered += AddWarning;

            // Phase Voltage
            _voltageVM.PhaseVoltage_L1_WarningTriggered += AddWarning;
            _voltageVM.PhaseVoltage_L2_WarningTriggered += AddWarning;
            _voltageVM.PhaseVoltage_L3_WarningTriggered += AddWarning;

            // Line Voltage
            _voltageVM.LineVoltage_L12_WarningTriggered += AddWarning;
            _voltageVM.LineVoltage_L23_WarningTriggered += AddWarning;
            _voltageVM.LineVoltage_L31_WarningTriggered += AddWarning;

            // Active Power
            _powerVM.ActivePower_L1_WarningTriggered += AddWarning;
            _powerVM.ActivePower_L2_WarningTriggered += AddWarning;
            _powerVM.ActivePower_L3_WarningTriggered += AddWarning;

            // Reactive Power
            _powerVM.ReactivePower_L1_WarningTriggered += AddWarning;
            _powerVM.ReactivePower_L2_WarningTriggered += AddWarning;
            _powerVM.ReactivePower_L3_WarningTriggered += AddWarning;

            // Apparent Power
            _powerVM.ApparentPower_L1_WarningTriggered += AddWarning;
            _powerVM.ApparentPower_L2_WarningTriggered += AddWarning;
            _powerVM.ApparentPower_L3_WarningTriggered += AddWarning;

            // Power Factor
            _powerVM.PowerFactor_L1_WarningTriggered += AddWarning;
            _powerVM.PowerFactor_L2_WarningTriggered += AddWarning;
            _powerVM.PowerFactor_L3_WarningTriggered += AddWarning;

        }

        // Current Properties
        public double Current_L1
        {
            get => _currentVM.Current_L1;
            set { _currentVM.Current_L1 = value; OnPropertyChanged(); }
        }

        public double Current_L2
        {
            get => _currentVM.Current_L2;
            set { _currentVM.Current_L2 = value; OnPropertyChanged(); }
        }

        public double Current_L3
        {
            get => _currentVM.Current_L3;
            set { _currentVM.Current_L3 = value; OnPropertyChanged(); }
        }

        // Phase Volatge Properties
        public double PhaseVoltage_L1
        {
            get => _voltageVM.PhaseVoltage_L1;
            set { _voltageVM.PhaseVoltage_L1 = value; OnPropertyChanged(); }
        }

        public double PhaseVoltage_L2
        {
            get => _voltageVM.PhaseVoltage_L2;
            set { _voltageVM.PhaseVoltage_L2 = value; OnPropertyChanged(); }
        }

        public double PhaseVoltage_L3
        {
            get => _voltageVM.PhaseVoltage_L3;
            set { _voltageVM.PhaseVoltage_L3 = value; OnPropertyChanged(); }
        }

        // Line Volatge Properties
        public double LineVoltage_L12
        {
            get => _voltageVM.LineVoltage_L12;
            set { _voltageVM.LineVoltage_L12 = value; OnPropertyChanged(); }
        }

        public double LineVoltage_L23
        {
            get => _voltageVM.LineVoltage_L23;
            set { _voltageVM.LineVoltage_L23 = value; OnPropertyChanged(); }
        }

        public double LineVoltage_L31
        {
            get => _voltageVM.LineVoltage_L31;
            set { _voltageVM.LineVoltage_L31 = value; OnPropertyChanged(); }
        }

        // Active Power Properties
        public double ActivePower_L1
        {
            get => _powerVM.ActivePower_L1;
            set { _powerVM.ActivePower_L1 = value; OnPropertyChanged(); }
        }

        public double ActivePower_L2
        {
            get => _powerVM.ActivePower_L2;
            set { _powerVM.ActivePower_L2 = value; OnPropertyChanged(); }
        }

        public double ActivePower_L3
        {
            get => _powerVM.ActivePower_L3;
            set { _powerVM.ActivePower_L3 = value; OnPropertyChanged(); }
        }

        // Reactive Power Properties
        public double ReactivePower_L1
        {
            get => _powerVM.ReactivePower_L1;
            set { _powerVM.ReactivePower_L1 = value; OnPropertyChanged(); }
        }

        public double ReactivePower_L2
        {
            get => _powerVM.ReactivePower_L2;
            set { _powerVM.ReactivePower_L2 = value; OnPropertyChanged(); }
        }

        public double ReactivePower_L3
        {
            get => _powerVM.ReactivePower_L3;
            set { _powerVM.ReactivePower_L3 = value; OnPropertyChanged(); }
        }

        // Apparent Power Properties
        public double ApparentPower_L1
        {
            get => _powerVM.ApparentPower_L1;
            set { _powerVM.ApparentPower_L1 = value; OnPropertyChanged(); }
        }

        public double ApparentPower_L2
        {
            get => _powerVM.ApparentPower_L2;
            set { _powerVM.ApparentPower_L2 = value; OnPropertyChanged(); }
        }

        public double ApparentPower_L3
        {
            get => _powerVM.ApparentPower_L3;
            set { _powerVM.ApparentPower_L3 = value; OnPropertyChanged(); }
        }

        // Power Factor Properties
        public double PowerFactor_L1
        {
            get => _powerVM.PowerFactor_L1;
            set { _powerVM.PowerFactor_L1 = value; OnPropertyChanged(); }
        }

        public double PowerFactor_L2
        {
            get => _powerVM.PowerFactor_L2;
            set { _powerVM.PowerFactor_L2 = value; OnPropertyChanged(); }
        }

        public double PowerFactor_L3
        {
            get => _powerVM.PowerFactor_L3;
            set { _powerVM.PowerFactor_L3 = value; OnPropertyChanged(); }
        }

        // Charts Properties
        // Current
        public double CurrentMin
        {
            get => this.Settings.CurrentMin;
        }

        public double CurrentMax
        {
            get => this.Settings.CurrentMax;
        }

        // Phase Voltage
        public double PhaseVoltageMin
        {
            get => this.Settings.PhaseVoltageMin;
        }

        public double PhaseVoltageMax
        {
            get => this.Settings.PhaseVoltageMax;
        }

        // Line Voltage
        public double LineVoltageMin
        {
            get => this.Settings.LineVoltageMin;
        }

        public double LineVoltageMax
        {
            get => this.Settings.LineVoltageMax;
        }

        // Active Power
        public double ActivePowerMin
        {
            get => this.Settings.ActivePowerMin;
        }

        public double ActivePowerMax
        {
            get => this.Settings.ActivePowerMax;
        }

        // Reactive Power
        public double ReactivePowerMin
        {
            get => this.Settings.ReactivePowerMin;
        }

        public double ReactivePowerMax
        {
            get => this.Settings.ReactivePowerMax;
        }

        // Apparent Power
        public double ApparentPowerMin
        {
            get => this.Settings.ApparentPowerMin;
        }

        public double ApparentPowerMax
        {
            get => this.Settings.ApparentPowerMax;
        }

        // Power Factor
        public double PowerFactorMin
        {
            get => this.Settings.PowerFactorMin;
        }

        public double PowerFactorMax
        {
            get => this.Settings.PowerFactorMax;
        }

        // Warning properties
        public bool CanAcknowledged { get => _canAcknowledged; set { _canAcknowledged = value; OnPropertyChanged(); } }

        public ObservableCollection<WarningModel> ActiveWarnings
        {
            get => _activeWarnings;
            set
            {
                _activeWarnings = value;
                OnPropertyChanged();
            }
        }

        // Color properties
        // Current
        public SolidColorBrush CurrentStatusColor_L1
        {
            get => _currentVM.CurrentStatusColor_L1;
            set
            {
                _currentVM.CurrentStatusColor_L1 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush CurrentStatusColor_L2
        {
            get => _currentVM.CurrentStatusColor_L2;
            set
            {
                _currentVM.CurrentStatusColor_L2 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush CurrentStatusColor_L3
        {
            get => _currentVM.CurrentStatusColor_L3;
            set
            {
                _currentVM.CurrentStatusColor_L3 = value;
                OnPropertyChanged();
            }
        }

        // Phase Voltage
        public SolidColorBrush PhaseVoltageStatusColor_L1
        {
            get => _voltageVM.PhaseVoltageStatusColor_L1;
            set
            {
                _voltageVM.PhaseVoltageStatusColor_L1 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush PhaseVoltageStatusColor_L2
        {
            get => _voltageVM.PhaseVoltageStatusColor_L2;
            set
            {
                _voltageVM.PhaseVoltageStatusColor_L2 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush PhaseVoltageStatusColor_L3
        {
            get => _voltageVM.PhaseVoltageStatusColor_L3;
            set
            {
                _voltageVM.PhaseVoltageStatusColor_L3 = value;
                OnPropertyChanged();
            }
        }

        // Line Voltage
        public SolidColorBrush LineVoltageStatusColor_L12
        {
            get => _voltageVM.LineVoltageStatusColor_L12;
            set
            {
                _voltageVM.LineVoltageStatusColor_L12 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush LineVoltageStatusColor_L23
        {
            get => _voltageVM.LineVoltageStatusColor_L23;
            set
            {
                _voltageVM.LineVoltageStatusColor_L23 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush LineVoltageStatusColor_L31
        {
            get => _voltageVM.LineVoltageStatusColor_L31;
            set
            {
                _voltageVM.LineVoltageStatusColor_L31 = value;
                OnPropertyChanged();
            }
        }

        // Active Power
        public SolidColorBrush ActivePowerStatusColor_L1
        {
            get => _powerVM.ActivePowerStatusColor_L1;
            set
            {
                _powerVM.ActivePowerStatusColor_L1 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush ActivePowerStatusColor_L2
        {
            get => _powerVM.ActivePowerStatusColor_L2;
            set
            {
                _powerVM.ActivePowerStatusColor_L2 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush ActivePowerStatusColor_L3
        {
            get => _powerVM.ActivePowerStatusColor_L3;
            set
            {
                _powerVM.ActivePowerStatusColor_L3 = value;
                OnPropertyChanged();
            }
        }

        // Reactive Power
        public SolidColorBrush ReactivePowerStatusColor_L1
        {
            get => _powerVM.ReactivePowerStatusColor_L1;
            set
            {
                _powerVM.ReactivePowerStatusColor_L1 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush ReactivePowerStatusColor_L2
        {
            get => _powerVM.ReactivePowerStatusColor_L2;
            set
            {
                _powerVM.ReactivePowerStatusColor_L2 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush ReactivePowerStatusColor_L3
        {
            get => _powerVM.ReactivePowerStatusColor_L3;
            set
            {
                _powerVM.ReactivePowerStatusColor_L3 = value;
                OnPropertyChanged();
            }
        }

        // Apparent Power
        public SolidColorBrush ApparentPowerStatusColor_L1
        {
            get => _powerVM.ApparentPowerStatusColor_L1;
            set
            {
                _powerVM.ApparentPowerStatusColor_L1 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush ApparentPowerStatusColor_L2
        {
            get => _powerVM.ApparentPowerStatusColor_L2;
            set
            {
                _powerVM.ApparentPowerStatusColor_L2 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush ApparentPowerStatusColor_L3
        {
            get => _powerVM.ApparentPowerStatusColor_L3;
            set
            {
                _powerVM.ApparentPowerStatusColor_L3 = value;
                OnPropertyChanged();
            }
        }

        // Power Factor
        public SolidColorBrush PowerFactorStatusColor_L1
        {
            get => _powerVM.PowerFactorStatusColor_L1;
            set
            {
                _powerVM.PowerFactorStatusColor_L1 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush PowerFactorStatusColor_L2
        {
            get => _powerVM.PowerFactorStatusColor_L2;
            set
            {
                _powerVM.PowerFactorStatusColor_L2 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush PowerFactorStatusColor_L3
        {
            get => _powerVM.PowerFactorStatusColor_L3;
            set
            {
                _powerVM.PowerFactorStatusColor_L3 = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand AcknowledgeWarningCommand { get; }
        public ICommand AcknowledgeAllWarningCommand { get; }

        // On Current Change Functions
        private void OnCurrent_L1_Changed(double newValue)
        {
            CurrentStatusColor_L1 = _currentVM.CurrentStatusColor_L1;
            Current_L1 = newValue;
        }

        private void OnCurrent_L2_Changed(double newValue)
        {
            CurrentStatusColor_L2 = _currentVM.CurrentStatusColor_L2;
            Current_L2 = newValue;
        }

        private void OnCurrent_L3_Changed(double newValue)
        {
            CurrentStatusColor_L3 = _currentVM.CurrentStatusColor_L3;
            Current_L3 = newValue;
        }

        // On Phase Voltage Change Functions
        private void OnPhaseVoltage_L1_Changed(double newValue)
        {
            PhaseVoltageStatusColor_L1 = _voltageVM.PhaseVoltageStatusColor_L1;
            PhaseVoltage_L1 = newValue;
        }

        private void OnPhaseVoltage_L2_Changed(double newValue)
        {
            PhaseVoltageStatusColor_L2 = _voltageVM.PhaseVoltageStatusColor_L2;
            PhaseVoltage_L2 = newValue;
        }

        private void OnPhaseVoltage_L3_Changed(double newValue)
        {
            PhaseVoltageStatusColor_L3 = _voltageVM.PhaseVoltageStatusColor_L3;
            PhaseVoltage_L3 = newValue;
        }

        // On Line Voltage Change Functions
        private void OnLineVoltage_L12_Changed(double newValue)
        {
            LineVoltageStatusColor_L12 = _voltageVM.LineVoltageStatusColor_L12;
            LineVoltage_L12 = newValue;
        }

        private void OnLineVoltage_L23_Changed(double newValue)
        {
            LineVoltageStatusColor_L23 = _voltageVM.LineVoltageStatusColor_L23;
            LineVoltage_L23 = newValue;
        }

        private void OnLineVoltage_L31_Changed(double newValue)
        {
            LineVoltageStatusColor_L31 = _voltageVM.LineVoltageStatusColor_L31;
            LineVoltage_L31 = newValue;
        }

        // On Active Power Changed Functions
        private void OnActivePower_L1_Changed(double newValue)
        {
            ActivePowerStatusColor_L1 = _powerVM.ActivePowerStatusColor_L1;
            ActivePower_L1 = newValue;
        }

        private void OnActivePower_L2_Changed(double newValue)
        {
            ActivePowerStatusColor_L2 = _powerVM.ActivePowerStatusColor_L2;
            ActivePower_L2 = newValue;
        }

        private void OnActivePower_L3_Changed(double newValue)
        {
            ActivePowerStatusColor_L3 = _powerVM.ActivePowerStatusColor_L3;
            ActivePower_L3 = newValue;
        }

        // On Reactive Power Changed Functions
        private void OnReactivePower_L1_Changed(double newValue)
        {
            ReactivePowerStatusColor_L1 = _powerVM.ReactivePowerStatusColor_L1;
            ReactivePower_L1 = newValue;
        }

        private void OnReactivePower_L2_Changed(double newValue)
        {
            ReactivePowerStatusColor_L2 = _powerVM.ReactivePowerStatusColor_L2;
            ReactivePower_L2 = newValue;
        }

        private void OnReactivePower_L3_Changed(double newValue)
        {
            ReactivePowerStatusColor_L3 = _powerVM.ReactivePowerStatusColor_L3;
            ReactivePower_L3 = newValue;
        }

        // On Apparent Power Changed Functions
        private void OnApparentPower_L1_Changed(double newValue)
        {
            ApparentPowerStatusColor_L1 = _powerVM.ApparentPowerStatusColor_L1;
            ApparentPower_L1 = newValue;
        }

        private void OnApparentPower_L2_Changed(double newValue)
        {
            ApparentPowerStatusColor_L2 = _powerVM.ApparentPowerStatusColor_L2;
            ApparentPower_L2 = newValue;
        }

        private void OnApparentPower_L3_Changed(double newValue)
        {
            ApparentPowerStatusColor_L3 = _powerVM.ApparentPowerStatusColor_L3;
            ApparentPower_L3 = newValue;
        }

        // On Power Factor Changed Functions
        private void OnPowerFactor_L1_Changed(double newValue)
        {
            PowerFactorStatusColor_L1 = _powerVM.PowerFactorStatusColor_L1;
            PowerFactor_L1 = newValue;
        }

        private void OnPowerFactor_L2_Changed(double newValue)
        {
            PowerFactorStatusColor_L2 = _powerVM.PowerFactorStatusColor_L2;
            PowerFactor_L2 = newValue;
        }

        private void OnPowerFactor_L3_Changed(double newValue)
        {
            PowerFactorStatusColor_L3 = _powerVM.PowerFactorStatusColor_L3;
            PowerFactor_L3 = newValue;
        }

        // Warning functions
        private void AddWarning(WarningModel warning)
        {
            if (_dispatcher.CheckAccess())
            {
                ActiveWarnings.Add(warning);
                CanAcknowledged = true;
            }
            else
            {
                _dispatcher.Invoke(() => ActiveWarnings.Add(warning));
                CanAcknowledged = true;
            }
        }

        private void AcknowledgeWarning(WarningModel warning)
        {
            if (warning != null)
            {
                warning.IsAcknowledged = true;
                // Force the UI to refresh by removing and re-adding the item
                int index = ActiveWarnings.IndexOf(warning);
                if (index != -1)
                {
                    ActiveWarnings.RemoveAt(index);
                    ActiveWarnings.Insert(index, warning);
                }
            }
        }

        private void AcknowledgeAllWarning(object obj)
        {
            for (int i = ActiveWarnings.Count - 1; i >= 0; i--)
            {
                WarningModel warning = ActiveWarnings[i];
                if (warning != null)
                {
                    warning.IsAcknowledged = true;
                    // Force the UI to refresh by removing and re-adding the item
                    ActiveWarnings.RemoveAt(i);
                    ActiveWarnings.Insert(i, warning);
                }
            }
        }

        // Singleton design pattern
        public static MainVM Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainVM();
                }
                return _instance;
            }
        }


    }
}
