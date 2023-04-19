using CodingChallenge_Nextech.Business.Dtos;
using CodingChallenge_Nextech.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodingChallenge_Nextech.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoryController : ControllerBase
    {
        private readonly ILogger<StoryController> _logger;
        private readonly IStoriesService _storiesService;

        public StoryController(ILogger<StoryController> logger, IStoriesService storiesService)
        {
            _logger = logger;
            _storiesService = storiesService;
        }

        [HttpGet]
        public async Task<NewStoriesGridDto> Get(int page = 1, string? titleOrId = "")
        {
            try
            {
                var rdo = await _storiesService.GetNewStories(page, titleOrId);

                List<StoryDto> stories = new();
                if (rdo != null && rdo.Item1 != null)
                {
                    var mapper = new StoryMapper();
                    stories = mapper.StoryListToStoryDtoList(rdo.Item1.ToList());
                }

                return new NewStoriesGridDto { Data = stories, Total = rdo?.Item2 ?? 0 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Get method");
                throw;
            }
        }
    }
}