using Xpense.Models;

namespace Xpense.Resources.Database.AccessLayers
{
    internal class CostAccessLayer : DatabaseAccessLayer<Cost>
    {
        public CostAccessLayer() : base()
        {
            
        }

        public async Task<List<Cost>> GetByRecurringBillingIdAsync(Guid recurringBillingId)
        {
            await Init();
            return await Database.Table<Cost>().Where(c => c.RecurringBillingId == recurringBillingId).ToListAsync();
        }
    }
}
