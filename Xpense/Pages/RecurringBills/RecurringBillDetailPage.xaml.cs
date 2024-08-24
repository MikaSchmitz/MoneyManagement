using Xpense.ViewModels.RecurringBills;

namespace Xpense.Pages.RecurringBills;

public partial class RecurringBillDetailPage : BaseContentPage
{
	public RecurringBillDetailPage(RecurringBillDetailViewModel recurringBillDetailViewModel) : base(recurringBillDetailViewModel)
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        IsBusy = true;
        if (BindingContext is RecurringBillDetailViewModel recurringBillDetailViewModel)
        {
            recurringBillDetailViewModel.RefreshRecurringBillCommand.Execute(this);
        }
        base.OnAppearing();
        IsBusy = false;
    }
}