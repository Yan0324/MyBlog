<template>
  <main class="main-content home-view">
    <HeroSection />
  </main>
</template>

<script>
import HeroSection from '../components/HeroSection.vue'

export default {
  name: 'HomeView',
  components: {
    HeroSection
  }
}
</script>

<style scoped>
.home-view {
  position: relative;
  z-index: 10;
}
</style>
