---
name: update-api-client
description: ASP.NET Core の API 契約の追加・変更、OpenAPI の出力、aspida の再生成、React からの API 呼び出しの実装・修正に使う。
---

# API クライアントの更新

1. `/api` 配下の C# エンドポイントでリクエスト・レスポンスの型、ステータス、必須・nullable の扱いを明示する。既存 API の変更は呼び出し元と互換性を確認する。
2. リポジトリのルートから次を実行する。アプリの開発サーバーは不要。

   ```sh
   npm --prefix src/TemplateDotnetReact/clientapp ci
   npm --prefix src/TemplateDotnetReact/clientapp run update-api
   ```

3. `clientapp/openapi.json` と `clientapp/src/api/` の差分で、パス・メソッド・型・ステータスが意図どおりか確認する。両方を C# の変更と一緒にコミットする。生成物の不備は API 定義または生成設定を直して再生成する。
4. React では `clientapp/src/client.ts` の型付きクライアントを使い、読み込み、成功、失敗を扱う。読み取りでは既存の SWR パターンを使う。相対 URL を使い、開発時の Vite プロキシと公開時の同一オリジン配信に対応する。
5. xUnit で実際の HTTP 契約、Vitest で画面の動作を確認する。コマンドは [verify-changes](../verify-changes/SKILL.md) を参照する。
