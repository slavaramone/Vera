using Domain.Enums;
using System;

namespace Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Patronymic { get; set; }

        public DateTime BirthDate { get; set; }

        public UserRole Role { get; set; }

        public string Phone { get; set; }

        public User(Guid id, string lastName, string firstName, string patronymic, DateTime birthday, UserRole role, string phone)
        {
            Id = id;
            LastName = lastName;
            FirstName = firstName;
            Patronymic = patronymic;
            BirthDate = birthday;
            Role = role;
            Phone = phone;
        }
    }
}
