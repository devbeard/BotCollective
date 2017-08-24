using System;
using System.Collections.Generic;
using System.Linq;
using BirthdayBot.Core.Models;
using Newtonsoft.Json;
using RestSharp;

namespace BirthdayBot.Core.Repositories
{
    public class SlackRepo
    {
        private readonly RestClient _client;
        private readonly string _token;
        private const string SlackBaseUrl = "https://slack.com/api/";

        public SlackRepo(string token)
        {
            _client = new RestClient(SlackBaseUrl);
            _token = token;
        }

        public IEnumerable<Channel> GetAllChannels()
        {
            var request = new RestRequest("channels.list", Method.POST);
            request.AddParameter("token", _token);

            var response = _client.Execute(request);
            var content = response.Content;

            dynamic slackResponse = JsonConvert.DeserializeObject(content);

            if (slackResponse.ok == false)
            {
                throw new Exception(slackResponse.error);
            }

            var list = new List<Channel>();

            foreach (var d in slackResponse.channels)
            {
                var channel = new Channel()
                {
                    Name = d.name,
                    Id = d.id,
                    IsGeneral = d.is_general
                };

                list.Add(channel);
            }

            return list;
        }

        private IEnumerable<Channel> _channels;

        public string GetChannelId(string channelName)
        {
            if (_channels == null)
            {
                _channels = GetAllChannels();
            }

            var channel = (from c in _channels
                           where c.Name == channelName
                           select c).Single();

            return channel.Id;
        }

        public IEnumerable<User> GetAllUsers()
        {
            var request = new RestRequest("users.list", Method.POST);

            request.AddParameter("token", _token);

            var response = _client.Execute(request);
            var content = response.Content;

            dynamic slackResponse = JsonConvert.DeserializeObject(content);

            if (slackResponse.ok == false)
            {
                throw new Exception(slackResponse.error);
            }

            var list = new List<User>();

            foreach (var d in slackResponse.members)
            {
                var user = new User()
                {
                    Id = d.id,
                    TeamId = d.team_id,
                    Name = d.name,
                    Deleted = d.deleted,
                    Color = d.color,
                    RealName = d.real_name,
                    Timezone = d.tz,
                    TimezoneLabel = d.tz_label,
                    TimezoneOffset = d.tz_offset,
                    Profile = new Profile()
                    {
                        Email = d.profile.email,
                        FirstName = d.profile.first_name,
                        Image192 = d.profile.image_192,
                        Image24 = d.profile.image_24,
                        Image32 = d.profile.image_32,
                        Image48 = d.profile.image_48,
                        Image72 = d.profile.image_72,
                        LastName = d.profile.last_name,
                        Phone = d.profile.phone,
                        RealName = d.profile.real_name
                    },
                    IsAdmin = d.is_admin,
                    IsOwner = d.is_owner,
                    IsPrimaryOwner = d.is_primary_owner,
                    IsRestricted = d.is_restricted,
                    IsUltraRestricted = d.is_ultra_restricted,
                    IsBot = d.is_bot,
                    IsAppUser = d.is_app_user,
                    HasFiles = d.has_files,
                    Updated = d.updated
                };

                list.Add(user);
            }

            return list;
        }

        public void PostMessage(Message message, bool asUser = true)
        {
            var request = new RestRequest("chat.postMessage", Method.POST);

            request.AddParameter("token", _token);
            request.AddObject(new { channel = message.Channel, parse = message.Parse, text = message.Text, as_user = asUser });

            _client.Execute(request);
        }

        public void PostCongratulations(IEnumerable<PersonEntity> people, string channel)
        {
            var pArr = people.ToArray();

            var date = DateTime.Today.ToString("dd.MM");

            var names = "";

            if (pArr.Length > 2)
            {
                var nc = pArr.Select(i => (i.GetMentionName())).ToArray();
                names = string.Join(", ", nc, 0, nc.Length - 1) + " og " + nc.Last();
            }
            else if (pArr.Length == 2)
            {
                names = $"{pArr.First().GetMentionName()} og {pArr.Last().GetMentionName()}";
            }
            else if (pArr.Length == 1)
            {
                names = $"{pArr.First().GetMentionName()}";
            }

            if (string.IsNullOrEmpty(names))
            {
                return;
            }

            var text = $"God morgen, mennesker! I dag, {date}, har {names} bursdag. Gratulerer så mye med dagen! :birthday: :flag-no: :champagne: :robot_face: BEEP BOOP";

            var message = new Message()
            {
                Channel = GetChannelId(channel),
                Text = text,
                Parse = "full"
            };

            PostMessage(message);
        }

        public void PostEarlyCongratulations(IEnumerable<PersonEntity> people, string channel)
        {
            var pArr = people.ToArray();

            if (!pArr.Any())
            {
                return;
            }

            var startText = $"BEEP BOOP. Det er fredag {DateTime.Today:dd.MM}, og vi har noen med bursdag i helgen som kommer:";

            var texts = new List<string>();

            foreach (var p in pArr)
            {
                if (p.Birthday.HasValue && p.ThisYearsBirthday().DayOfWeek == DayOfWeek.Saturday)
                {
                    texts.Add($"På lørdag har {p.GetMentionName()} bursdag!");
                }
                else if (p.Birthday.HasValue && p.ThisYearsBirthday().DayOfWeek == DayOfWeek.Sunday)
                {
                    texts.Add($"På søndag har {p.GetMentionName()} bursdag!");
                }
            }

            if (!texts.Any())
            {
                return;
            }

            var message = new Message()
            {
                Channel = GetChannelId(channel),
                Text = startText,
                Parse = "full"
            };

            PostMessage(message);

            foreach (var s in texts)
            {
                var m = new Message()
                {
                    Channel = GetChannelId(channel),
                    Text = s,
                    Parse = "full"
                };

                PostMessage(m);
            }

            var end = new Message()
            {
                Channel = GetChannelId(channel),
                Text = ">I need your clothes, your boots, and your motorcycle.",
                Parse = "full"
            };

            PostMessage(end);

            var end2 = new Message()
            {
                Channel = GetChannelId(channel),
                Text = "God helg!",
                Parse = "full"
            };

            PostMessage(end2);
        }

        public void PostMessage(string message, string channelName)
        {
            var m = new Message()
            {
                Channel = GetChannelId(channelName),
                Text = message,
                Parse = "full"
            };

            PostMessage(m);
        }

        public void Delete(string ts, string general)
        {
            var request = new RestRequest("chat.update", Method.POST);

            request.AddParameter("token", _token);
            request.AddParameter("ts", ts);
            request.AddParameter("channel", GetChannelId(general));
            request.AddParameter("text", "***************");

            _client.Execute(request);            
        }

        public void NotifyManagersOfAnniversary(PersonEntity[] roundDay, string[] managers)
        {
            var managerListString = "";

            foreach (var m in managers)
            {
                if (managerListString == "")
                {
                    managerListString = m;
                }
                else
                {
                    managerListString += ", " + m;
                }
            }

            foreach (var message in roundDay.SelectMany(p => managers.Select(manager => new Message()
            {
                Channel = manager,
                Text = $"Hei {manager}! {p.Name} har rund dag om få dager. Fødselsdag er {p.Birthday.GetValueOrDefault():dd.MM.yyyy}, og {(p.Gender == "Male"?"han":"hun")} fyller {p.Age} år. Denne meldingen er sendt automatisk til {managerListString}. Dersom den er feil eller du ikke ønsker motta lengre, ta kontakt med @henrik eller @ingrid.",
                Parse = "full"
            })))
            {
                PostMessage(message);
            }
        }

        
    }
}
