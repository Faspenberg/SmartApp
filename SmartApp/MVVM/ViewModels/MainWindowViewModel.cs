using CommunityToolkit.Mvvm.ComponentModel;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Services;
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace SmartApp.MVVM.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly ZeniAppDbContext _context;
        private readonly IotHubManager _iotHubManager;

        public MainWindowViewModel(ZeniAppDbContext context, IotHubManager iotHubManager)
        {
            _context = context;
            _iotHubManager = iotHubManager;
            CheckConfigurationAsync().ConfigureAwait(false);

        }


        private async Task CheckConfigurationAsync()
        {
            try
            {
                if (await _context.Settings.AnyAsync())
                {
                    await _iotHubManager.Initialize();
                   
                }

            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
        [ObservableProperty]

        private ObservableObject? _currentViewModel;




    }
}
