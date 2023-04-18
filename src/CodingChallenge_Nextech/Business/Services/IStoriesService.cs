using CodingChallenge_Nextech.Model;

namespace CodingChallenge_Nextech.Business.Services
{
    public interface IStoriesService
    {
        public Task<Tuple<IEnumerable<Story>?, int>> GetNewStories(int page, string? titleOrId);
        public Task<Story?> GetStoryById(int storyId);
    }
}
