using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TemplateDotnetReact.Tests;

/// <summary>テスト用ホストで、React が依存する API の HTTP 契約を確認します。</summary>
/// <param name="factory">実際のルーティングと JSON 設定を使うテスト用ホストを作成します。</param>
public sealed class ApiTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    /// <summary>画面が挨拶文を読めるよう、応答のステータス、形式、JSON の項目名と値を確認します。</summary>
    /// <returns>API の応答を受け取り、検証を終えるまでのタスクです。</returns>
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

    /// <summary>API のパスを間違えたとき、SPA の HTML が成功として返らず、呼び出し元が 404 を検知できることを確認します。</summary>
    /// <returns>未知のパスへの要求とステータスの検証が終わるまでのタスクです。</returns>
    [Fact]
    public async Task UnknownApiReturnsNotFound()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/api/missing");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
