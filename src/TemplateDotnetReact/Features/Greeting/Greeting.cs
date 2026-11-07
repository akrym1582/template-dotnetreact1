namespace TemplateDotnetReact.Features.Greeting;

/// <summary>サンプル画面が API との接続を確認するための応答です。</summary>
/// <param name="Message">画面に表示する挨拶文です。JSON では message として返します。</param>
internal sealed record Greeting(string Message);
