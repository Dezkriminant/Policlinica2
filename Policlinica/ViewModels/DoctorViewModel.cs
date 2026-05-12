using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Policlinica.DB;

namespace Policlinica.ViewModels;

public partial class DoctorViewModel : ViewModelBase
{
    private readonly IServiceProvider _provider;
    private readonly Navigation _navigation;
    
    [ObservableProperty] string username;
    [ObservableProperty] string name;
    [ObservableProperty] List<Doctor> _doctorList;
    [ObservableProperty] Doctor selectedDoctor;
    [ObservableProperty] string g;

    public DoctorViewModel(IServiceProvider provider, DoctorRepository repository)
    {
        _provider = provider;
        _doctorList = repository.GetDoctorsByTest();
    }

    [RelayCommand]
    public void StartTest()
    {
        
        if (SelectedDoctor == null)
            return;
        
        var vm = _provider.GetRequiredService<ServiceViewModel>();
        _navigation.Navigate(vm);
    }
    
}