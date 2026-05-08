using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    private readonly RecordRep _recordRep;
    [ObservableProperty] private string _password;
    [ObservableProperty] private string _login;
    [ObservableProperty] public string _eror;
    [ObservableProperty] ObservableCollection<Record>  _recordsList = new();
    [ObservableProperty] private Record _selectedRecord;

    public AdminWindowViewModel(IServiceProvider serviceProvider, Navigation navigation, IServiceProvider provider, RecordRep recordRep)
    {
        _serviceProvider = serviceProvider;
        _navigation = navigation;
        _provider = provider;
        _recordRep = recordRep;
        
        RecordsList = new ObservableCollection<Record>(recordRep.GetRecord());
    }
    
}