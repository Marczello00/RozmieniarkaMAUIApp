using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RozmieniarkaApp.Enums;
using RozmieniarkaApp.Models;
using RozmieniarkaApp.Services;

namespace RozmieniarkaApp.ViewModels
{
    partial class WithdrawnPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private int numberof20zlBanknotes;
        [ObservableProperty]
        private double twentyZlBanknoteGridOpacity;
        [ObservableProperty]
        private int numberof5zlCoins;
        [ObservableProperty]
        private double fiveZlCoinsGridOpacity;
        [ObservableProperty]
        private int numberof2zlCoins;
        [ObservableProperty]
        private double twoZlCoinsGridOpacity;
        [ObservableProperty]
        private int numberof1zlCoins;
        [ObservableProperty]
        private double oneZlCoinsGridOpacity;
        [ObservableProperty]
        private int withdrawnTotalSum;
        [ObservableProperty]
        private int withdrawnTotalCount;
        [ObservableProperty]
        private double withdrawnTotalGridOpacity;
        [ObservableProperty]
        private bool isPageRefreshing;
        public WithdrawnPageViewModel()
        {
            Numberof20zlBanknotes = 0;
            TwentyZlBanknoteGridOpacity = 0.5;
            FiveZlCoinsGridOpacity = 0.5;
            TwoZlCoinsGridOpacity = 0.5;
            OneZlCoinsGridOpacity = 0.5;
            WithdrawnTotalCount = 0;
            SetAllOpacityLow();
            IsPageRefreshing = false;
        }
        public void InsertCurrencyWithdrawnItem(CurrencyModel currencyItem)
        {
            switch (currencyItem.currencyType)
            {
                case CurrencyType.TwentyBanknote:
                    Numberof20zlBanknotes = currencyItem.currencyCount;
                    TwentyZlBanknoteGridOpacity = 1;
                    break;
                case CurrencyType.FiveCoin:
                    Numberof5zlCoins = currencyItem.currencyCount;
                    FiveZlCoinsGridOpacity = 1;
                    break;
                case CurrencyType.TwoCoin:
                    Numberof2zlCoins = currencyItem.currencyCount;
                    TwoZlCoinsGridOpacity = 1;
                    break;
                case CurrencyType.OneCoin:
                    Numberof1zlCoins = currencyItem.currencyCount;
                    OneZlCoinsGridOpacity = 1;
                    break;
            }

        }
        public void CalculateWithdrawnTotalSum()
        {
            WithdrawnTotalSum = Numberof20zlBanknotes * 20 + Numberof5zlCoins * 5 + Numberof2zlCoins * 2 + Numberof1zlCoins;
            WithdrawnTotalCount = Numberof20zlBanknotes + Numberof5zlCoins + Numberof2zlCoins + Numberof1zlCoins;
            WithdrawnTotalGridOpacity = 1;
        }
        async public void InsertDataPage(List<CurrencyModel> currencyList)
        {
            if (currencyList.Count != 0)
            {
                List<Task> tasks = new();
                foreach (CurrencyModel currency in currencyList)
                {
                    tasks.Add(Task.Run(() => InsertCurrencyWithdrawnItem(currency)));
                    await Task.Delay(100);
                }
                await Task.WhenAll(tasks);
            }
            else
                WithdrawnPageViewModel.SendMessageNoData();
            CalculateWithdrawnTotalSum();
            IsPageRefreshing = false;
        }

        private static async void SendMessageNoData()
        {
            string text = "Brak wypłat";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;
            var toast = Toast.Make(text, duration, fontSize);
            await toast.Show();
        }

        [RelayCommand]
        async public Task RefreshWithdrawnPage()
        {
            ClearData();
            SetAllOpacityLow();
            string status = await DownloadDataService.DownloadStatus(DataQueryType.Withdrawn);
            //status = "001006";
            if (status[..2] == "Er")
            {
                //await Shell.Current.DisplayAlert("Błąd", string.Concat(status.Substring(7)), "OK");
                await Application.Current.MainPage.DisplayAlert("Błąd", string.Concat(status.Substring(7)), "OK");
            }
            else
            {
                List<CurrencyModel> currencyList = AnalyzeReplyService.AnalyzeCurrencyReply(status);
                InsertDataPage(currencyList);
            }
            IsPageRefreshing = false;
        }
        [RelayCommand]
        public void ClearData()
        {
            Numberof20zlBanknotes = 0;
            Numberof5zlCoins = 0;
            Numberof2zlCoins = 0;
            Numberof1zlCoins = 0;
            WithdrawnTotalSum = 0;
            WithdrawnTotalCount = 0;
        }
        public void SetAllOpacityLow()
        {
            TwentyZlBanknoteGridOpacity = 0.5;
            FiveZlCoinsGridOpacity = 0.5;
            TwoZlCoinsGridOpacity = 0.5;
            OneZlCoinsGridOpacity = 0.5;
            WithdrawnTotalGridOpacity = 0.5;
        }
    }
}
