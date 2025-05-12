using System;
using System.Timers;

namespace EMS.MVVM.Model
{
    internal class PowerModel : ModbusMasterModel
    {
        private readonly System.Timers.Timer _timer; // Timer for continuous reading

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

        // Power warning fields
        // Active
        private WarningModel _activePowerWarning_L1;
        private WarningModel _activePowerWarning_L2;
        private WarningModel _activePowerWarning_L3;

        // Reactive
        private WarningModel _reactivePowerWarning_L1;
        private WarningModel _reactivePowerWarning_L2;
        private WarningModel _reactivePowerWarning_L3;

        // Apparent
        private WarningModel _apparentPowerWarning_L1;
        private WarningModel _apparentPowerWarning_L2;
        private WarningModel _apparentPowerWarning_L3;

        // Power Factor
        private WarningModel _powerFactorWarning_L1;
        private WarningModel _powerFactorWarning_L2;
        private WarningModel _powerFactorWarning_L3;

        public PowerModel()
        {
            // Get reading from the connected slave
            // Active
            _activePower_L1 = this.GetReadingActivePower["P_L1"];
            _activePower_L2 = this.GetReadingActivePower["P_L2"];
            _activePower_L3 = this.GetReadingActivePower["P_L3"];

            // Reactive
            _reactivePower_L1 = this.GetReadingReactivePower["VARQ1_L1"];
            _reactivePower_L2 = this.GetReadingReactivePower["VARQ1_L2"];
            _reactivePower_L3 = this.GetReadingReactivePower["VARQ1_L3"];

            // Apparent
            _apparentPower_L1 = this.GetReadingApparentPower["VA_L1"];
            _apparentPower_L2 = this.GetReadingApparentPower["VA_L2"];
            _apparentPower_L3 = this.GetReadingApparentPower["VA_L3"];

            // Power Factor
            _powerFactor_L1 = (double)this.GetReadingPowerFactor["COS1_L1"] / 100;
            _powerFactor_L2 = (double)this.GetReadingPowerFactor["COS1_L2"] / 100;
            _powerFactor_L3 = (double)this.GetReadingPowerFactor["COS1_L3"] / 100;

            // Initated Power warnings
            // Active
            _activePowerWarning_L1 = new WarningModel();
            _activePowerWarning_L2 = new WarningModel();
            _activePowerWarning_L3 = new WarningModel();

            // Reactive
            _reactivePowerWarning_L1 = new WarningModel();
            _reactivePowerWarning_L2 = new WarningModel();
            _reactivePowerWarning_L3 = new WarningModel();

            // Apparent
            _apparentPowerWarning_L1 = new WarningModel();
            _apparentPowerWarning_L2 = new WarningModel();
            _apparentPowerWarning_L3 = new WarningModel();

            // Power Factor
            _powerFactorWarning_L1 = new WarningModel();
            _powerFactorWarning_L2 = new WarningModel();
            _powerFactorWarning_L3 = new WarningModel();

            // Initiated timer
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += UpdateData;
            _timer.Start();

        }

        // Power changed events
        // Active
        public event Action<double> ActivePower_L1_Changed;
        public event Action<double> ActivePower_L2_Changed;
        public event Action<double> ActivePower_L3_Changed;

        // Reactive
        public event Action<double> ReactivePower_L1_Changed;
        public event Action<double> ReactivePower_L2_Changed;
        public event Action<double> ReactivePower_L3_Changed;

        // Apparent
        public event Action<double> ApparentPower_L1_Changed;
        public event Action<double> ApparentPower_L2_Changed;
        public event Action<double> ApparentPower_L3_Changed;

        // Power Factor
        public event Action<double> PowerFactor_L1_Changed;
        public event Action<double> PowerFactor_L2_Changed;
        public event Action<double> PowerFactor_L3_Changed;

        // Power warning events
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

        // Power properties
        // Active
        public double ActivePower_L1
        {
            get => _activePower_L1;
            set
            {
                if (IsDeviceAvailable())
                {
                    _activePower_L1 = value;
                    _activePowerWarning_L1 = CheckThreshold("Active Power", "1", _activePower_L1, this.Settings.ActivePowerLowerThreshold, this.Settings.ActivePowerUpperThreshold, "kW", ActivePower_L1_WarningTriggered);
                    ActivePower_L1_Changed?.Invoke(_activePower_L1);
                }
            }
        }

        public double ActivePower_L2
        {
            get => _activePower_L2;
            set
            {
                if (IsDeviceAvailable())
                {
                    _activePower_L2 = value;
                    _activePowerWarning_L2 = CheckThreshold("Active Power", "2", _activePower_L2, this.Settings.ActivePowerLowerThreshold, this.Settings.ActivePowerUpperThreshold, "kW", ActivePower_L2_WarningTriggered);
                    ActivePower_L2_Changed?.Invoke(_activePower_L2);
                }
            }
        }

        public double ActivePower_L3
        {
            get => _activePower_L3;
            set
            {
                if (IsDeviceAvailable())
                {
                    _activePower_L3 = value;
                    _activePowerWarning_L3 = CheckThreshold("Active Power", "3", _activePower_L3, this.Settings.ActivePowerLowerThreshold, this.Settings.ActivePowerUpperThreshold, "kW", ActivePower_L3_WarningTriggered);
                    ActivePower_L3_Changed?.Invoke(_activePower_L3);
                }
            }
        }

        // Reactive
        public double ReactivePower_L1
        {
            get => _reactivePower_L1;
            set
            {
                if (IsDeviceAvailable())
                {
                    _reactivePower_L1 = value;
                    _reactivePowerWarning_L1 = CheckThreshold("Reactive Power", "1", _reactivePower_L1, this.Settings.ReactivePowerLowerThreshold, this.Settings.ReactivePowerUpperThreshold, "kVAR", ReactivePower_L1_WarningTriggered);
                    ReactivePower_L1_Changed?.Invoke(_reactivePower_L1);
                }
            }
        }

        public double ReactivePower_L2
        {
            get => _reactivePower_L2;
            set
            {
                if (IsDeviceAvailable())
                {
                    _reactivePower_L2 = value;
                    _reactivePowerWarning_L2 = CheckThreshold("Reactive Power", "2", _reactivePower_L2, this.Settings.ReactivePowerLowerThreshold, this.Settings.ReactivePowerUpperThreshold, "kVAR", ReactivePower_L1_WarningTriggered);
                    ReactivePower_L2_Changed?.Invoke(_reactivePower_L2);
                }
            }
        }

        public double ReactivePower_L3
        {
            get => _reactivePower_L3;
            set
            {
                if (IsDeviceAvailable())
                {
                    _reactivePower_L3 = value;
                    _reactivePowerWarning_L3 = CheckThreshold("Reactive Power", "3", _reactivePower_L3, this.Settings.ReactivePowerLowerThreshold, this.Settings.ReactivePowerUpperThreshold, "kVAR", ReactivePower_L3_WarningTriggered);
                    ReactivePower_L3_Changed?.Invoke(_reactivePower_L3);
                }
            }
        }

        // Apparent
        public double ApparentPower_L1
        {
            get => _apparentPower_L1;
            set
            {
                if (IsDeviceAvailable())
                {
                    _apparentPower_L1 = value;
                    _apparentPowerWarning_L1 = CheckThreshold("Apparent Power", "1", _apparentPower_L1, this.Settings.ApparentPowerLowerThreshold, this.Settings.ApparentPowerUpperThreshold, "kVA", ApparentPower_L1_WarningTriggered);
                    ApparentPower_L1_Changed?.Invoke(_apparentPower_L1);
                }
            }
        }

        public double ApparentPower_L2
        {
            get => _apparentPower_L2;
            set
            {
                if (IsDeviceAvailable())
                {
                    _apparentPower_L2 = value;
                    _apparentPowerWarning_L2 = CheckThreshold("Apparent Power", "2", _apparentPower_L2, this.Settings.ApparentPowerLowerThreshold, this.Settings.ApparentPowerUpperThreshold, "kVA", ApparentPower_L2_WarningTriggered);
                    ApparentPower_L2_Changed?.Invoke(_apparentPower_L2);
                }
            }
        }

        public double ApparentPower_L3
        {
            get => _apparentPower_L3;
            set
            {
                if (IsDeviceAvailable())
                {
                    _apparentPower_L3 = value;
                    _apparentPowerWarning_L3 = CheckThreshold("Apparent Power", "3", _apparentPower_L3, this.Settings.ApparentPowerLowerThreshold, this.Settings.ApparentPowerUpperThreshold, "kVA", ApparentPower_L3_WarningTriggered);
                    ApparentPower_L3_Changed?.Invoke(_apparentPower_L3);
                }
            }
        }

        // Power Factor
        public double PowerFactor_L1
        {
            get => _powerFactor_L1;
            set
            {
                if (IsDeviceAvailable())
                {
                    _powerFactor_L1 = value;
                    _powerFactorWarning_L1 = CheckThreshold("Power Factor", "1", _powerFactor_L1, this.Settings.PowerFactorLowerThreshold, this.Settings.PowerFactorUpperThreshold, string.Empty, PowerFactor_L1_WarningTriggered);
                    PowerFactor_L1_Changed?.Invoke(_powerFactor_L1);
                }
            }
        }

        public double PowerFactor_L2
        {
            get => _powerFactor_L2;
            set
            {
                if (IsDeviceAvailable())
                {
                    _powerFactor_L2 = value;
                    _powerFactorWarning_L2 = CheckThreshold("Power Factor", "2", _powerFactor_L2, this.Settings.PowerFactorLowerThreshold, this.Settings.PowerFactorUpperThreshold, string.Empty, PowerFactor_L2_WarningTriggered);
                    PowerFactor_L2_Changed?.Invoke(_powerFactor_L2);
                }
            }
        }

        public double PowerFactor_L3
        {
            get => _powerFactor_L3;
            set
            {
                if (IsDeviceAvailable())
                {
                    _powerFactor_L3 = value;
                    _powerFactorWarning_L3 = CheckThreshold("Power Factor", "3", _powerFactor_L3, this.Settings.PowerFactorLowerThreshold, this.Settings.PowerFactorUpperThreshold, string.Empty, PowerFactor_L3_WarningTriggered);
                    PowerFactor_L3_Changed?.Invoke(_powerFactor_L3);
                }
            }
        }

        // Power warning properties
        // Active
        public WarningModel ActivePowerWarning_L1
        {
            get => _activePowerWarning_L1;
            set
            {
                _activePowerWarning_L1 = value;
            }
        }

        public WarningModel ActivePowerWarning_L2
        {
            get => _activePowerWarning_L2;
            set
            {
                _activePowerWarning_L2 = value;
            }
        }

        public WarningModel ActivePowerWarning_L3
        {
            get => _activePowerWarning_L3;
            set
            {
                _activePowerWarning_L3 = value;
            }
        }

        // Reactive
        public WarningModel ReactivePowerWarning_L1
        {
            get => _reactivePowerWarning_L1;
            set
            {
                _reactivePowerWarning_L1 = value;
            }
        }

        public WarningModel ReactivePowerWarning_L2
        {
            get => _reactivePowerWarning_L2;
            set
            {
                _reactivePowerWarning_L2 = value;
            }
        }

        public WarningModel ReactivePowerWarning_L3
        {
            get => _reactivePowerWarning_L3;
            set
            {
                _reactivePowerWarning_L3 = value;
            }
        }

        // Apparent
        public WarningModel ApparentPowerWarning_L1
        {
            get => _apparentPowerWarning_L1;
            set
            {
                _apparentPowerWarning_L1 = value;
            }
        }

        public WarningModel ApparentPowerWarning_L2
        {
            get => _apparentPowerWarning_L2;
            set
            {
                _apparentPowerWarning_L2 = value;
            }
        }

        public WarningModel ApparentPowerWarning_L3
        {
            get => _apparentPowerWarning_L3;
            set
            {
                _apparentPowerWarning_L3 = value;
            }
        }

        // Power Factor
        public WarningModel PowerFactorWarning_L1
        {
            get => _powerFactorWarning_L1;
            set
            {
                _powerFactorWarning_L1 = value;
            }
        }

        public WarningModel PowerFactorWarning_L2
        {
            get => _powerFactorWarning_L2;
            set
            {
                _powerFactorWarning_L2 = value;
            }
        }

        public WarningModel PowerFactorWarning_L3
        {
            get => _powerFactorWarning_L3;
            set
            {
                _powerFactorWarning_L3 = value;
            }
        }

        // Update data function
        public void UpdateData(object sender, ElapsedEventArgs e)
        {
            this.GetReadingRegisters();

            ActivePower_L1 = this.GetReadingActivePower["P_L1"];
            ActivePower_L2 = this.GetReadingActivePower["P_L2"];
            ActivePower_L3 = this.GetReadingActivePower["P_L3"];

            ReactivePower_L1 = this.GetReadingReactivePower["VARQ1_L1"];
            ReactivePower_L2 = this.GetReadingReactivePower["VARQ1_L2"];
            ReactivePower_L3 = this.GetReadingReactivePower["VARQ1_L3"];

            ApparentPower_L1 = this.GetReadingApparentPower["VA_L1"];
            ApparentPower_L2 = this.GetReadingApparentPower["VA_L2"];
            ApparentPower_L3 = this.GetReadingApparentPower["VA_L3"];

            PowerFactor_L1 = (double)this.GetReadingPowerFactor["COS1_L1"];
            PowerFactor_L2 = (double)this.GetReadingPowerFactor["COS1_L2"];
            PowerFactor_L3 = (double)this.GetReadingPowerFactor["COS1_L3"];

        }
    }
}
