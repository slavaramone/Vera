using Domain.Enums;
using Domain.Utils;

namespace Domain.Exceptions
{
    public class SmsDelayException : BusinessErrorException
    {
        public SmsDelayException() : base(ErrorCode.SmsHasDelay, Common.GetAttrDescription(ErrorCode.SmsHasDelay))
        { }
        public SmsDelayException(string additionalMessage) : base(ErrorCode.SmsHasDelay, Common.GetAttrDescription(ErrorCode.SmsHasDelay) + " " + additionalMessage)
        { }
    }
}