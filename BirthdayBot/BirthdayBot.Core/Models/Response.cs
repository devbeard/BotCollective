using RestSharp.Deserializers;

namespace BirthdayBot.Core.Models
{
    public class UsersResponse
    {
        [DeserializeAs(Name = "ok")]
        public bool Ok { get; set; }

        [DeserializeAs(Name = "error")]
        public string Error { get; set; }

        [DeserializeAs(Name = "warning")]
        public string Warning { get; set; }

        [DeserializeAs(Name = "content")]
        public dynamic Content { get; set; }
    }
}
