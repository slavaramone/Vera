using Domain.Enums;
using System;

namespace Domain.Models
{
    public class Measurement
    {
        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public Guid UploaderUserId { get; private set; }

        public DateTime CreationDate { get; private set; }

        public MeasurementType MeasurementType { get; private set; }

        public double ValueMin { get; private set; }

        public double? ValueMax { get; private set; }

        public string ValueText { get; private set; }

        public Measurement(Guid id, Guid userId, Guid uploaderUserId, DateTime creationDate, MeasurementType measurementType, double valueMin, double? valueMax = null, string valueText = null)
        {
            Id = id;
            UserId = userId;
            UploaderUserId = uploaderUserId;
            CreationDate = creationDate;
            MeasurementType = measurementType;
            ValueMin = valueMin;
            ValueMax = valueMax;
            ValueText = valueText;
        }
    }
}
