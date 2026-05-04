using Avalonia;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Policlinica.DB;
using Policlinica.ViewModels;
using Policlinica.Views;



namespace Policlinica;

sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder().
            ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsetting.json")
                    .AddEnvironmentVariables();
            }).
            ConfigureServices((c,s) =>
            {
                s.Configure<DatabaseConnection>(c.Configuration.
                    GetSection("DatabaseConnection"));
                    //окна
                s.AddTransient<AutorizationView>();
                s.AddTransient<AutorizationViewModel>();
                
                s.AddTransient<AdminWindow>();
                s.AddTransient<AdminWindowViewModel>();
                
                //Репозитории
                s.AddTransient<DoctorRepository>();
                s.AddTransient<UserRepository>();
                
                s.AddTransient<RegistrationViewModel>();
                s.AddTransient<RegistrationView>();
                
                s.AddTransient<StartViewModel>();
                s.AddTransient<Startview>();
                
                s.AddSingleton<Navigation>();
                
                s.AddTransient<Records>();
                s.AddTransient<RecordViewModel>();
            }).
            Build();
        BuildAvaloniaApp(host.Services)
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp(IServiceProvider serviceProvider)
        => AppBuilder.Configure(()=> new App(serviceProvider))
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}