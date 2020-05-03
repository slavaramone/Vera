using Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    [Table("Measurements")]
    public class Measurement
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid UploaderUserId { get; set; }

        public User UploaderUser { get; set; }

        public DateTime CreationDate { get; set; }

        public MeasurementType MeasurementType { get; set; }

        public double ValueMin { get; set; }

        public double? ValueMax { get; set; }

        public string ValueText { get; set; }
    }
}
