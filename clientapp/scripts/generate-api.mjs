import { execFileSync } from "node:child_process";
import { readFileSync, rmSync } from "node:fs";

const schema = JSON.parse(readFileSync("openapi.json", "utf8"));
if (!schema.openapi || !Object.keys(schema.paths ?? {}).length) {
  throw new Error(
    "openapi.json に API 定義がありません。npm run export-api を実行してください。",
  );
}

rmSync("src/api", { recursive: true, force: true });
execFileSync(
  process.execPath,
  [
    "node_modules/openapi2aspida/bin/index.js",
    "-i",
    "openapi.json",
    "-o",
    "src/api",
  ],
  { stdio: "inherit" },
);
