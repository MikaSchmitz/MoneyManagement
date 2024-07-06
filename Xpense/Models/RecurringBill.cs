using SQLite;
using System.Collections.ObjectModel;
using Xpense.Resources.Database;
using Xpense.Resources.Enums;
using Xpense.Resources.Extensions;

namespace Xpense.Models
{
    public class RecurringBill : IdentityModel
    {
        public string Name { get; set; }
        public TimeInterval RecurrenceInterval { get; set; }
        public int RecurrenceIntervalMultiplier { get; set; }
        public string? Note { get; set; }
        public bool Active { get; set; }

        // relational
        [Ignore]
        public ObservableCollection<Cost> Costs { get; set; }

        // calculations
        public DateTime NextBillingDate => CalculateNextBillingDate();
        public int DaysUntillBilled => CalculateDaysUntillBilled();
        public decimal AmountToBeBilled => GetMostRelevantPriceIndication().Amount;
        public string AmountToBeBilledToString => AmountToBeBilled.ToEuroString();

        public RecurringBill() : base()
        {
            Name = string.Empty;
            RecurrenceInterval = TimeInterval.Monthly;
            RecurrenceIntervalMultiplier = 1;
            Active = true;
            Costs = new ObservableCollection<Cost>();
        }

        private Cost GetMostRelevantPriceIndication()
        {
            // get all costs that have already been billed at least once
            var costs = Costs.Where(x => x.StartDate.Date <= DateTime.Now.Date);

            // get the latest cost
            var mostRelevantPrice = costs.OrderByDescending(x => x.StartDate).FirstOrDefault();

            if (mostRelevantPrice == null)
            {
                throw new ArgumentNullException(nameof(mostRelevantPrice));
            }

            return mostRelevantPrice;
        }

        private DateTime CalculateNextBillingDate()
        {
            // get the next billing date
            var priceIndication = GetMostRelevantPriceIndication();
            return priceIndication.NextBillingDate(RecurrenceInterval, RecurrenceIntervalMultiplier);
        }

        private int CalculateDaysUntillBilled()
        {
            DateTime nextBillingDate = CalculateNextBillingDate();
            DateTime currentDate = DateTime.UtcNow;

            // Calculate the number of days until the next billing date
            TimeSpan timeSpan = nextBillingDate.Date - currentDate.Date;
            return (int)timeSpan.TotalDays;
        }

        
    }
}
