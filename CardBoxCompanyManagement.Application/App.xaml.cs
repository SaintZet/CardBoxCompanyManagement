﻿using CardBoxCompanyManagement.Infrastructure;
using CardBoxCompanyManagement.Services;
using CardBoxCompanyManagement.View;
using CardBoxCompanyManagement.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace CardBoxCompanyManagement;

public partial class App : System.Windows.Application
{
    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddTransient<MainViewModel>();
                services.AddSingleton(s => new MainWindow(s.GetRequiredService<MainViewModel>()));

                services.AddTransient(s => new CRUDCompanyViewModel(s.GetRequiredService<ICategoriesRepository>()));

                services.AddSingleton<IWindowService, WindowService>();
                services.AddSingleton<ICategoriesRepository, CategoriesRepository>();
                services.AddSingleton<ICompaniesRepository, CompaniesRepository>();
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