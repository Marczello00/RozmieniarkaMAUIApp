﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;

namespace RozmieniarkaApp.Services
{
    public static class TaxWizardConnectionService
    {
        private static string CreateJsonRequest(bool status)
        {
            string jsonData;
            if (status) {
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
        public static async Task<int> SetTaxingStatus(bool status)
        {
            string jsonData = CreateJsonRequest(status);
            HttpClient _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://192.168.1.228");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("/taxing", content);
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                JObject jsonResponse = JObject.Parse(responseData);
                bool taxingValue = jsonResponse.Value<bool>("taxing");
                return taxingValue ? 1 : 0;
            }
            else
            {
                return -1;
            }
        }
        public static async Task<int> GetTaxingStatus()
        {
            HttpClient _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://192.168.1.228/taxing");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            HttpResponseMessage response = await _httpClient.GetAsync("/taxing");
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                JObject jsonResponse = JObject.Parse(responseData);
                bool taxingValue = jsonResponse.Value<bool>("taxing");
                return taxingValue ? 1 : 0;
            }
            else
            {
                return -1;
            }
        }
    }
}
