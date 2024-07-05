using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Xpense.ViewModels
{
    public abstract partial class BaseViewModel : ObservableObject
    {
        [RelayCommand]
        protected async Task GoToPreviousPage()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
