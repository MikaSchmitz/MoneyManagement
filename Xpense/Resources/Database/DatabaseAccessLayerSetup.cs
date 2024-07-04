using Xpense.Resources.Database.AccessLayers;

namespace Xpense.Resources.Database
{
    internal static class DatabaseAccessLayerSetup
    {
        internal static void AddDatabaseAccessLayers(this IServiceCollection services)
        {
            services.AddSingleton<RecurringBillAccessLayer>();
            services.AddSingleton<CostAccessLayer>();
        }
    }
}
