using System.Configuration;
using System.Linq;
using System.Web.Http;
using BirthdayBot.Core.Repositories;

namespace BirthdayBot.Api.Controllers
{
    public class BirthdayController : ApiController
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

            // CASE 1: People with birthday today            
            var everyone = databaseController.GetAllPersonEntities().ToArray();
            var extraSpecialPeople = birthdayController.BirthdayTodayAndNotNotified(everyone).ToArray();
            slackController.PostCongratulations(extraSpecialPeople, channel);
            databaseController.SetCongratulated(extraSpecialPeople);

            return "BirthdayBot sier hei hei";
        }       
    }
}
