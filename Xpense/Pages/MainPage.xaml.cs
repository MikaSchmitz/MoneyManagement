using Xpense.ViewModels;

namespace Xpense.Pages
{
    public partial class MainPage : BaseContentPage
    {
        int count = 0;

        public MainPage(MainViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}
