using Domain.Enums;
using Domain.Utils;

namespace Domain.Exceptions
{
    public class InvalidSmsCodeException : BusinessErrorException
    {
        public InvalidSmsCodeException() : base(ErrorCode.InvalidSmsCode, Common.GetAttrDescription(ErrorCode.InvalidSmsCode))
        { }
    }
}
