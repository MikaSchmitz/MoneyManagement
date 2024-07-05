using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xpense.Models;
using Xpense.Pages.RecurringBills;
using Xpense.Resources.Database.AccessLayers;

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

            AllRecurringBills.OrderBy(x => x.DaysUntillBilled);
            DueAmountThisYear = AllRecurringBills.Where(x => x.DaysUntillBilled <= 365).Sum(x => x.AmountToBeBilled);
            DueAmountThisMonth = AllRecurringBills.Where(x => x.DaysUntillBilled <= 31).Sum(x => x.AmountToBeBilled);
            DueAmountThisWeek = AllRecurringBills.Where(x => x.DaysUntillBilled <= 7).Sum(x => x.AmountToBeBilled);
            DueAmountThisDay = AllRecurringBills.Where(x => x.DaysUntillBilled <= 31).Sum(x => x.AmountToBeBilled);

            FilteredRecurringBills = AllRecurringBills;
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
        async Task GoToAddRecurringBillPage()
        {
            await Shell.Current.GoToAsync(nameof(AddRecurringBillPage));
        }
    }
}
