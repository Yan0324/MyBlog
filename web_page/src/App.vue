<template>
  <!-- 加载动画 -->
  <LoadingAnimation v-if="isLoading" />
  
  <!-- 主要内容 -->
  <div v-else class="app-shell" :class="{ 'app-shell--home': isHomeRoute }">
    <ParticleCanvas />
    <NavBar :theme="theme" @toggle-theme="toggleTheme" />
    <main class="page-frame" :class="{ 'page-frame--home': isHomeRoute }">
      <router-view v-slot="{ Component, route }">
        <transition name="page-fade" mode="out-in">
          <component :is="Component" :key="route.fullPath" />
        </transition>
      </router-view>
    </main>
    <footer v-if="!isHomeRoute" class="footer">
      <p>&copy; 2026 Cyan. All Rights Reserved.</p>
    </footer>
    <LottieAnimation />
  </div>
</template>

<script>
import NavBar from './components/NavBar.vue'
import ParticleCanvas from './components/ParticleCanvas.vue'
import LottieAnimation from './components/LottieAnimation.vue'
import LoadingAnimation from './components/LoadingAnimation.vue'
import logoUrl from './assets/logo.png'

export default {
  name: 'App',
  components: { NavBar, ParticleCanvas, LottieAnimation, LoadingAnimation },
  data() {
    return {
      theme: localStorage.getItem('theme') || 'light',
      isLoading: true
    }
  },
  computed: {
    isHomeRoute() {
      return this.$route.name === 'home'
    }
  },
  watch: {
    theme(val) {
      document.body.setAttribute('data-theme', val)
    }
  },
  mounted() {
    document.body.setAttribute('data-theme', this.theme)
    this.setCircularFavicon(logoUrl)
    
    // 模拟页面加载过程
    setTimeout(() => {
      this.isLoading = false
    }, 2000) // 2秒后隐藏加载动画
  },
  methods: {
    toggleTheme() {
      this.theme = this.theme === 'dark' ? 'light' : 'dark'
      localStorage.setItem('theme', this.theme)
    },
    setCircularFavicon(src) {
      const size = 64
      const canvas = document.createElement('canvas')
      canvas.width = size
      canvas.height = size
      const ctx = canvas.getContext('2d')
      const img = new Image()
      img.onload = () => {
        ctx.beginPath()
        ctx.arc(size / 2, size / 2, size / 2, 0, Math.PI * 2)
        ctx.closePath()
        ctx.clip()
        ctx.drawImage(img, 0, 0, size, size)
        let link = document.querySelector("link[rel='icon']")
        if (!link) {
          link = document.createElement('link')
          link.rel = 'icon'
          document.head.appendChild(link)
        }
        link.type = 'image/png'
        link.href = canvas.toDataURL('image/png')
      }
      img.src = src
    }
  }
}
</script>

<style>
.app-shell {
  position: relative;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

.page-frame {
  flex: 1;
  position: relative;
  z-index: 10;
  display: flex;
  flex-direction: column;
}

.page-frame--home {
  min-height: 100vh;
  min-height: 100svh;
  overflow: hidden;
}

.footer {
  padding: 2rem;
  text-align: center;
  font-family: var(--font-mono);
  font-size: 0.8rem;
  color: var(--text-secondary);
  opacity: 0.5;
  position: relative;
  z-index: 10;
}

.page-fade-enter-active,
.page-fade-leave-active {
  transition: opacity 0.35s ease, transform 0.35s ease;
}

.page-fade-enter-from,
.page-fade-leave-to {
  opacity: 0;
  transform: translateY(14px);
}
</style>
