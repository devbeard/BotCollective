using System.Configuration;
using System.Linq;
using BirthdayBot.Core.Models;
using BirthdayBot.Core.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BirthdayBot.Tests
{
    [TestClass]
    public class SlackTests
    {
        [TestMethod]
        public void GetAllUsersFromSlack()
        {
            var token = ConfigurationManager.AppSettings["SlackToken"];
            var slack = new SlackRepo(token);

            var users = slack.GetAllUsers();

            Assert.IsTrue(users.Count() > 50);
        }

        [TestMethod]
        public void GetSingleNamedUserFromSlack()
        {
            const string username = "henrik";

            var token = ConfigurationManager.AppSettings["SlackToken"];
            var slack = new SlackRepo(token);
            var users = slack.GetAllUsers();

            var user = from u in users
                where u.Name == username
                select u;

            Assert.IsTrue(user.Count() == 1);
        }

        [TestMethod]
        public void GetAllChannelsFromSlack()
        {
            var token = ConfigurationManager.AppSettings["SlackToken"];
            var slack = new SlackRepo(token);

            var users = slack.GetAllChannels();

            Assert.IsTrue(users.Count() > 10);
        }

        [TestMethod]
        public void GetSingleNamedChannelFromSlack()
        {
            const string channelname = "fjas";
            var token = ConfigurationManager.AppSettings["SlackToken"];
            var slack = new SlackRepo(token);
            var channels = slack.GetAllChannels();

            var channel = from c in channels
                       where c.Name == channelname
                       select c;

            Assert.IsTrue(channel.Count() == 1);
        }

        [TestMethod]
        public void GetGeneralChannelFromSlack()
        {
            const string channelname = "general";
            var token = ConfigurationManager.AppSettings["SlackToken"];
            var slack = new SlackRepo(token);
            var channels = slack.GetAllChannels();

            var channel = from c in channels
                          where c.Name == channelname && c.IsGeneral.GetValueOrDefault()
                          select c;

            Assert.IsTrue(channel.Count() == 1);
        }

        [TestMethod]
        public void PostTestMessageToChannel()
        {
            const string channelname = "birthday_api";
            var token = ConfigurationManager.AppSettings["SlackToken"];
            var slack = new SlackRepo(token);

            var channels = slack.GetAllChannels();

            var channel = (from c in channels
                where c.Name == channelname
                select c).Single();

            var message = new Message()
            {
                Channel = channel.Id,
                Text = "Dette er en mention-test med @henrik og #fjas",
                Parse = "full"
            };

            slack.PostMessage(message);
        }
    }
}
