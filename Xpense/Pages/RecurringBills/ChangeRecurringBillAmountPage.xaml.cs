using Xpense.ViewModels.RecurringBills;

namespace Xpense.Pages.RecurringBills;

public partial class ChangeRecurringBillAmountPage : BaseContentPage
{
	public ChangeRecurringBillAmountPage(ChangeRecurringBillAmountViewModel changeRecurringBillAmountViewModel) : base(changeRecurringBillAmountViewModel)
	{
		InitializeComponent();
	}
}