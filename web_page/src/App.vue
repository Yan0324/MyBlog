<template>
  <ParticleCanvas />
  <NavBar :theme="theme" @toggle-theme="toggleTheme" />
  <main id="home" class="main-content">
    <HeroSection />
  </main>
  <footer class="footer">
    <p>&copy; 2026 Cyan. All Rights Reserved.</p>
  </footer>
</template>

<script>
import NavBar from './components/NavBar.vue'
import ParticleCanvas from './components/ParticleCanvas.vue'
import HeroSection from './components/HeroSection.vue'

export default {
  name: 'App',
  components: { NavBar, ParticleCanvas, HeroSection },
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
  },
  methods: {
    toggleTheme() {
      this.theme = this.theme === 'dark' ? 'light' : 'dark'
      localStorage.setItem('theme', this.theme)
    }
  }
}
</script>
