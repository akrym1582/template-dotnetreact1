import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import useAspidaSWR from "@aspida/swr";
import { client } from "./client";
import "./style.css";

function App() {
  const { data, error, isLoading } = useAspidaSWR(client.api.greeting);
  return (
    <main className="mx-auto max-w-2xl px-6 py-16">
      <h1 className="text-3xl font-bold">ASP.NET Core + React</h1>
      <p className="mt-4" role="status">
        {isLoading && "API に接続しています…"}
        {error && "API の呼び出しに失敗しました。"}
        {data?.message}
      </p>
    </main>
  );
}

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <App />
  </StrictMode>,
);
