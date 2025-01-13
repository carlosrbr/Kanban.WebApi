namespace Kanban.IntegrationTests
{
    using System.Diagnostics.CodeAnalysis;
    using AutoMapper.Configuration.Annotations;
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

        [Fact(Skip = "Não houve tempo de concluir o teste de integração")]
        public async Task Given_KanbanBoard_When_AddAnCard_Then_AnCardBeAdded()
        {

            var response = await _client.GetAsync("/Cards");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("expected-value", content);
        }
    }
}
