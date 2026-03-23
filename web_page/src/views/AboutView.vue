<template>
  <div class="about-view">
    <AboutSection />
  </div>
</template>

<script>
import AboutSection from '../components/AboutSection.vue'

export default {
  name: 'AboutView',
  components: {
    AboutSection
  }
}
</script>

<style scoped>
.about-view {
  position: relative;
  z-index: 10;
  padding-top: clamp(6rem, 10vw, 7.5rem);
}
</style>
