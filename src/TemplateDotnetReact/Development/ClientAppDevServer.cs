using System.Diagnostics;

namespace TemplateDotnetReact.Development;

/// <summary>
/// 開発時に ASP.NET Core と一緒に Vite を動かします。
/// ASP.NET Core が終了したときは Vite も止め、再起動したアプリが同じポートを使えるようにします。
/// </summary>
/// <param name="environment">起動した場所にかかわらず、Web プロジェクト配下の clientapp を見つけるために使います。</param>
/// <param name="lifetime">Vite が予期せず終了したときに、API だけが動き続けるのを防ぐために使います。</param>
/// <param name="logger">開発者が画面の URL と起動失敗の原因を確認するために使います。</param>
public sealed class ClientAppDevServer(IHostEnvironment environment, IHostApplicationLifetime lifetime, ILogger<ClientAppDevServer> logger) : BackgroundService
{
    /// <summary>API の起動と同時に開発画面を使えるよう、ホストのバックグラウンド処理として Vite を起動します。</summary>
    /// <param name="stoppingToken">ホストの停止を検知し、Vite の終了を待つ処理から後片付けへ移るための通知です。</param>
    /// <returns>Vite とその起動プロセスの後片付けが終わるまでのタスクです。</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var start = new ProcessStartInfo(OperatingSystem.IsWindows() ? "cmd.exe" : "npm")
        {
            WorkingDirectory = Path.Combine(environment.ContentRootPath, "clientapp"),
            UseShellExecute = false,
        };
        if (OperatingSystem.IsWindows())
        {
            start.ArgumentList.Add("/c");
            start.ArgumentList.Add("npm");
        }

        start.ArgumentList.Add("run");
        start.ArgumentList.Add("dev");
        using var process = Process.Start(start) ?? throw new InvalidOperationException("Viteを起動できません。npm ciを実行してください。");
        try
        {
            logger.LogInformation("開発画面: http://localhost:5173 (APIは.NETへプロキシします)");
            await process.WaitForExitAsync(stoppingToken);
            if (!stoppingToken.IsCancellationRequested)
            {
                logger.LogError("Viteが終了しました。終了コード: {ExitCode}", process.ExitCode);
                lifetime.StopApplication();
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // ホスト停止による待機の中断は正常な終了として扱う。
        }
        finally
        {
            if (!process.HasExited)
            {
                // npm の子プロセスである Vite も終了させ、開発用ポートを解放する。
                process.Kill(entireProcessTree: true);
            }

            await process.WaitForExitAsync(CancellationToken.None);
        }
    }
}
