namespace TemplateDotnetReact.Features.Greeting;

/// <summary>画面が ASP.NET Core との接続を確認するための API をまとめます。</summary>
internal static class GreetingEndpoints
{
    /// <summary>開発時の Vite 経由と公開後の同一オリジン配信で共通の API パスを登録します。</summary>
    /// <param name="app">アプリの API ルートを登録するホストです。</param>
    internal static void Map(WebApplication app)
    {
        app.MapGet("/api/greeting", () => TypedResults.Ok(new Greeting("ASP.NET Core からこんにちは")))
            .WithName("GetGreeting");
    }
}
