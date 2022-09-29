using CardBox.ApiClient.Services;
using CardBox.CompanyManagement.Services;
using CardBox.CompanyManagement.View;
using CardBox.CompanyManagement.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using CardBox.ApiClient.Contracts;

namespace CardBox.CompanyManagement;

public partial class App : Application
{
    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<MainViewModel>();

                services.AddSingleton(s => new MainWindow(s.GetRequiredService<MainViewModel>()));
                services.AddTransient(s => new CRUDCompanyViewModel(s.GetRequiredService<ICategoriesService>(), new SelectImageService()));

                services.AddSingleton<IWindowService, WindowService>();
                services.AddSingleton<ICategoriesService, CategoriesService>();
                services.AddSingleton<ICompaniesService, CompaniesService>();
            })
            .Build();
    }

    public static IHost? AppHost { get; private set; }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
        startupForm.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        AppHost.Dispose();

        base.OnExit(e);
    }
}