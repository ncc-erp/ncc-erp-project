using Abp.Runtime.Session;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectManagement.Services.ExchangeRate
{
    public class ExchangeRateService 
    {
        private const string uri = "https://api.exchangerate.host/";

        public ExchangeRateService()
        {
        }

        public async Task<Object> GetExchangeRate(string date, string baseCurrency, string symbols, int places)
        {
            using(var http = new HttpClient())
            {
                var url = new Uri($"{uri}{date}?base={baseCurrency}&symbols={symbols}&places={places}");
                var response = await http.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Object>(json);
            }
        }
    }
}