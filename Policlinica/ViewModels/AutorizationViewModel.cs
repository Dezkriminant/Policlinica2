using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Policlinica.DB;
using Policlinica.Views;

namespace Policlinica.ViewModels;

public partial class AutorizationViewModel : ViewModelBase
{
    private readonly IServiceProvider _provider;
    private readonly Navigation _navigation;

    [ObservableProperty] string _login;
    [ObservableProperty] string _password;
    [ObservableProperty] public string _eror;

    public AutorizationViewModel(IServiceProvider provider, Navigation navigation)
    {
        _provider = provider;
        _navigation = navigation;
    }

    [RelayCommand]
    public void Conti()
    {
        List<User> SpUser = new();

        using (UserRepository repository = _provider.GetRequiredService<UserRepository>())
        {
            SpUser = repository.CheckLoginAndPassword(Login, Password);
        }

        if (SpUser.Count == 0)
        {
            Eror = "Неверный логин или пароль";
            return;
        }
        
        CurrentUser.login = SpUser[0].Name;
        
        var vm = ActivatorUtilities.CreateInstance<AdminViewModel>(_provider);
        _navigation.Navigate(vm);
    }


    [RelayCommand]
    void OpenRegWin()
    {
        var vm = _provider.GetRequiredService<RegistrationViewModel>();
        _navigation.Navigate(vm);
    }

}