import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { GreetingPage } from "./features/greeting/GreetingPage";
import "@fontsource/noto-sans-jp/400.css";
import "@fontsource/noto-sans-jp/700.css";
import "./style.css";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <GreetingPage />
  </StrictMode>,
);
