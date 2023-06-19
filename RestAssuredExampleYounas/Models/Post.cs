using Newtonsoft.Json;

namespace RestAssuredExampleYounas.Models
{
    internal class Post
    {
        [JsonProperty("createdUserId")]
        public string CreatedUserId { get; set; } = string.Empty;

        [JsonProperty("firstName")]
        public string FirstName { get; set; } = string.Empty;
    }
}
