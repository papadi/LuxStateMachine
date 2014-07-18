using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

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
            this.ActionName = info.GetString("ActionName");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException("info");
            info.AddValue("ActionName", this.ActionName);
            base.GetObjectData(info, context);
        }
    }
}
