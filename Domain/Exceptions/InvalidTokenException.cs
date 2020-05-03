using Domain.Enums;
using Domain.Utils;

namespace Domain.Exceptions
{
    public class InvalidTokenException : BusinessErrorException
    {
        public InvalidTokenException() : base(ErrorCode.InvalidToken, Common.GetAttrDescription(ErrorCode.InvalidToken))
        { }
    }
}
