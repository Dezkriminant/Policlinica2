using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Animation.Easings;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Policlinica.DB;
using Policlinica.Views;

namespace Policlinica.ViewModels;

public partial class ServiceViewModel : ViewModelBase
{

    private readonly IServiceProvider _provider;
    private readonly Navigation _navigation;
    ServiceRepository _serviceRepository;
    
    [ObservableProperty] string surname;
    [ObservableProperty] string name;
    [ObservableProperty]  ObservableCollection<ServiceSelected> _services;
    [ObservableProperty] string _login;
    [ObservableProperty] Doctor _selectedDoctor;

    public ServiceViewModel(IServiceProvider provider, Navigation navigation, Doctor selectedDoctor,
        ServiceRepository repository)
    {
        _provider = provider;
        _navigation = navigation;
        _selectedDoctor = selectedDoctor;
        _serviceRepository = repository;
        Services =  new ObservableCollection<ServiceSelected>(repository.GetServicesByDoctors(selectedDoctor.Id).Select(service => new ServiceSelected(service)).ToList());
        
        //Console.WriteLine(CurrentUser.login);
    }


    [RelayCommand]

    public void Dobavlenie()
    {
        {
            List<Service> services = new List<Service>();

            foreach (ServiceSelected s in Services)
            {
                if (s.IsSelected == true)
                {
                    services.Add(s.Service);
                }
            }

            var vm = ActivatorUtilities.CreateInstance<ServiceViewModel>(_provider, SelectedDoctor);
            _navigation.Navigate(vm);
         
        }

        }

    }
