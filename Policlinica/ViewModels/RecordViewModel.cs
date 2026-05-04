using System;

namespace Policlinica.ViewModels;

public partial class RecordViewModel:ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;

    public RecordViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
}