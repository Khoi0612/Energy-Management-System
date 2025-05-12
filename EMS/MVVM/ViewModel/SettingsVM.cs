using EMS.Core;
using EMS.MVVM.Model;
using Newtonsoft.Json;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace EMS.MVVM.ViewModel
{
    internal class SettingsVM : BaseVM
    {
        private static SettingsVM _instance; // Initiated an instance

        private SettingsModel _settingsModel;
        private readonly string _settingsFilePath = "EMSSettings.json"; // File path

        // Threshold string fields
        // Current
        private string _currentLowerThresholdString;
        private string _currentUpperThresholdString;

        // Phase Voltage
        private string _phaseVoltageLowerThresholdString;
        private string _phaseVoltageUpperThresholdString;

        // Line Voltage
        private string _lineVoltageLowerThresholdString;
        private string _lineVoltageUpperThresholdString;

        // Active Power
        private string _activePowerLowerThresholdString;
        private string _activePowerUpperThresholdString;

        // Reactive Power
        private string _reactivePowerLowerThresholdString;
        private string _reactivePowerUpperThresholdString;

        // Apparent Power
        private string _apparentPowerLowerThresholdString;
        private string _apparentPowerUpperThresholdString;

        // Power Factor
        private string _powerFactorLowerThresholdString;
        private string _powerFactorUpperThresholdString;

        public SettingsVM()
        {
            _settingsModel = SettingsModel.Instance;
            SaveSettingsCommand = new RelayCommand(SaveSettings);
        }

        // Setting model property
        public SettingsModel SettingsModel { get => _settingsModel; set { _settingsModel = value; OnPropertyChanged(); } }

        // Threshold setting properties
        // Current
        public string CurrentLowerThresholdString { get => _currentLowerThresholdString; set { _currentLowerThresholdString = value; OnPropertyChanged(); } }
        public string CurrentUpperThresholdString { get => _currentUpperThresholdString; set { _currentUpperThresholdString = value; OnPropertyChanged(); } }

        // Phase Voltage
        public string PhaseVoltageLowerThresholdString { get => _phaseVoltageLowerThresholdString; set { _phaseVoltageLowerThresholdString = value; OnPropertyChanged(); } }
        public string PhaseVoltageUpperThresholdString { get => _phaseVoltageUpperThresholdString; set { _phaseVoltageUpperThresholdString = value; OnPropertyChanged(); } }

        // Line Voltage
        public string LineVoltageLowerThresholdString { get => _lineVoltageLowerThresholdString; set { _lineVoltageLowerThresholdString = value; OnPropertyChanged(); } }
        public string LineVoltageUpperThresholdString { get => _lineVoltageUpperThresholdString; set { _lineVoltageUpperThresholdString = value; OnPropertyChanged(); } }

        // Active Power
        public string ActivePowerLowerThresholdString { get => _activePowerLowerThresholdString; set { _activePowerLowerThresholdString = value; OnPropertyChanged(); } }
        public string ActivePowerUpperThresholdString { get => _activePowerUpperThresholdString; set { _activePowerUpperThresholdString = value; OnPropertyChanged(); } }

        // Reactive Power
        public string ReactivePowerLowerThresholdString { get => _reactivePowerLowerThresholdString; set { _reactivePowerLowerThresholdString = value; OnPropertyChanged(); } }
        public string ReactivePowerUpperThresholdString { get => _reactivePowerUpperThresholdString; set { _reactivePowerUpperThresholdString = value; OnPropertyChanged(); } }

        // Apparent Power
        public string ApparentPowerLowerThresholdString { get => _apparentPowerLowerThresholdString; set { _apparentPowerLowerThresholdString = value; OnPropertyChanged(); } }
        public string ApparentPowerUpperThresholdString { get => _apparentPowerUpperThresholdString; set { _apparentPowerUpperThresholdString = value; OnPropertyChanged(); } }

        // Power Factor
        public string PowerFactorLowerThresholdString { get => _powerFactorLowerThresholdString; set { _powerFactorLowerThresholdString = value; OnPropertyChanged(); } }
        public string PowerFactorUpperThresholdString { get => _powerFactorUpperThresholdString; set { _powerFactorUpperThresholdString = value; OnPropertyChanged(); } }

        // Command
        public ICommand SaveSettingsCommand { get; }

        // Parse and save users' setting to setting model, then save setting model to json file
        private void SaveSettings(object obj)
        {
            // Current
            if (double.TryParse(CurrentLowerThresholdString, out double currentLow))
            {
                _settingsModel.CurrentLowerThreshold = currentLow;
            }

            if (double.TryParse(CurrentUpperThresholdString, out double currentHigh))
            {
                _settingsModel.CurrentUpperThreshold = currentHigh;
            }

            // Phase Voltage
            if (double.TryParse(PhaseVoltageLowerThresholdString, out double phaseVoltageLow))
            {
                _settingsModel.PhaseVoltageLowerThreshold = phaseVoltageLow;
            }

            if (double.TryParse(PhaseVoltageUpperThresholdString, out double phaseVoltageHigh))
            {
                _settingsModel.PhaseVoltageUpperThreshold = phaseVoltageHigh;
            }

            // Line Voltage
            if (double.TryParse(LineVoltageLowerThresholdString, out double lineVoltageLow))
            {
                _settingsModel.LineVoltageLowerThreshold = lineVoltageLow;
            }

            if (double.TryParse(LineVoltageUpperThresholdString, out double lineVoltageHigh))
            {
                _settingsModel.LineVoltageUpperThreshold = lineVoltageHigh;
            }

            // Active Power
            if (double.TryParse(ActivePowerLowerThresholdString, out double activePowerLow))
            {
                _settingsModel.ActivePowerLowerThreshold = activePowerLow;
            }

            if (double.TryParse(ActivePowerUpperThresholdString, out double activePowerHigh))
            {
                _settingsModel.ActivePowerUpperThreshold = activePowerHigh;
            }

            // Reactive Power
            if (double.TryParse(ReactivePowerLowerThresholdString, out double reactivePowerLow))
            {
                _settingsModel.ReactivePowerLowerThreshold = reactivePowerLow;
            }

            if (double.TryParse(ReactivePowerUpperThresholdString, out double reactivePowerHigh))
            {
                _settingsModel.ReactivePowerUpperThreshold = reactivePowerHigh;
            }

            // Apparent Power
            if (double.TryParse(ApparentPowerLowerThresholdString, out double apparentPowerLow))
            {
                _settingsModel.ApparentPowerLowerThreshold = apparentPowerLow;
            }

            if (double.TryParse(ApparentPowerUpperThresholdString, out double apparentPowerHigh))
            {
                _settingsModel.ApparentPowerUpperThreshold = apparentPowerHigh;
            }

            // Power Factor
            if (double.TryParse(PowerFactorLowerThresholdString, out double powerFactorLow))
            {
                _settingsModel.PowerFactorLowerThreshold = powerFactorLow;
            }

            if (double.TryParse(PowerFactorUpperThresholdString, out double powerFactorHigh))
            {
                _settingsModel.PowerFactorUpperThreshold = powerFactorHigh;
            }

            SettingsModel.UpdateInstance(_settingsModel);
            string json = JsonConvert.SerializeObject(_settingsModel, Formatting.Indented);
            File.WriteAllText(_settingsFilePath, json);
            MessageBox.Show("Threshold settings saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Singleton design pattern
        public static SettingsVM Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SettingsVM();
                }
                return _instance;
            }
        }
    }
}
