namespace BirthdayBot.Core.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool? Deactivated => Deleted;
        public bool? Deleted { get; set; }
        public string Color { get; set; }
        public Profile Profile { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsOwner { get; set; }
        public bool? IsPrimaryOwner { get; set; }
        public bool? IsRestricted { get; set; }
        public bool? IsUltraRestricted { get; set; }
        public bool? HasTwoFactorAuthentication { get; set; }
        public string TwoFactorType { get; set; }
        public bool? HasFiles { get; set; }
        public string TeamId { get; set; }
        public string RealName { get; set; }
        public string Timezone { get; set; }
        public string TimezoneLabel { get; set; }
        public long? TimezoneOffset { get; set; }
        public bool? IsBot { get; set; }
        public bool? IsAppUser { get; set; }
        public long? Updated { get; set; }
    }
}
