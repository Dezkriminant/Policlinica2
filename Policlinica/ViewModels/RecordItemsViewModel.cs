using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Policlinica.DB;
/*
namespace Policlinica.ViewModels;

public partial class RecordItemsViewModel : ViewModelBase
{
     
    private readonly IServiceProvider _provider;
    private readonly Navigation _navigation;
    
    [ObservableProperty] List<Service> _services;
    [ObservableProperty] Service _selectedService;
    private RecordItemsRepository _repository;




    public RecordItemsViewModel(IServiceProvider provider, Service selectedService, List<Service>  services, RecordItemsRepository repository)
    {
        _provider = provider;
        _services = services;
        _selectedService = selectedService;
        _repository =  repository;
        
    }

    
   

    [RelayCommand]
    public void SaveDB()
    {
     
        _repository.GetRecordItemsByTest(Records, Services);
        if (SelectedDoctor == null)
            return;
        var vm = ActivatorUtilities.CreateInstance<AdminViewModel>(_provider);
        _navigation.Navigate(vm);
        
        
    }
    
    [RelayCommand]
    public void Start()
    {
       if (SelectedService == null)
           return;
       var vm = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        var win = _serviceProvider.GetRequiredService<MainWindow>();
        
        vm.SetClose(win.Close);
        win.DataContext = vm;
         win.Show();
        close();
    }
    private Action close;

    public void SetClose(Action close)
    {
        this.close = close;
    }
}
*/