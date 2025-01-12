namespace Kanban.IntegrationTests
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestPlatform.TestHost;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public  class CardTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {

        private readonly HttpClient _client;

        public CardTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Given_KanbanBoard_When_AddAnCard_Then_AnCardBeAdded()
        {

            var response = await _client.GetAsync("/Cards");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("expected-value", content);
        }
    }
}
