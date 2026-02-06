import { defineConfig } from "vite";
import react from "@vitejs/plugin-react-swc";
import path from "path";
import { componentTagger } from "lovable-tagger";

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => ({
  server: {
    host: "::",
    port: 8080,
  },
  plugins: [react(), mode === "development" && componentTagger()].filter(Boolean),
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "./src"),
    },
  },
  build: {
    // Otimiza o tamanho do bundle
    target: "esnext",
    minify: "esbuild",
    sourcemap: mode === "development",
    // Code splitting manual para melhor cache
    rollupOptions: {
      output: {
        manualChunks: {
          // Vendor chunks - bibliotecas que mudam raramente
          "vendor-react": ["react", "react-dom", "react-router-dom"],
          "vendor-query": ["@tanstack/react-query"],
          "vendor-ui": [
            "@radix-ui/react-dialog",
            "@radix-ui/react-dropdown-menu",
            "@radix-ui/react-tabs",
            "@radix-ui/react-avatar",
            "@radix-ui/react-tooltip",
          ],
          "vendor-form": ["react-hook-form", "@hookform/resolvers", "zod"],
          "vendor-utils": ["axios", "date-fns", "clsx", "tailwind-merge"],
        },
      },
    },
    // Aumenta o limite de warning para chunks
    chunkSizeWarningLimit: 500,
  },
  // Otimiza dependências em desenvolvimento
  optimizeDeps: {
    include: [
      "react",
      "react-dom",
      "react-router-dom",
      "@tanstack/react-query",
      "axios",
    ],
  },
}));
