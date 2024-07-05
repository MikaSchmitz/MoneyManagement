using Xpense.Resources.Database;
using Xpense.Resources.Enums;
using Xpense.Resources.Extensions;

namespace Xpense.Models
{
    public class Cost : IdentityModel
    {
        public Guid RecurringBillingId { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Amount { get; set; }
        public string? Note { get; set; }

        public Cost() : base()
        {
            StartDate = DateTime.UtcNow;
            Amount = 0.00m;
        }

        public DateTime NextBillingDate(TimeInterval timeInterval, int multiplier)
        {
            var nextBillingDate = StartDate;
            while (nextBillingDate < DateTime.UtcNow) 
            {
                nextBillingDate = nextBillingDate.AddTimeInterval(timeInterval, multiplier);
            }

            return nextBillingDate;
        }
    }
}
