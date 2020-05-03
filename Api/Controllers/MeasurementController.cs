using Api.Security;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementController : ControllerBase
    {
        private readonly IClaimsAccessor _claimsAccessor;
        private readonly IVeraDbRepository _repo;

        public MeasurementController(IClaimsAccessor claimsAccessor, IVeraDbRepository repo)
        {
            _claimsAccessor = claimsAccessor;
            _repo = repo;
        }

        [HttpPost()]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Upload([FromBody] UploadMeasurementRequest request)
        {
            Guid uploaderUserId = GetUserId();

            foreach (var measurement in request.Measurements)
            {
                await _repo.CreateMeasurement(measurement);
            }
            return Ok();
        }

        private Guid GetUserId()
        {
            if (_claimsAccessor.TryGetValue(ClientApiSecurity.Claims.UserToken, out string userId))
            {
                return new Guid(userId);
            }
            else
            {
                throw new InvalidTokenException();
            }
        }
    }
}