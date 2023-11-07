using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RozmieniarkaApp.Enums;
using RozmieniarkaApp.Models;
using RozmieniarkaApp.Services;
using System;

namespace RozmieniarkaApp.ViewModels
{
    partial class InsertedPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private int numberof100zlBanknotes;
        [ObservableProperty]
        private double hundredZlBanknoteGridOpacity;
        [ObservableProperty]
        private int numberof50zlBanknotes;
        [ObservableProperty]
        private double fiftyZlBanknoteGridOpacity;
        [ObservableProperty]
        private int numberof20zlBanknotes;
        [ObservableProperty]
        private double twentyZlBanknoteGridOpacity;
        [ObservableProperty]
        private int numberof10zlBanknotes;
        [ObservableProperty]
        private double tenZlBanknoteGridOpacity;
        [ObservableProperty]
        private int insertedTotalSum;
        [ObservableProperty]
        private int insertedTotalCount;
        [ObservableProperty]
        private double insertedTotalGridOpacity;
        [ObservableProperty]
        private bool isPageRefreshing;
        public InsertedPageViewModel()
        {
            Numberof100zlBanknotes = 0;
            Numberof50zlBanknotes = 0;
            Numberof20zlBanknotes = 0;
            Numberof10zlBanknotes = 0;
            InsertedTotalSum = 0;
            InsertedTotalCount = 0;
            IsPageRefreshing = false;
            SetAllOpacityLow();
        }
        private void InsertCurrencyInsertedItem(CurrencyModel currencyItem)
        {
            switch (currencyItem.currencyType)
            {
                case CurrencyType.HundredBanknote:
                    Numberof100zlBanknotes = currencyItem.currencyCount;
                    HundredZlBanknoteGridOpacity = 1;
                    break;
                case CurrencyType.FiftyBanknote:
                    Numberof50zlBanknotes = currencyItem.currencyCount;
                    FiftyZlBanknoteGridOpacity = 1;
                    break;
                case CurrencyType.TwentyBanknote:
                    Numberof20zlBanknotes = currencyItem.currencyCount;
                    TwentyZlBanknoteGridOpacity = 1;
                    break;
                case CurrencyType.TenBanknote:
                    Numberof10zlBanknotes = currencyItem.currencyCount;
                    TenZlBanknoteGridOpacity = 1;
                    break;
            }
        }
        private void CalculateInsertedTotalSum()
        {
            InsertedTotalSum = Numberof100zlBanknotes * 100 + Numberof50zlBanknotes * 50 + Numberof20zlBanknotes * 20 + Numberof10zlBanknotes * 10;
            InsertedTotalCount = Numberof100zlBanknotes + Numberof50zlBanknotes + Numberof20zlBanknotes + Numberof10zlBanknotes;
            InsertedTotalGridOpacity = 1;
        }
        [RelayCommand]
        async public Task RefreshInsertedPage()
        {
            ClearData();
            SetAllOpacityLow();
            string status = await DownloadDataService.DownloadStatus(DataQueryType.Inserted);
            //status = "001006";
            if (status[..2] == "Er")
            {
                await Shell.Current.DisplayAlert("Błąd", string.Concat("Nie udało się połączyć z urządzeniem:\n", status.AsSpan(7)), "OK");
            }
            else
            {
                List<CurrencyModel> currencyList = AnalyzeReplyService.AnalyzeCurrencyReply(status);
                InsertDataPage(currencyList);
            }
            IsPageRefreshing = false;
        }

        private async void InsertDataPage(List<CurrencyModel> currencyList)
        {
            if (currencyList.Count != 0)
            {
                List<Task> tasks = new();
                foreach (CurrencyModel currency in currencyList)
                {
                    tasks.Add(Task.Run(() => InsertCurrencyInsertedItem(currency)));
                    await Task.Delay(100);
                }
                await Task.WhenAll(tasks);
            }
            else
                InsertedPageViewModel.SendMessageNoData();
            CalculateInsertedTotalSum();
        }

        private static async void SendMessageNoData()
        {
            string text = "Brak wpłat";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;
            var toast = Toast.Make(text, duration, fontSize);
            await toast.Show();
        }
        private void ClearData()
        {
            Numberof100zlBanknotes = 0;
            Numberof50zlBanknotes = 0;
            Numberof20zlBanknotes = 0;
            Numberof10zlBanknotes = 0;
            InsertedTotalCount = 0;
            InsertedTotalSum = 0;
        }
        private void SetAllOpacityLow()
        {
            HundredZlBanknoteGridOpacity = 0.5;
            FiftyZlBanknoteGridOpacity = 0.5;
            TwentyZlBanknoteGridOpacity = 0.5;
            TenZlBanknoteGridOpacity = 0.5;
            InsertedTotalGridOpacity = 0.5;
        }
    }
}
