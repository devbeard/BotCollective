using System.Configuration;
using System.Linq;
using System.Web.Http;
using BirthdayBot.Core.Repositories;

namespace BirthdayBot.Api.Controllers
{
    [Authorize]
    public class AnniversaryController : ApiController
    {
        // GET api/values
        public string Get()
        {
            var slacktoken = ConfigurationManager.AppSettings["SlackToken"];       
            var peopleToAlert = ConfigurationManager.AppSettings["AnniversaryAlertList"].Split(';');
            var partition = ConfigurationManager.AppSettings["PartitionKey"];

            var connectionString = ConfigurationManager.ConnectionStrings["BirthdayTableCstr"].ConnectionString;
            var databaseController = new DatabaseController(connectionString, partition);
            var slackController = new SlackRepo(slacktoken);
            var birthdayController = new Core.Repositories.BirthdayController();

            // CASE 3: Someone is 20,30,40,50,60,70 in 14 days
            var everyone = databaseController.GetAllPersonEntities().ToArray();
            var roundDay = birthdayController.AnniversaryUpcomingIn14Days(everyone).ToArray();
            
            slackController.NotifyManagersOfAnniversary(roundDay, peopleToAlert);

            return "BirthdayBot feirer når noen har rund dag!";
        }
    }
}
