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
    internal class VoltageVM : BaseVM
    {
        private readonly VoltageModel _voltageModel;
        private static VoltageVM _instance; // Initiated an instance

        // Voltage fields
        // Phase
        private double _phaseVoltage_L1;
        private double _phaseVoltage_L2;
        private double _phaseVoltage_L3;

        // Line
        private double _lineVoltage_L12;
        private double _lineVoltage_L23;
        private double _lineVoltage_L31;

        // Chart fields
        // Phase
        private double _phaseVoltageChartTypeIndex;
        private object _phaseVoltageChart;

        // Line
        private double _lineVoltageChartTypeIndex;
        private object _lineVoltageChart;

        // Report fields
        // Phase
        private DateTime? _phaseVoltageStartDate;
        private DateTime? _phaseVoltageEndDate;
        private string _phaseVoltageFrequency;
        private bool _phaseVoltageCanExport;
        private ObservableCollection<PhaseVoltageReading> _phaseVoltageReadings;

        // Line
        private DateTime? _lineVoltageStartDate;
        private DateTime? _lineVoltageEndDate;
        private string _lineVoltageFrequency;
        private bool _lineVoltageCanExport;
        private ObservableCollection<LineVoltageReading> _lineVoltageReadings;

        // Warning fields
        // Phase
        private SolidColorBrush _phaseVoltageStatusColor_L1;
        private SolidColorBrush _phaseVoltageStatusColor_L2;
        private SolidColorBrush _phaseVoltageStatusColor_L3;

        // Line
        private SolidColorBrush _lineVoltageStatusColor_L12;
        private SolidColorBrush _lineVoltageStatusColor_L23;
        private SolidColorBrush _lineVoltageStatusColor_L31;

        private VoltageVM()
        {
            _voltageModel = new VoltageModel();

            // Subscribed to Voltage change events
            // Phase
            _voltageModel.PhaseVoltage_L1_Changed += OnPhaseVoltage_L1_Changed;
            _voltageModel.PhaseVoltage_L2_Changed += OnPhaseVoltage_L2_Changed;
            _voltageModel.PhaseVoltage_L3_Changed += OnPhaseVoltage_L3_Changed;

            // Line
            _voltageModel.LineVoltage_L12_Changed += OnLineVoltage_L12_Changed;
            _voltageModel.LineVoltage_L23_Changed += OnLineVoltage_L23_Changed;
            _voltageModel.LineVoltage_L31_Changed += OnLineVoltage_L31_Changed;

            // Initiated commands
            // Phase
            PlotPhaseVoltageCommand = new RelayCommand(PlotPhaseVoltageChart);
            SwitchPhaseVoltageCommand = new RelayCommand(SwitchPhaseVoltageChartType);
            StopPlottingPhaseVoltageCommand = new RelayCommand(StopPlottingPhaseVoltageChart);
            LoadReportPhaseVoltageCommand = new AsyncRelayCommand(LoadPhaseVoltageData);
            ExportReportPhaseVoltageCommand = new RelayCommand(PhaseVoltageExportToCsv);

            // Line
            PlotLineVoltageCommand = new RelayCommand(PlotLineVoltageChart);
            SwitchLineVoltageCommand = new RelayCommand(SwitchLineVoltageChartType);
            StopPlottingLineVoltageCommand = new RelayCommand(StopPlottingLineVoltageChart);
            LoadReportLineVoltageCommand = new AsyncRelayCommand(LoadLineVoltageData);
            ExportReportLineVoltageCommand = new RelayCommand(LineVoltageExportToCsv);

            // Initiated time elapsed
            TimeElapsed = value => TimeSpan.FromSeconds(value).ToString(@"mm\:ss");

            // Initiated Voltage chart values
            // Phase
            PhaseVoltageValues_L1 = new ChartValues<double>();
            PhaseVoltageValues_L2 = new ChartValues<double>();
            PhaseVoltageValues_L3 = new ChartValues<double>();

            // Line
            LineVoltageValues_L12 = new ChartValues<double>();
            LineVoltageValues_L23 = new ChartValues<double>();
            LineVoltageValues_L31 = new ChartValues<double>();


            // Initiated Voltage report fields
            // Phase
            _phaseVoltageCanExport = false;
            PhaseVoltageStartDate = null;
            PhaseVoltageEndDate = null;

            // Line
            _lineVoltageCanExport = false;
            LineVoltageStartDate = null;
            LineVoltageEndDate = null;

            // Invoked warning events
            // Phase
            _voltageModel.PhaseVoltage_L1_WarningTriggered += (warning) =>
            {
                PhaseVoltage_L1_WarningTriggered?.Invoke(warning);
            };

            _voltageModel.PhaseVoltage_L2_WarningTriggered += (warning) =>
            {
                PhaseVoltage_L2_WarningTriggered?.Invoke(warning);
            };

            _voltageModel.PhaseVoltage_L3_WarningTriggered += (warning) =>
            {
                PhaseVoltage_L3_WarningTriggered?.Invoke(warning);
            };

            // Line
            _voltageModel.LineVoltage_L12_WarningTriggered += (warning) =>
            {
                LineVoltage_L12_WarningTriggered?.Invoke(warning);
            };

            _voltageModel.LineVoltage_L23_WarningTriggered += (warning) =>
            {
                LineVoltage_L23_WarningTriggered?.Invoke(warning);
            };

            _voltageModel.LineVoltage_L31_WarningTriggered += (warning) =>
            {
                LineVoltage_L31_WarningTriggered?.Invoke(warning);
            };
        }

        // Warning events
        // Phase
        public event Action<WarningModel> PhaseVoltage_L1_WarningTriggered;
        public event Action<WarningModel> PhaseVoltage_L2_WarningTriggered;
        public event Action<WarningModel> PhaseVoltage_L3_WarningTriggered;

        // Line
        public event Action<WarningModel> LineVoltage_L12_WarningTriggered;
        public event Action<WarningModel> LineVoltage_L23_WarningTriggered;
        public event Action<WarningModel> LineVoltage_L31_WarningTriggered;

        // Phase Voltage Properties
        public double PhaseVoltage_L1
        {
            get => _phaseVoltage_L1;
            set { _phaseVoltage_L1 = value; OnPropertyChanged(); }
        }

        public double PhaseVoltage_L2
        {
            get => _phaseVoltage_L2;
            set { _phaseVoltage_L2 = value; OnPropertyChanged(); }
        }

        public double PhaseVoltage_L3
        {
            get => _phaseVoltage_L3;
            set { _phaseVoltage_L3 = value; OnPropertyChanged(); }
        }

        // Line Voltage Properties
        public double LineVoltage_L12
        {
            get => _lineVoltage_L12;
            set { _lineVoltage_L12 = value; OnPropertyChanged(); }
        }

        public double LineVoltage_L23
        {
            get => _lineVoltage_L23;
            set { _lineVoltage_L23 = value; OnPropertyChanged(); }
        }

        public double LineVoltage_L31
        {
            get => _lineVoltage_L31;
            set { _lineVoltage_L31 = value; OnPropertyChanged(); }
        }

        // Chart properties
        // Time elaspased
        public Func<double, string> TimeElapsed { get; set; }

        // Phase
        public ChartValues<double> PhaseVoltageValues_L1 { get; set; }
        public ChartValues<double> PhaseVoltageValues_L2 { get; set; }
        public ChartValues<double> PhaseVoltageValues_L3 { get; set; }

        public object PhaseVoltageChart
        {
            get => _phaseVoltageChart;
            set { _phaseVoltageChart = value; OnPropertyChanged(); }
        }

        public double PhaseVoltageMin
        {
            get => this.Settings.PhaseVoltageMin;
        }

        public double PhaseVoltageMax
        {
            get => this.Settings.PhaseVoltageMax;
        }

        // Line
        public ChartValues<double> LineVoltageValues_L12 { get; set; }
        public ChartValues<double> LineVoltageValues_L23 { get; set; }
        public ChartValues<double> LineVoltageValues_L31 { get; set; }

        public object LineVoltageChart
        {
            get => _lineVoltageChart;
            set { _lineVoltageChart = value; OnPropertyChanged(); }
        }

        public double LineVoltageMin
        {
            get => this.Settings.LineVoltageMin;
        }

        public double LineVoltageMax
        {
            get => this.Settings.LineVoltageMax;
        }

        // Report properties
        // Phase
        public DateTime? PhaseVoltageStartDate
        {
            get => _phaseVoltageStartDate;
            set { _phaseVoltageStartDate = value; OnPropertyChanged(); }
        }

        public DateTime? PhaseVoltageEndDate
        {
            get => _phaseVoltageEndDate;
            set { _phaseVoltageEndDate = value; OnPropertyChanged(); }
        }

        public string PhaseVoltageFrequency
        {
            get => _phaseVoltageFrequency;
            set { _phaseVoltageFrequency = value; OnPropertyChanged(); }
        }

        public bool PhaseVoltageCanExport
        {
            get => _phaseVoltageCanExport;
            set
            {
                _phaseVoltageCanExport = value; OnPropertyChanged();
            }
        }

        public ObservableCollection<PhaseVoltageReading> PhaseVoltageReadings
        {
            get => _phaseVoltageReadings;
            set
            {
                _phaseVoltageReadings = value;
                OnPropertyChanged();
            }
        }

        // Line
        public DateTime? LineVoltageStartDate
        {
            get => _lineVoltageStartDate;
            set { _lineVoltageStartDate = value; OnPropertyChanged(); }
        }

        public DateTime? LineVoltageEndDate
        {
            get => _lineVoltageEndDate;
            set { _lineVoltageEndDate = value; OnPropertyChanged(); }
        }

        public string LineVoltageFrequency
        {
            get => _lineVoltageFrequency;
            set { _lineVoltageFrequency = value; OnPropertyChanged(); }
        }

        public bool LineVoltageCanExport
        {
            get => _lineVoltageCanExport;
            set
            {
                _lineVoltageCanExport = value; OnPropertyChanged();
            }
        }

        public ObservableCollection<LineVoltageReading> LineVoltageReadings
        {
            get => _lineVoltageReadings;
            set
            {
                _lineVoltageReadings = value;
                OnPropertyChanged();
            }
        }

        // Warning Properties
        // Phase
        public SolidColorBrush PhaseVoltageStatusColor_L1
        {
            get => _phaseVoltageStatusColor_L1;
            set
            {
                _phaseVoltageStatusColor_L1 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush PhaseVoltageStatusColor_L2
        {
            get => _phaseVoltageStatusColor_L2;
            set
            {
                _phaseVoltageStatusColor_L2 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush PhaseVoltageStatusColor_L3
        {
            get => _phaseVoltageStatusColor_L3;
            set
            {
                _phaseVoltageStatusColor_L3 = value;
                OnPropertyChanged();
            }
        }

        // Linee
        public SolidColorBrush LineVoltageStatusColor_L12
        {
            get => _lineVoltageStatusColor_L12;
            set
            {
                _lineVoltageStatusColor_L12 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush LineVoltageStatusColor_L23
        {
            get => _lineVoltageStatusColor_L23;
            set
            {
                _lineVoltageStatusColor_L23 = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush LineVoltageStatusColor_L31
        {
            get => _lineVoltageStatusColor_L31;
            set
            {
                _lineVoltageStatusColor_L31 = value;
                OnPropertyChanged();
            }
        }

        // Commands
        // Phase
        public ICommand PlotPhaseVoltageCommand { get; }
        public ICommand SwitchPhaseVoltageCommand { get; }
        public ICommand StopPlottingPhaseVoltageCommand { get; }
        public ICommand LoadReportPhaseVoltageCommand { get; set; }
        public ICommand ExportReportPhaseVoltageCommand { get; set; }

        // Line 
        public ICommand PlotLineVoltageCommand { get; }
        public ICommand SwitchLineVoltageCommand { get; }
        public ICommand StopPlottingLineVoltageCommand { get; }
        public ICommand LoadReportLineVoltageCommand { get; set; }
        public ICommand ExportReportLineVoltageCommand { get; set; }

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

        // On Voltage change functions
        private void OnValueChanged(ChartValues<double> chartValue, double currentValue, double newValue)
        {
            chartValue.Add(currentValue);
            RunningWindowHandler(chartValue);
            currentValue = newValue;
        }

        // Phase
        private void OnPhaseVoltage_L1_Changed(double newValue)
        {
            PhaseVoltageStatusColor_L1 = UpdateStatusColor(_voltageModel.PhaseVoltageWarning_L1.Category);
            OnValueChanged(PhaseVoltageValues_L1, PhaseVoltage_L1, newValue);
        }

        private void OnPhaseVoltage_L2_Changed(double newValue)
        {
            PhaseVoltageStatusColor_L2 = UpdateStatusColor(_voltageModel.PhaseVoltageWarning_L2.Category);
            OnValueChanged(PhaseVoltageValues_L2, PhaseVoltage_L2, newValue);
        }

        private void OnPhaseVoltage_L3_Changed(double newValue)
        {
            PhaseVoltageStatusColor_L3 = UpdateStatusColor(_voltageModel.PhaseVoltageWarning_L3.Category);
            OnValueChanged(PhaseVoltageValues_L3, PhaseVoltage_L3, newValue);
        }

        // Line
        private void OnLineVoltage_L12_Changed(double newValue)
        {
            LineVoltageStatusColor_L12 = UpdateStatusColor(_voltageModel.LineVoltageWarning_L12.Category);
            OnValueChanged(LineVoltageValues_L12, LineVoltage_L12, newValue);
        }

        private void OnLineVoltage_L23_Changed(double newValue)
        {
            LineVoltageStatusColor_L23 = UpdateStatusColor(_voltageModel.LineVoltageWarning_L23.Category);
            OnValueChanged(LineVoltageValues_L23, LineVoltage_L23, newValue);
        }

        private void OnLineVoltage_L31_Changed(double newValue)
        {
            LineVoltageStatusColor_L31 = UpdateStatusColor(_voltageModel.LineVoltageWarning_L31.Category);
            OnValueChanged(LineVoltageValues_L31, LineVoltage_L31, newValue);
        }

        // Voltage charts functions
        // Phase
        private void PlotPhaseVoltageLineChart()
        {
            PhaseVoltageChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = PhaseVoltageValues_L1,
                        Title = "V_L1"
                    },
                    new LineSeries
                    {
                        Values = PhaseVoltageValues_L2,
                        Title = "V_L2"
                    },
                    new LineSeries
                    {
                        Values = PhaseVoltageValues_L3,
                        Title = "V_L3"
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
                        Title = "Phase Voltage (V)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotPhaseVoltageBarChart()
        {
            PhaseVoltageChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Values = PhaseVoltageValues_L1,
                        Title = "V_L1"
                    },
                    new ColumnSeries
                    {
                        Values = PhaseVoltageValues_L2,
                        Title = "V_L2"
                    },
                    new ColumnSeries
                    {
                        Values = PhaseVoltageValues_L3,
                        Title = "V_L3"
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
                        Title = "Phase Voltage (V)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotPhaseVoltageScatterChart()
        {
            PhaseVoltageChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new ScatterSeries
                    {
                        Values = PhaseVoltageValues_L1,
                        Title = "V_L1"
                    },
                    new ScatterSeries
                    {
                        Values = PhaseVoltageValues_L2,
                        Title = "V_L2"
                    },
                    new ScatterSeries
                    {
                        Values = PhaseVoltageValues_L3,
                        Title = "V_L3"
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
                        Title = "Phase Voltage (V)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotPhaseVoltageChart(object obj)
        {
            PlotPhaseVoltageLineChart();
        }

        private void SwitchPhaseVoltageChartType(object obj)
        {
            _phaseVoltageChartTypeIndex = (_phaseVoltageChartTypeIndex + 1) % 3;

            switch (_phaseVoltageChartTypeIndex)
            {
                case 0: // Line Chart
                    PlotPhaseVoltageLineChart();
                    break;

                case 1: // Bar Chart
                    PlotPhaseVoltageBarChart();
                    break;

                case 2: // Scatter Chart
                    PlotPhaseVoltageScatterChart();
                    break;
            }
        }

        private void StopPlottingPhaseVoltageChart(object obj)
        {
            PhaseVoltageChart = null;
        }

        // Line
        private void PlotLineVoltageLineChart()
        {
            LineVoltageChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = LineVoltageValues_L12,
                        Title = "V_L12"
                    },
                    new LineSeries
                    {
                        Values = LineVoltageValues_L23,
                        Title = "V_L23"
                    },
                    new LineSeries
                    {
                        Values = LineVoltageValues_L31,
                        Title = "V_L31"
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
                        Title = "Phase Voltage (V)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotLineVoltageBarChart()
        {
            LineVoltageChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Values = LineVoltageValues_L12,
                        Title = "V_L12"
                    },
                    new ColumnSeries
                    {
                        Values = LineVoltageValues_L23,
                        Title = "V_L23"
                    },
                    new ColumnSeries
                    {
                        Values = LineVoltageValues_L31,
                        Title = "V_L31"
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
                        Title = "Phase Voltage (V)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotLineVoltageScatterChart()
        {
            LineVoltageChart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new ScatterSeries
                    {
                        Values = LineVoltageValues_L12,
                        Title = "V_L12"
                    },
                    new ScatterSeries
                    {
                        Values = LineVoltageValues_L23,
                        Title = "V_L23"
                    },
                    new ScatterSeries
                    {
                        Values = LineVoltageValues_L31,
                        Title = "V_L31"
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
                        Title = "Phase Voltage (V)"
                    }
                },
                LegendLocation = LegendLocation.Right,

            };
        }

        private void PlotLineVoltageChart(object obj)
        {
            PlotLineVoltageLineChart();
        }

        private void SwitchLineVoltageChartType(object obj)
        {
            _lineVoltageChartTypeIndex = (_lineVoltageChartTypeIndex + 1) % 3;

            switch (_lineVoltageChartTypeIndex)
            {
                case 0: // Line Chart
                    PlotLineVoltageLineChart();
                    break;

                case 1: // Bar Chart
                    PlotLineVoltageBarChart();
                    break;

                case 2: // Scatter Chart
                    PlotLineVoltageScatterChart();
                    break;
            }
        }

        private void StopPlottingLineVoltageChart(object obj)
        {
            LineVoltageChart = null;
        }

        // Voltage report functions
        // Phase
        private async Task LoadPhaseVoltageData()
        {
            await GetFullData(PhaseVoltageStartDate, PhaseVoltageEndDate, PhaseVoltageFrequency);

            if (_fullData != null)
            {
                PhaseVoltageReadings = new ObservableCollection<PhaseVoltageReading>(
                _fullData.Select(reading => new PhaseVoltageReading
                {
                    Start_Timestamp = reading.Start_Timestamp,
                    End_Timestamp = reading.End_Timestamp,
                    V_L1 = reading.V_L1,
                    V_L2 = reading.V_L2,
                    V_L3 = reading.V_L3
                })
            );
                PhaseVoltageCanExport = true;
            }

        }

        private void PhaseVoltageExportToCsv(object obj)
        {
            try
            {
                string startDate = PhaseVoltageStartDate?.ToString("dd_MM_yy");
                string endDate = PhaseVoltageEndDate?.ToString("dd_MM_yy");
                string filePath = $"Phase_voltage_readings_from_{startDate}_to_{endDate}.csv";
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Start Timestamp,End Timestamp,V_L1,V_L2,V_L3");
                foreach (var reading in PhaseVoltageReadings)
                {
                    csvContent.AppendLine($"{reading.Start_Timestamp},{reading.End_Timestamp},{reading.V_L1},{reading.V_L2},{reading.V_L3}");
                }

                File.WriteAllText(filePath, csvContent.ToString());
                System.Windows.MessageBox.Show("Data exported successfully to " + filePath, "Export Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error exporting data: " + ex.Message, "Export Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        // Line
        private async Task LoadLineVoltageData()
        {
            await GetFullData(LineVoltageStartDate, LineVoltageEndDate, LineVoltageFrequency);

            if (_fullData != null)
            {
                LineVoltageReadings = new ObservableCollection<LineVoltageReading>(
                _fullData.Select(reading => new LineVoltageReading
                {
                    Start_Timestamp = reading.Start_Timestamp,
                    End_Timestamp = reading.End_Timestamp,
                    V_L12 = reading.V_L12,
                    V_L23 = reading.V_L23,
                    V_L31 = reading.V_L31
                })
            );
                LineVoltageCanExport = true;
            }
        }

        private void LineVoltageExportToCsv(object obj)
        {
            try
            {
                string startDate = LineVoltageStartDate?.ToString("dd_MM_yy");
                string endDate = LineVoltageEndDate?.ToString("dd_MM_yy");
                string filePath = $"Line_voltage_readings_from_{startDate}_to_{endDate}.csv";
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Start Timestamp,End Timestamp,V_L12,V_L23,V_L31");
                foreach (var reading in LineVoltageReadings)
                {
                    csvContent.AppendLine($"{reading.Start_Timestamp},{reading.End_Timestamp},{reading.V_L12},{reading.V_L23},{reading.V_L31}");
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
        public static VoltageVM Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new VoltageVM();
                }
                return _instance;
            }
        }
    }
}
