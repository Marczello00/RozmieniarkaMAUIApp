using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
        public static async Task CancelButtonClickedAsync()
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
            MachineIPaddress = Preferences.Get("MachineIPaddress", "10.0.0.7");
            MachinePort = Preferences.Get("MachinePort", 5555).ToString();
        }
    }
}
