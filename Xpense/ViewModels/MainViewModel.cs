using CommunityToolkit.Mvvm.Input;
using Xpense.Pages.RecurringBills;

namespace Xpense.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        [RelayCommand]
        async Task GoToAddRecurringBillPage()
        {
            await Shell.Current.GoToAsync(nameof(AddRecurringBillPage));
        }
    }
}
