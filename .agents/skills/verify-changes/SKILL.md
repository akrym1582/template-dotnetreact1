---
name: verify-changes
description: C# と React の変更のレビュー、StyleCop・ESLint・Prettier の確認、xUnit・Vitest のユニットテストと結合テスト、公開検証、パフォーマンス測定に使う。
---

# 変更の検証

## コード品質

- C# の StyleCop ルールと例外はルートの `.editorconfig` を参照する。違反は原因を修正し、既存コード全体の整形は別の変更に分ける。
- React は ESLint で静的検査し、Prettier で変更した手書きファイルを整形する。生成物は再生成で揃える。
- リポジトリのルートから変更に関係するコマンドを実行する。公開経路の変更は publish 後の画面と API も確認する。

```sh
dotnet test -warnaserror
npm --prefix src/TemplateDotnetReact/clientapp ci
npm --prefix src/TemplateDotnetReact/clientapp run lint
npm --prefix src/TemplateDotnetReact/clientapp run format:check
npm --prefix src/TemplateDotnetReact/clientapp test
npm --prefix src/TemplateDotnetReact/clientapp run build
dotnet publish src/TemplateDotnetReact -c Release -o artifacts/publish
```

## テストの分け方

- 単体テストは、分岐や境界値を持つ業務ロジックを xUnit で、表示や操作を Vitest と React Testing Library で確認する。テストデータと外部依存はテストごとに分離する。
- API の結合テストは `tests/TemplateDotnetReact.Tests/` の `WebApplicationFactory` を使い、ルーティング、HTTP ステータス、JSON、入力エラーなど契約に関係する動作を確認する。画面の API 成功・失敗はモックを使う。
- ルーティング、静的配信、ブラウザー固有の操作を変更した場合は、必要な範囲で実ブラウザーまたは公開物を起動して確認する。実行できなかった確認は結果に明記する。

## パフォーマンス

大量データ、頻繁な再描画、ホットパス、起動や配信サイズに影響する変更で測定する。変更前後を同じビルド設定・データ量・環境で比較し、ウォームアップ後に複数回測る。対象に応じて p50・p95 の遅延、割り当て、描画回数、バンドルサイズなどを選び、測定条件と結果を記録する。
