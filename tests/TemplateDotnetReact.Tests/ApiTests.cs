using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TemplateDotnetReact.Tests;

public sealed class ApiTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GreetingReturnsExpectedJson()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/api/greeting");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("ASP.NET Core からこんにちは", body.RootElement.GetProperty("message").GetString());
    }

    [Fact]
    public async Task UnknownApiReturnsNotFound()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/api/missing");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
