using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xpense.Models;
using Xpense.Resources.Database.AccessLayers;
using Xpense.Resources.Enums;

namespace Xpense.ViewModels.RecurringBills
{
    public partial class AddRecurringBillViewModel : BaseViewModel
    {
        private readonly RecurringBillAccessLayer _recurringBillAccessLayer;
        private readonly CostAccessLayer _costAccessLayer;

        [ObservableProperty]
        RecurringBill recurringBill;

        [ObservableProperty]
        Cost cost;

        [ObservableProperty]
        ObservableCollection<TimeInterval> availableTimeIntervals;

        public AddRecurringBillViewModel()
        {
            _recurringBillAccessLayer = new RecurringBillAccessLayer();
            _costAccessLayer = new CostAccessLayer();
            recurringBill = new RecurringBill();
            cost = new Cost();
            availableTimeIntervals = new ObservableCollection<TimeInterval>((TimeInterval[])Enum.GetValues(typeof(TimeInterval)));
        }

        [RelayCommand]
        async Task CreateNewRecurringBill()
        {
            if (string.IsNullOrWhiteSpace(RecurringBill.Name)) 
            {
                Debug.WriteLine($"{nameof(RecurringBill.Name)} can not be empty when creating a new expense.");
                return;
            }

            if (RecurringBill.RecurrenceIntervalMultiplier < 1)
            {
                Debug.WriteLine($"{nameof(RecurringBill.RecurrenceIntervalMultiplier)} can not be smaller than 1 when creating a new expense.");
                return;
            }

            if (Cost.Amount < 0) 
            {
                Debug.WriteLine($"{nameof(Cost.Amount)} can not be smaller than 0.00 when creating a new expense.");
                return;
            }

            Cost.RecurringBillingId = RecurringBill.Id;

            await _recurringBillAccessLayer.SaveAsync(RecurringBill);
            await _costAccessLayer.SaveAsync(Cost);

            await GoToPreviousPage();
        }
    }
}
