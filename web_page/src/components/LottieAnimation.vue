<template>
  <div class="lottie-container">
    <DotLottieVue
      style="height: 200px; width: 200px"
      autoplay
      loop
      :src="animationUrl"
    />
  </div>
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
  }
}
</script>

<style scoped>
.lottie-container {
  position: fixed;
  bottom: 20px;
  right: 20px;
  z-index: 1000;
  pointer-events: none;
}
</style>