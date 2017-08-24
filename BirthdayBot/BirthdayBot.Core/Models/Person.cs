using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.WindowsAzure.Storage.Table;

namespace BirthdayBot.Core.Models
{
    
    public class PersonEntity : TableEntity
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string SlackUserName { get; set; }

        [Required]
        public DateTime? Birthday { get; set; }
        
        public DateTime? LastCongratulation { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 4)]
        public string Gender { get; set; }

        public bool Active { get; set; }

        public DateTime ThisYearsBirthday()
        {
            return new DateTime(
                DateTime.Today.Year, 
                Birthday.GetValueOrDefault().Month, 
                Birthday.GetValueOrDefault().Day);
        }

        [ScaffoldColumn(false)]
        public int Age => DateTime.Now.Year - Birthday.GetValueOrDefault(DateTime.Today).Year;

        public string GetMentionName()
        {
            if (string.IsNullOrEmpty(SlackUserName))
            {
                return Name;
            }

            if (!SlackUserName.StartsWith("@"))
            {
                return "@"+SlackUserName;
            }

            return SlackUserName;
        }
    }
}