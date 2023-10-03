using CommunityToolkit.Mvvm.ComponentModel;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Services;
using DataAccess.Contexts;

namespace SmartApp.MVVM.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IotHubManager _iotHubManager;
        private readonly ZeniAppDbContext _context;
        public MainWindowViewModel(IServiceProvider serviceProvider, IotHubManager iotHubManager, ZeniAppDbContext context)
        {
            _serviceProvider = serviceProvider;
            CurrentViewModel = _serviceProvider.GetRequiredService<HomeViewModel>();
            _iotHubManager = iotHubManager;
            _context = context;
            
        }

        [ObservableProperty]

        private ObservableObject? _currentViewModel;

    }
}
