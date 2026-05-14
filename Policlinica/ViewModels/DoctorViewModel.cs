using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Policlinica.DB;

namespace Policlinica.ViewModels;

public partial class DoctorViewModel : ViewModelBase
{
    private readonly IServiceProvider _provider;
    private readonly Navigation _navigation;
    
    
    [ObservableProperty] string _surname;
    [ObservableProperty] string _name;
    [ObservableProperty] ObservableCollection<Doctor> _doctorList;
    [ObservableProperty] Doctor _selectedDoctor;

    public DoctorViewModel(IServiceProvider provider, DoctorRepository repository, Navigation navigation)
    {
        _provider = provider;
        _doctorList = new ObservableCollection<Doctor>(repository.GetDoctorsByTest());
        _navigation = navigation;
        
        
        Chuvak.name = Name;
        Chuvak.surname = Surname;
    }

    [RelayCommand]
    public void StartTest()
    {
        
        if (Name == null)
            return;
        if (Surname == null)
        return;
        if (SelectedDoctor == null)
            return;
        var vm = ActivatorUtilities.CreateInstance<ServiceViewModel>(_provider, SelectedDoctor);
        _navigation.Navigate(vm);
    }
    
}