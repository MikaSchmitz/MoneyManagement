using CommunityToolkit.Mvvm.ComponentModel;
using Xpense.Models;
using Xpense.Resources.Database.AccessLayers;

namespace Xpense.ViewModels.RecurringBills
{
    public partial class AddRecurringBillViewModel : BaseViewModel
    {
        private readonly RecurringBillAccessLayer _recurringBillAccessLayer;

        [ObservableProperty]
        RecurringBill recurringBill;

        public AddRecurringBillViewModel()
        {
            _recurringBillAccessLayer = new RecurringBillAccessLayer();
            recurringBill = new RecurringBill();
        }
    }
}
