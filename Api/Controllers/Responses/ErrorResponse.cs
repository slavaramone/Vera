using Newtonsoft.Json;

namespace Api.Controllers
{
    public class ErrorResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
