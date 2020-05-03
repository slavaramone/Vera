using Api.Security;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementController : ControllerBase
    {
        private readonly IClaimsAccessor _claimsAccessor;

        public MeasurementController(IClaimsAccessor claimsAccessor)
        {
            _claimsAccessor = claimsAccessor;
        }

        [HttpPost()]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public IActionResult Upload([FromBody] UploadMeasurementRequest request)
        {
            Guid uploaderUserId = GetUserId();

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