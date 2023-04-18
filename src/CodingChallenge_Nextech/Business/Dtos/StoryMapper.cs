using CodingChallenge_Nextech.Model;
using Riok.Mapperly.Abstractions;

namespace CodingChallenge_Nextech.Business.Dtos
{
    [Mapper]
    public partial class StoryMapper
    {
        public partial StoryDto StoryToStoryDto(Story story);

        public partial List<StoryDto> StoryListToStoryDtoList(List<Story> stories);
    }
}
