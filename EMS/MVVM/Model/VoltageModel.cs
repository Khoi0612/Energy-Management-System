using System;
using System.Timers;

namespace EMS.MVVM.Model
{
    internal class VoltageModel : ModbusMasterModel
    {

        private readonly System.Timers.Timer _timer; // Timer for continuous reading

        // Voltage fields
        // Phase
        private double _phaseVoltage_L1;
        private double _phaseVoltage_L2;
        private double _phaseVoltage_L3;

        // Line
        private double _lineVoltage_L12;
        private double _lineVoltage_L23;
        private double _lineVoltage_L31;

        // Voltage warning fields
        // Phase
        private WarningModel _phaseVoltageWarning_L1;
        private WarningModel _phaseVoltageWarning_L2;
        private WarningModel _phaseVoltageWarning_L3;

        // Line
        private WarningModel _lineVoltageWarning_L12;
        private WarningModel _lineVoltageWarning_L23;
        private WarningModel _lineVoltageWarning_L31;

        public VoltageModel()
        {
            // Get reading from the connected slave
            // Phase
            _phaseVoltage_L1 = this.GetReadingPhaseVoltage["V_L1"];
            _phaseVoltage_L2 = this.GetReadingPhaseVoltage["V_L2"];
            _phaseVoltage_L3 = this.GetReadingPhaseVoltage["V_L3"];

            // Line
            _lineVoltage_L12 = this.GetReadingLineVoltage["V_L12"];
            _lineVoltage_L23 = this.GetReadingLineVoltage["V_L23"];
            _lineVoltage_L31 = this.GetReadingLineVoltage["V_L31"];

            // Initated Voltage warnings
            // Phase
            _phaseVoltageWarning_L1 = new WarningModel();
            _phaseVoltageWarning_L2 = new WarningModel();
            _phaseVoltageWarning_L3 = new WarningModel();

            // Line
            _lineVoltageWarning_L12 = new WarningModel();
            _lineVoltageWarning_L23 = new WarningModel();
            _lineVoltageWarning_L31 = new WarningModel();

            // Initiated timer
            _timer = new System.Timers.Timer(1000); // Set interval to 1 second
            _timer.Elapsed += UpdateData;
            _timer.Start();
        }

        // Voltage changed events
        // Phase
        public event Action<double> PhaseVoltage_L1_Changed;
        public event Action<double> PhaseVoltage_L2_Changed;
        public event Action<double> PhaseVoltage_L3_Changed;

        // Line
        public event Action<double> LineVoltage_L12_Changed;
        public event Action<double> LineVoltage_L23_Changed;
        public event Action<double> LineVoltage_L31_Changed;

        // Voltage warning events
        // Phase
        public event Action<WarningModel> PhaseVoltage_L1_WarningTriggered;
        public event Action<WarningModel> PhaseVoltage_L2_WarningTriggered;
        public event Action<WarningModel> PhaseVoltage_L3_WarningTriggered;

        // Line
        public event Action<WarningModel> LineVoltage_L12_WarningTriggered;
        public event Action<WarningModel> LineVoltage_L23_WarningTriggered;
        public event Action<WarningModel> LineVoltage_L31_WarningTriggered;

        // Voltage properties
        // Phase
        public double PhaseVoltage_L1
        {
            get => _phaseVoltage_L1;
            set
            {
                if (IsDeviceAvailable())
                {
                    _phaseVoltage_L1 = value;
                    _phaseVoltageWarning_L1 = CheckThreshold("Phase Voltage", "1", _phaseVoltage_L1, this.Settings.PhaseVoltageLowerThreshold, this.Settings.PhaseVoltageUpperThreshold, "V", PhaseVoltage_L1_WarningTriggered);
                    PhaseVoltage_L1_Changed?.Invoke(_phaseVoltage_L1);
                }
            }
        }

        public double PhaseVoltage_L2
        {
            get => _phaseVoltage_L2;
            set
            {
                if (IsDeviceAvailable())
                {
                    _phaseVoltage_L2 = value;
                    _phaseVoltageWarning_L2 = CheckThreshold("Phase Voltage", "2", _phaseVoltage_L2, this.Settings.PhaseVoltageLowerThreshold, this.Settings.PhaseVoltageUpperThreshold, "V", PhaseVoltage_L2_WarningTriggered);
                    PhaseVoltage_L2_Changed?.Invoke(_phaseVoltage_L2);
                }
            }
        }

        public double PhaseVoltage_L3
        {
            get => _phaseVoltage_L3;
            set
            {
                if (IsDeviceAvailable())
                {
                    _phaseVoltage_L3 = value;
                    _phaseVoltageWarning_L3 = CheckThreshold("Phase Voltage", "3", _phaseVoltage_L3, this.Settings.PhaseVoltageLowerThreshold, this.Settings.PhaseVoltageUpperThreshold, "V", PhaseVoltage_L3_WarningTriggered);
                    PhaseVoltage_L3_Changed?.Invoke(_phaseVoltage_L3);
                }
            }
        }

        // Line
        public double LineVoltage_L12
        {
            get => _lineVoltage_L12;
            set
            {
                if (IsDeviceAvailable())
                {
                    _lineVoltage_L12 = value;
                    _lineVoltageWarning_L12 = CheckThreshold("Line Voltage", "12", _lineVoltage_L12, this.Settings.LineVoltageLowerThreshold, this.Settings.LineVoltageUpperThreshold, "V", LineVoltage_L12_WarningTriggered);
                    LineVoltage_L12_Changed?.Invoke(_lineVoltage_L12);
                }
            }
        }

        public double LineVoltage_L23
        {
            get => _lineVoltage_L23;
            set
            {
                if (IsDeviceAvailable())
                {
                    _lineVoltage_L23 = value;
                    _lineVoltageWarning_L23 = CheckThreshold("Line Voltage", "23", _lineVoltage_L23, this.Settings.LineVoltageLowerThreshold, this.Settings.LineVoltageUpperThreshold, "V", LineVoltage_L23_WarningTriggered);
                    LineVoltage_L23_Changed?.Invoke(_lineVoltage_L23);
                }
            }
        }

        public double LineVoltage_L31
        {
            get => _lineVoltage_L31;
            set
            {
                if (IsDeviceAvailable())
                {
                    _lineVoltage_L31 = value;
                    _lineVoltageWarning_L31 = CheckThreshold("Line Voltage", "31", _lineVoltage_L31, this.Settings.LineVoltageLowerThreshold, this.Settings.LineVoltageUpperThreshold, "V", LineVoltage_L31_WarningTriggered);
                    LineVoltage_L31_Changed?.Invoke(_lineVoltage_L31);
                }
            }
        }

        // Voltage warning properties
        // Phase
        public WarningModel PhaseVoltageWarning_L1
        {
            get => _phaseVoltageWarning_L1;
            set
            {
                _phaseVoltageWarning_L1 = value;
            }
        }

        public WarningModel PhaseVoltageWarning_L2
        {
            get => _phaseVoltageWarning_L2;
            set
            {
                _phaseVoltageWarning_L2 = value;
            }
        }

        public WarningModel PhaseVoltageWarning_L3
        {
            get => _phaseVoltageWarning_L3;
            set
            {
                _phaseVoltageWarning_L3 = value;
            }
        }

        // Line
        public WarningModel LineVoltageWarning_L12
        {
            get => _lineVoltageWarning_L12;
            set
            {
                _lineVoltageWarning_L12 = value;
            }
        }

        public WarningModel LineVoltageWarning_L23
        {
            get => _lineVoltageWarning_L23;
            set
            {
                _lineVoltageWarning_L23 = value;
            }
        }

        public WarningModel LineVoltageWarning_L31
        {
            get => _lineVoltageWarning_L31;
            set
            {
                _lineVoltageWarning_L31 = value;
            }
        }

        // Update data function
        public void UpdateData(object sender, ElapsedEventArgs e)
        {
            this.GetReadingRegisters();

            PhaseVoltage_L1 = this.GetReadingPhaseVoltage["V_L1"];
            PhaseVoltage_L2 = this.GetReadingPhaseVoltage["V_L2"];
            PhaseVoltage_L3 = this.GetReadingPhaseVoltage["V_L3"];

            LineVoltage_L12 = this.GetReadingLineVoltage["V_L12"];
            LineVoltage_L23 = this.GetReadingLineVoltage["V_L23"];
            LineVoltage_L31 = this.GetReadingLineVoltage["V_L31"];

        }
    }
}
