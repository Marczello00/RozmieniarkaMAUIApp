using RozmieniarkaApp.Enums;
using RozmieniarkaApp.Models;

namespace RozmieniarkaApp.Services
{
    internal static class AnalyzeReplyService
    {
        private static CurrencyType IdentificateCurrency(string line)
        {
            return line switch
            {
                "PLN100" => CurrencyType.HundredBanknote,
                "PLN050" => CurrencyType.FiftyBanknote,
                "PLN020" => CurrencyType.TwentyBanknote,
                "PLN010" => CurrencyType.TenBanknote,
                "PLM005" => CurrencyType.FiveCoin,
                "PLM002" => CurrencyType.TwoCoin,
                "PLM001" => CurrencyType.OneCoin,
                _ => throw new ArgumentException("Unknown coin type."),
            };
        }
        public static List<CurrencyModel> AnalyzeCurrencyReply(string reply)
        {
            List<CurrencyModel> output = new();
            if (reply.Length == 6)
            {
                //throw new ArgumentException("No data");
                return output;
            }
            int numberOfCurrnecyTypes = Convert.ToInt32(reply.Substring(4, 1));
            for (int i = 0; i < numberOfCurrnecyTypes; i++)
                output.Add(new CurrencyModel(IdentificateCurrency(reply.Substring(6 + i * 10, 6)),
                                       Convert.ToInt32(reply.Substring(12 + i * 10, 4))));
            return output;
        }
    }
}
