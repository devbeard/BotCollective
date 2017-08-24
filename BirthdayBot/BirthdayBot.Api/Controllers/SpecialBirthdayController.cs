using System.Configuration;
using System.Linq;
using System.Web.Http;
using BirthdayBot.Core.Repositories;

namespace BirthdayBot.Api.Controllers
{
    public class SpecialBirthdayController : ApiController
    {
        // GET api/values
        public string Get()
        {
            var slacktoken = ConfigurationManager.AppSettings["SlackToken"];
            var channel = ConfigurationManager.AppSettings["AnnouncementChannel"];

            var connectionString = ConfigurationManager.ConnectionStrings["BirthdayTableCstr"].ConnectionString;
            var partition = ConfigurationManager.AppSettings["PartitionKey"];

            var databaseController = new DatabaseController(connectionString, partition);
            var slackController = new SlackRepo(slacktoken);
            var birthdayController = new Core.Repositories.BirthdayController();

            // CASE 2: It is friday, and someone has a birthday on saturday or sunday
            var everyone = databaseController.GetAllPersonEntities().ToArray();
            var specialPeople = birthdayController.BirthdayThisWeekendAndNotNotified(everyone).ToArray();
            slackController.PostEarlyCongratulations(specialPeople, channel);
            databaseController.SetCongratulated(specialPeople);

            return "BirthdayBot har utført helgevarsling";
        }       
    }
}
