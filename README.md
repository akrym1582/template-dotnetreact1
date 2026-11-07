# ASP.NET Core + React テンプレート

[JobRunner](https://github.com/akrym1582/JobRunner) の Web 構成を元にしたスターターです。ASP.NET Core (.NET 10) の 1 つの Web プロジェクトに React 19、TypeScript、Vite、Tailwind CSS v4 を同梱します。画面のサンプルは aspida と SWR で `GET /api/greeting` を呼び出します。

.NET 10 SDK と Node.js 22 以上が必要です。エディターに関係なく、以下のコマンドで開発できます。プロジェクトの共通ルールは [AGENTS.md](AGENTS.md) にあります。

## 開発

リポジトリのルートで実行します。Web プロジェクトと React のソースは `src/TemplateDotnetReact/` にあります。

```sh
npm --prefix src/TemplateDotnetReact/clientapp ci
dotnet run --project src/TemplateDotnetReact
```

[画面](http://localhost:5173) を開いてください。ASP.NET Core はポート 5080、Vite はポート 5173 で起動します。Vite は `/api` と `/swagger` を ASP.NET Core に転送します。[Swagger UI](http://localhost:5080/swagger) で API を確認できます。

API だけを起動したい場合は `dotnet run --project src/TemplateDotnetReact --no-launch-profile --urls http://localhost:5080` を使います。開発ポートを変更する場合は `src/TemplateDotnetReact/Properties/launchSettings.json` と Vite のプロキシ先を合わせてください。プロキシ先は `API_URL` 環境変数でも上書きできます。

## API クライアントの再生成

```sh
npm --prefix src/TemplateDotnetReact/clientapp run update-api
```

ASP.NET Core から `src/TemplateDotnetReact/clientapp/openapi.json` を出力し、`openapi2aspida` で `src/TemplateDotnetReact/clientapp/src/api/` を再生成します。スキーマ出力用ホストは一時ポートを使うため、開発サーバーの起動中でも実行できます。生成された両方の差分を API の変更と一緒にコミットします。

## 検証と公開

```sh
npm --prefix src/TemplateDotnetReact/clientapp run lint
npm --prefix src/TemplateDotnetReact/clientapp run format:check
dotnet publish src/TemplateDotnetReact -c Release -o artifacts/publish
```

`dotnet publish` が npm の依存関係を復元し、フロントエンドをビルドして公開先の `wwwroot/` に含めます。公開環境では ASP.NET Core が画面と API を同じオリジンで配信します。Node.js は公開物の作成時に使用します。

公開物をローカルで確認するには、`artifacts/publish` へ移動して `dotnet TemplateDotnetReact.dll --urls http://localhost:5080` を実行します。
