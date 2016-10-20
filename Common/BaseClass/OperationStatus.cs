using System;
using System.Diagnostics;

namespace Mn.NewsCms.Common.BaseClass
{
    [DebuggerDisplay("Status: {Status}")]
    public class OperationStatus
    {
        public bool Status { get; set; }
        public int RecordsAffected { get; set; }
        public string Message { get; set; }
        public object OperationID { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
        public string ExceptionInnerMessage { get; set; }
        public string ExceptionInnerStackTrace { get; set; }
        public Exception Exception { get; set; }
        public static OperationStatus CreateFromException(string message, Exception ex)
        {
            OperationStatus opStatus = new OperationStatus
            {
                Status = false,
                Message = message,
                OperationID = null
            };

            if (ex != null)
            {
                opStatus.ExceptionMessage = ex.Message;
                opStatus.ExceptionStackTrace = ex.StackTrace;
                opStatus.ExceptionInnerMessage = (ex.InnerException == null) ? null : ex.InnerException.Message;
                opStatus.ExceptionInnerStackTrace = (ex.InnerException == null) ? null : ex.InnerException.StackTrace;
            }
            return opStatus;
        }
        public JOperationResult ToJOperationResult()
        {
            if (!this.Status && this.Exception == null && this.RecordsAffected == 0)
                Status = true;

            return new JOperationResult() { Status = Status, Message = Message, RecordsAffected = RecordsAffected };
        }
    }

    public class JOperationResult
    {
        public bool Status { get; set; }

        public string Message { get; set; }

        public string Data { get; set; }

        public int RecordsAffected { get; set; }
    }
}
