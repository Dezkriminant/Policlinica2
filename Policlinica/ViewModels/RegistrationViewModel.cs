using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Policlinica.DB;
using Policlinica.Views;

namespace Policlinica.ViewModels;

public partial class RegistrationViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Navigation _navigation;
    private readonly IServiceProvider _provider;

    [ObservableProperty] private string _password = "";
    [ObservableProperty] private string _login = "";
    [ObservableProperty] public string _eror = "";

    public RegistrationViewModel(IServiceProvider serviceProvider, Navigation navigation, IServiceProvider provider)
    {
        _serviceProvider = serviceProvider;
        _navigation = navigation;
        _provider = provider;
    }

    partial void OnPasswordChanged(string value)
    {
        // Ограничиваем пароль до 8 символов
        if (value != null && value.Length > 8)
        {
            Password = value.Substring(0, 8);
        }
    }

    partial void OnLoginChanged(string value)
    {
        // Ограничиваем логин до 15 символов
        if (value != null && value.Length > 15)
        {
            Login = value.Substring(0, 15);
        }
    }

    [RelayCommand]
    void OpenAutorization()
    {
        var vm = _serviceProvider.GetRequiredService<AutorizationViewModel>();
        _navigation.Navigate(vm);
    }

    [RelayCommand]
    void Registration()
    {
        if (string.IsNullOrWhiteSpace(Login))
        {
            Eror = "Логин не может быть пустым";
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            Eror = "Пароль не может быть пустым";
            return;
        }

        if (Password.Length < 4)
        {
            Eror = "Пароль должен быть минимум 4 символа";
            return;
        }

        using (UserRepository repository = _provider.GetRequiredService<UserRepository>())
        {
            var count = repository.CheckLogin(Login).Count;
            if (count > 0)
            {
                Eror = "Такой логин уже существует";
                return;
            }
            
            var user = new User()
            {
                Name = Login,
                Password = Password,
            };

            using (var rep = _serviceProvider.GetRequiredService<UserRepository>())
            {
                rep.AddUser(user);
            }
            
            var vm = _serviceProvider.GetRequiredService<AutorizationViewModel>();
            _navigation.Navigate(vm);
        }
    }
}
