using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    private readonly User _user;
    private readonly UserRepository _userRepository;
    [ObservableProperty] string _login;
    [ObservableProperty] int _id;
    [ObservableProperty] ObservableCollection<Record> _recordsList = new();
    [ObservableProperty] private Record _selectedRecord;
    [ObservableProperty] private ObservableCollection<User> userList = new ObservableCollection<User>();

    public AdminViewModel(Navigation navigation, IServiceProvider provider, RecordRep recordRep,User user,UserRepository userRepository)
    {

        _navigation = navigation;
        _provider = provider;
        _recordRep = recordRep;
        _user = user;
        _userRepository = userRepository;
        
        UserList = new ObservableCollection<User>(userRepository.GetUserId(user.Login,user.Password));

        foreach (var obj in UserList)
        {
            Id = obj.Id;
        }



        RecordsList = new ObservableCollection<Record>(recordRep.GetRecord(Id));
    }

    [RelayCommand]
    void DeleteRecord()
    {
        _recordRep.Delete(SelectedRecord.Id);
        RecordsList = new ObservableCollection<Record>(_recordRep.GetRecord(Id));
    }

    [RelayCommand]
    void GoService()
    {
        var vm = ActivatorUtilities.CreateInstance<DoctorViewModel>(_provider);
        _navigation.Navigate(vm);
    }

}