using Domain.Enums;
using Domain.Utils;

namespace Domain.Exceptions
{
    public class SmsCodeOutdatedException : BusinessErrorException
    {
        public SmsCodeOutdatedException() : base(ErrorCode.SmsCodeOutdated, Common.GetAttrDescription(ErrorCode.SmsCodeOutdated))
        { }
    }
}
