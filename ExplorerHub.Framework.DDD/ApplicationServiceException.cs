using System;

namespace ExplorerHub.Framework.Domain
{
    public class ApplicationServiceException : Exception
    {
        public string ApplicationName { get; }
        public string MethodName { get; }
        public string ErrorMessage { get; }

        public ApplicationServiceException(string applicationName, string methodName, string errorMessage)
        {
            ApplicationName = applicationName;
            MethodName = methodName;
            ErrorMessage = errorMessage;
        }

        public override string Message => $"应用程序服务异常。服务名: {ApplicationName}.{MethodName}\n{ErrorMessage}";
    }
}
