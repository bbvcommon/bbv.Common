namespace bbv.Common.EventBroker
{
    using System;
    using System.Reflection;

    public static class ExceptionExensionMethods
    {
        public static void PreserveStackTrace(this TargetInvocationException targetInvocationException)
        {
            var remoteStackTraceString = typeof(Exception).GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);
         
            remoteStackTraceString.SetValue(targetInvocationException.InnerException, targetInvocationException.InnerException.StackTrace + Environment.NewLine);
        }
    }
}