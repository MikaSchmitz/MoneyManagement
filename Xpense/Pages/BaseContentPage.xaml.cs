using Xpense.ViewModels;

namespace Xpense.Pages;

public abstract partial class BaseContentPage : ContentPage
{
    /// <summary>
    /// Updates once when the page is created
    /// </summary>
    /// <param name="viewModel"></param>
    public BaseContentPage(BaseViewModel viewModel)
    {
        IsBusy = true;
        BindingContext = viewModel;
        InitializeComponent();
        IsBusy = false;
    }

    /// <summary>
    /// Updates each time the page comes into view
    /// </summary>
    protected override void OnAppearing()
    {
        IsBusy = true;
        base.OnAppearing();
        IsBusy = false;
    }
}