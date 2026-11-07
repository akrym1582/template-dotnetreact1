# 開発ルール

- ASP.NET Core の Web プロジェクトを `src/TemplateDotnetReact/` に 1 つ置き、React + TypeScript を `src/TemplateDotnetReact/clientapp/` に置く。
- API は `/api` 配下に追加し、OpenAPI に公開する。型付きレスポンスを定義し、既存 API のパスとレスポンスの互換性を保つ。
- フロントエンドは `src/TemplateDotnetReact/clientapp/src/client.ts` の aspida クライアントを使う。データ取得には既存の SWR パターンを使う。
- API 契約を変更したら `npm --prefix src/TemplateDotnetReact/clientapp run update-api` を実行し、`src/TemplateDotnetReact/clientapp/openapi.json` と `src/TemplateDotnetReact/clientapp/src/api/` の差分を API の変更と一緒にコミットする。生成物は手で編集しない。
- 開発時は Vite のプロキシ、公開時は ASP.NET Core の同一オリジン配信を使う。API の呼び出し先は相対パスにする。
- 変更後は関連するチェックを実行する。標準の起動、生成、検証コマンドは [README.md](README.md) を参照する。
