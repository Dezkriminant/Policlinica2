using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Policlinica.DB;
using Policlinica.Views;


namespace Policlinica.ViewModels;

public partial class AdminViewModel : ViewModelBase
{
    private readonly Navigation _navigation;
    private readonly IServiceProvider _provider;
    private readonly RecordRep _recordRep;
    [ObservableProperty] string _login; 
    [ObservableProperty] ObservableCollection<Record>  _recordsList = new();
    [ObservableProperty] private Record _selectedRecord;
    

    public AdminViewModel( Navigation navigation, IServiceProvider provider, RecordRep recordRep)
    {
        
        _navigation = navigation;
        _provider = provider;
        _recordRep = recordRep;
        
        RecordsList = new ObservableCollection<Record>(recordRep.GetRecord());
    }

    [RelayCommand]
    void DeleteRecord()
    {
        _recordRep.Delete(SelectedRecord.Id);
        RecordsList = new ObservableCollection<Record>(_recordRep.GetRecord());
    }

    [RelayCommand]
    void GoService()
    {
        var vm = ActivatorUtilities.CreateInstance<DoctorViewModel>(_provider);
        _navigation.Navigate(vm);
    }
}