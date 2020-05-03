using Api.Security;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IVeraDbRepository _repo;
        private readonly ISmsSender _smsSender;
        private readonly AuthOptions _authOptions;
        private readonly JwtFactory _jwtFactory;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public AuthController(IVeraDbRepository repo, ISmsSender smsSender, IOptions<AuthOptions> authOptions, JwtFactory jwtFactory,
            ILogger<AuthController> logger, IConfiguration configuration)
        {
            _repo = repo;
            _smsSender = smsSender;
            _jwtFactory = jwtFactory;
            _authOptions = authOptions.Value;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        public async Task<IActionResult> Auth([FromBody] AuthRequest request)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;    

            var user = await _repo.GetUser(request.LastName, request.FirstName, request.MiddleName, request.Phone);
            
            string smsCode = "000000";
            bool isSent = true;
            //if (!request.FirstName.Equals("test1") && !request.FirstName.Equals("test2"))
            //{
            //	smsCode = Common.GenerateSmsCode();
            //	isSent = await _smsSender.SendAsync(request.Phone, $"Код для входа в личный кабинет {smsCode}");
            //}

            if (isSent)
            {
                Guid sessionId = await _repo.CreateUserSmsSession(user.Id, smsCode);
                var response = new AuthResponse
                {
                    SessionId = sessionId
                };

                _logger.LogWarning($"Sms session created: Phone={request.Phone} SessionId={sessionId}");

                return Ok(response);
            }
            else
            {
                throw new SmsSendingFailedException();
            }
        }

        /// <summary>
        /// Отправка кода подтверждения
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("smscode")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(SmsResponse), 200)]
        public async Task<IActionResult> SmsCode([FromBody] SmsRequest request)
        {
            
			var validateSmsCodeResult = await _repo.ValidateSmsCode(request.SessionId, request.SmsCode.ToString());

            if (validateSmsCodeResult.IsSmsCodeValid)
			{
                var debtorIdClaim = new Claim(ClientApiSecurity.Claims.UserToken, validateSmsCodeResult.DebtorId.ToString());
                var canImportDataClaim = new Claim(ClientApiSecurity.Claims.CanImportData, "false");

                string token = _jwtFactory.Create(_authOptions.Key, debtorIdClaim, canImportDataClaim);
                var response = new SmsResponse
                {
                    Token = token
                };

                _logger.LogWarning($"Auth token issued: SessionId={request.SessionId}");

                return Ok(response);
            }
			else
			{
                throw new InvalidSmsCodeException();
			}
        }
    }
}
