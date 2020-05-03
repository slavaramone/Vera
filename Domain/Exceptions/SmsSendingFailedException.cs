using Domain.Enums;
using Domain.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class SmsSendingFailedException : BusinessErrorException
    {
        public SmsSendingFailedException() : base(ErrorCode.SmsSendingFailed, Common.GetAttrDescription(ErrorCode.SmsSendingFailed))
        { }
    }
}
