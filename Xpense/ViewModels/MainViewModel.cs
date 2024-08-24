using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xpense.Models;
using Xpense.Pages.RecurringBills;
using Xpense.Resources.Database.AccessLayers;
using Xpense.Resources.Extensions;
using Xpense.ViewModels.DTOs;

namespace Xpense.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly RecurringBillAccessLayer _recurringBillAccessLayer;
        private readonly CostAccessLayer _costAccessLayer;

        [ObservableProperty]
        ObservableCollection<Expense> allExpenses;

        [ObservableProperty]
        ObservableCollection<Expense> filteredExpenses;

        [ObservableProperty]
        Expense? selectedExpense;

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
            AllExpenses = new ObservableCollection<Expense>();
            FilteredExpenses = new ObservableCollection<Expense>();
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
                FilteredExpenses = AllExpenses;
            }
            else
            {
                FilteredExpenses = new ObservableCollection<Expense>(
                    AllExpenses.Where(b => b.Bill.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                );
            }
            OnPropertyChanged(nameof(FilteredExpenses));
        }

        [RelayCommand]
        async Task Delete(Expense dto)
        {
            foreach(var cost in dto.Costs)
            {
                await _costAccessLayer.DeleteAsync(cost);
            }
            await _recurringBillAccessLayer.DeleteAsync(dto.Bill);
            FilteredExpenses.Remove(dto);
            CalculateDueAmounts();
        }

        [RelayCommand]
        async Task GoToAddRecurringBillPage()
        {
            await Shell.Current.GoToAsync(nameof(AddRecurringBillPage));
        }

        [RelayCommand]
        async Task GoToDetailPage(Guid id)
        {
            await Shell.Current.GoToAsync($"{nameof(RecurringBillDetailPage)}?RecurringBillId={id}");
        }

        private async Task RefreshRecurringBillsAsync()
        {
            AllExpenses.Clear();

            var bills = await _recurringBillAccessLayer.GetAllAsync();
            foreach (var bill in bills)
            {
                var costs = await _costAccessLayer.GetByRecurringBillingIdAsync(bill.Id);
                AllExpenses.Add(new Expense(bill, costs));
                Debug.WriteLine($"Expense `{bill.Name}` loaded with `{costs.Count}` costs.");
            }
            Debug.WriteLine($"Finished loading recurring bills, `{AllExpenses.Count}` found.");

            FilteredExpenses = new ObservableCollection<Expense>(AllExpenses.Where(x => x.Bill.Active).OrderBy(x => x.DaysUntilBilled));

            CalculateDueAmounts();
        }

        private void CalculateDueAmounts()
        {
            var dateTimeNow = DateTime.UtcNow;
            DueAmountThisYear = FilteredExpenses.Where(x => x.DaysUntilBilled <= dateTimeNow.DaysRemainingInYear()).Sum(x => x.AmountToBeBilled);
            DueAmountThisMonth = FilteredExpenses.Where(x => x.DaysUntilBilled <= dateTimeNow.DaysRemainingInMonth()).Sum(x => x.AmountToBeBilled);
            DueAmountThisWeek = FilteredExpenses.Where(x => x.DaysUntilBilled <= dateTimeNow.DaysRemainingInWeek()).Sum(x => x.AmountToBeBilled);
            DueAmountThisDay = FilteredExpenses.Where(x => x.DaysUntilBilled == 0).Sum(x => x.AmountToBeBilled);
        }
    }
}
