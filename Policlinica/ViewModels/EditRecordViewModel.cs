using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Policlinica.DB;

namespace Policlinica.ViewModels;

public class ServiceWithSelected : ObservableObject
{
    private bool _isSelected;
    private Action _onSelectionChanged;

    public int Id { get; set; }
    public string ServiceName { get; set; }
    public decimal Price { get; set; }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (SetProperty(ref _isSelected, value))
            {
                _onSelectionChanged?.Invoke();
            }
        }
    }

    public void SetOnSelectionChanged(Action onSelectionChanged)
    {
        _onSelectionChanged = onSelectionChanged;
    }
}

public partial class EditRecordViewModel : ViewModelBase
{
    private readonly Record _record;
    private readonly RecordRep _recordRep;
    private readonly DoctorRepository _doctorRepository;
    private readonly ServiceRepository _serviceRepository;
    private readonly RecordItemsRepository _recordItemsRepository;
    private Action _closeAction;

    [ObservableProperty] string editClientName;
    [ObservableProperty] string editClientSurname;
    [ObservableProperty] ObservableCollection<Doctor> doctorList = new();
    [ObservableProperty] Doctor editSelectedDoctor;
    [ObservableProperty] ObservableCollection<ServiceWithSelected> editServiceList = new();
    [ObservableProperty] decimal editTotalAmount = 0;
    [ObservableProperty] string editRecordDate = "";
    [ObservableProperty] string statusMessage = "";

    public EditRecordViewModel(Record record, RecordRep recordRep, DoctorRepository doctorRepository, ServiceRepository serviceRepository, RecordItemsRepository recordItemsRepository)
    {
        _record = record;
        _recordRep = recordRep;
        _doctorRepository = doctorRepository;
        _serviceRepository = serviceRepository;
        _recordItemsRepository = recordItemsRepository;

        DoctorList = new ObservableCollection<Doctor>(doctorRepository.GetDoctorsByTest());

        EditClientName = record.ClientName;
        EditClientSurname = record.ClientSurname;
        EditSelectedDoctor = DoctorList.FirstOrDefault(d => d.Id == record.DoctorId);
        EditTotalAmount = record.TotalAmount;
        EditRecordDate = record.RecordDate.ToString("yyyy-MM-dd");

        LoadServicesForDoctor();
    }

    public void SetCloseAction(Action closeAction)
    {
        _closeAction = closeAction;
    }

    private void LoadServicesForDoctor()
    {
        if (EditSelectedDoctor != null)
        {
            var services = _serviceRepository.GetServicesByDoctors(EditSelectedDoctor.Id);
            EditServiceList.Clear();
            foreach (var service in services)
            {
                var serviceWithSelected = new ServiceWithSelected
                {
                    Id = service.Id,
                    ServiceName = service.ServiceName,
                    Price = service.Price,
                    IsSelected = false
                };
                serviceWithSelected.SetOnSelectionChanged(CalculateTotalAmount);
                EditServiceList.Add(serviceWithSelected);
            }
        }
    }

    partial void OnEditSelectedDoctorChanged(Doctor value)
    {
        if (value != null)
        {
            LoadServicesForDoctor();
            CalculateTotalAmount();
        }
        else
        {
            EditServiceList.Clear();
            EditTotalAmount = 0;
        }
    }

    private void CalculateTotalAmount()
    {
        EditTotalAmount = EditServiceList
            .Where(s => s.IsSelected)
            .Sum(s => s.Price);
    }

    [RelayCommand]
    void SaveRecord()
    {
        if (string.IsNullOrWhiteSpace(EditClientName) || string.IsNullOrWhiteSpace(EditClientSurname))
        {
            StatusMessage = "Имя и фамилия клиента обязательны";
            return;
        }

        if (EditSelectedDoctor == null)
        {
            StatusMessage = "Выберите врача";
            return;
        }

        var selectedServices = EditServiceList.Where(s => s.IsSelected).ToList();
        if (selectedServices.Count == 0)
        {
            StatusMessage = "Выберите хотя бы одну услугу";
            return;
        }

        try
        {
            _record.ClientName = EditClientName;
            _record.ClientSurname = EditClientSurname;
            _record.DoctorId = EditSelectedDoctor.Id;
            _record.TotalAmount = EditTotalAmount;

            if (DateTime.TryParse(EditRecordDate, out DateTime recordDate))
            {
                _record.RecordDate = recordDate;
            }

            bool updated = _recordRep.UpdateRecord(_record);
            if (updated)
            {
                // Удаляем старые услуги
                _recordItemsRepository.DeleteByRecordId(_record.Id);

                // Добавляем новые услуги
                foreach (var service in selectedServices)
                {
                    _recordItemsRepository.InsertRecordItem(new RecordItem
                    {
                        RecordId = _record.Id,
                        ServiceId = service.Id,
                        ServicePrice = service.Price
                    });
                }

                StatusMessage = "Запись успешно обновлена";
                _closeAction?.Invoke();
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

    [RelayCommand]
    void CancelEdit()
    {
        _closeAction?.Invoke();
    }
}
