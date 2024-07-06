using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xpense.Models;
using Xpense.Pages.RecurringBills;
using Xpense.Resources.Database.AccessLayers;
using Xpense.Resources.Extensions;

namespace Xpense.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly RecurringBillAccessLayer _recurringBillAccessLayer;
        private readonly CostAccessLayer _costAccessLayer;

        [ObservableProperty]
        ObservableCollection<RecurringBill> allRecurringBills;

        [ObservableProperty]
        ObservableCollection<RecurringBill> filteredRecurringBills;

        [ObservableProperty]
        string searchText;

        [ObservableProperty]
        decimal dueAmountThisYear;

        [ObservableProperty]
        decimal dueAmountThisMonth;

        [ObservableProperty]
        decimal dueAmountThisWeek;

        [ObservableProperty]
        decimal dueAmountThisDay;

        public MainViewModel()
        {
            _recurringBillAccessLayer = new RecurringBillAccessLayer();
            _costAccessLayer = new CostAccessLayer();
            AllRecurringBills = new ObservableCollection<RecurringBill>();
            FilteredRecurringBills = new ObservableCollection<RecurringBill>();
            SearchText = string.Empty;
        }

        [RelayCommand]
        async Task LoadAllRecurringBills()
        {
            await RefreshRecurringBillsAsync();
        }

        [RelayCommand]
        async Task FilterRecurringBills()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredRecurringBills = AllRecurringBills;
            }
            else
            {
                FilteredRecurringBills = new ObservableCollection<RecurringBill>(
                    AllRecurringBills.Where(b => b.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                );
            }
            OnPropertyChanged(nameof(FilteredRecurringBills));
        }

        [RelayCommand]
        async Task Delete(RecurringBill recurringBill)
        {
            foreach(var cost in recurringBill.Costs)
            {
                await _costAccessLayer.DeleteAsync(cost);
            }
            await _recurringBillAccessLayer.DeleteAsync(recurringBill);
            FilteredRecurringBills.Remove(recurringBill);
            CalculateDueAmounts();
        }

        [RelayCommand]
        async Task GoToAddRecurringBillPage()
        {
            await Shell.Current.GoToAsync(nameof(AddRecurringBillPage));
        }

        private async Task RefreshRecurringBillsAsync()
        {
            AllRecurringBills.Clear();

            var bills = await _recurringBillAccessLayer.GetAllAsync();
            foreach (var bill in bills)
            {
                var costs = await _costAccessLayer.GetByRecurringBillingIdAsync(bill.Id);
                bill.Costs = new ObservableCollection<Cost>(costs);
                AllRecurringBills.Add(bill);
                Debug.WriteLine($"Expense `{bill.Name}` loaded with `{bill.Costs.Count}` costs.");
            }
            Debug.WriteLine($"Finished loading recurring bills, `{AllRecurringBills.Count}` found.");

            FilteredRecurringBills = new ObservableCollection<RecurringBill>(AllRecurringBills.Where(x => x.Active).OrderBy(x => x.DaysUntillBilled));

            CalculateDueAmounts();
        }

        private void CalculateDueAmounts()
        {
            var dateTimeNow = DateTime.UtcNow;
            DueAmountThisYear = FilteredRecurringBills.Where(x => x.DaysUntillBilled <= dateTimeNow.DaysRemainingInYear()).Sum(x => x.AmountToBeBilled);
            DueAmountThisMonth = FilteredRecurringBills.Where(x => x.DaysUntillBilled <= dateTimeNow.DaysRemainingInMonth()).Sum(x => x.AmountToBeBilled);
            DueAmountThisWeek = FilteredRecurringBills.Where(x => x.DaysUntillBilled <= dateTimeNow.DaysRemainingInWeek()).Sum(x => x.AmountToBeBilled);
            DueAmountThisDay = FilteredRecurringBills.Where(x => x.DaysUntillBilled == 0).Sum(x => x.AmountToBeBilled);
        }
    }
}
