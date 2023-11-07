using RozmieniarkaApp.Enums;

namespace RozmieniarkaApp.Models
{
    class CurrencyModel
    {
        public CurrencyType currencyType;
        public int currencyCount;
        public CurrencyModel(CurrencyType currencyType, int banknoteCount)
        {
            this.currencyType = currencyType;
            this.currencyCount = banknoteCount;
        }
    }
}
