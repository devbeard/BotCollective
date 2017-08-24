using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BirthdayBot.Core.Models;
using BirthdayBot.Core.Repositories;
// ReSharper disable InconsistentNaming

namespace BirthdayBot.Api.Controllers
{
    public class BirthdayWebController : Controller
    {
        // GET: Birthday
        public ActionResult Index(string Message, bool? Success)
        {
            if (Message != null && Success != null)
            {
                ViewBag.Message = Message;
                ViewBag.Success = Success.Value;
            }

            var connectionString = ConfigurationManager.ConnectionStrings["BirthdayTableCstr"].ConnectionString;
            var partition = ConfigurationManager.AppSettings["PartitionKey"];

            var databaseController = new DatabaseController(connectionString, partition);
            
            var people =
                databaseController.GetAllPersonEntities()
                    .OrderBy(i => (i.Active ? "A" : "B"))
                    .ThenBy(i => i.Name)
                    .ToArray();
            return View(people);
        }

        // GET: Birthday/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Birthday/Create
        [HttpPost]
        public ActionResult Create(PersonEntity person, string save, string cancel, FormCollection collection)
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["BirthdayTableCstr"].ConnectionString;
                var partition = ConfigurationManager.AppSettings["PartitionKey"];

                if (string.IsNullOrEmpty(save))
                {
                    return RedirectToAction("Index", new { Message = "Avbrutt opprettelse", Success = false });
                }

                if (person.Name == null || person.Birthday == null)
                {
                    throw new HttpException(400, "Bad request. Name and birthday must be set");
                }

                person.Active = true;
                person.RowKey = Guid.NewGuid().ToString();
                person.LastCongratulation = DateTime.Now.AddYears(-10);
                person.PartitionKey = partition;
                person.Name = person.Name.Trim();
                person.SlackUserName = person.SlackUserName.Trim();                

                var databaseController = new DatabaseController(connectionString, partition);
                databaseController.InsertOrReplacePersonEntity(person);

                return RedirectToAction("Index", new {Message = $"Opprettet {person.Name}", Success = true});
            }
            catch (HttpException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new HttpException(500, "Error occured: " + e.Message);
            }
        }

        // GET: Birthday/Edit/5
        public ActionResult Edit(string RowKey)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["BirthdayTableCstr"].ConnectionString;
            var partition = ConfigurationManager.AppSettings["PartitionKey"];

            var databaseController = new DatabaseController(connectionString, partition);
            var people = databaseController.GetAllPersonEntities().Where(p => p.RowKey == RowKey).ToArray();

            if (!people.Any())
            {
                throw new HttpException(404, "There was no person with row key " + RowKey);
            }

            return View(people.First());
        }

        // POST: Birthday/Edit/5
        [HttpPost]
        public ActionResult Edit(string RowKey, string Name, string save, string cancel, DateTime? Birthday, string Gender, string SlackUserName, bool? Active, FormCollection collection)
        {
            try
            {
                if (string.IsNullOrEmpty(save) || RowKey == null)
                {
                    return RedirectToAction("Index");
                }

                if (Name == null || Birthday == null)
                {
                    throw new HttpException(400, "Bad request. Name and birthday must be set");
                }

                var connectionString = ConfigurationManager.ConnectionStrings["BirthdayTableCstr"].ConnectionString;
                var partition = ConfigurationManager.AppSettings["PartitionKey"];

                var databaseController = new DatabaseController(connectionString, partition);
                var people = databaseController.GetAllPersonEntities().Where(p => p.RowKey == RowKey).ToArray();
                var person = people.Single();

                if (!string.IsNullOrEmpty(Name))
                {
                    person.Name = Name.Trim();
                }

                person.Birthday = Birthday;

                if (!string.IsNullOrEmpty(Gender))
                {
                    person.Gender = Gender;
                }

                if (!string.IsNullOrEmpty(SlackUserName))
                {
                    person.SlackUserName = SlackUserName.Trim();
                }

                if (Active != null)
                {
                    person.Active = Active.Value;
                }

                databaseController.InsertOrReplacePersonEntity(person);                    

                return RedirectToAction("Index", new { Message = $"Oppdaterte {person.Name}", Success = true });
            }
            catch (HttpException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new HttpException(500, "Error occured: " + e.Message);
            }
        }
    }
}
