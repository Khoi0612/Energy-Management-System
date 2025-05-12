using EMS.MVVM.Model;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EMS.MVVM.ViewModel
{
    internal class MosbusMasterVM : BaseVM
    {
        private readonly ModbusMasterModel _masterModel; // Model field

        // Slave id connection fields
        private string _slaveIdString;
        private byte _slaveId;

        private string _connectionStatus;
        private string _connectionColor;

        public MosbusMasterVM()
        {
            _masterModel = new ModbusMasterModel();

            ConnectionStatus = "Status: Ready";
            ConnectionColor = "Green";

            _slaveIdString = "0";
        }

        // Slave id connection properties
        public string SlaveIdString
        {
            get => _slaveIdString;
            set
            {
                if (_slaveIdString != value)
                {
                    _slaveIdString = value;
                    OnPropertyChanged();

                    if (byte.TryParse(_slaveIdString, out byte result))
                    {
                        SlaveId = result;
                        _masterModel.SlaveId = _slaveId;
                    }
                }
            }
        }

        public byte SlaveId
        {
            get => _slaveId;
            set
            {
                if (_slaveId != value)
                {
                    _slaveId = value;
                    OnPropertyChanged(nameof(SlaveId));
                }
            }
        }

        public string ConnectionStatus
        {
            get => _connectionStatus;
            set
            {
                _connectionStatus = value;
                OnPropertyChanged();
            }
        }

        public string ConnectionColor
        {
            get => _connectionColor;
            set
            {
                _connectionColor = value;
                OnPropertyChanged();
            }
        }

        // Functions to restrict textbox to accept only number
        public static void AttachNumericOnly(TextBox textBox)
        {
            textBox.PreviewTextInput += TextBox_PreviewTextInput;
            DataObject.AddPastingHandler(textBox, OnPaste);
        }

        private static void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextNumeric(e.Text);
        }

        private static void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextNumeric(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private static bool IsTextNumeric(string text)
        {
            Regex regex = new Regex("^[0-9]+$");
            return regex.IsMatch(text);
        }

        // Function to check for Modbus slave connection
        public void CheckConnection()
        {
            if (_masterModel.IsDeviceAvailable())
            {
                ConnectionStatus = "Status: Connected";
                ConnectionColor = "Green";
            }
            else
            {
                ConnectionStatus = "Status: Device Unavailable";
                ConnectionColor = "Red";
            }
        }
    }
}
