using Xpense.Pages.RecurringBills;

namespace Xpense
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddRecurringBillPage), typeof(AddRecurringBillPage));
            Routing.RegisterRoute(nameof(RecurringBillDetailPage), typeof(RecurringBillDetailPage));
            Routing.RegisterRoute(nameof(ChangeRecurringBillAmountPage), typeof(ChangeRecurringBillAmountPage));
        }
    }
}
