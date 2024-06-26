using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RozmieniarkaApp.Enums;
using RozmieniarkaApp.Models;
using RozmieniarkaApp.Services;
using RozmieniarkaApp.Views;
using System;

namespace RozmieniarkaApp.ViewModels
{
    public partial class StatusPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private Color isMachineOkColor;
        [ObservableProperty]
        private Color isHopper5OkColor;
        [ObservableProperty]
        private Color isHopper2OkColor;
        [ObservableProperty]
        private Color isHopper1OkColor;
        [ObservableProperty]
        private Color isCasetteOkColor;
        [ObservableProperty]
        private Color isDoorOkColor;
        [ObservableProperty]
        private Color isReaderOkColor;
        [ObservableProperty]
        private bool isPageRefreshing;
        [ObservableProperty]
        private int numberOfBanknotesAvailable;
        [ObservableProperty]
        private string lastUpdatedLine;

        public StatusPageViewModel()
        {
            try
            {
                Task.Run(() => { RefreshMachineStatus(); });
            }
            catch (Exception)
            {
                try
                {
                    MachineStatusModel machineStatus = new();
                    machineStatus = RetrieveFromStorageStatusData();
                    FillInPage(machineStatus);
                }
                catch (Exception)
                {
                    //await Shell.Current.DisplayAlert("Błąd", "Nie można pobrać danych z pamięci urządzenia!", "OK");
                    //await Application.Current.MainPage.DisplayAlert("Błąd", "Nie można pobrać danych z pamięci urządzenia!", "OK");
                    ClearPageData();
                }
            }
        }

        private MachineStatusModel RetrieveFromStorageStatusData()
        {
            MachineStatusModel machineStatus = new();
            string status = SecureStorage.GetAsync("StatusData").Result;
            machineStatus.FillMachineStatusFromStatusQuery(status.Substring(6, 10));
            return machineStatus;
        }

        private void ClearPageData()
        {
            IsMachineOkColor = Colors.White;
            IsHopper5OkColor = Colors.White;
            IsHopper2OkColor = Colors.White;
            IsHopper1OkColor = Colors.White;
            IsCasetteOkColor = Colors.White;
            IsDoorOkColor = Colors.White;
            IsReaderOkColor = Colors.White;
            NumberOfBanknotesAvailable = -1;
        }

        private void FillInPage(MachineStatusModel machineStatus)
        {
            ClearPageData();
            IsMachineOkColor = machineStatus.isMachineOkColor;
            IsHopper5OkColor = machineStatus.isHopper5OkColor;
            IsHopper2OkColor = machineStatus.isHopper2OkColor;
            IsHopper1OkColor = machineStatus.isHopper1OkColor;
            IsCasetteOkColor = machineStatus.isCasetteOkColor;
            IsDoorOkColor = machineStatus.isDoorOkColor;
            IsReaderOkColor = machineStatus.isReaderOkColor;
            NumberOfBanknotesAvailable = machineStatus.numberOfBanknotesAvailable;
            InsertLastUpdatedLine();
        }
        [RelayCommand]
        async public Task RefreshMachineStatus()
        {
            ClearPageData();
            string status = await DownloadDataService.DownloadStatus(DataQueryType.Status);
            if (status[..2] == "Er")
            {
                //await Shell.Current.DisplayAlert("Błąd", string.Concat(status.Substring(7)), "OK");
                await Application.Current.MainPage.DisplayAlert("Błąd", string.Concat(status.Substring(7)), "OK");
            }
            else
            {
                MachineStatusModel machineStatus = new();
                try
                {
                    machineStatus.FillMachineStatusFromStatusQuery(status.Substring(6, 10));
                    await SecureStorage.Default.SetAsync("StatusData", status);
                    SaveCurrentTime();
                    FillInPage(machineStatus);

                }
                catch (Exception ex)
                {
                    switch (ex)
                    {
                        case ArgumentException:
                            //await Shell.Current.DisplayAlert("Błąd", " ", "OK");
                            await Application.Current.MainPage.DisplayAlert("Błąd", "Niepoprawna odpowiedź statusu rozmieniarki.\nCzyżby zaciął się czytnik?", "OK");
                            break;
                        default:
                            //await Shell.Current.DisplayAlert("Błąd", " ", "OK");
                            await Application.Current.MainPage.DisplayAlert("Błąd", "Niepoprawna odpowiedź statusu rozmieniarki!", "OK");
                            break;
                    }
                }
            }
            IsPageRefreshing = false;
        }
        [RelayCommand]
        public static async Task GotoSettingsPage()
        {
            //await Shell.Current.Navigation.PushAsync(new SettingsPage());
            await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage(), animated: true);
        }
        private void SaveCurrentTime()
        {
            SecureStorage.Default.SetAsync("StatusDataTime", DateTime.Now.ToString());
        }
        private DateTime RetrieveSavedTime()
        {
            string time = SecureStorage.GetAsync("StatusDataTime").Result;
            return DateTime.Parse(time);
        }
        private string CreateLastUpdatedLine()
        {
            //Rozdziel też na mniej niz 5 i wiecej niz 1 oraz odswiezaj przy na przyklad onForeground event
            int timeNumber;
            string timeUnit;
            TimeSpan timeSpan = DateTime.Now - RetrieveSavedTime();
            if (timeSpan.Days > 0)
            {
                timeNumber = timeSpan.Days;
                timeUnit = timeNumber > 1 ? " dni" : "dzień";
            }
            else if (timeSpan.Hours > 0)
            {
                timeNumber = timeSpan.Hours;
                timeUnit = timeNumber > 1 ? " godz" : "godzinę";
            }
            else if (timeSpan.Minutes > 0)
            {
                timeNumber = timeSpan.Minutes;
                timeUnit = timeNumber > 1 ? " min" : "minutę";
            }
            else if (timeSpan.Seconds > 0)
            {
                timeNumber = timeSpan.Seconds;
                timeUnit = timeNumber > 1 ? " sek" : "sekundę";
            }
            else
            {
                return "przed chwilą";
            }
            return (timeNumber > 1 ? timeNumber.ToString() + timeUnit : timeUnit) + " temu";
        }
        private void InsertLastUpdatedLine()
        {
            LastUpdatedLine = CreateLastUpdatedLine();
        }
    }
}
