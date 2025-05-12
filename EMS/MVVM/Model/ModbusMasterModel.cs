using EMS.Core.Data;
using EMS.Core.Interfaces;
using EMS.Core.Models;
using EMS.Core.Services;
using Modbus.Device;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace EMS.MVVM.Model
{
    internal class ModbusMasterModel
    {
        private ushort[] _registers; // List to store all data read

        // Connected slave fields
        private static byte _slaveId;
        private static string _slaveIp;
        private static int _slavePort;

        // Field to classify data read 
        private EnergyReading _reading;

        // Fields to connect and monitor the database
        private readonly IEnergyReadingRepository _repository;
        private readonly EnergyMonitoringService _monitoringService;

        // Setting fields
        private readonly string _settingsFilePath = "EMSSettings.json";
        private SettingsModel _settings;

        public ModbusMasterModel()
        {
            _registers = new ushort[87]; // Initiated new ushort list

            // Initiated connected slave fields
            _slaveIp = "127.0.0.1"; // Local Host IP. NEEDED TO BE CHANGED TO CONNECTED IP ADDRESS
            _slavePort = 502;

            // Initated data classification field
            _reading = new EnergyReading();

            // Initiated database services fields
            _repository = new EnergyReadingRepository();
            _monitoringService = new EnergyMonitoringService(_repository);

            // Apply threshold settings 
            ApplySettings();
        }

        // Connected slave events
        public static event Action<byte> IdChanged;
        public static event Action<string> IpChanged;
        public static event Action<int> PortChanged;

        // Connected slave properties
        public byte SlaveId
        {
            get
            {
                return _slaveId;
            }
            set
            {
                _slaveId = value;
            }
        }

        public string SlaveIp
        {
            get
            {
                return _slaveIp;
            }
        }

        public int SlavePort
        {
            get
            {
                return _slavePort;
            }
            set
            {
                _slavePort = value;
            }
        }

        // Parameter dictionat properties
        // Current
        public Dictionary<string, ushort> GetReadingCurrent
        {
            get
            {
                Dictionary<string, ushort> currents = new Dictionary<string, ushort>
                {
                    { "I_L1", _registers[16] },
                    { "I_L2", _registers[18] },
                    { "I_L3", _registers[20] }
                };
                return currents;
            }

        }

        // Phase Voltage
        public Dictionary<string, ushort> GetReadingPhaseVoltage
        {
            get
            {
                Dictionary<string, ushort> phaseVoltages = new Dictionary<string, ushort>
                {
                    { "V_L1", _registers[4] },
                    { "V_L2", _registers[6] },
                    { "V_L3", _registers[8] }
                };
                return phaseVoltages;
            }

        }

        // Line Voltage
        public Dictionary<string, ushort> GetReadingLineVoltage
        {
            get
            {
                Dictionary<string, ushort> lineVoltages = new Dictionary<string, ushort>
                {
                    { "V_L12", _registers[10] },
                    { "V_L23", _registers[12] },
                    { "V_L31", _registers[14] }
                };
                return lineVoltages;
            }
        }

        // Active Power
        public Dictionary<string, ushort> GetReadingActivePower
        {
            get
            {
                Dictionary<string, ushort> activePowers = new Dictionary<string, ushort>
                {
                    { "P_L1", _registers[28] },
                    { "P_L2", _registers[30] },
                    { "P_L3", _registers[32] }
                };
                return activePowers;
            }
        }

        // Reactive Power
        public Dictionary<string, ushort> GetReadingReactivePower
        {
            get
            {
                Dictionary<string, ushort> reactivePowers = new Dictionary<string, ushort>
                {
                    { "VARQ1_L1", _registers[34] },
                    { "VARQ1_L2", _registers[36] },
                    { "VARQ1_L3", _registers[38] }
                };
                return reactivePowers;
            }
        }

        // Apparent Power
        public Dictionary<string, ushort> GetReadingApparentPower
        {
            get
            {
                Dictionary<string, ushort> apparentPowers = new Dictionary<string, ushort>
                {
                    { "VA_L1", _registers[22] },
                    { "VA_L2", _registers[24] },
                    { "VA_L3", _registers[26] }
                };
                return apparentPowers;
            }
        }

        // Power Factor
        public Dictionary<string, ushort> GetReadingPowerFactor
        {
            get
            {
                Dictionary<string, ushort> powerFactors = new Dictionary<string, ushort>
                {
                    { "COS1_L1", _registers[80] },
                    { "COS1_L2", _registers[82] },
                    { "COS1_L3", _registers[84] }
                };
                return powerFactors;
            }
        }

        // Data classification property
        public EnergyReading Reading
        {
            get
            {
                return _reading;
            }
            set
            {
                _reading = value;
            }
        }

        // Setting property
        public SettingsModel Settings
        {
            get => _settings;
        }

        // Function to connect to the slave defined in constructor and get data
        public void GetReadingRegisters()
        {

            if (IsDeviceAvailable())
            {
                // Establish a TCP connection to the slave
                using (TcpClient client = new TcpClient(SlaveIp, SlavePort))
                using (var master = ModbusIpMaster.CreateIp(client))
                {
                    // Reading Holding Registers (Function Code 03) from address 0
                    ushort startAddress = 30000;
                    ushort numRegisters = 87; // For Current, Phase Voltage, Line Voltage, Active Power, Reactive Power, Power Factor in that order 

                    _registers = master.ReadHoldingRegisters(SlaveId, startAddress, numRegisters);

                    Reading = new EnergyReading
                    {
                        Start_Timestamp = DateTime.Now,
                        End_Timestamp = DateTime.Now,

                        I_L1 = _registers[16], // Simulated from 0 to 100 A
                        I_L2 = _registers[18],
                        I_L3 = _registers[20],

                        V_L1 = _registers[4], // Simulated from 0 to 300 V
                        V_L2 = _registers[6],
                        V_L3 = _registers[8],

                        V_L12 = _registers[10], // Simulated from 0 to 500 V
                        V_L23 = _registers[12],
                        V_L31 = _registers[14],

                        P_L1 = _registers[28], // Simulated from 0 to 50 kW
                        P_L2 = _registers[30],
                        P_L3 = _registers[32],

                        VARQ1_L1 = _registers[34], // Simulated from 0 to 50 kVAR
                        VARQ1_L2 = _registers[36],
                        VARQ1_L3 = _registers[38],

                        VA_L1 = _registers[22], // Simulated from 0 to 50 kVA
                        VA_L2 = _registers[24],
                        VA_L3 = _registers[26],

                        COS1_L1 = _registers[80], // Simulated from 0 to 1
                        COS1_L2 = _registers[82],
                        COS1_L3 = _registers[84]
                    };

                    _monitoringService.EnqueueReading(_reading);
                }
            }
        }

        // Function to check if the slave defined is available to connect
        public bool IsDeviceAvailable()
        {
            try
            {
                using (var client = new TcpClient())
                {
                    var result = client.BeginConnect(SlaveIp, SlavePort, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
                    if (success)
                    {
                        client.EndConnect(result);
                        return true;
                    }
                }
            }
            catch
            {

            }
            return false;
        }

        // Function to check whether the value read is within the threshold set
        protected WarningModel CheckThreshold(string parameter, string lineOrPhase, double value, double lowerThreshold, double upperThreshold, string unit, Action<WarningModel> warningAction)
        {
            WarningModel warning;
            if (value > upperThreshold)
            {
                warning = new WarningModel
                {
                    Message = $"WARNING: High {parameter} in L{lineOrPhase}: {value} {unit} > {upperThreshold} {unit}",
                    Timestamp = this.Reading.Start_Timestamp,
                    Category = "High",
                    IsAcknowledged = false
                };
                warningAction?.Invoke(warning);
            }
            else if (value < lowerThreshold)
            {
                warning = new WarningModel
                {
                    Message = $"WARNING: Low {parameter} in L{lineOrPhase}: {value} {unit} < {lowerThreshold} {unit}",
                    Timestamp = this.Reading.Start_Timestamp,
                    Category = "Low",
                    IsAcknowledged = false
                };
                warningAction?.Invoke(warning);
            }
            else
            {

                warning = new WarningModel
                {
                    Category = "Normal",
                    IsAcknowledged = false
                };
            }
            return warning;
        }

        // Function to apply the setting in 'EMSSetting.json'
        private void ApplySettings()
        {
            if (File.Exists(_settingsFilePath))
            {
                string json = File.ReadAllText(_settingsFilePath);
                _settings = JsonConvert.DeserializeObject<SettingsModel>(json);
                SettingsModel.UpdateInstance(_settings);
            }
            else
            {
                _settings = SettingsModel.Instance;
            }
        }

    }
}
