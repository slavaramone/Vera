using Domain.Enums;
using System;

namespace Domain.Models
{
    public class User
    {
        public Guid Id { get; private set; }

        public string LastName { get; private set; }

        public string FirstName { get; private set; }

        public string Patronymic { get; private set; }

        public DateTime BirthDate { get; private set; }

        public UserRole Role { get; private set; }

        public string Phone { get; private set; }

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
