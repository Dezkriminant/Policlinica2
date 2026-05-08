using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace Policlinica.ViewModels;

public partial class InfoViewModel : ViewModelBase

    {
        private readonly Navigation _navigation1;
        [ObservableProperty] private ViewModelBase _currentPage1;

        public InfoViewModel(IServiceProvider sv1, Navigation navigation1)
        {
            _navigation1 = navigation1;
            //_navigation1.SetCurrentView(this);
            _navigation1.Navigate(sv1.GetRequiredService<AdminWindowViewModel>());
        }

        Action closeAction;

        public void SetClose(Action closeAction)
        {
            this.closeAction = closeAction;
        }

        public void Close()
        {
            this.closeAction?.Invoke();
        }
    }
