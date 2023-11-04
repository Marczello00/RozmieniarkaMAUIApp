using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RozmieniarkaApp.ViewModels
{
    partial class SettingsPageViewModel:ObservableObject
    {
        [ObservableProperty]
        private string machineIPaddress;
        [ObservableProperty]
        private string machinePort;
        public SettingsPageViewModel()
        {
            //Preferences.Clear();
            LoadMachineData();
        }
        [RelayCommand]
        public async Task CancelButtonClickedAsync()
        {
            await Shell.Current.GoToAsync("..",true);
        }
        [RelayCommand]
        public async Task SaveButtonClickedAsync()
        {
            Preferences.Set("MachineIPaddress", MachineIPaddress);
            Preferences.Set("MachinePort", Convert.ToInt32(MachinePort));
            await Shell.Current.GoToAsync("..", true);
        }
        private void LoadMachineData()
        {
            MachineIPaddress = Preferences.Get("MachineIPaddress", "192.168.1.61");
            MachinePort = Preferences.Get("MachinePort", 5555).ToString();
        }
    }
}
