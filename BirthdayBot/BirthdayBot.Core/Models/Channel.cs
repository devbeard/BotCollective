namespace BirthdayBot.Core.Models
{
    public class Channel
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public string IsChannel { get; set; }

        public long Created { get; set; }

        public string Creator { get; set; }

        public bool? IsArchived { get; set; }

        public bool? IsGeneral { get; set; }

        public string[] Members { get; set; }

        public Topic Topic { get; set; }

        public Purpose Purpose { get; set; }

        public bool? IsMember { get; set; }

        public string LastRead { get; set; }

        public string Latest { get; set; }

        public int UnreadCount { get; set; }

        public int UnreadCountDisplay { get; set; }
    }
}
