namespace EMS.MVVM.Model
{
    internal class SettingsModel
    {
        private static SettingsModel _instance; // Initiated an instance
        private static readonly object _lock = new object(); // Ensure the instance can only be changed via UpdateInstance function

        // Parameter fields
        // Current
        private double _currentMin;
        private double _currentMax;
        private double _currentLowerThreshold;
        private double _currentUpperThreshold;

        // Phase Voltage
        private double _phaseVoltageMin;
        private double _phaseVoltageMax;
        private double _phaseVoltageLowerThreshold;
        private double _phaseVoltageUpperThreshold;

        // Line Voltage
        private double _lineVoltageMin;
        private double _lineVoltageMax;
        private double _lineVoltageLowerThreshold;
        private double _lineVoltageUpperThreshold;

        // Active Power
        private double _activePowerMin;
        private double _activePowerMax;
        private double _activePowerLowerThreshold;
        private double _activePowerUpperThreshold;

        // Reactive Power
        private double _reactivePowerMin;
        private double _reactivePowerMax;
        private double _reactivePowerLowerThreshold;
        private double _reactivePowerUpperThreshold;

        // Apparent Power
        private double _apparentPowerMin;
        private double _apparentPowerMax;
        private double _apparentPowerLowerThreshold;
        private double _apparentPowerUpperThreshold;

        // Power Factor
        private double _powerFactorMin;
        private double _powerFactorMax;
        private double _powerFactorLowerThreshold;
        private double _powerFactorUpperThreshold;

        public SettingsModel()
        {
            // Initiated default settings
            // Current
            _currentMin = 0;
            _currentMax = 100;
            _currentLowerThreshold = 20; // Default values to be 20% of simulated value
            _currentUpperThreshold = 80; // Default values to be 80% of simulated value

            // Phase Voltage
            _phaseVoltageMin = 0;
            _phaseVoltageMax = 300;
            _phaseVoltageLowerThreshold = 60; // Default values to be 20% of simulated value
            _phaseVoltageUpperThreshold = 240; // Default values to be 80% of simulated value

            // Line Voltage
            _lineVoltageMin = 0;
            _lineVoltageMax = 500;
            _lineVoltageLowerThreshold = 100; // Default values to be 20% of simulated value
            _lineVoltageUpperThreshold = 400; // Default values to be 80% of simulated value

            // Active Power
            _activePowerMin = 0;
            _activePowerMax = 50;
            _activePowerLowerThreshold = 10; // Default values to be 20% of simulated value
            _activePowerUpperThreshold = 40; // Default values to be 80% of simulated value

            // Reactive Power
            _reactivePowerMin = 0;
            _reactivePowerMax = 50;
            _reactivePowerLowerThreshold = 10; // Default values to be 20% of simulated value
            _reactivePowerUpperThreshold = 40; // Default values to be 80% of simulated value

            // Apparent Power
            _apparentPowerMin = 0;
            _apparentPowerMax = 50;
            _apparentPowerLowerThreshold = 10; // Default values to be 20% of simulated value
            _apparentPowerUpperThreshold = 40; // Default values to be 80% of simulated value

            // Power Factor
            _powerFactorMin = 0;
            _powerFactorMax = 1;
            _powerFactorLowerThreshold = 0.4; // Default values to be 20% of simulated value
            _powerFactorUpperThreshold = 0.85; // Default values to be 80% of simulated value
        }

        // Parameters properties
        // Current
        public double CurrentMin { get => _currentMin; set => _currentMin = value; }
        public double CurrentMax { get => _currentMax; set => _currentMax = value; }
        public double CurrentLowerThreshold
        {
            get => _currentLowerThreshold;
            set
            {
                if (value > _currentMin && value < _currentMax && value < _currentUpperThreshold)
                {
                    _currentLowerThreshold = value;
                }
            }
        }
        public double CurrentUpperThreshold
        {
            get => _currentUpperThreshold;
            set
            {
                if (value > _currentMin && value < _currentMax && value > _currentLowerThreshold)
                {
                    _currentUpperThreshold = value;
                }
            }
        }

        // Phase Voltage
        public double PhaseVoltageMin { get => _phaseVoltageMin; set => _phaseVoltageMin = value; }
        public double PhaseVoltageMax { get => _phaseVoltageMax; set => _phaseVoltageMax = value; }
        public double PhaseVoltageLowerThreshold
        {
            get => _phaseVoltageLowerThreshold;
            set
            {
                if (value > _phaseVoltageMin && value < _phaseVoltageMax && value < _phaseVoltageUpperThreshold)
                {
                    _phaseVoltageLowerThreshold = value;
                }
            }
        }
        public double PhaseVoltageUpperThreshold
        {
            get => _phaseVoltageUpperThreshold;
            set
            {
                if (value > _phaseVoltageMin && value < _phaseVoltageMax && value > _phaseVoltageLowerThreshold)
                {
                    _phaseVoltageUpperThreshold = value;
                }
            }
        }

        // Line Voltage
        public double LineVoltageMin { get => _lineVoltageMin; set => _lineVoltageMin = value; }
        public double LineVoltageMax { get => _lineVoltageMax; set => _lineVoltageMax = value; }
        public double LineVoltageLowerThreshold
        {
            get => _lineVoltageLowerThreshold;
            set
            {
                if (value > _lineVoltageMin && value < _lineVoltageMax && value < _lineVoltageUpperThreshold)
                {
                    _lineVoltageLowerThreshold = value;
                }
            }
        }
        public double LineVoltageUpperThreshold
        {
            get => _lineVoltageUpperThreshold;
            set
            {
                if (value > _lineVoltageMin && value < _lineVoltageMax && value > _lineVoltageLowerThreshold)
                {
                    _lineVoltageUpperThreshold = value;
                }
            }
        }

        // Active Power
        public double ActivePowerMin { get => _activePowerMin; set => _activePowerMin = value; }
        public double ActivePowerMax { get => _activePowerMax; set => _activePowerMax = value; }
        public double ActivePowerLowerThreshold
        {
            get => _activePowerLowerThreshold;
            set
            {
                if (value > _activePowerMin && value < _activePowerMax && value < _activePowerUpperThreshold)
                {
                    _activePowerLowerThreshold = value;
                }
            }
        }
        public double ActivePowerUpperThreshold
        {
            get => _activePowerUpperThreshold;
            set
            {
                if (value > _activePowerMin && value < _activePowerMax && value > _activePowerLowerThreshold)
                {
                    _activePowerUpperThreshold = value;
                }
            }
        }

        // Reactive Power
        public double ReactivePowerMin { get => _reactivePowerMin; set => _reactivePowerMin = value; }
        public double ReactivePowerMax { get => _reactivePowerMax; set => _reactivePowerMax = value; }
        public double ReactivePowerLowerThreshold
        {
            get => _reactivePowerLowerThreshold;
            set
            {
                if (value > _reactivePowerMin && value < _reactivePowerMax && value < _reactivePowerUpperThreshold)
                {
                    _reactivePowerLowerThreshold = value;
                }
            }
        }
        public double ReactivePowerUpperThreshold
        {
            get => _reactivePowerUpperThreshold;
            set
            {
                if (value > _reactivePowerMin && value < _reactivePowerMax && value > _reactivePowerLowerThreshold)
                {
                    _reactivePowerUpperThreshold = value;
                }
            }
        }

        // Apparent Power
        public double ApparentPowerMin { get => _apparentPowerMin; set => _apparentPowerMin = value; }
        public double ApparentPowerMax { get => _apparentPowerMax; set => _apparentPowerMax = value; }
        public double ApparentPowerLowerThreshold
        {
            get => _apparentPowerLowerThreshold;
            set
            {
                if (value > _apparentPowerMin && value < _apparentPowerMax && value < _apparentPowerUpperThreshold)
                {
                    _apparentPowerLowerThreshold = value;
                }
            }
        }
        public double ApparentPowerUpperThreshold
        {
            get => _apparentPowerUpperThreshold;
            set
            {
                if (value > _apparentPowerMin && value < _apparentPowerMax && value > _apparentPowerLowerThreshold)
                {
                    _apparentPowerUpperThreshold = value;
                }
            }
        }

        // Power Factor
        public double PowerFactorMin { get => _powerFactorMin; set => _powerFactorMin = value; }
        public double PowerFactorMax { get => _powerFactorMax; set => _powerFactorMax = value; }
        public double PowerFactorLowerThreshold
        {
            get => _powerFactorLowerThreshold;
            set
            {
                if (value > _powerFactorMin && value < _powerFactorMax && value < _powerFactorUpperThreshold)
                {
                    _powerFactorLowerThreshold = value;
                }
            }
        }
        public double PowerFactorUpperThreshold
        {
            get => _powerFactorUpperThreshold;
            set
            {
                if (value > _powerFactorMin && value < _powerFactorMax && value > _powerFactorLowerThreshold)
                {
                    _powerFactorUpperThreshold = value;
                }
            }
        }


        // Singleton design pattern
        public static SettingsModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new SettingsModel();
                        }
                    }
                }
                return _instance;
            }
        }

        // Update a setting instance
        public static void UpdateInstance(SettingsModel newSettings)
        {
            lock (_lock)
            {
                if (_instance != null)
                {

                    _instance.CurrentLowerThreshold = newSettings.CurrentLowerThreshold;
                    _instance.CurrentUpperThreshold = newSettings.CurrentUpperThreshold;

                    _instance.PhaseVoltageLowerThreshold = newSettings.PhaseVoltageLowerThreshold;
                    _instance.PhaseVoltageUpperThreshold = newSettings.PhaseVoltageUpperThreshold;

                    _instance.LineVoltageLowerThreshold = newSettings.LineVoltageLowerThreshold;
                    _instance.LineVoltageUpperThreshold = newSettings.LineVoltageUpperThreshold;

                    _instance.ActivePowerLowerThreshold = newSettings.ActivePowerLowerThreshold;
                    _instance.ActivePowerUpperThreshold = newSettings.ActivePowerUpperThreshold;

                    _instance.ReactivePowerLowerThreshold = newSettings.ReactivePowerLowerThreshold;
                    _instance.ReactivePowerUpperThreshold = newSettings.ReactivePowerUpperThreshold;

                    _instance.ApparentPowerLowerThreshold = newSettings.ApparentPowerLowerThreshold;
                    _instance.ApparentPowerUpperThreshold = newSettings.ApparentPowerUpperThreshold;

                    _instance.PowerFactorLowerThreshold = newSettings.PowerFactorLowerThreshold;
                    _instance.PowerFactorUpperThreshold = newSettings.PowerFactorUpperThreshold;
                }
                else
                {
                    _instance = newSettings;
                }
            }
        }
    }
}
