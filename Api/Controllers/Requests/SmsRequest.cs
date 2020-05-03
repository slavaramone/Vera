using System;

namespace Api.Controllers
{
    public class SmsRequest
    {
        public Guid SessionId { get; set; }

        public string SmsCode { get; set; }
    }
}
