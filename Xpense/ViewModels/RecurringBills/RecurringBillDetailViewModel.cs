using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Xpense.Models;
using Xpense.Pages.RecurringBills;
using Xpense.Resources.Database.AccessLayers;
using Xpense.ViewModels.DTOs;

namespace Xpense.ViewModels.RecurringBills
{
    [QueryProperty("RecurringBillId", "RecurringBillId")]
    public partial class RecurringBillDetailViewModel : BaseViewModel
    {
        private readonly RecurringBillAccessLayer _recurringBillAccessLayer;
        private readonly CostAccessLayer _costAccessLayer;

        [ObservableProperty]
        string? recurringBillId;

        [ObservableProperty]
        Expense? expense;

        public RecurringBillDetailViewModel()
        {
            _recurringBillAccessLayer = new RecurringBillAccessLayer();
            _costAccessLayer = new CostAccessLayer();
        }

        [RelayCommand]
        async Task RefreshRecurringBill()
        {
            Expense = null;
            if (string.IsNullOrWhiteSpace(RecurringBillId))
            {
                await GoToPreviousPage();
                return;
            }

            var id = new Guid(RecurringBillId);
            var bill = await _recurringBillAccessLayer.GetByIdAsync(id);
            if (bill == null) 
            {
                await Shell.Current.DisplayAlert("Not found", "The expense was not found", "OK");
                await GoToPreviousPage();
                return;
            }

            var costs = new ObservableCollection<Cost>(await _costAccessLayer.GetByRecurringBillingIdAsync(id));

            Expense = new Expense(bill, costs);
        }

        [RelayCommand]
        async Task GoToChangeBilledAmountPage(Guid id)
        {
            await Shell.Current.GoToAsync($"{nameof(ChangeRecurringBillAmountPage)}?RecurringBillId={id}");
        }

        [RelayCommand]
        async Task Delete(Expense dto)
        {
            foreach (var cost in dto.Costs)
            {
                await _costAccessLayer.DeleteAsync(cost);
            }
            await _recurringBillAccessLayer.DeleteAsync(dto.Bill);
            await GoToPreviousPage();
        }
    }
}
