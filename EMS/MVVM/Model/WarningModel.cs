using System;

namespace EMS.MVVM.Model
{
    internal class WarningModel
    {
        private string _message;
        private DateTime _timestamp;
        private string _category;
        private bool _isAcknowledged;

        public string Message { get => _message; set => _message = value; }
        public DateTime Timestamp { get => _timestamp; set => _timestamp = value; }
        public string Category { get => _category; set => _category = value; }
        public bool IsAcknowledged { get => _isAcknowledged; set => _isAcknowledged = value; }
    }
}
