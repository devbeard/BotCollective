using System;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using BirthdayBot.Core.Models;
using BirthdayBot.Core.Repositories;

namespace BirthdayBot.Api.Controllers
{
    public class MissingPersonController : ApiController
    {
        public string Get()
        {
            var slacktoken = ConfigurationManager.AppSettings["SlackToken"];
            var connectionString = ConfigurationManager.ConnectionStrings["BirthdayTableCstr"].ConnectionString;
            var partition = ConfigurationManager.AppSettings["PartitionKey"];

            var databaseController = new DatabaseController(connectionString, partition);
            var slackController = new SlackRepo(slacktoken);

            var allSlackUsers = slackController.GetAllUsers().ToList();
            var allDatabaseEntries = databaseController.GetAllPersonEntities().ToList();

            var existingUsers = (from u in allSlackUsers
                                 where allDatabaseEntries.Where(i => !string.IsNullOrEmpty(i.SlackUserName)).Select(i => i.SlackUserName.Replace("@", "").Trim()).Contains(u.Name)
                                 select u).ToList();

            var missingUsers = allSlackUsers.Except(existingUsers).ToList();

            var counter = 0;

            foreach (var missingUser in missingUsers)
            {
                if (missingUser.IsBot.GetValueOrDefault())
                {
                    continue;
                }
                if (missingUser.IsRestricted.GetValueOrDefault())
                {
                    continue;
                }
                if (missingUser.IsUltraRestricted.GetValueOrDefault())
                {
                    continue;
                }
                if (missingUser.Deactivated.GetValueOrDefault())
                {
                    continue;
                }
                if (missingUser.Deleted.GetValueOrDefault())
                {
                    continue;
                }
                if (missingUser.Name.Equals("slackbot"))
                {
                    continue;
                }

                var person = new PersonEntity
                {
                    Name = missingUser.RealName ?? missingUser.Name,
                    SlackUserName = missingUser.Name,
                    Active = false,                    
                    Gender = null,
                    Birthday = new DateTime(1900, 01, 01),
                    LastCongratulation = new DateTime(1900, 01, 01),
                    PartitionKey = partition,
                    RowKey = Guid.NewGuid().ToString()
                };

                databaseController.InsertOrReplacePersonEntity(person);

                counter++;
            }

            return $"Added {counter} new entries to the database";
        }
    }
}
