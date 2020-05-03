using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Domain.Utils
{
    public class SmsDeliveryService : ISmsSender
    {
        private readonly ILogger _logger;
        private readonly SmscOptions _options;
        private readonly HttpClient _httpClient;

        public SmsDeliveryService(ILogger<SmsDeliveryService> logger, IOptions<SmscOptions> options)
        {
            _logger = logger;
            _options = options.Value;
            _httpClient = new HttpClient();
        }

        public async Task<bool> SendAsync(string phone, string message)
        {
            phone = phone.Replace("+", "");
            var url = $"{_options.Host}?login={_options.Login}&psw={_options.Password}&phones={phone}&mes={message}&charset=utf-8";

            _logger.LogInformation(url);

            var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK || content.Contains("ERROR"))
            {
                throw new SmsSendingFailedException();
            }
            return true;
        }
    }
}
