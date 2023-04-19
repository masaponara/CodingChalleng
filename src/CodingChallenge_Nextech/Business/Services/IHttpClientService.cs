namespace CodingChallenge_Nextech.Business.Services
{
    public interface IHttpClientService
    {
        public Task<T?> GetDataAsync<T>(string uri) where T : class;
    }
}
