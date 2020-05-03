using System;

namespace Api.Controllers
{
    public class AuthRequest
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string Phone { get; set; }
    }
}
