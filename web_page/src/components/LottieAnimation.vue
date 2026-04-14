<template>
  <button
    type="button"
    class="lottie-container"
    @click="scrollToTop"
    aria-label="回到顶部"
    title="回到顶部"
  >
    <DotLottieVue
      style="height: 200px; width: 200px"
      autoplay
      loop
      :src="animationUrl"
    />
  </button>
</template>

<script>
import { DotLottieVue } from '@lottiefiles/dotlottie-vue'

export default {
  name: 'LottieAnimation',
  components: {
    DotLottieVue
  },
  data() {
    return {
      animationUrl: '/Animation.lottie'
    }
  },
  methods: {
    scrollToTop() {
      window.scrollTo({
        top: 0,
        behavior: 'smooth'
      })
    }
  }
}
</script>

<style scoped>
.lottie-container {
  position: fixed;
  bottom: 20px;
  right: 20px;
  z-index: 1000;
  padding: 0;
  border: 0;
  background: transparent;
  cursor: pointer;
}

.lottie-container :deep(canvas),
.lottie-container :deep(svg) {
  pointer-events: none;
}
</style>
