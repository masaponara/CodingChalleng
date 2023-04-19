using Newtonsoft.Json;

namespace CodingChallenge_Nextech.Business.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
