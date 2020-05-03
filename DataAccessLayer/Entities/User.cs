using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Patronymic { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }

        public virtual List<UserSmsSession> UserSessions { get; set; }
    }
}
