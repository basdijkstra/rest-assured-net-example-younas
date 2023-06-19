using Newtonsoft.Json;

namespace RestAssuredExampleYounas.Models
{
    internal class PostWrapper
    {
        [JsonProperty("data")]
        public Post? Data { get; set; }
    }
}
