using Xpense.Models;

namespace Xpense.Resources.Database.AccessLayers
{
    internal class CostAccessLayer : DatabaseAccessLayer<Cost>
    {
        public CostAccessLayer() : base()
        {
            
        }

        public Task<List<Cost>> GetByRecurringBillingIdAsync(Guid recurringBillingId)
        {
            return Database.Table<Cost>().Where(c => c.RecurringBillingId == recurringBillingId).ToListAsync();
        }
    }
}
