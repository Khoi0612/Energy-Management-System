using System;
using System.Timers;

namespace EMS.MVVM.Model
{
    internal class CurrentModel : ModbusMasterModel
    {
        private readonly System.Timers.Timer _timer; // Timer for continuous reading

        // Current fields
        private double _current_L1;
        private double _current_L2;
        private double _current_L3;

        // Warning fields
        private WarningModel _currentWarning_L1;
        private WarningModel _currentWarning_L2;
        private WarningModel _currentWarning_L3;

        public CurrentModel()
        {
            // Get reading from the connected slave
            _current_L1 = this.GetReadingCurrent["I_L1"];
            _current_L2 = this.GetReadingCurrent["I_L2"];
            _current_L3 = this.GetReadingCurrent["I_L3"];

            // Initated Current warnings
            _currentWarning_L1 = new WarningModel();
            _currentWarning_L2 = new WarningModel();
            _currentWarning_L3 = new WarningModel();

            // Initiated timer
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += UpdateData;
            _timer.Start();
        }

        // Current changed events
        public event Action<double> Current_L1_Changed;
        public event Action<double> Current_L2_Changed;
        public event Action<double> Current_L3_Changed;

        // Current warning events
        public event Action<WarningModel> Current_L1_WarningTriggered;
        public event Action<WarningModel> Current_L2_WarningTriggered;
        public event Action<WarningModel> Current_L3_WarningTriggered;

        // Current properties
        public double Current_L1
        {
            get => _current_L1;
            set
            {
                if (IsDeviceAvailable())
                {
                    _current_L1 = value;
                    _currentWarning_L1 = CheckThreshold("Current", "1", _current_L1, this.Settings.CurrentLowerThreshold, this.Settings.CurrentUpperThreshold, "A", Current_L1_WarningTriggered);
                    Current_L1_Changed?.Invoke(_current_L1);
                }
            }
        }

        public double Current_L2
        {
            get => _current_L2;
            set
            {
                if (IsDeviceAvailable())
                {
                    _current_L2 = value;
                    _currentWarning_L2 = CheckThreshold("Current", "2", _current_L2, this.Settings.CurrentLowerThreshold, this.Settings.CurrentUpperThreshold, "A", Current_L2_WarningTriggered); ;
                    Current_L2_Changed?.Invoke(_current_L2);
                }
            }
        }

        public double Current_L3
        {
            get => _current_L3;
            set
            {
                if (IsDeviceAvailable())
                {
                    _current_L3 = value;
                    _currentWarning_L3 = CheckThreshold("Current", "3", _current_L3, this.Settings.CurrentLowerThreshold, this.Settings.CurrentUpperThreshold, "A", Current_L3_WarningTriggered);
                    Current_L3_Changed?.Invoke(_current_L3);
                }
            }
        }

        // Current warning properties
        public WarningModel CurrentWarning_L1
        {
            get => _currentWarning_L1;
            set
            {
                _currentWarning_L1 = value;
            }
        }

        public WarningModel CurrentWarning_L2
        {
            get => _currentWarning_L2;
            set
            {
                _currentWarning_L2 = value;
            }
        }

        public WarningModel CurrentWarning_L3
        {
            get => _currentWarning_L3;
            set
            {
                _currentWarning_L3 = value;
            }
        }

        // Update data function
        public void UpdateData(object sender, ElapsedEventArgs e)
        {
            this.GetReadingRegisters();
            Current_L1 = this.GetReadingCurrent["I_L1"];
            Current_L2 = this.GetReadingCurrent["I_L2"];
            Current_L3 = this.GetReadingCurrent["I_L3"];
        }

    }
}
