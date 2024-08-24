using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Xpense.Models;
using Xpense.Resources.Database.AccessLayers;

namespace Xpense.ViewModels.RecurringBills
{
    [QueryProperty("RecurringBillId", "RecurringBillId")]
    public partial class ChangeRecurringBillAmountViewModel : BaseViewModel
    {
        private readonly CostAccessLayer _costAccessLayer;

        [ObservableProperty]
        string? recurringBillId;

        [ObservableProperty]
        Cost cost;

        public ChangeRecurringBillAmountViewModel()
        {
            _costAccessLayer = new CostAccessLayer();
            Cost = new Cost();
        }

        [RelayCommand]
        async Task UpdatePricing() 
        {
            if (string.IsNullOrWhiteSpace(RecurringBillId))
            {
                await GoToPreviousPage();
                return;
            }

            var id = new Guid(RecurringBillId);
            Cost.RecurringBillingId = id;

            await _costAccessLayer.SaveAsync(Cost);

            await Shell.Current.DisplayAlert("Succes", "The price has been updated", "OK");
            await GoToPreviousPage();
        }
    }
}
