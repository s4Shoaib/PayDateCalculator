using System;
using System.Linq;

namespace DueDateCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DateTime funday = new DateTime(2022, 7, 7);

            var bankHolidays = new[]
            {
                new DateTime(2022, 7, 11),
                new DateTime(2022, 7, 15),
            };
            string paySpan = "weekly";
            DateTime payDay = new DateTime(2022, 7, 11);
            bool hasDirectDeposit = true;

            PayDateCalculator calculate = new PayDateCalculator();
            Console.WriteLine("Due Date = " + calculate.calculateDueDate(funday, bankHolidays, paySpan, payDay, hasDirectDeposit));
            Console.ReadLine();
        }
    }
    public class PayDateCalculator
    {
        public DateTime calculateDueDate(DateTime fundDay, DateTime[] holidays, string paySpan, DateTime payDay, bool hasDirectDeposit)
        {
            //dueDate = 1st Pay Date after Funday
            DateTime dueDate = payDay;
            string loopType = "Forward";

            do
            {
            checkDepositType:
                if (!hasDirectDeposit)
                {
                    //if not Direct Deposit + 1 Day
                    dueDate = dueDate.AddDays(1);
                }

            checkWeekend:
                if (dueDate.DayOfWeek != DayOfWeek.Saturday && dueDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    if (holidays.Where(x => x.Date == dueDate.Date).Count() >= 1)
                    {
                        loopType = "Reverse";
                    }
                    else
                    {
                        loopType = "Forward";
                        if (dueDate >= fundDay.AddDays(10))
                        {
                            //Return the final Due Date
                            return dueDate;
                        }
                        else
                        {
                            dueDate = UpdateDueDateBasedOnPaySpan(dueDate, paySpan, payDay);
                            goto checkDepositType;
                        }
                    }
                }

                dueDate = UpdateDueDateBasedOnWeekendOrHoliday(dueDate, loopType);
                goto checkWeekend;
            }
            while (dueDate < fundDay.AddDays(10));
        }

        private DateTime UpdateDueDateBasedOnWeekendOrHoliday(DateTime dueDate, string loopType)
        {
            switch (loopType)
            {
                case
                        "Forward":
                    dueDate = dueDate.AddDays(1);
                    break;
                case
                        "Reverse":
                    dueDate = dueDate.AddDays(-1);
                    break;
            }

            return dueDate;
        }

        private DateTime UpdateDueDateBasedOnPaySpan(DateTime dueDate, string paySpan, DateTime payDay)
        {
            switch (paySpan)
            {
                case
                    "weekly":
                    dueDate = payDay.AddDays(7);
                    break;
                case
                    "bi-weekly":
                    dueDate = payDay.AddDays(14);
                    break;
                case
                    "monthly":
                    dueDate = payDay.AddMonths(1);
                    break;
            }

            return dueDate;
        }
    }
}
