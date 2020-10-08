using System;

namespace XmlToCsv
{
    public class LogEventArgs : EventArgs
    {
        public LogEventArgs(string logMessage)
        {
            this.LogMessage = logMessage;
        }

        public string LogMessage { get; }
    }
}
