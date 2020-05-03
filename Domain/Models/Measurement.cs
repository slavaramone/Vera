using Domain.Enums;
using System;

namespace Domain.Models
{
    public class Measurement
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid UploaderUserId { get; set; }

        public DateTime CreationDate { get; set; }

        public MeasurementType MeasurementType { get; set; }

        public double ValueMin { get; set; }

        public double? ValueMax { get; set; }

        public string ValueText { get; set; }

        public Measurement(Guid id, Guid userId, Guid uploaderUserId, MeasurementType measurementType, double valueMin, double? valueMax = null, string valueText = null)
        {
            Id = id;
            UserId = userId;
            UploaderUserId = uploaderUserId;
            CreationDate = DateTime.Now;
            MeasurementType = measurementType;
            ValueMin = valueMin;
            ValueMax = valueMax;
            ValueText = valueText;
        }
    }
}
