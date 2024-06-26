using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using RozmieniarkaApp.Models;
using RozmieniarkaApp.Converters;

namespace RozmieniarkaApp.Services
{
    public static class TaxWizardConnectionService
    {
        private static readonly HttpClient client = new HttpClient();
        private static string CreateJsonRequest(bool status)
        {
            string jsonData;
            if (status)
            {
                jsonData = @"{
""time"": ""03/18/2024 13:39:40"",
""taxing"": 1,
""checksum"": ""MDMvMTgvMjAyNCAxMzozOTo0MHPDs2w=""}";
            }
            else
            {
                jsonData = @"{
""time"": ""03/18/2024 13:39:40"",
""taxing"": 0,
""checksum"": ""MDMvMTgvMjAyNCAxMzozOTo0MHPDs2w=""}";
            }

            return jsonData;
        }
        public static async Task SetTaxingStatus(bool status)
        {
            string jsonData = CreateJsonRequest(status);
            string ipAddress = getIPAddress();
            HttpClient _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri($"http://{ipAddress}/taxing");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("/taxing", content);
            response.EnsureSuccessStatusCode();
        }
        public static async Task TopUpCarWashCredit(int amount)
        {
            string ipAddress = getIPAddress();
            var url = $"http://{ipAddress}/transaction";
            var requestBody = new { creditCount = amount };
            string json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
        }
        public static async Task<TWStatusModel> GetStatuses()
        {
            TWStatusModel output = new();
            string ipAddress = getIPAddress();
            var url = $"http://{ipAddress}/myStatus";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                options.Converters.Add(new BoolToIntConverter());
                output = JsonSerializer.Deserialize<TWStatusModel>(jsonResponse, options);
            }
            return output;
        }
        private static string getIPAddress()
        {
            return Preferences.Get("TWMachineIPaddress", "192.168.1.123");
        }
    }
}
