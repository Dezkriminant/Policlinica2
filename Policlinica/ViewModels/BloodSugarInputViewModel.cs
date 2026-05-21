using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Policlinica.DB;

namespace Policlinica.ViewModels;

public partial class BloodSugarInputViewModel : ViewModelBase
{
    private readonly BloodSugarRepository _bloodSugarRepository;
    private readonly Navigation _navigation;
    private readonly IServiceProvider _provider;
    private Record _selectedRecord;

    [ObservableProperty] decimal sugarLevel = 0;
    [ObservableProperty] string measurementDate = DateTime.Now.ToString("yyyy-MM-dd");
    [ObservableProperty] ObservableCollection<BloodSugarRecord> bloodSugarHistory = new();
    [ObservableProperty] string statusMessage = "";
    [ObservableProperty] string patientInfo = "";
    [ObservableProperty] int recordId = 0;

    private Action _closeAction;

    public BloodSugarInputViewModel(BloodSugarRepository bloodSugarRepository, Navigation navigation, IServiceProvider provider)
    {
        _bloodSugarRepository = bloodSugarRepository;
        _navigation = navigation;
        _provider = provider;
    }

    public void SetSelectedRecord(Record record)
    {
        _selectedRecord = record;
        if (record != null)
        {
            RecordId = record.Id;
            PatientInfo = $"Пациент: {record.ClientName} {record.ClientSurname}";
            LoadBloodSugarHistory();
        }
    }

    public void SetCloseAction(Action closeAction)
    {
        _closeAction = closeAction;
    }

    private void LoadBloodSugarHistory()
    {
        if (RecordId <= 0) return;
        var records = _bloodSugarRepository.GetBloodSugarByRecord(RecordId);
        BloodSugarHistory = new ObservableCollection<BloodSugarRecord>(records);
    }

    [RelayCommand]
    void AddBloodSugar()
    {
        if (RecordId <= 0)
        {
            StatusMessage = "Ошибка: запись не выбрана";
            return;
        }

        if (SugarLevel <= 0)
        {
            StatusMessage = "Введите корректный уровень сахара";
            return;
        }

        if (!DateTime.TryParse(MeasurementDate, out DateTime date))
        {
            StatusMessage = "Неверный формат даты (YYYY-MM-DD)";
            return;
        }

        try
        {
            bool inserted = _bloodSugarRepository.InsertBloodSugar(RecordId, SugarLevel, date);
            if (inserted)
            {
                StatusMessage = "Данные сахара успешно сохранены";
                SugarLevel = 0;
                MeasurementDate = DateTime.Now.ToString("yyyy-MM-dd");
                LoadBloodSugarHistory();
            }
            else
            {
                StatusMessage = "Ошибка при сохранении данных";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
            Console.WriteLine($"Error adding blood sugar: {ex}");
        }
    }

    [RelayCommand]
    void DeleteRecord(BloodSugarRecord record)
    {
        if (record == null)
        {
            StatusMessage = "Выберите запись для удаления";
            return;
        }

        try
        {
            bool deleted = _bloodSugarRepository.DeleteBloodSugar(record.Id);
            if (deleted)
            {
                StatusMessage = "Запись удалена";
                LoadBloodSugarHistory();
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
    void GoBack()
    {
        var vm = ActivatorUtilities.CreateInstance<AdminViewModel>(_provider);
        _navigation.Navigate(vm);
    }
}
