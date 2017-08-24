using System;
using System.Configuration;
using System.Linq;
using BirthdayBot.Core.Models;
using BirthdayBot.Core.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BirthdayBot.Tests
{
    [TestClass]
    public class DatabaseTests
    {
        [TestMethod]
        public void Test_GetAllPeople()
        {
            var cs = ConfigurationManager.ConnectionStrings["BirthdayTableCstr"].ConnectionString;
            var partition = ConfigurationManager.AppSettings["PartitionKey"];

            var controller = new DatabaseController(cs, partition);

            var people = controller.GetAllPersonEntities();

            Assert.IsNotNull(people);
            Assert.IsTrue(people.Any());
        }

        [TestMethod]
        public void Test_NoneAreAgedZero()
        {
            var cs = ConfigurationManager.ConnectionStrings["BirthdayTableCstr"].ConnectionString;
            var partition = ConfigurationManager.AppSettings["PartitionKey"];            

            var controller = new DatabaseController(cs, partition);

            var people = controller.GetAllPersonEntities();

            var agedZero = from p in people where p.Age == 0 select p;

            var personEntities = agedZero as PersonEntity[] ?? agedZero.ToArray();
            foreach (var p in personEntities)
            {
                Console.WriteLine($"{p.Name};{p.Birthday}");
            }

            Assert.Equals(personEntities.Length, 0);
        }
    }
}