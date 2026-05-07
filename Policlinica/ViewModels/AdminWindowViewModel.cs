using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Policlinica.DB;
using Policlinica.Views;

namespace Policlinica.ViewModels;

public partial class AdminWindowViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Navigation _navigation;
    private readonly IServiceProvider _provider;
    [ObservableProperty] private string _password;
    [ObservableProperty] private string _login;
    [ObservableProperty] public string _eror;

    public AdminWindowViewModel(IServiceProvider serviceProvider, Navigation navigation, IServiceProvider provider)
    {
        _serviceProvider = serviceProvider;
        _navigation = navigation;
        _provider = provider;

    }
}