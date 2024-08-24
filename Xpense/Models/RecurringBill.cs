using Xpense.Resources.Database;
using Xpense.Resources.Enums;

namespace Xpense.Models
{
    public class RecurringBill : IdentityModel
    {
        public string Name { get; set; }
        public TimeInterval RecurrenceInterval { get; set; }
        public int RecurrenceIntervalMultiplier { get; set; }
        public string? Note { get; set; }
        public bool Active { get; set; }

        public RecurringBill() : base()
        {
            Name = string.Empty;
            RecurrenceInterval = TimeInterval.Monthly;
            RecurrenceIntervalMultiplier = 1;
            Active = true;
        }
    }
}
