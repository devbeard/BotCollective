using System.Configuration;
using System.Web.Http;
using BirthdayBot.Core.Repositories;

namespace BirthdayBot.Api.Controllers
{
    public class MessageController : ApiController
    {
        // GET api/values
        public string Get([FromUri] string message)
        {
            // Generic message posted as BirthdayBot
            // Examples: "Jeg hører menneskene @henrik og @ingrid fikk gavekort for bursdags - bot og flagging. BEEP BOOP. Minner om at det er jeg som står for det automatiske grovarbeidet hver dag."

            var slacktoken = ConfigurationManager.AppSettings["SlackToken"];
            var channel = ConfigurationManager.AppSettings["AnnouncementChannel"];

            var slackController = new SlackRepo(slacktoken);

            // POST AD HOC CUSTOM MESSAGE
            if (string.IsNullOrEmpty(message))
            {
                return "Custom message was empty";                
            }

            slackController.PostMessage(message, channel);
            return "Custom message was posted to channel";
        }
    }
}
