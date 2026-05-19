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
    private readonly DoctorRepository _doctorRepository;
    private readonly ServiceRepository _serviceRepository;

    [ObservableProperty] string _login;
    [ObservableProperty] int _id;
    [ObservableProperty] ObservableCollection<Record> _recordsList = new();
    [ObservableProperty] private Record _selectedRecord;
    [ObservableProperty] private ObservableCollection<User> userList = new ObservableCollection<User>();
    [ObservableProperty] string statusMessage = "";
    
    [ObservableProperty] string editClientName = "";
    [ObservableProperty] string editClientSurname = "";
    [ObservableProperty] ObservableCollection<Doctor> doctorList = new();
    [ObservableProperty] Doctor editSelectedDoctor;
    [ObservableProperty] ObservableCollection<Service> editServiceList = new(); 
    [ObservableProperty] Service editSelectedService;
    [ObservableProperty] decimal editTotalAmount = 0;
    [ObservableProperty] string editRecordDate = "";

    public AdminViewModel(Navigation navigation, IServiceProvider provider, RecordRep recordRep, User user, UserRepository userRepository, DoctorRepository doctorRepository, ServiceRepository serviceRepository)
    {
        _navigation = navigation;
        _provider = provider;
        _recordRep = recordRep;
        _user = user;
        _userRepository = userRepository;
        _doctorRepository = doctorRepository;
        _serviceRepository = serviceRepository;
        
        UserList = new ObservableCollection<User>(userRepository.GetUserId(user.Login, user.Password));

        foreach (var obj in UserList)
        {
            Id = obj.Id;
        }

        RecordsList = new ObservableCollection<Record>(recordRep.GetRecord(Id));
        DoctorList = new ObservableCollection<Doctor>(doctorRepository.GetDoctorsByTest());
    }

    partial void OnSelectedRecordChanged(Record value)
    {
        if (value != null)
        {
            EditClientName = value.ClientName;
            EditClientSurname = value.ClientSurname;
            EditSelectedDoctor = DoctorList.FirstOrDefault(d => d.Id == value.DoctorId);
            EditTotalAmount = value.TotalAmount;
            EditRecordDate = value.RecordDate.ToString("yyyy-MM-dd");
        }
    }

    partial void OnEditSelectedDoctorChanged(Doctor value)
    {
        if (value != null)
        {
            var services = _serviceRepository.GetServicesByDoctors(value.Id);
            EditServiceList = new ObservableCollection<Service>(services);
            EditSelectedService = null; 
        }
        else
        {
            EditServiceList.Clear();
            EditSelectedService = null;
        }
    }

    partial void OnEditSelectedServiceChanged(Service value)
    {
        if (value != null)
        {
            EditTotalAmount = value.Price;
        }
    }

    [RelayCommand]
    void UpdateRecord()
    {
        if (SelectedRecord == null)
        {
            StatusMessage = "Выберите запись для обновления";
            return;
        }

        if (string.IsNullOrWhiteSpace(EditClientName) || string.IsNullOrWhiteSpace(EditClientSurname))
        {
            StatusMessage = "Имя и фамилия клиента обязательны";
            return;
        }

        if (EditSelectedDoctor == null || EditSelectedService == null)
        {
            StatusMessage = "Выберите врача и услугу";
            return;
        }

        try
        {
            SelectedRecord.ClientName = EditClientName;
            SelectedRecord.ClientSurname = EditClientSurname;
            SelectedRecord.DoctorId = EditSelectedDoctor.Id;
            SelectedRecord.ServiceId = EditSelectedService.Id;
            SelectedRecord.TotalAmount = EditTotalAmount;
            
            if (DateTime.TryParse(EditRecordDate, out DateTime recordDate))
            {
                SelectedRecord.RecordDate = recordDate;
            }

            bool updated = _recordRep.UpdateRecord(SelectedRecord);
            if (updated)
            {
                StatusMessage = "Запись успешно обновлена";
                RecordsList = new ObservableCollection<Record>(_recordRep.GetRecord(Id));
                SelectedRecord = null;
                ClearEditFields();
            }
            else
            {
                StatusMessage = "Ошибка при обновлении записи";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
            Console.WriteLine($"Error updating record: {ex}");
        }
    }

    private void ClearEditFields()
    {
        EditClientName = "";
        EditClientSurname = "";
        EditSelectedDoctor = null;
        EditServiceList.Clear();
        EditSelectedService = null;
        EditTotalAmount = 0;
        EditRecordDate = "";
    }

    [RelayCommand]
    void DeleteRecord()
    {
        if (SelectedRecord == null)
        {
            StatusMessage = "Выберите запись для удаления";
            Console.WriteLine("No record selected for deletion");
            return;
        }

        try
        {
            bool deleted = _recordRep.Delete(SelectedRecord.Id);
            if (deleted)
            {
                StatusMessage = "Запись успешно удалена";
                RecordsList = new ObservableCollection<Record>(_recordRep.GetRecord(Id));
                SelectedRecord = null;
                ClearEditFields();
            }
            else
            {
                StatusMessage = "Ошибка при удалении записи";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
            Console.WriteLine($"Error deleting record: {ex}");
        }
    }

    [RelayCommand]
    void GoService()
    {
        var vm = ActivatorUtilities.CreateInstance<DoctorViewModel>(_provider);
        _navigation.Navigate(vm);
    }

    [RelayCommand]
    void GoSugarCheck()
    {
        var vm = ActivatorUtilities.CreateInstance<SugarCheckViewModel>(_provider);
        _navigation.Navigate(vm);
    }

}
