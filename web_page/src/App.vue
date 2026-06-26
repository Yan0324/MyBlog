<template>
  <transition name="app-boot" mode="out-in">
    <!-- 加载动画 -->
    <LoadingAnimation v-if="isLoading" key="loading" />

    <!-- 主要内容 -->
    <div v-else key="app" class="app-shell" :class="{ 'app-shell--home': isHomeRoute }">
      <ParticleCanvas />
      <header class="app-topbar">
        <RouterLink
          v-if="!isHomeRoute"
          to="/"
          class="home-avatar-link"
          aria-label="返回首页"
          title="返回首页"
        >
          <img :src="logoUrl" alt="Cyan avatar" class="home-avatar-image" />
        </RouterLink>
        <NavBar />
      </header>
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
  </transition>
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
      isLoading: true,
      loadingTimer: null,
      logoUrl
    }
  },
  computed: {
    isHomeRoute() {
      return this.$route.name === 'home'
    }
  },
  mounted() {
    this.setCircularFavicon(logoUrl)

    const prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches
    const initialDelay = prefersReducedMotion ? 120 : 850

    this.loadingTimer = window.setTimeout(() => {
      this.isLoading = false
    }, initialDelay)
  },
  beforeUnmount() {
    if (this.loadingTimer) {
      clearTimeout(this.loadingTimer)
    }
  },
  methods: {
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
.app-boot-enter-active,
.app-boot-leave-active {
  transition: opacity 0.45s ease, transform 0.45s ease;
}

.app-boot-enter-from,
.app-boot-leave-to {
  opacity: 0;
  transform: translateY(10px);
}

.app-shell {
  --nav-offset: clamp(4.6rem, 6vw, 5.4rem);
  position: relative;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

.app-topbar {
  position: sticky;
  top: 0;
  z-index: 140;
  height: var(--nav-offset);
  flex-shrink: 0;
  background:
    linear-gradient(180deg, color-mix(in srgb, var(--bg-color) 92%, transparent) 0%, color-mix(in srgb, var(--bg-color) 82%, transparent) 72%, transparent 100%);
  backdrop-filter: blur(12px);
}

.home-avatar-link {
  position: absolute;
  top: 1.1rem;
  left: max(1.5rem, calc((100vw - 1160px) / 2));
  z-index: 140;
  width: 2.8rem;
  height: 2.8rem;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  border-radius: 999px;
  border: 1px solid rgba(102, 102, 102, 0.16);
  background: rgba(255, 255, 255, 0.72);
  box-shadow: 0 12px 28px rgba(0, 0, 0, 0.08);
  backdrop-filter: blur(10px);
  transition: transform 0.28s ease, box-shadow 0.28s ease, border-color 0.28s ease, background-color 0.28s ease;
}

.home-avatar-link:hover {
  transform: translateY(-2px) scale(1.03);
  border-color: rgba(184, 212, 227, 0.85);
  background: rgba(255, 255, 255, 0.88);
  box-shadow: 0 16px 32px rgba(0, 0, 0, 0.12);
}

.home-avatar-image {
  width: calc(100% - 0.4rem);
  height: calc(100% - 0.4rem);
  display: block;
  object-fit: cover;
  border-radius: inherit;
}

.page-frame {
  flex: 1;
  position: relative;
  z-index: 10;
  display: flex;
  flex-direction: column;
}

.page-frame--home {
  min-height: calc(100vh - var(--nav-offset));
  min-height: calc(100svh - var(--nav-offset));
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

@media (max-width: 768px) {
  .app-topbar {
    --nav-offset: 4.4rem;
  }

  .home-avatar-link {
    top: 0.85rem;
    left: 0.75rem;
    width: 2.45rem;
    height: 2.45rem;
  }
}
</style>
