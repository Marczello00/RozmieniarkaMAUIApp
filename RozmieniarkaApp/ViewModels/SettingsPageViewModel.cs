using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RozmieniarkaApp.ViewModels
{
    partial class SettingsPageViewModel:ObservableObject
    {
        [ObservableProperty]
        private string machineIPaddress;
        [ObservableProperty]
        private string tWMachineIPaddress;
        [ObservableProperty]
        private string machinePort;
        [ObservableProperty]
        private string appVersion;
        public SettingsPageViewModel()
        {
            appVersion = "Wersja " + AppInfo.VersionString;
            //Preferences.Clear();
            LoadMachineData();
        }
        [RelayCommand]
        public static async Task CancelButtonClickedAsync()
        {
            //await Shell.Current.GoToAsync("..",true);
            await Application.Current.MainPage.Navigation.PopAsync(animated: true);
        }
        [RelayCommand]
        public async Task SaveButtonClickedAsync()
        {
            bool preventSave = false;
            int portNumber=0;
            Preferences.Set("MachineIPaddress", MachineIPaddress);
            Preferences.Set("TWMachineIPaddress", TWMachineIPaddress);
            try
            {
                portNumber = Convert.ToInt32(Convert.ToDouble(MachinePort));
                if (portNumber < 0 || portNumber > 65535)
                    throw new ArgumentOutOfRangeException();
            }
            catch (Exception ex)
            {
                preventSave = true;
                switch (ex)
                {
                    case FormatException:
                        //await Shell.Current.DisplayAlert("Błąd", "Podano niepoprawny port!", "OK");
                        await Application.Current.MainPage.DisplayAlert("Błąd", "Podano niepoprawny port!", "OK");
                        break;
                    case OverflowException:
                        //await Shell.Current.DisplayAlert("Błąd", "Podano niepoprawny port!", "OK");
                        await Application.Current.MainPage.DisplayAlert("Błąd", "Podano niepoprawny port!", "OK");
                        break;
                    case ArgumentOutOfRangeException:
                        //await Shell.Current.DisplayAlert("Błąd", "Taki port nie istnieje!", "OK");
                        await Application.Current.MainPage.DisplayAlert("Błąd", "Taki port nie istnieje!", "OK");
                        break;
                    default:
                        //await Shell.Current.DisplayAlert("Błąd", "Nieznany błąd przy zapisywaniu portu!", "OK");
                        await Application.Current.MainPage.DisplayAlert("Błąd", "Nieznany błąd przy zapisywaniu portu!", "OK");
                        break;
                }
            }
            if (!preventSave)
            {
                Preferences.Set("MachinePort", portNumber);
                //await Shell.Current.GoToAsync("..", true);
                await Application.Current.MainPage.Navigation.PopAsync(animated: true);
            }
        }
        private void LoadMachineData()
        {
            MachineIPaddress = Preferences.Get("MachineIPaddress", "10.0.0.7");
            MachinePort = Preferences.Get("MachinePort", 5555).ToString();
            TWMachineIPaddress = Preferences.Get("TWMachineIPaddress", "192.168.1.123");
        }
    }
}
