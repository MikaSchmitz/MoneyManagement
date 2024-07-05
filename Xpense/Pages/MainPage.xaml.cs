using Xpense.ViewModels;

namespace Xpense.Pages
{
    public partial class MainPage : BaseContentPage
    {

        public MainPage(MainViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            IsBusy = true;
            if (BindingContext is MainViewModel mainViewModel)
            {
                mainViewModel.LoadAllRecurringBillsCommand.Execute(this);
            }
            base.OnAppearing();
            IsBusy = false;
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsBusy = true;
            if (BindingContext is MainViewModel mainViewModel)
            {
                mainViewModel.FilterRecurringBillsCommand.Execute(this);
            }
            base.OnAppearing();
            IsBusy = false;
        }
    }

}
