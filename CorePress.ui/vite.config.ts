import { defineConfig } from 'vite';
import solidPlugin from 'vite-plugin-solid';
import suidPlugin from "@suid/vite-plugin";
import postcssPresetEnv from 'postcss-preset-env';
import { VitePWA } from 'vite-plugin-pwa'
export default defineConfig({
  plugins: [suidPlugin(), solidPlugin(),VitePWA()],
  server: {
    port: 3000,
  },
  css:{
    postcss:{
      plugins: [postcssPresetEnv()],
    }
  },
  build: {
    target: 'esnext',
  },
});
