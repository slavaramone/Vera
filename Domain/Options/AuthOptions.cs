using System;

namespace Domain.Options
{
    public class AuthOptions
    {
        public string Key { get; set; }

        public TimeSpan? Lifetime { get; set; }
    }
}
