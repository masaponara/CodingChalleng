using CodingChallenge_Nextech.Model;
using Newtonsoft.Json;
using System.Net.Http;

namespace CodingChallenge_Nextech.Business.Services
{
    public class StoriesService : IStoriesService
    {
        private readonly HttpClientService _httpClientService;
        public StoriesService()
        {
            _httpClientService = new HttpClientService();
        }

        public async Task<Tuple<IEnumerable<Story>?, int>> GetNewStories(int page, string? titleOrId)
        {
            var newStoriesIds = await _httpClientService.GetDataAsync<string[]>("https://hacker-news.firebaseio.com/v0/newstories.json");
            int totalRows = 0;
            
            List<Story> stories = new();
            if (newStoriesIds != null && newStoriesIds.Length > 0)
            {
               await Parallel.ForEachAsync(newStoriesIds, async (storyId, _) =>
                {
                    Story story = await _httpClientService.GetDataAsync<Story>($"https://hacker-news.firebaseio.com/v0/item/{storyId}.json") ?? new Story();
                    stories.Add(story);
                });

                //Filter
                if (!string.IsNullOrWhiteSpace(titleOrId))
                {
                    stories = stories.Where(s => 
                                        (!string.IsNullOrWhiteSpace(s.Title) && s.Title.Contains(titleOrId, StringComparison.CurrentCultureIgnoreCase))
                                        ||
                                        s.Id.ToString().Contains(titleOrId)).ToList();
                }
                totalRows = stories.Count;


                
                //Pagination
                stories = stories.Skip(page <= 1? 0 : (page - 1) * 10).Take(10).ToList();
            }

            return new Tuple<IEnumerable<Story>?, int>(stories, totalRows);
        }

        public async Task<Story?> GetStoryById(int storyId)
        {
            return await _httpClientService.GetDataAsync<Story>($"https://hacker-news.firebaseio.com/v0/item/{storyId}.json");
        }
    }
}
