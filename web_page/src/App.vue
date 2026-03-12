<template>
  <ParticleCanvas />
  <NavBar :theme="theme" @toggle-theme="toggleTheme" />
  <main id="home" class="main-content">
    <HeroSection />
  </main>
  <footer class="footer">
    <p>&copy; 2026 Cyan. All Rights Reserved.</p>
  </footer>
  <LottieAnimation />
</template>

<script>
import NavBar from './components/NavBar.vue'
import ParticleCanvas from './components/ParticleCanvas.vue'
import HeroSection from './components/HeroSection.vue'
import LottieAnimation from './components/LottieAnimation.vue'
import logoUrl from './assets/logo.png'

export default {
  name: 'App',
  components: { NavBar, ParticleCanvas, HeroSection, LottieAnimation },
  data() {
    return {
      theme: localStorage.getItem('theme') || 'light'
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
