using Domain.Enums;
using System;

namespace Domain.Exceptions
{
    public class BusinessErrorException : Exception
    {
		public BusinessErrorException(ErrorCode code, string message) : base(message)
		{
			Code = code;
		}

		public BusinessErrorException(ErrorCode code, string message, string additionalInfo) : base(GetMessage(message, additionalInfo))
        {
            Code = code;
        }

        public BusinessErrorException(ErrorCode code, string message, object details) : base(GetMessage(message, details))
        {
            Code = code;
            Details = details;
        }

		private static string GetMessage(string message, string additionalInfo)
		{
			return $"{message} ({additionalInfo})";
		}

		private static string GetMessage(string message, object details)
		{
			return $"{message} ({details})";
		}

		public ErrorCode Code { get; private set; }

        public object Details { get; private set; }
    }
}
