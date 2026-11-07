import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import tailwind from "@tailwindcss/vite";

const apiUrl = process.env.API_URL || "http://localhost:5080";

export default defineConfig({
  plugins: [react(), tailwind()],
  server: {
    proxy: {
      "/api": apiUrl,
      "/swagger": apiUrl,
    },
  },
});
