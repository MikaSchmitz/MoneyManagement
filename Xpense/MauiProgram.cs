using Microsoft.Extensions.Logging;
using Xpense.Pages;
using Xpense.ViewModels;
using Xpense.Resources.Database;
using Xpense.Pages.RecurringBills;
using Xpense.ViewModels.RecurringBills;

namespace Xpense
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddDatabaseAccessLayers();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddTransient<AddRecurringBillPage>();
            builder.Services.AddTransient<AddRecurringBillViewModel>();

            builder.Services.AddTransient<RecurringBillDetailPage>();
            builder.Services.AddTransient<RecurringBillDetailViewModel>();

            builder.Services.AddTransient<ChangeRecurringBillAmountPage>();
            builder.Services.AddTransient<ChangeRecurringBillAmountViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            
            return builder.Build();
        }
    }
}
