using EMS.Core;
using EMS.Core.Models;
using EMS.MVVM.Model;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace EMS.MVVM.ViewModel
{
    internal class PowerVM : BaseVM
    {
        private readonly PowerModel _powerModel;
        private static PowerVM _instance; // Initiated an instance

        // Power fields
        // Active
        private double _activePower_L1;
        private double _activePower_L2;
        private double _activePower_L3;

        // Reactive
        private double _reactivePower_L1;
        private double _reactivePower_L2;
        private double _reactivePower_L3;

        // Apparent
        private double _apparentPower_L1;
        private double _apparentPower_L2;
        private double _apparentPower_L3;

        // Power Factor
        private double _powerFactor_L1;
        private double _powerFactor_L2;
        private double _powerFactor_L3;

        // Chart fields
        // Active
        private double _activePowerChartTypeIndex;
        private object _activePowerChart;

        // Reactive
        private double _reactivePowerChartTypeIndex;
        private object _reactivePowerChart;

        // Apparent
        private double _apparentPowerChartTypeIndex;
        private object _apparentPowerChart;

        // Power Factor
        private double _powerFactorChartTypeIndex;
        private object _powerFactorChart;

        // Report fields
        // Active
        private DateTime? _activePowerStartDate;
        private DateTime? _activePowerEndDate;
        private string _activePowerFrequency;
        private bool _activePowerCanExport;
        private ObservableCollection<ActivePowerReading> _activePowerReadings;

        // Reactive
        private DateTime? _reactivePowerStartDate;
        private DateTime? _reactivePowerEndDate;
        private string _reactivePowerFrequency;
        private bool _reactivePowerCanExport;
        private ObservableCollection<ReactivePowerReading> _reactivePowerReadings;

        // Apparent
        private DateTime? _apparentPowerStartDate;
        private DateTime? _apparentPowerEndDate;
        private string _apparentPowerFrequency;
        private bool _apparentPowerCanExport;
        private ObservableCollection<ApparentPowerReading> _apparentPowerReadings;

        // Power Factor
        private DateTime? _powerFactorStartDate;
        private DateTime? _powerFactorEndDate;
        private string _powerFactorFrequency;
        private bool _powerFactorCanExport;
        private ObservableCollection<PowerFactorReading> _powerFactorReadings;

        // Warning fields
        // Active
        private SolidColorBrush _activePowerStatusColor_L1;
        private SolidColorBrush _activePowerStatusColor_L2;
        private SolidColorBrush _activePowerStatusColor_L3;

        // Reactive
        private SolidColorBrush _reactivePowerStatusColor_L1;
        private SolidColorBrush _reactivePowerStatusColor_L2;
        private SolidColorBrush _reactivePowerStatusColor_L3;

        // Apparent
        private SolidColorBrush _apparentPowerStatusColor_L1;
        private SolidColorBrush _apparentPowerStatusColor_L2;
        private SolidColorBrush _apparentPowerStatusColor_L3;

        // Power Factor
        private SolidColorBrush _powerFactorStatusColor_L1;
        private SolidColorBrush _powerFactorStatusColor_L2;
        private SolidColorBrush _powerFactorStatusColor_L3;

        private PowerVM()
        {
            _powerModel = new PowerModel();

            // Subscribed to Power change events
            // Active
            _powerModel.ActivePower_L1_Changed += OnActivePower_L1_Changed;
            _powerModel.ActivePower_L2_Changed += OnActivePower_L2_Changed;
            _powerModel.ActivePower_L3_Changed += OnActivePower_L3_Changed;

            // Reactive
            _powerModel.ReactivePower_L1_Changed += OnReactivePower_L1_Changed;
            _powerModel.ReactivePower_L2_Changed += OnReactivePower_L2_Changed;
            _powerModel.ReactivePower_L3_Changed += OnReactivePower_L3_Changed;

            // Apparent
            _powerModel.ApparentPower_L1_Changed += OnApparentPower_L1_Changed;
            _powerModel.ApparentPower_L2_Changed += OnApparentPower_L2_Changed;
            _powerModel.ApparentPower_L3_Changed += OnApparentPower_L3_Changed;

            // Power Factor
            _powerModel.PowerFactor_L1_Changed += OnPowerFactor_L1_Changed;
            _powerModel.PowerFactor_L2_Changed += OnPowerFactor_L2_Changed;
            _powerModel.PowerFactor_L3_Changed += OnPowerFactor_L3_Changed;

            // Initiated commands
            // Active
            PlotActivePowerCommand = new RelayCommand(PlotActivePowerChart);
            SwitchActivePowerCommand = new RelayCommand(SwitchActivePowerChartType);
            StopPlottingActivePowerCommand = new RelayCommand(StopPlottingActivePowerChart);
            LoadReportActivePowerCommand = new AsyncRelayCommand(LoadActivePowerData);
            ExportReportActivePowerCommand = new RelayCommand(ActivePowerExportToCsv);

            // Reactive
            PlotReactivePowerCommand = new RelayCommand(PlotReactivePowerChart);
            SwitchReactivePowerCommand = new RelayCommand(SwitchReactivePowerChartType);
            StopPlottingReactivePowerCommand = new RelayCommand(StopPlottingReactivePowerChart);
            LoadReportReactivePowerCommand = new AsyncRelayCommand(LoadReactivePowerData);
            ExportReportReactivePowerCommand = new RelayCommand(ReactivePowerExportToCsv);

            // Apparent
            PlotApparentPowerCommand = new RelayCommand(PlotApparentPowerChart);
            SwitchApparentPowerCommand = new RelayCommand(SwitchApparentPowerChartType);
            StopPlottingApparentPowerCommand = new RelayCommand(StopPlottingApparentPowerChart);
            LoadReportApparentPowerCommand = new AsyncRelayCommand(LoadApparentPowerData);
            ExportReportApparentPowerCommand = new RelayCommand(ApparentPowerExportToCsv);

            // Power Factor
            PlotPowerFactorCommand = new RelayCommand(PlotPowerFactorChart);
            SwitchPowerFactorCommand = new RelayCommand(SwitchPowerFactorChartType);
            StopPlottingPowerFactorCommand = new RelayCommand(StopPlottingPowerFactorChart);
            LoadReportPowerFactorCommand = new AsyncRelayCommand(LoadPowerFactorData);
            ExportReportPowerFactorCommand = new RelayCommand(PowerFactorExportToCsv);

            // Initiated time elapsed
            TimeElapsed = value => TimeSpan.FromSeconds(value).ToString(@"mm\:ss");

            // Initiated Power chart values
            // Active
            ActivePowerValues_L1 = new ChartValues<double>();
            ActivePowerValues_L2 = new ChartValues<double>();
            ActivePowerValues_L3 = new ChartValues<double>();

            // Reactive
            ReactivePowerValues_L1 = new ChartValues<double>();
            ReactivePowerValues_L2 = new ChartValues<double>();
            ReactivePowerValues_L3 = new ChartValues<double>();

            // Apparent
            ApparentPowerValues_L1 = new ChartValues<double>();
            ApparentPowerValues_L2 = new ChartValues<double>();
            ApparentPowerValues_L3 = new ChartValues<double>();

            // Power Factor
            PowerFactorValues_L1 = new ChartValues<double>();
            PowerFactorValues_L2 = new ChartValues<double>();
            PowerFactorValues_L3 = new ChartValues<double>();

            // Initiated Power report fields
            // Active
            _activePowerCanExport = false;
            ActivePowerStartDate = null;
            ActivePowerEndDate = null;

            // Reactive
            _reactivePowerCanExport = false;
            ReactivePowerStartDate = null;
            ReactivePowerEndDate = null;

            // Apparent
            _apparentPowerCanExport = false;
            ApparentPowerStartDate = null;
            ApparentPowerEndDate = null;

            // Power Factor
            _powerFactorCanExport = false;
            PowerFactorStartDate = null;
            PowerFactorEndDate = null;

            // Invoked warning events
            // Active
            _powerModel.ActivePower_L1_WarningTriggered += (warning) =>
            {
                ActivePower_L1_WarningTriggered?.Invoke(warning);
            };

            _powerModel.ActivePower_L2_WarningTriggered += (warning) =>
            {
                ActivePower_L2_WarningTriggered?.Invoke(warning);
            };

            _powerModel.ActivePower_L3_WarningTriggered += (warning) =>
            {
                ActivePower_L3_WarningTriggered?.Invoke(warning);
            };

            // Reactive
            _powerModel.ReactivePower_L1_WarningTriggered += (warning) =>
            {
                ReactivePower_L1_WarningTriggered?.Invoke(warning);
            };

            _powerModel.ReactivePower_L2_WarningTriggered += (warning) =>
            {
                ReactivePower_L2_WarningTriggered?.Invoke(warning);
            };

            _powerModel.ReactivePower_L3_WarningTriggered += (warning) =>
            {
                ReactivePower_L3_WarningTriggered?.Invoke(warning);
            };

            // Apparent
            _powerModel.ApparentPower_L1_WarningTriggered += (warning) =>
            {
                ApparentPower_L1_WarningTriggered?.Invoke(warning);
            };

            _powerModel.ApparentPower_L2_WarningTriggered += (warning) =>
            {
                ApparentPower_L2_WarningTriggered?.Invoke(warning);
            };

            _powerModel.ApparentPower_L3_WarningTriggered += (warning) =>
            {
                ApparentPower_L3_WarningTriggered?.Invoke(warning);
            };

            // Power Factor
            _powerModel.PowerFactor_L1_WarningTriggered += (warning) =>
            {
                PowerFactor_L1_WarningTriggered?.Invoke(warning);
            };

            _powerModel.PowerFactor_L2_WarningTriggered += (warning) =>
            {
                PowerFactor_L2_WarningTriggered?.Invoke(warning);
            };

            _powerModel.PowerFactor_L3_WarningTriggered += (warning) =>
            {
                PowerFactor_L3_WarningTriggered?.Invoke(warning);
            };
        }

        // Active Power Properties
        public double ActivePower_L1
        {
            get => _activePower_L1;
            set { _activePower_L1 = value; OnPropertyChanged(); }
        }

        public double ActivePower_L2
        {
            get => _activePower_L2;
            set { _activePower_L2 = value; OnPropertyChanged(); }
        }

        public double ActivePower_L3
        {
            get => _activePower_L3;
            set { _activePower_L3 = value; OnPropertyChanged(); }
        }

        // Reactive Power Properties
        public double ReactivePower_L1
        {
            get => _reactivePower_L1;
            set { _reactivePower_L1 = value; OnPropertyChanged(); }
        }

        public double ReactivePower_L2
        {
            get => _reactivePower_L2;
            set { _reactivePower_L2 = value; OnPropertyChanged(); }
        }

        public double ReactivePower_L3
        {
            get => _reactivePower_L3;
            set { _reactivePower_L3 = value; OnPropertyChanged(); }
        }

        // Apparent Power Properties
        public double ApparentPower_L1
        {
            get => _apparentPower_L1;
            set { _apparentPower_L1 = value; OnPropertyChanged(); }
        }

        public double ApparentPower_L2
        {
            get => _apparentPower_L2;
            set { _apparentPower_L2 = value; OnPropertyChanged(); }
        }

        public double ApparentPower_L3
        {
            get => _apparentPower_L3;
            set { _apparentPower_L3 = value; OnPropertyChanged(); }
        }

        // Power Factor Properties
        public double PowerFactor_L1
        {
            get => _powerFactor_L1;
            set { _powerFactor_L1 = value; OnPropertyChanged(); }
        }

        public double PowerFactor_L2
        {
            get => _powerFactor_L2;
            set { _powerFactor_L2 = value; OnPropertyChanged(); }
        }

        public double PowerFactor_L3
        {
            get => _powerFactor_L3;
            set { _powerFactor_L3 = value; OnPropertyChanged(); }
        }

        // Chart properties
        // Time elaspased
        public Func<double, string> TimeElapsed { get; set; }

        // Active
        public ChartValues<double> ActivePowerValues_L1 { get; set; }
        public ChartValues<double> ActivePowerValues_L2 { get; set; }
        public ChartValues<double> ActivePowerValues_L3 { get; set; }

        public object ActivePowerChart
        {
            get => _activePowerChart;
            set { _activePowerChart = value; OnPropertyChanged(); }
        }

        public double ActivePowerMin
        {
            get => this.Settings.ActivePowerMin;
        }

        public double ActivePowerMax
        {
            get => this.Settings.ActivePowerMax;
        }

        // Reactive
        public ChartValues<double> ReactivePowerValues_L1 { get; set; }
        public ChartValues<double> ReactivePowerValues_L2 { get; set; }
        public ChartValues<double> ReactivePowerValues_L3 { get; set; }

        public object ReactivePowerChart
        {
            get => _reactivePowerChart;
            set { _reactivePowerChart = value; OnPropertyChanged(); }
        }

        public double ReactivePowerMin
        {
            get => this.Settings.ReactivePowerMin;
        }

        public double ReactivePowerMax
        {
            get => this.Settings.ReactivePowerMax;
        }

        // Apparent
        public ChartValues<double> ApparentPowerValues_L1 { get; set; }
        public ChartValues<double> ApparentPowerValues_L2 { get; set; }
        public ChartValues<double> ApparentPowerValues_L3 { get; set; }

        public object ApparentPowerChart
        {
            get => _apparentPowerChart;
            set { _apparentPowerChart = value; OnPropertyChanged(); }
        }

        public double ApparentPowerMin
        {
            get => this.Settings.ApparentPowerMin;
        }

        public double ApparentPowerMax
        {
            get => this.Settings.ApparentPowerMax;
        }

        // Power Factor
        public ChartValues<double> PowerFactorValues_L1 { get; set; }
        public ChartValues<double> PowerFactorValues_L2 { get; set; }
        public ChartValues<double> PowerFactorValues_L3 { get; set; }

        public object PowerFactorChart
        {
            get => _powerFactorChart;
            set { _powerFactorChart = value; OnPropertyChanged(); }
        }

        public double PowerFactorMin
        {
            get => this.Settings.PowerFactorMin;
        }

        public double PowerFactorMax
        {
            get => this.Settings.PowerFactorMax;
        }

        // Report properties
        // Active
        public DateTime? ActivePowerStartDate
        {
            get => _activePowerStartDate;
            set { _activePowerStartDate = value; OnPropertyChanged(); }
        }

        public DateTime? ActivePowerEndDate
        {
            get => _activePowerEndDate;
            set { _activePowerEndDate = value; OnPropertyChanged(); }
        }

        public string ActivePowerFrequency
        {
            get => _activePowerFrequency;
            set { _activePowerFrequency = value; OnPropertyChanged(); }
        }

        public bool ActivePowerCanExport
        {
            get => _activePowerCanExport;
            set
            {
                _activePowerCanExport = value; OnPropertyChanged();
            }
        }

        public ObservableCollection<ActivePowerReading> ActivePowerReadings
        {
            get => _activePowerReadings;
            set
            {
                _activePowerReadings = value;
                OnPropertyChanged();
            }
        }

        // Reactive
        public DateTime? ReactivePowerStartDate
        {
            get => _reactivePowerStartDate;
            set { _reactivePowerStartDate = value; OnPropertyChanged(); }
        }

        public DateTime? ReactivePowerEndDate
        {
            get => _reactivePowerEndDate;
            set { _reactivePowerEndDate = value; OnPropertyChanged(); }
        }

        public string ReactivePowerFrequency
        {
            get => _reactivePowerFrequency;
            set { _reactivePowerFrequency = value; OnPropertyChanged(); }
        }

        public bool ReactivePowerCanExport
        {
            get => _reactivePowerCanExport;
            set
            {
                _reactivePowerCanExport = value; OnPropertyChanged();
            }
        }

        public ObservableCollection<ReactivePowerReading> ReactivePowerReadings
        {
            get => _reactivePowerReadings;
            set
            {
                _reactivePowerReadings = value;
                OnPropertyChanged();
            }
        }

        // Apparent
        public DateTime? ApparentPowerStartDate
        {
            get => _apparentPowerStartDate;
            set { _apparentPowerStartDate = value; OnPropertyChanged(); }
        }

        public DateTime? ApparentPowerEndDate
        {
            get => _apparentPowerEndDate;
            set { _apparentPowerEndDate = value; OnPropertyChanged(); }
        }

        public string ApparentPowerFrequency
        {
            get => _apparentPowerFrequency;
            set { _apparentPowerFrequency = value; OnPropertyChanged(); }
        }

        public bool ApparentPowerCanExport
        {
            get => _apparentPowerCanExport;
            set
            {
                _apparentPowerCanExport = value; OnPropertyChanged();
            }
        }

        public ObservableCollection<ApparentPowerReading> ApparentPowerReadings
        {
            get => _apparentPowerReadings;
            set
            {
                _apparentPowerReadings = value;
                OnPropertyChanged();
            }
        }

        // Power Factor
        public DateTime? PowerFactorStartDate
        {
            get => _powerFactorStartDate;
            set { _powerFactorStartDate = value; OnPropertyChanged(); }
        }

        public DateTime? PowerFactorEndDate
        {
            get => _powerFactorEndDate;
            set { _powerFactorEndDate = value; OnPropertyChanged(); }
        }

        public string PowerFactorFrequency
        {
            get => _powerFactorFrequency;
            set { _powerFactorFrequency = value; OnPropertyChanged(); }
        }

        public bool PowerFactorCanExport
        {
            get => _powerFactorCanExport;
            set
            {
                _powerFactorCanExport = value; OnPropertyChanged();
            }
        }

        public ObservableCollection<PowerFactorReading> PowerFactorReadings
        {
            get => _powerFactorReadings;
            set
            {
                _powerFactorReadings = value;
                OnPropertyChanged();
            }
        }

        // Warning Properties
        // Active
        public SolidColorBrush ActivePowerStatusColor_L1
        {
            get => _activePowerStatusColor_L1;
            set
            {
                _activePowerStatusColor_L1 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush ActivePowerStatusColor_L2
        {
            get => _activePowerStatusColor_L2;
            set
            {
                _activePowerStatusColor_L2 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush ActivePowerStatusColor_L3
        {
            get => _activePowerStatusColor_L3;
            set
            {
                _activePowerStatusColor_L3 = value;
                OnPropertyChanged();
            }
        }

        // Reactive
        public SolidColorBrush ReactivePowerStatusColor_L1
        {
            get => _reactivePowerStatusColor_L1;
            set
            {
                _reactivePowerStatusColor_L1 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush ReactivePowerStatusColor_L2
        {
            get => _reactivePowerStatusColor_L2;
            set
            {
                _reactivePowerStatusColor_L2 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush ReactivePowerStatusColor_L3
        {
            get => _reactivePowerStatusColor_L3;
            set
            {
                _reactivePowerStatusColor_L3 = value;
                OnPropertyChanged();
            }
        }

        // Apparent
        public SolidColorBrush ApparentPowerStatusColor_L1
        {
            get => _apparentPowerStatusColor_L1;
            set
            {
                _apparentPowerStatusColor_L1 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush ApparentPowerStatusColor_L2
        {
            get => _apparentPowerStatusColor_L2;
            set
            {
                _apparentPowerStatusColor_L2 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush ApparentPowerStatusColor_L3
        {
            get => _apparentPowerStatusColor_L3;
            set
            {
                _apparentPowerStatusColor_L3 = value;
                OnPropertyChanged();
            }
        }

        // Power Factor
        public SolidColorBrush PowerFactorStatusColor_L1
        {
            get => _powerFactorStatusColor_L1;
            set
            {
                _powerFactorStatusColor_L1 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush PowerFactorStatusColor_L2
        {
            get => _powerFactorStatusColor_L2;
            set
            {
                _powerFactorStatusColor_L2 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush PowerFactorStatusColor_L3
        {
            get => _powerFactorStatusColor_L3;
            set
            {
                _powerFactorStatusColor_L3 = value;
                OnPropertyChanged();
            }
        }

        // Warning events
        // Active
        public event Action<WarningModel> ActivePower_L1_WarningTriggered;
        public event Action<WarningModel> ActivePower_L2_WarningTriggered;
        public event Action<WarningModel> ActivePower_L3_WarningTriggered;

        // Reactive
        public event Action<WarningModel> ReactivePower_L1_WarningTriggered;
        public event Action<WarningModel> ReactivePower_L2_WarningTriggered;
        public event Action<WarningModel> ReactivePower_L3_WarningTriggered;

        // Apparent
        public event Action<WarningModel> ApparentPower_L1_WarningTriggered;
        public event Action<WarningModel> ApparentPower_L2_WarningTriggered;
        public event Action<WarningModel> ApparentPower_L3_WarningTriggered;

        // Power Factor
        public event Action<WarningModel> PowerFactor_L1_WarningTriggered;
        public event Action<WarningModel> PowerFactor_L2_WarningTriggered;
        public event Action<WarningModel> PowerFactor_L3_WarningTriggered;

        // Commands
        // Active
        public ICommand PlotActivePowerCommand { get; }
        public ICommand SwitchActivePowerCommand { get; }
        public ICommand StopPlottingActivePowerCommand { get; }
        public ICommand LoadReportActivePowerCommand { get; set; }
        public ICommand ExportReportActivePowerCommand { get; set; }

        // Reactive
        public ICommand PlotReactivePowerCommand { get; }
        public ICommand SwitchReactivePowerCommand { get; }
        public ICommand StopPlottingReactivePowerCommand { get; }
        public ICommand LoadReportReactivePowerCommand { get; set; }
        public ICommand ExportReportReactivePowerCommand { get; set; }

        // Apparent
        public ICommand PlotApparentPowerCommand { get; }
        public ICommand SwitchApparentPowerCommand { get; }
        public ICommand StopPlottingApparentPowerCommand { get; }
        public ICommand LoadReportApparentPowerCommand { get; set; }
        public ICommand ExportReportApparentPowerCommand { get; set; }

        // Power Factor
        public ICommand PlotPowerFactorCommand { get; }
        public ICommand SwitchPowerFactorCommand { get; }
        public ICommand StopPlottingPowerFactorCommand { get; }
        public ICommand LoadReportPowerFactorCommand { get; set; }
        public ICommand ExportReportPowerFactorCommand { get; set; }

        // Function handling setting up a running window
        public void RunningWindowHandler(ChartValues<double> values)
        {
            var elapsedTime = (DateTime.Now - StartTime).TotalSeconds;
            if (elapsedTime > WindowSize)
            {
                MinValue = elapsedTime - WindowSize;
                MaxValue = elapsedTime;

                while (values.Count > 0 && values[0] < MinValue)
                {
                    values.RemoveAt(0);
                }
            }
            else
            {
                MaxValue = WindowSize;
            }
        }



        // On Power Changed Functions
        private void OnValueChanged(ChartValues<double> chartValue, double currentValue, double newValue)
        {
            chartValue.Add(currentValue);
            RunningWindowHandler(chartValue);
            currentValue = newValue;
        }

        // Active
        private void OnActivePower_L1_Changed(double newValue)
        {
            ActivePowerStatusColor_L1 = UpdateStatusColor(_powerModel.ActivePowerWarning_L1.Category);
            OnValueChanged(ActivePowerValues_L1, ActivePower_L1, newValue);
        }

        private void OnActivePower_L2_Changed(double newValue)
        {
            ActivePowerStatusColor_L2 = UpdateStatusColor(_powerModel.ActivePowerWarning_L2.Category);
            OnValueChanged(ActivePowerValues_L2, ActivePower_L2, newValue);
        }

        private void OnActivePower_L3_Changed(double newValue)
        {
            ActivePowerStatusColor_L3 = UpdateStatusColor(_powerModel.ActivePowerWarning_L3.Category);
            OnValueChanged(ActivePowerValues_L3, ActivePower_L3, newValue);
        }

        // Reactive
        private void OnReactivePower_L1_Changed(double newValue)
        {
            ReactivePowerStatusColor_L1 = UpdateStatusColor(_powerModel.ReactivePowerWarning_L1.Category);
            OnValueChanged(ReactivePowerValues_L1, ReactivePower_L1, newValue);
        }

        private void OnReactivePower_L2_Changed(double newValue)
        {
            ReactivePowerStatusColor_L2 = UpdateStatusColor(_powerModel.ReactivePowerWarning_L2.Category);
            OnValueChanged(ReactivePowerValues_L2, ReactivePower_L2, newValue);
        }

        private void OnReactivePower_L3_Changed(double newValue)
        {
            ReactivePowerStatusColor_L3 = UpdateStatusColor(_powerModel.ReactivePowerWarning_L3.Category);
            OnValueChanged(ReactivePowerValues_L3, ReactivePower_L3, newValue);
        }

        // Apparent
        private void OnApparentPower_L1_Changed(double newValue)
        {
            ApparentPowerStatusColor_L1 = UpdateStatusColor(_powerModel.ApparentPowerWarning_L1.Category);
            OnValueChanged(ApparentPowerValues_L1, ApparentPower_L1, newValue);
        }

        private void OnApparentPower_L2_Changed(double newValue)
        {
            ApparentPowerStatusColor_L2 = UpdateStatusColor(_powerModel.ApparentPowerWarning_L2.Category);
            OnValueChanged(ApparentPowerValues_L2, ApparentPower_L2, newValue);
        }

        private void OnApparentPower_L3_Changed(double newValue)
        {
            ApparentPowerStatusColor_L3 = UpdateStatusColor(_powerModel.ApparentPowerWarning_L3.Category);
            OnValueChanged(ApparentPowerValues_L3, ApparentPower_L3, newValue);
        }

        // Power Factor
        private void OnPowerFactor_L1_Changed(double newValue)
        {
            PowerFactorStatusColor_L1 = UpdateStatusColor(_powerModel.PowerFactorWarning_L1.Category);
            OnValueChanged(PowerFactorValues_L1, PowerFactor_L1, newValue);
        }

        private void OnPowerFactor_L2_Changed(double newValue)
        {
            PowerFactorStatusColor_L2 = UpdateStatusColor(_powerModel.PowerFactorWarning_L2.Category);
            OnValueChanged(PowerFactorValues_L2, PowerFactor_L2, newValue);
        }

        private void OnPowerFactor_L3_Changed(double newValue)
        {
            PowerFactorStatusColor_L3 = UpdateStatusColor(_powerModel.PowerFactorWarning_L3.Category);
            OnValueChanged(PowerFactorValues_L3, PowerFactor_L3, newValue);
        }

        // Power charts functions
        // Active
        private void PlotActivePowerLineChart()
        {
            ActivePowerChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = ActivePowerValues_L1,
                        Title = "P_L1"
                    },
                    new LineSeries
                    {
                        Values = ActivePowerValues_L2,
                        Title = "P_L2"
                    },
                    new LineSeries
                    {
                        Values = ActivePowerValues_L3,
                        Title = "P_L3"
                    }
                },
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Time (secs)",
                        LabelFormatter = TimeElapsed,
                        MaxValue = MaxValue,
                        MinValue = MinValue
                    }
                },
                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Active Power (W)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotActivePowerBarChart()
        {
            ActivePowerChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Values = ActivePowerValues_L1,
                        Title = "P_L1"
                    },
                    new ColumnSeries
                    {
                        Values = ActivePowerValues_L2,
                        Title = "P_L2"
                    },
                    new ColumnSeries
                    {
                        Values = ActivePowerValues_L3,
                        Title = "P_L3"
                    }
                },
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Time (secs)",
                        LabelFormatter = TimeElapsed,
                        MaxValue = MaxValue,
                        MinValue = MinValue
                    }
                },
                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Active Power (W)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotActivePowerScatterChart()
        {
            ActivePowerChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new ScatterSeries
                    {
                        Values = ActivePowerValues_L1,
                        Title = "P_L1"
                    },
                    new ScatterSeries
                    {
                        Values = ActivePowerValues_L2,
                        Title = "P_L2"
                    },
                    new ScatterSeries
                    {
                        Values = ActivePowerValues_L3,
                        Title = "P_L3"
                    }
                },
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Time (secs)",
                        LabelFormatter = TimeElapsed,
                        MaxValue = MaxValue,
                        MinValue = MinValue
                    }
                },
                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Active Power (W)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotActivePowerChart(object obj)
        {
            PlotActivePowerLineChart();
        }

        private void StopPlottingActivePowerChart(object obj)
        {
            ActivePowerChart = null;
        }

        private void SwitchActivePowerChartType(object obj)
        {
            // Rotate through Line, Bar, and Scatter
            _activePowerChartTypeIndex = (_activePowerChartTypeIndex + 1) % 3;

            switch (_activePowerChartTypeIndex)
            {
                case 0: // Line Chart
                    PlotActivePowerLineChart();
                    break;

                case 1: // Bar Chart
                    PlotActivePowerBarChart();
                    break;

                case 2: // Scatter Chart
                    PlotActivePowerScatterChart();
                    break;
            }
        }

        // Reactive
        private void PlotReactivePowerLineChart()
        {
            ReactivePowerChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = ReactivePowerValues_L1,
                        Title = "VARQ1_L1"
                    },
                    new LineSeries
                    {
                        Values = ReactivePowerValues_L2,
                        Title = "VARQ1_L2"
                    },
                    new LineSeries
                    {
                        Values = ReactivePowerValues_L3,
                        Title = "VARQ1_L3"
                    }
                },
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Time (secs)",
                        LabelFormatter = TimeElapsed,
                        MaxValue = MaxValue,
                        MinValue = MinValue
                    }
                },
                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Reactive Power (VAR)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotReactivePowerBarChart()
        {
            ReactivePowerChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Values = ReactivePowerValues_L1,
                        Title = "VARQ1_L1"
                    },
                    new ColumnSeries
                    {
                        Values = ReactivePowerValues_L2,
                        Title = "VARQ1_L2"
                    },
                    new ColumnSeries
                    {
                        Values = ReactivePowerValues_L3,
                        Title = "VARQ1_L3"
                    }
                },
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Time (secs)",
                        LabelFormatter = TimeElapsed,
                        MaxValue = MaxValue,
                        MinValue = MinValue
                    }
                },
                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Reactive Power (VAR)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotReactivePowerScatterChart()
        {
            ReactivePowerChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new ScatterSeries
                    {
                        Values = ReactivePowerValues_L1,
                        Title = "VARQ1_L1"
                    },
                    new ScatterSeries
                    {
                        Values = ReactivePowerValues_L2,
                        Title = "VARQ1_L2"
                    },
                    new ScatterSeries
                    {
                        Values = ReactivePowerValues_L3,
                        Title = "VARQ1_L3"
                    }
                },
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Time (secs)",
                        LabelFormatter = TimeElapsed,
                        MaxValue = MaxValue,
                        MinValue = MinValue
                    }
                },
                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Reactive Power (VAR)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotReactivePowerChart(object obj)
        {
            PlotReactivePowerLineChart();
        }

        private void StopPlottingReactivePowerChart(object obj)
        {
            ReactivePowerChart = null;
        }

        private void SwitchReactivePowerChartType(object obj)
        {
            // Rotate through Line, Bar, and Scatter
            _reactivePowerChartTypeIndex = (_reactivePowerChartTypeIndex + 1) % 3;

            switch (_reactivePowerChartTypeIndex)
            {
                case 0: // Line Chart
                    PlotReactivePowerLineChart();
                    break;

                case 1: // Bar Chart
                    PlotReactivePowerBarChart();
                    break;

                case 2: // Scatter Chart
                    PlotReactivePowerScatterChart();
                    break;
            }
        }

        // Apparent
        private void PlotApparentPowerLineChart()
        {
            ApparentPowerChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = ApparentPowerValues_L1,
                        Title = "VA_L1"
                    },
                    new LineSeries
                    {
                        Values = ApparentPowerValues_L2,
                        Title = "VA_L2"
                    },
                    new LineSeries
                    {
                        Values = ApparentPowerValues_L3,
                        Title = "VA_L3"
                    }
                },
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Time (secs)",
                        LabelFormatter = TimeElapsed,
                        MaxValue = MaxValue,
                        MinValue = MinValue
                    }
                },
                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Apparent Power (VA)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotApparentPowerBarChart()
        {
            ApparentPowerChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Values = ApparentPowerValues_L1,
                        Title = "VA_L1"
                    },
                    new ColumnSeries
                    {
                        Values = ApparentPowerValues_L2,
                        Title = "VA_L2"
                    },
                    new ColumnSeries
                    {
                        Values = ApparentPowerValues_L3,
                        Title = "VA_L3"
                    }
                },
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Time (secs)",
                        LabelFormatter = TimeElapsed,
                        MaxValue = MaxValue,
                        MinValue = MinValue
                    }
                },
                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Apparent Power (VA)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotApparentPowerScatterChart()
        {
            ApparentPowerChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new ScatterSeries
                    {
                        Values = ApparentPowerValues_L1,
                        Title = "VA_L1"
                    },
                    new ScatterSeries
                    {
                        Values = ApparentPowerValues_L2,
                        Title = "VA_L2"
                    },
                    new ScatterSeries
                    {
                        Values = ApparentPowerValues_L3,
                        Title = "VA_L3"
                    }
                },
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Time (secs)",
                        LabelFormatter = TimeElapsed,
                        MaxValue = MaxValue,
                        MinValue = MinValue
                    }
                },
                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Apparent Power (VA)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotApparentPowerChart(object obj)
        {
            PlotApparentPowerLineChart();
        }

        private void StopPlottingApparentPowerChart(object obj)
        {
            ApparentPowerChart = null;
        }

        private void SwitchApparentPowerChartType(object obj)
        {
            // Rotate through Line, Bar, and Scatter
            _apparentPowerChartTypeIndex = (_apparentPowerChartTypeIndex + 1) % 3;

            switch (_apparentPowerChartTypeIndex)
            {
                case 0: // Line Chart
                    PlotApparentPowerLineChart();
                    break;

                case 1: // Bar Chart
                    PlotApparentPowerBarChart();
                    break;

                case 2: // Scatter Chart
                    PlotApparentPowerScatterChart();
                    break;
            }
        }

        // Power Factor
        private void PlotPowerFactorLineChart()
        {
            PowerFactorChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = PowerFactorValues_L1,
                        Title = "COS1_L1"
                    },
                    new LineSeries
                    {
                        Values = PowerFactorValues_L2,
                        Title = "COS1_L2"
                    },
                    new LineSeries
                    {
                        Values = PowerFactorValues_L3,
                        Title = "COS1_L3"
                    }
                },
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Time (secs)",
                        LabelFormatter = TimeElapsed,
                        MaxValue = MaxValue,
                        MinValue = MinValue
                    }
                },
                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Power Factor (COS ϕ)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotPowerFactorBarChart()
        {
            PowerFactorChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Values = PowerFactorValues_L1,
                        Title = "COS1_L1"
                    },
                    new ColumnSeries
                    {
                        Values = PowerFactorValues_L2,
                        Title = "COS1_L2"
                    },
                    new ColumnSeries
                    {
                        Values = PowerFactorValues_L3,
                        Title = "COS1_L3"
                    }
                },
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Time (secs)",
                        LabelFormatter = TimeElapsed,
                        MaxValue = MaxValue,
                        MinValue = MinValue
                    }
                },
                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Power Factor (COS ϕ)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotPowerFactorScatterChart()
        {
            PowerFactorChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new ScatterSeries
                    {
                        Values = PowerFactorValues_L1,
                        Title = "COS1_L1"
                    },
                    new ScatterSeries
                    {
                        Values = PowerFactorValues_L1,
                        Title = "COS1_L2"
                    },
                    new ScatterSeries
                    {
                        Values = PowerFactorValues_L1,
                        Title = "COS1_L3"
                    }
                },
                AxisX = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Time (secs)",
                        LabelFormatter = TimeElapsed,
                        MaxValue = MaxValue,
                        MinValue = MinValue
                    }
                },
                AxisY = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Power Factor (COS ϕ)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotPowerFactorChart(object obj)
        {
            PlotPowerFactorLineChart();
        }

        private void StopPlottingPowerFactorChart(object obj)
        {
            PowerFactorChart = null;
        }

        private void SwitchPowerFactorChartType(object obj)
        {
            // Rotate through Line, Bar, and Scatter
            _powerFactorChartTypeIndex = (_powerFactorChartTypeIndex + 1) % 3;

            switch (_powerFactorChartTypeIndex)
            {
                case 0: // Line Chart
                    PlotPowerFactorLineChart();
                    break;

                case 1: // Bar Chart
                    PlotPowerFactorBarChart();
                    break;

                case 2: // Scatter Chart
                    PlotPowerFactorScatterChart();
                    break;
            }
        }

        // Power report functions
        // Active
        private async Task LoadActivePowerData()
        {
            await GetFullData(ApparentPowerStartDate, ActivePowerEndDate, ActivePowerFrequency);

            if (_fullData != null)
            {
                ActivePowerReadings = new ObservableCollection<ActivePowerReading>(
                _fullData.Select(reading => new ActivePowerReading
                {
                    Start_Timestamp = reading.Start_Timestamp,
                    End_Timestamp = reading.End_Timestamp,
                    P_L1 = reading.P_L1,
                    P_L2 = reading.P_L2,
                    P_L3 = reading.P_L3
                })
            );
                ActivePowerCanExport = true;
            }
        }

        private void ActivePowerExportToCsv(object obj)
        {
            try
            {
                string startDate = ActivePowerStartDate?.ToString("dd_MM_yy");
                string endDate = ActivePowerEndDate?.ToString("dd_MM_yy");
                string filePath = $"active_power_readings_from_{startDate}_to_{endDate}.csv";
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Start Timestamp,End Timestamp,P_L1,P_L2,P_L3");
                foreach (var reading in ActivePowerReadings)
                {
                    csvContent.AppendLine($"{reading.Start_Timestamp},{reading.End_Timestamp},{reading.P_L1},{reading.P_L2},{reading.P_L3}");
                }

                File.WriteAllText(filePath, csvContent.ToString());
                System.Windows.MessageBox.Show("Data exported successfully to " + filePath, "Export Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error exporting data: " + ex.Message, "Export Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        // Reactive
        private async Task LoadReactivePowerData()
        {
            await GetFullData(ReactivePowerStartDate, ReactivePowerEndDate, ReactivePowerFrequency);

            if (_fullData != null)
            {
                ReactivePowerReadings = new ObservableCollection<ReactivePowerReading>(
                _fullData.Select(reading => new ReactivePowerReading
                {
                    Start_Timestamp = reading.Start_Timestamp,
                    End_Timestamp = reading.End_Timestamp,
                    VARQ1_L1 = reading.VARQ1_L1,
                    VARQ1_L2 = reading.VARQ1_L2,
                    VARQ1_L3 = reading.VARQ1_L3
                })
            );
                ReactivePowerCanExport = true;
            }
        }

        private void ReactivePowerExportToCsv(object obj)
        {
            try
            {
                string startDate = ReactivePowerStartDate?.ToString("dd_MM_yy");
                string endDate = ReactivePowerEndDate?.ToString("dd_MM_yy");
                string filePath = $"reactive_power_readings_from_{startDate}_to_{endDate}.csv";
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Start Timestamp,End Timestamp,VARQ1_L1,VARQ1_L2,VARQ1_L3");
                foreach (var reading in ReactivePowerReadings)
                {
                    csvContent.AppendLine($"{reading.Start_Timestamp},{reading.End_Timestamp},{reading.VARQ1_L1},{reading.VARQ1_L2},{reading.VARQ1_L3}");
                }

                File.WriteAllText(filePath, csvContent.ToString());
                System.Windows.MessageBox.Show("Data exported successfully to " + filePath, "Export Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error exporting data: " + ex.Message, "Export Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        // Apparent
        private async Task LoadApparentPowerData()
        {
            await GetFullData(ApparentPowerStartDate, ApparentPowerEndDate, ApparentPowerFrequency);

            if (_fullData != null)
            {
                ApparentPowerReadings = new ObservableCollection<ApparentPowerReading>(
                _fullData.Select(reading => new ApparentPowerReading
                {
                    Start_Timestamp = reading.Start_Timestamp,
                    End_Timestamp = reading.End_Timestamp,
                    VA_L1 = reading.VA_L1,
                    VA_L2 = reading.VA_L2,
                    VA_L3 = reading.VA_L3
                })
            );
                ApparentPowerCanExport = true;
            }
        }

        private void ApparentPowerExportToCsv(object obj)
        {
            try
            {
                string startDate = ApparentPowerStartDate?.ToString("dd_MM_yy");
                string endDate = ApparentPowerEndDate?.ToString("dd_MM_yy");
                string filePath = $"apparent_power_readings_from_{startDate}_to_{endDate}.csv";
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Start Timestamp,End Timestamp,VA_L1,VA_L2,VA_L3");
                foreach (var reading in ApparentPowerReadings)
                {
                    csvContent.AppendLine($"{reading.Start_Timestamp},{reading.End_Timestamp},{reading.VA_L1},{reading.VA_L2},{reading.VA_L3}");
                }

                File.WriteAllText(filePath, csvContent.ToString());
                System.Windows.MessageBox.Show("Data exported successfully to " + filePath, "Export Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error exporting data: " + ex.Message, "Export Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        // Power Factor
        private async Task LoadPowerFactorData()
        {
            await GetFullData(PowerFactorStartDate, PowerFactorEndDate, PowerFactorFrequency);

            if (_fullData != null)
            {
                PowerFactorReadings = new ObservableCollection<PowerFactorReading>(
                _fullData.Select(reading => new PowerFactorReading
                {
                    Start_Timestamp = reading.Start_Timestamp,
                    End_Timestamp = reading.End_Timestamp,
                    COS1_L1 = reading.COS1_L1,
                    COS1_L2 = reading.COS1_L2,
                    COS1_L3 = reading.COS1_L3
                })
            );
                PowerFactorCanExport = true;
            }
        }

        private void PowerFactorExportToCsv(object obj)
        {
            try
            {
                string startDate = PowerFactorStartDate?.ToString("dd_MM_yy");
                string endDate = PowerFactorEndDate?.ToString("dd_MM_yy");
                string filePath = $"power_factor_readings_from_{startDate}_to_{endDate}.csv";
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Start Timestamp,End Timestamp,COS1_L1,COS1_L2,COS1_L3");
                foreach (var reading in PowerFactorReadings)
                {
                    csvContent.AppendLine($"{reading.Start_Timestamp},{reading.End_Timestamp},{reading.COS1_L1},{reading.COS1_L2},{reading.COS1_L3}");
                }

                File.WriteAllText(filePath, csvContent.ToString());
                System.Windows.MessageBox.Show("Data exported successfully to " + filePath, "Export Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error exporting data: " + ex.Message, "Export Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        // Singleton desgin pattern
        public static PowerVM Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PowerVM();
                }
                return _instance;
            }
        }
    }
}
