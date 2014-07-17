using System;
using System.Runtime.Serialization;

namespace LuxStateMachine
{
    [Serializable]
    public class InvalidActionException : ApplicationException
    {
        public string ActionName { get; set; }

        public InvalidActionException(string actionName)
        {
            this.ActionName = actionName;
        }

        public InvalidActionException(string actionName, string message) : base(message)
        {
            this.ActionName = actionName;
        }

        protected InvalidActionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
