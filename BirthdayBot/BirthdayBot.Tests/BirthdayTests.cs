using System;
using System.Linq;
using BirthdayBot.Core.Models;
using BirthdayBot.Core.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BirthdayBot.Tests
{
    [TestClass]
    public class BirthdayTests
    {

        [TestMethod]
        public void TestBirthdayThisWeekendZero()
        {
            var list = new[]
            {
                new PersonEntity()
                {
                    Name = "Test 1",
                    Birthday = GetNextDayOfWeek(DayOfWeek.Friday),
                    SlackUserName = "henrik",
                    Active = true,
                    PartitionKey = "",
                    Gender = "male",
                    LastCongratulation = DateTime.MinValue,
                    RowKey = ""
                }
            };

            var controller = new BirthdayController();

            var results = controller.BirthdayThisWeekendAndNotNotified(list);

            Assert.IsTrue(!results.Any());
        }

        [TestMethod]
        public void TestBirthdayThisWeekendOne()
        {
            var list = new[]
            {
                new PersonEntity()
                {
                    Name = "Test 1",
                    Birthday = GetNextDayOfWeek(DayOfWeek.Saturday),
                    SlackUserName = "test1",
                    Active = true,
                    PartitionKey = "",
                    Gender = "male",
                    LastCongratulation = DateTime.MinValue,
                    RowKey = ""
                },
                new PersonEntity()
                {
                    Name = "Test 2",
                    Birthday = GetNextDayOfWeek(DayOfWeek.Friday),
                    SlackUserName = "test2",
                    Active = true,
                    PartitionKey = "",
                    Gender = "male",
                    LastCongratulation = DateTime.MinValue,
                    RowKey = ""
                },
                new PersonEntity()
                {
                    Name = "Test 3",
                    Birthday = GetNextDayOfWeek(DayOfWeek.Saturday),
                    SlackUserName = "test3",
                    Active = true,
                    PartitionKey = "",
                    Gender = "male",
                    LastCongratulation = DateTime.MaxValue,
                    RowKey = ""
                }
            };

            var controller = new BirthdayController();

            var results = controller.BirthdayThisWeekendAndNotNotified(list);

            if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
                Assert.IsTrue(results.Count() == 1);
            else
            {
                Assert.IsTrue(!results.Any());
            }
        }

        [TestMethod]
        public void Test_OnePersonHasAnniversaryIn14Days()
        {
            var list = new[]
            {
                new PersonEntity()
                {
                    Name = "Rund Dagesen 1",
                    Birthday = DateTime.Today.AddDays(14).AddYears(-50),
                    SlackUserName = "@rund1",
                    Active = true,
                    PartitionKey = "test-partition",
                    Gender = "male",
                    LastCongratulation = DateTime.MinValue,
                    RowKey = Guid.NewGuid().ToString()
                },
                new PersonEntity()
                {
                    Name = "Rund Dagesen 2",
                    Birthday = DateTime.Today.AddDays(15).AddYears(-50),
                    SlackUserName = "@rund2",
                    Active = true,
                    PartitionKey = "test-partition",
                    Gender = "male",
                    LastCongratulation = DateTime.MinValue,
                    RowKey = Guid.NewGuid().ToString()
                },
                new PersonEntity()
                {
                    Name = "Rund Dagesen 3",
                    Birthday = DateTime.Today.AddDays(16).AddYears(-50),
                    SlackUserName = "@rund3",
                    Active = true,
                    PartitionKey = "test-partition",
                    Gender = "male",
                    LastCongratulation = DateTime.MinValue,
                    RowKey = Guid.NewGuid().ToString()
                },
            };

            var controller = new BirthdayController();

            var results = controller.AnniversaryUpcomingIn14Days(list);

            Assert.IsTrue(results.Count() == 1);
        }

        [TestMethod]
        public void Test_TwoPeopleHasAnniversaryIn14Days()
        {
            var list = new[]
            {
                new PersonEntity()
                {
                    Name = "Rund Dagesen 1",
                    Birthday = DateTime.Today.AddDays(14).AddYears(-50),
                    SlackUserName = "@rund1",
                    Active = true,
                    PartitionKey = "test-partition",
                    Gender = "male",
                    LastCongratulation = DateTime.MinValue,
                    RowKey = Guid.NewGuid().ToString()
                },
                new PersonEntity()
                {
                    Name = "Rund Dagesen 2",
                    Birthday = DateTime.Today.AddDays(14).AddYears(-30),
                    SlackUserName = "@rund2",
                    Active = true,
                    PartitionKey = "test-partition",
                    Gender = "male",
                    LastCongratulation = DateTime.MinValue,
                    RowKey = Guid.NewGuid().ToString()
                },
                new PersonEntity()
                {
                    Name = "Rund Dagesen 3",
                    Birthday = DateTime.Today.AddDays(16).AddYears(-50),
                    SlackUserName = "@rund3",
                    Active = true,
                    PartitionKey = "test-partition",
                    Gender = "male",
                    LastCongratulation = DateTime.MinValue,
                    RowKey = Guid.NewGuid().ToString()
                },
            };

            var controller = new BirthdayController();

            var results = controller.AnniversaryUpcomingIn14Days(list);

            Assert.IsTrue(results.Count() == 2);
        }

        [TestMethod]
        public void Test_NoPeopleHasAnniversaryIn14Days()
        {
            var list = new[]
            {
                new PersonEntity()
                {
                    Name = "Rund Dagesen 1",
                    Birthday = DateTime.Today.AddDays(14).AddYears(-55),
                    SlackUserName = "@rund1",
                    Active = true,
                    PartitionKey = "test-partition",
                    Gender = "male",
                    LastCongratulation = DateTime.MinValue,
                    RowKey = Guid.NewGuid().ToString()
                },
                new PersonEntity()
                {
                    Name = "Rund Dagesen 2",
                    Birthday = DateTime.Today.AddDays(11).AddYears(-30),
                    SlackUserName = "@rund2",
                    Active = true,
                    PartitionKey = "test-partition",
                    Gender = "male",
                    LastCongratulation = DateTime.MinValue,
                    RowKey = Guid.NewGuid().ToString()
                },
                new PersonEntity()
                {
                    Name = "Rund Dagesen 3",
                    Birthday = DateTime.Today.AddDays(16).AddYears(-50),
                    SlackUserName = "@rund3",
                    Active = true,
                    PartitionKey = "test-partition",
                    Gender = "male",
                    LastCongratulation = DateTime.MinValue,
                    RowKey = Guid.NewGuid().ToString()
                },
            };

            var controller = new BirthdayController();

            var results = controller.AnniversaryUpcomingIn14Days(list);

            Assert.IsTrue(!results.Any());
        }


        [TestMethod]
        public void TestBirthdayThisWeekendMultiple()
        {
            var list = new[]
            {
                new PersonEntity()
                {
                    Name = "Test 1",
                    Birthday = GetNextDayOfWeek(DayOfWeek.Saturday),
                    SlackUserName = "test1",
                    Active = true,
                    PartitionKey = "test-partition",
                    Gender = "male",
                    LastCongratulation = DateTime.MinValue,
                    RowKey = ""
                },
                new PersonEntity()
                {
                    Name = "Test 2",
                    Birthday = GetNextDayOfWeek(DayOfWeek.Sunday),
                    SlackUserName = "test2",
                    Active = true,
                    PartitionKey = "test-partition",
                    Gender = "male",
                    LastCongratulation = DateTime.MinValue,
                    RowKey = ""
                }
            };

            var controller = new BirthdayController();

            var results = controller.BirthdayThisWeekendAndNotNotified(list);

            if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
                Assert.IsTrue(results.Count() == 2);
            else
            {
                Assert.IsTrue(!results.Any());
            }
        }

        

        private DateTime GetNextDayOfWeek(DayOfWeek dayOfWeek)
        {
            var date = DateTime.Now;

            while (true)
            {
                if (date.DayOfWeek == dayOfWeek)
                {
                    break;
                }
                else
                {
                    date = date.AddDays(1);
                }
            }

            return date;
        }
    }
}