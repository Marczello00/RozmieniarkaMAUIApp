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
        public StatusPageViewModel()
        {
            ClearPageData();
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
            IsMachineOkColor = machineStatus.isMachineOkColor;
            IsHopper5OkColor = machineStatus.isHopper5OkColor;
            IsHopper2OkColor = machineStatus.isHopper2OkColor;
            IsHopper1OkColor = machineStatus.isHopper1OkColor;
            IsCasetteOkColor = machineStatus.isCasetteOkColor;
            IsDoorOkColor = machineStatus.isDoorOkColor;
            IsReaderOkColor = machineStatus.isReaderOkColor;
            NumberOfBanknotesAvailable = machineStatus.numberOfBanknotesAvailable;
        }
        [RelayCommand]
        async public Task RefreshMachineStatus()
        {
            ClearPageData();
            string status = await DownloadDataService.DownloadStatus(DataQueryType.Status);
            if (status[..2] == "Er")
            {
                await Shell.Current.DisplayAlert("Błąd", string.Concat(status.Substring(7)), "OK");
            }
            else
            {
                MachineStatusModel machineStatus = new();
                machineStatus.FillMachineStatusFromStatusQuery(status.Substring(6, 10));
                FillInPage(machineStatus);
            }
            IsPageRefreshing = false;
        }
        [RelayCommand]
        public static async Task GotoSettingsPage()
        {
            await Shell.Current.Navigation.PushAsync(new SettingsPage());
        }
    }
}
