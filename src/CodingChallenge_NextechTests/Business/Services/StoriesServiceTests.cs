using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using Moq;
using Moq.Protected;
using Microsoft.Extensions.Caching.Memory;
using CodingChallenge_Nextech.Model;
using NSubstitute;
using Newtonsoft.Json.Linq;

namespace CodingChallenge_Nextech.Business.Services.Tests
{
    [TestClass()]
    public class StoriesServiceTests
    {
        [TestMethod()]
        public async Task GetStoryByIdTest()
        {
            var inMemoryCacheMock = new Mock<IMemoryCache>();
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{ ""id"": 8863, ""title"": ""My YC app: Dropbox - Throw away your USB drive"", ""by"": ""dhouston"", ""url"": ""http://www.getdropbox.com/u/2/screencast.html""}")
            };

            handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(handlerMock.Object);
            var httpClientService = new HttpClientService(httpClient);
            var storiesService = new StoriesService(inMemoryCacheMock.Object, httpClientService);

            var ret = await storiesService.GetStoryById(8863);

            Assert.IsNotNull(ret);
            handlerMock.Protected().Verify(
              "SendAsync",
              Times.Exactly(1),
              ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
              ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod()]
        public async Task GetNewStoriesWithoutAnyDataInMemoryCacheTest()
        {
            var inMemoryCacheMock = new Mock<IMemoryCache>();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            int page = 1;
            string title = string.Empty;
            //NewStoriesIds
            var idsResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"[""35621318"", ""35621309"", ""35621260"", ""35621256"", ""35621238"", ""35621234"", ""35621229"" ]")
            };
            //NewStories List
            var storiesResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{ ""id"": 8863, ""title"": ""My YC app: Dropbox - Throw away your USB drive"", ""by"": ""dhouston"", ""url"": ""http://www.getdropbox.com/u/2/screencast.html""}")
            };

            inMemoryCacheMock
                 .Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(Mock.Of<ICacheEntry>);

            var handlerPart = handlerMock
           .Protected()
           .SetupSequence<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
           );

            handlerPart = handlerPart.ReturnsAsync(idsResponse);
            handlerPart = handlerPart.ReturnsAsync(storiesResponse);
            handlerPart = handlerPart.ReturnsAsync(storiesResponse);
            handlerPart = handlerPart.ReturnsAsync(storiesResponse);
            handlerPart = handlerPart.ReturnsAsync(storiesResponse);
            handlerPart = handlerPart.ReturnsAsync(storiesResponse);
            handlerPart = handlerPart.ReturnsAsync(storiesResponse);
            handlerPart = handlerPart.ReturnsAsync(storiesResponse);

            var httpClient = new HttpClient(handlerMock.Object);
            var httpClientService = new HttpClientService(httpClient);
            var storiesService = new StoriesService(inMemoryCacheMock.Object, httpClientService);

            //Act
            var ret = await storiesService.GetNewStories(page, title);

            //Assert
            Assert.IsNotNull(ret);
            handlerMock.Protected().Verify(
              "SendAsync",
              Times.Exactly(8),
              ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
              ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod()]
        public async Task GetNewStoriesWithDataInMemoryCacheTest()
        {
            var inMemoryCacheMock = new Mock<IMemoryCache>();
            var handlerMock = new Mock<HttpMessageHandler>();
            int page = 1;
            string title = string.Empty;
            List<Story> storylist = new() { new Story{ Id = 8863, Title = "My YC app: Dropbox - Throw away your USB drive", By = "dhouston", Url = "http://www.getdropbox.com/u/2/screencast.html" }
                                            , new Story{Id = 35620082, Title = "Iowa's Senate advances bill to loosen child labor laws", By = "consumer451", Url = "https://www.washingtonpost.com/politics/2023/04/18/iowa-child-labor-bill/b8becf0c-de2a-11ed-a78e-9a7c2418b00c_story.html" }
                                          };

            var mockMemoryCache = Substitute.For<IMemoryCache>();
            mockMemoryCache.TryGetValue(Arg.Is<string>(x => x.Equals("NewStoriesList")), out List<Story> list)
                          .Returns(x =>
                          {
                              x[1] = storylist;
                              return true;
                          });

            var httpClient = new HttpClient(handlerMock.Object);
            var httpClientService = new HttpClientService(httpClient);
            var storiesService = new StoriesService(mockMemoryCache, httpClientService);

            //Act
            var ret = await storiesService.GetNewStories(page, title);

            //Assert
            Assert.IsNotNull(ret);
            handlerMock.Protected().Verify(
              "SendAsync",
              Times.Exactly(0),
              ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
              ItExpr.IsAny<CancellationToken>());
        }


    }
}