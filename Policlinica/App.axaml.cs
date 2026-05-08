using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Policlinica.ViewModels;
using Policlinica.Views;

namespace Policlinica;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        
    }

    public App(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            
            DisableAvaloniaDataAnnotationValidation();
            
            var vm = _serviceProvider.GetRequiredService<StartViewModel>();
            var win = _serviceProvider.GetRequiredService<Startview>();
            vm.SetClose(win.Close);
            win.DataContext = vm;
            desktop.MainWindow = win;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}