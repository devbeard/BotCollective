using Newtonsoft.Json;

namespace BirthdayBot.Core.Models
{
    public class Message
    {
        [JsonProperty(PropertyName = "channel")]
        public string Channel { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "parse")]
        public string Parse { get; set; }       
    }
}
