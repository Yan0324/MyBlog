const { defineConfig } = require('@vue/cli-service')

module.exports = defineConfig({
  transpileDependencies: true,
  devServer: {
    // 开发时把 /api 转发到本地 ASP.NET blog_server
    proxy: {
      '/api': {
        target: 'http://localhost:5115',
        changeOrigin: true
      }
    }
  }
})
