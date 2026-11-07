import { cleanup, render, screen } from "@testing-library/react";
import { afterAll, afterEach, expect, test, vi } from "vitest";
import { SWRConfig } from "swr";
import { GreetingPage } from "./GreetingPage";

const fetchMock = vi.hoisted(() => {
  const mock = vi.fn<typeof fetch>();
  vi.stubGlobal("fetch", mock);
  return mock;
});

afterEach(() => {
  cleanup();
  fetchMock.mockReset();
});
afterAll(() => vi.unstubAllGlobals());

function renderApp() {
  render(
    <SWRConfig value={{ provider: () => new Map(), shouldRetryOnError: false }}>
      <GreetingPage />
    </SWRConfig>,
  );
}

test("renders the greeting returned by the API", async () => {
  fetchMock.mockResolvedValue(Response.json({ message: "テスト用の挨拶" }));
  renderApp();

  expect(await screen.findByText("テスト用の挨拶")).toBeTruthy();
  expect(fetchMock.mock.calls[0]?.[0]).toBe("/api/greeting");
});

test("shows an error when the API request fails", async () => {
  fetchMock.mockResolvedValue(new Response(null, { status: 500 }));
  renderApp();

  expect(
    await screen.findByText("API の呼び出しに失敗しました。"),
  ).toBeTruthy();
});
