using Domain.Models;
using System;
using System.Collections.Generic;

namespace Api.Controllers
{
    public class UploadMeasurementRequest
    {
        public List<Measurement> Measurements { get; set; }

        public Guid UserId { get; set; }
    }
}
