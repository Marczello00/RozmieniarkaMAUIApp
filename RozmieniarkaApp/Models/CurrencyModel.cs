using RozmieniarkaApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
