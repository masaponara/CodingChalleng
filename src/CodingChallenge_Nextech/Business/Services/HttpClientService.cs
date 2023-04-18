using CodingChallenge_Nextech.Model;
using Newtonsoft.Json;

namespace CodingChallenge_Nextech.Business.Services
{
    public class HttpClientService
    {
        private readonly HttpClient _httpClient;
        public HttpClientService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<T?> GetDataAsync<T>(string uri) where T : class
        {

            using HttpResponseMessage response = _httpClient.GetAsync(uri).Result;

            if (response.IsSuccessStatusCode)
            {
                var customerJsonString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T?>(custome‌​rJsonString);
            }
            else
            {
                return null;
            }
        }
    }
}
