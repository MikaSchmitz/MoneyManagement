using Xpense.Resources.Database;
using Xpense.Resources.Enums;

namespace Xpense.Models
{
    internal class RecurringBill : IdentityModel
    {
        public string Name { get; set; }
        public TimeInterval RecurrenceInterval { get; set; }
        public int RecurrenceIntervalMultiplier { get; set; }
        public string? Note { get; set; }

        public RecurringBill()
        {
            Name = "New expense";
        }
    }
}
