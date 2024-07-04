using Xpense.Resources.Database;

namespace Xpense.Models
{
    internal class Cost : IdentityModel
    {
        public Guid RecurringBillingId { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Amount { get; set; }
        public string? Note { get; set; }

        public Cost()
        {
            
        }
    }
}
