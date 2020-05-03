using System;

namespace Domain.Models
{
    public class ValidateSmsCodeResult
    {
        public bool IsSmsCodeValid { get; private set; }

        public Guid SessionId { get; private set; }

        public Guid DebtorId { get; private set; }

        public ValidateSmsCodeResult(bool isSmsCodeValid)
        {
            IsSmsCodeValid = isSmsCodeValid;
        }

        public ValidateSmsCodeResult(bool isSmsCodeValid, Guid sessionId, Guid debtorId)
            : this(isSmsCodeValid)
        {
            SessionId = sessionId;
            DebtorId = debtorId;
        }
    }
}
