using CodingChallenge_Nextech.Model;
using Microsoft.Extensions.Caching.Memory;

namespace CodingChallenge_Nextech.Business.Services
{
    public class StoriesService : IStoriesService
    {
        private const string _newStoriesList = "NewStoriesList";
        private readonly IHttpClientService _httpClientService;
        private readonly IMemoryCache _memoryCache;

        public StoriesService(IMemoryCache memoryCache, IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _memoryCache = memoryCache;
        }

        public async Task<Tuple<IEnumerable<Story>?, int>> GetNewStories(int page, string? titleOrId)
        {

            if (_memoryCache.TryGetValue(_newStoriesList, out List<Story> storylist))
            {
                int rows = 0;
                storylist = ApplyFilterAndPagination(storylist, page, titleOrId, ref rows);
                return new Tuple<IEnumerable<Story>?, int>(storylist, rows);
            }

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

                SaveInMemoryCache(stories);

                stories = ApplyFilterAndPagination(stories, page, titleOrId, ref totalRows);
            }

            return new Tuple<IEnumerable<Story>?, int>(stories, totalRows);
        }

        private void SaveInMemoryCache(List<Story> stories)
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10))
                .SetAbsoluteExpiration(TimeSpan.FromHours(8));

            _memoryCache.Set(_newStoriesList, stories, cacheOptions);
        }

        private List<Story> ApplyFilterAndPagination(List<Story> stories, int page, string? titleOrId, ref int totalRows)
        {
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
            stories = stories.Skip(page <= 1 ? 0 : (page - 1) * 10).Take(10).ToList();

            return stories;
        }

        public async Task<Story?> GetStoryById(int storyId)
        {
            return await _httpClientService.GetDataAsync<Story>($"https://hacker-news.firebaseio.com/v0/item/{storyId}.json");
        }
    }
}
