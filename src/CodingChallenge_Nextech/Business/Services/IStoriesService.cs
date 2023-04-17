using CodingChallenge_Nextech.Model;

namespace CodingChallenge_Nextech.Business.Services
{
    public interface IStoriesService
    {
        public Task<IEnumerable<Story>> GetNewStories();
        public Task<Story?> GetStoryById(int storyId);
    }
}
