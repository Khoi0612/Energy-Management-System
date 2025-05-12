using EMS.Core;
using EMS.Core.Data;
using EMS.Core.Models;
using EMS.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace EMS.MVVM.ViewModel
{
    internal class BaseVM : INotifyPropertyChanged
    {
        // Chart fields
        protected const int WindowSize = 10;
        private DateTime _startTime;
        private double _minValue;
        private double _maxValue;

        // Report fields
        private DateTime _startDate;
        private DateTime _endDate;
        private string _frequency;
        protected ObservableCollection<EnergyReading> _fullData;
        public event PropertyChangedEventHandler PropertyChanged;

        // Warning fields
        protected static readonly SolidColorBrush HighStatusColor = new SolidColorBrush(Color.FromRgb(0xFF, 0x57, 0x22));
        protected static readonly SolidColorBrush LowStatusColor = new SolidColorBrush(Color.FromRgb(0x21, 0x96, 0xF3));
        protected static readonly SolidColorBrush NormalStatusColor = new SolidColorBrush(Color.FromRgb(0xFF, 0xEB, 0x3B));

        // Setting field
        private SettingsModel _settings;

        public BaseVM()
        {
            _startTime = DateTime.Now;
            _minValue = 0;
            _maxValue = WindowSize;
            Settings = SettingsModel.Instance;

            ReportCommand = new AsyncRelayCommand(LoadFullData);
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Running window properties
        public DateTime StartTime
        {
            get => _startTime;
            set { _startTime = value; OnPropertyChanged(); }
        }

        public double MinValue
        {
            get => _minValue;
            set { _minValue = value; OnPropertyChanged(); }
        }

        public double MaxValue
        {
            get => _maxValue;
            set { _maxValue = value; OnPropertyChanged(); }
        }

        // Report properties
        public ObservableCollection<string> Frequencies { get; } = new ObservableCollection<string>
        {
            "Weekly",
            "Monthly",
            "Yearly"
        };

        public DateTime StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(); }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(); }
        }

        public string Frequency
        {
            get => _frequency;
            set { _frequency = value; OnPropertyChanged(); }
        }

        public ObservableCollection<EnergyReading> FullData
        {
            get => _fullData;
            set
            {
                _fullData = value;
                OnPropertyChanged();
            }
        }

        // Setting property
        public SettingsModel Settings { get => _settings; set => _settings = value; }

        // Command
        public ICommand ReportCommand { get; set; }

        // Report functions
        protected async Task GetFullData(DateTime? startDate, DateTime? endDate, string frequency)
        {

            if (string.IsNullOrWhiteSpace(frequency) || startDate == null || endDate == null)
            {
                MessageBox.Show("Please select a start date, end dare, and frequency.");
                return;
            }

            if (startDate > endDate)
            {
                MessageBox.Show("Start date cannot be later than end date.");
                return;
            }

            // Retrieve fullData from the database
            EnergyReadingRepository repository = new EnergyReadingRepository();
            _fullData = await repository.GetReadingsAsync(startDate, endDate, frequency);

            if (_fullData == null || _fullData.Count == 0)
            {
                MessageBox.Show("No data found for the selected date range and frequency.");
                return;
            }
        }

        private async Task LoadFullData()
        {
            await GetFullData(StartDate, EndDate, Frequency);
            bool empty = FullData == null;
            MessageBox.Show(empty.ToString());
        }

        // Warning function
        protected SolidColorBrush UpdateStatusColor(string category)
        {
            switch (category)
            {
                case "High":
                    return HighStatusColor;
                case "Low":
                    return LowStatusColor;
                case "Normal":
                default:
                    return NormalStatusColor;
            }
        }

    }
}
