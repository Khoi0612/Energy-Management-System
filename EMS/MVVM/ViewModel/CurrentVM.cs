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
    internal class CurrentVM : BaseVM
    {
        private readonly CurrentModel _currentModel; // Model field
        private static CurrentVM _instance; // Initiated an instance

        // Current fields
        private double _current_L1;
        private double _current_L2;
        private double _current_L3;

        // Chart fields
        private double _currentChartTypeIndex;
        private object _currentChart;

        // Report fields
        private DateTime? _currentStartDate;
        private DateTime? _currentEndDate;
        private string _currentFrequency;
        private bool _currentCanExport;
        private ObservableCollection<CurrentReading> _currentReadings;

        // Warning fields
        private SolidColorBrush _currentStatusColor_L1;
        private SolidColorBrush _currentStatusColor_L2;
        private SolidColorBrush _currentStatusColor_L3;

        private CurrentVM()
        {
            _currentModel = new CurrentModel();

            // Subscribed to Current change events
            _currentModel.Current_L1_Changed += OnCurrent_L1_Changed;
            _currentModel.Current_L2_Changed += OnCurrent_L2_Changed;
            _currentModel.Current_L3_Changed += OnCurrent_L3_Changed;

            // Initiated commands
            PlotCurrentCommand = new RelayCommand(PlotCurrentChart);
            SwitchCurrentCommand = new RelayCommand(SwitchCurrentChartType);
            StopPlottingCurrentCommand = new RelayCommand(StopPlottingCurrentChart);
            LoadReportCurrentCommand = new AsyncRelayCommand(LoadCurrentData);
            ExportReportCurrentCommand = new RelayCommand(CurrentExportToCsv);

            // Initiated time elapsed
            TimeElapsed = value => TimeSpan.FromSeconds(value).ToString(@"mm\:ss");

            // Initated chart index
            _currentChartTypeIndex = 0;

            // Initiated Current chart values
            CurrentValues_L1 = new ChartValues<double>();
            CurrentValues_L2 = new ChartValues<double>();
            CurrentValues_L3 = new ChartValues<double>();

            // Initiated Current report fields
            _currentCanExport = false;
            CurrentStartDate = null;
            CurrentEndDate = null;

            // Invoked warning events
            _currentModel.Current_L1_WarningTriggered += (warning) =>
            {
                Current_L1_WarningTriggered?.Invoke(warning);
            };

            _currentModel.Current_L2_WarningTriggered += (warning) =>
            {
                Current_L2_WarningTriggered?.Invoke(warning);
            };

            _currentModel.Current_L3_WarningTriggered += (warning) =>
            {
                Current_L3_WarningTriggered?.Invoke(warning);
            };
        }

        // Warning events
        public event Action<WarningModel> Current_L1_WarningTriggered;
        public event Action<WarningModel> Current_L2_WarningTriggered;
        public event Action<WarningModel> Current_L3_WarningTriggered;

        // Current Properties
        public double Current_L1
        {
            get => _current_L1;
            set { _current_L1 = value; OnPropertyChanged(); }
        }

        public double Current_L2
        {
            get => _current_L2;
            set { _current_L2 = value; OnPropertyChanged(); }
        }

        public double Current_L3
        {
            get => _current_L3;
            set { _current_L3 = value; OnPropertyChanged(); }
        }

        // Chart Properties
        public Func<double, string> TimeElapsed { get; set; }
        public ChartValues<double> CurrentValues_L1 { get; set; }
        public ChartValues<double> CurrentValues_L2 { get; set; }
        public ChartValues<double> CurrentValues_L3 { get; set; }

        public object CurrentChart
        {
            get => _currentChart;
            set { _currentChart = value; OnPropertyChanged(); }
        }

        public double CurrentMin
        {
            get => this.Settings.CurrentMin;
        }

        public double CurrentMax
        {
            get => this.Settings.CurrentMax;
        }

        // Report Properties
        public DateTime? CurrentStartDate
        {
            get => _currentStartDate;
            set { _currentStartDate = value; OnPropertyChanged(); }
        }

        public DateTime? CurrentEndDate
        {
            get => _currentEndDate;
            set { _currentEndDate = value; OnPropertyChanged(); }
        }

        public string CurrentFrequency
        {
            get => _currentFrequency;
            set { _currentFrequency = value; OnPropertyChanged(); }
        }

        public bool CurrentCanExport
        {
            get => _currentCanExport;
            set
            {
                _currentCanExport = value; OnPropertyChanged();
            }
        }

        public ObservableCollection<CurrentReading> CurrentReadings
        {
            get => _currentReadings;
            set
            {
                _currentReadings = value;
                OnPropertyChanged();
            }
        }

        // Warning Properties
        public SolidColorBrush CurrentStatusColor_L1
        {
            get => _currentStatusColor_L1;
            set
            {
                _currentStatusColor_L1 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush CurrentStatusColor_L2
        {
            get => _currentStatusColor_L2;
            set
            {
                _currentStatusColor_L2 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush CurrentStatusColor_L3
        {
            get => _currentStatusColor_L3;
            set
            {
                _currentStatusColor_L3 = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand PlotCurrentCommand { get; }
        public ICommand SwitchCurrentCommand { get; }
        public ICommand StopPlottingCurrentCommand { get; }
        public ICommand LoadReportCurrentCommand { get; set; }
        public ICommand ExportReportCurrentCommand { get; set; }

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

        // On Current Changes functions
        private void OnValueChanged(ChartValues<double> chartValue, double currentValue, double newValue, string phase)
        {
            chartValue.Add(currentValue);
            RunningWindowHandler(chartValue);
            currentValue = newValue;

        }

        public void OnCurrent_L1_Changed(double newValue)
        {
            CurrentStatusColor_L1 = UpdateStatusColor(_currentModel.CurrentWarning_L1.Category);
            OnValueChanged(CurrentValues_L1, Current_L1, newValue, "L1");

        }

        public void OnCurrent_L2_Changed(double newValue)
        {
            CurrentStatusColor_L2 = UpdateStatusColor(_currentModel.CurrentWarning_L2.Category);
            OnValueChanged(CurrentValues_L2, Current_L2, newValue, "L2");
        }

        public void OnCurrent_L3_Changed(double newValue)
        {
            CurrentStatusColor_L3 = UpdateStatusColor(_currentModel.CurrentWarning_L3.Category);
            OnValueChanged(CurrentValues_L3, Current_L3, newValue, "L3");
        }

        // Chart functions
        private void PlotCurrentLineChart()
        {
            CurrentChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = CurrentValues_L1,
                        Title = "I_L1"
                    },
                    new LineSeries
                    {
                        Values = CurrentValues_L2,
                        Title = "I_L2"
                    },
                    new LineSeries
                    {
                        Values = CurrentValues_L3,
                        Title = "I_L3"
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
                        Title = "Current (A)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotCurrentBarChart()
        {
            CurrentChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Values = CurrentValues_L1,
                        Title = "I_L1"
                    },
                    new ColumnSeries
                    {
                        Values = CurrentValues_L2,
                        Title = "I_L2"
                    },
                    new ColumnSeries
                    {
                        Values = CurrentValues_L3,
                        Title = "I_L3"
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
                        Title = "Current (A)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotCurrentScatterChart()
        {
            CurrentChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Values = CurrentValues_L1,
                        Title = "I_L1"
                    },
                    new ColumnSeries
                    {
                        Values = CurrentValues_L2,
                        Title = "I_L2"
                    },
                    new ColumnSeries
                    {
                        Values = CurrentValues_L3,
                        Title = "I_L3"
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
                        Title = "Current (A)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotCurrentChart(object obj)
        {
            PlotCurrentLineChart();
        }

        private void SwitchCurrentChartType(object obj)
        {
            // Rotate through Line, Bar, and Scatter
            _currentChartTypeIndex = (_currentChartTypeIndex + 1) % 3;

            switch (_currentChartTypeIndex)
            {
                case 0: // Line Chart
                    PlotCurrentLineChart();
                    break;

                case 1: // Bar Chart
                    PlotCurrentBarChart();
                    break;

                case 2: // Scatter Chart
                    PlotCurrentScatterChart();
                    break;
            }
        }

        private void StopPlottingCurrentChart(object obj)
        {
            CurrentChart = null;
        }


        // Report functions
        private async Task LoadCurrentData()
        {
            await GetFullData(CurrentStartDate, CurrentEndDate, CurrentFrequency);

            if (_fullData != null)
            {
                CurrentReadings = new ObservableCollection<CurrentReading>(
                _fullData.Select(reading => new CurrentReading
                {
                    Start_Timestamp = reading.Start_Timestamp,
                    End_Timestamp = reading.End_Timestamp,
                    I_L1 = reading.I_L1,
                    I_L2 = reading.I_L2,
                    I_L3 = reading.I_L3
                })
            );
                CurrentCanExport = true;
            }
        }

        private void CurrentExportToCsv(object obj)
        {
            try
            {
                string startDate = CurrentStartDate?.ToString("dd_MM_yy");
                string endDate = CurrentEndDate?.ToString("dd_MM_yy");
                string filePath = $"Current_readings_from_{startDate}_to_{endDate}.csv";
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Start Timestamp,End Timestamp,I_L1,I_L2,I_L3");
                foreach (var reading in CurrentReadings)
                {
                    csvContent.AppendLine($"{reading.Start_Timestamp},{reading.End_Timestamp},{reading.I_L1},{reading.I_L2},{reading.I_L3}");
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
        public static CurrentVM Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CurrentVM();
                }
                return _instance;
            }
        }
    }
}
