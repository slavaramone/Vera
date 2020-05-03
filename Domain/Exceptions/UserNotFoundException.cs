using Domain.Enums;
using Domain.Utils;

namespace Domain.Exceptions
{
    public class UserNotFoundException : BusinessErrorException
    {
        public UserNotFoundException() : base(ErrorCode.UserNotFound, Common.GetAttrDescription(ErrorCode.UserNotFound))
        { }
    }
}
