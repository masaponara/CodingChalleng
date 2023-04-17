using CodingChallenge_Nextech.Model;
using Newtonsoft.Json;

namespace CodingChallenge_Nextech.Business.Services
{
    public class StoriesService : IStoriesService
    {        
        public Task<IEnumerable<Story>> GetNewStories()
        {
            throw new NotImplementedException();
        }

        public async Task<Story?> GetStoryById(int storyId)
        {
            var _httpClient = new HttpClient();

            HttpResponseMessage response = _httpClient.GetAsync("https://hacker-news.firebaseio.com/v0/item/35557848.json").Result;  // Blocking call!  
            if (response.IsSuccessStatusCode)
            {
                var customerJsonString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Story?>(custome‌​rJsonString);
            }
            else
            {
                return null;
            }

        }
    }
}
