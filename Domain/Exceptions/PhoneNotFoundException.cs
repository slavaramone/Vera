using Domain.Enums;
using Domain.Utils;

namespace Domain.Exceptions
{
    public class PhoneNotFoundException : BusinessErrorException
    {
        public PhoneNotFoundException() : base(ErrorCode.PhoneNotFound, Common.GetAttrDescription(ErrorCode.PhoneNotFound))
        { }
    }
}