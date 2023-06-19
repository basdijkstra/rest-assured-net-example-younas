using Newtonsoft.Json.Linq;
using NuGet.Frameworks;
using RestAssuredExampleYounas.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using static RestAssured.Dsl;

namespace RestAssuredExampleYounas
{
    public class DeserializationTests
    {
        private WireMockServer server;

        private PostWrapper originalPostWrapper;

        [SetUp]
        public void StartWireMock()
        {
            server = WireMockServer.Start(9876);

            Post post = new Post
            {
                CreatedUserId = "123",
                FirstName = "Younas"
            };

            originalPostWrapper = new PostWrapper
            {
                Data = post
            };
        }

        [Test]
        public void UseWrapperClassContainingDataElement()
        {
            SetupStub();

            PostWrapper postWrapper = (PostWrapper)Given()
                .When()
                .Get("http://localhost:9876/data")
                .Then()
                .StatusCode(200)
                .DeserializeTo(typeof(PostWrapper));

            Assert.That(postWrapper.Data.CreatedUserId, Is.EqualTo("123"));
        }

        [Test]
        public void ExtractResponseAndDeserializeElement()
        {
            SetupStub();

            HttpResponseMessage response = Given()
                .When()
                .Get("http://localhost:9876/data")
                .Then()
                .StatusCode(200)
                .Extract().Response();

            string responseBody = response.Content.ReadAsStringAsync().Result;

            JObject responseAsJson = JObject.Parse(responseBody);
            JToken responsePost = responseAsJson.GetValue("data");
            Post post = responsePost.ToObject<Post>();

            Assert.That(post.CreatedUserId, Is.EqualTo("123"));
        }

        [TearDown]
        public void StopServer()
        {
            server.Stop();
        }

        private void SetupStub()
        {
            server.Given(Request.Create().WithPath("/data").UsingGet())
                .RespondWith(Response.Create()
                .WithHeader("Content-Type", "application/json")
                .WithBodyAsJson(originalPostWrapper)
                .WithStatusCode(200));
        }
    }
}