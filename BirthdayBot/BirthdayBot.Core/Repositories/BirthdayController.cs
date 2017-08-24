using System;
using System.Collections.Generic;
using System.Linq;
using BirthdayBot.Core.Models;

namespace BirthdayBot.Core.Repositories
{
    public class BirthdayController
    {
        public IEnumerable<PersonEntity> BirthdayTodayAndNotNotified(IEnumerable<PersonEntity> people)
        {
            var set = from p in people
                      where p.Active && IsToday(p.ThisYearsBirthday()) && !IsToday(p.LastCongratulation)
                      select p;

            return set;
        }

        public IEnumerable<PersonEntity> BirthdayThisWeekendAndNotNotified(IEnumerable<PersonEntity> people)
        {
            var set = from p in people
                      where IsItFriday(DateTime.Today) && IsThisWeekend(p.ThisYearsBirthday()) && !IsToday(p.LastCongratulation) && p.Active
                      select p;

            return set;
        }

        public IEnumerable<PersonEntity> AnniversaryUpcomingIn14Days(IEnumerable<PersonEntity> people)
        {
            var set = from p in people
                where NextBirthdayIsAnniversary(p.Birthday) && BirthdayIn14Days(p.ThisYearsBirthday()) && p.Active
                      select p;

            return set;
        }

        private static bool BirthdayIn14Days(DateTime? dt)
        {
            if (!dt.HasValue)
            {
                return false;
            }

            var today = DateTime.Today;
            var birthday = dt.Value.Date;

            return today.AddDays(14) == birthday;
        }

        private static bool NextBirthdayIsAnniversary(DateTime? dt)
        {
            if (!dt.HasValue)
            {
                return false;
            }

            var today = DateTime.Today;
            var birthday = dt.Value.Date;
            var ageThisYear = today.Year - birthday.Year;

            switch (ageThisYear)
            {
                case 20:
                case 30:
                case 40:
                case 50:
                case 60:
                case 70:
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsThisWeekend(DateTime? dt)
        {
            if (!dt.HasValue)
            {
                return false;
            }

            var isOnSunday = dt.Value.DayOfWeek == DayOfWeek.Sunday && IsToday(dt.Value.AddDays(-2));
            var isOnSaturday = dt.Value.DayOfWeek == DayOfWeek.Saturday && IsToday(dt.Value.AddDays(-1));

            return (isOnSunday || isOnSaturday);
        }

        private static bool IsToday(DateTime? dt)
        {
            if (!dt.HasValue)
            {
                return false;
            }

            return dt.Value.Date == DateTime.Today;
        }

        private static bool IsItFriday(DateTime? dt)
        {
            return dt?.DayOfWeek == DayOfWeek.Friday;
        }
    }
}
