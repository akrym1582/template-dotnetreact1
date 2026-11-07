import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { GreetingPage } from "./features/greeting/GreetingPage";
import "./style.css";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <GreetingPage />
  </StrictMode>,
);
