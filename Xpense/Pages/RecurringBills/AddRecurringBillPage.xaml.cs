using Xpense.ViewModels.RecurringBills;

namespace Xpense.Pages.RecurringBills;

public partial class AddRecurringBillPage : BaseContentPage
{
	public AddRecurringBillPage(AddRecurringBillViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();
	}
}