<template>
  <div class="loading-container">
    <div class="loading-content">
      <DotLottieVue 
        style="height: 500px; width: 500px" 
        autoplay 
        loop 
        :src="animationUrl" 
      />
      <div class="loading-text">加载中...</div>
    </div>
  </div>
</template>

<script setup>
import { DotLottieVue } from '@lottiefiles/dotlottie-vue'
import { ref } from 'vue'

const animationUrl = ref(new URL('../assets/load.lottie', import.meta.url).href)
</script>

<style scoped>
.loading-container {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background: rgba(255, 255, 255, 0.95);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999;
  backdrop-filter: blur(5px);
}

.loading-content {
  text-align: center;
}

.loading-text {
  margin-top: 20px;
  font-size: 18px;
  color: #333;
  font-weight: 500;
  animation: fadeInOut 2s ease-in-out infinite;
}

@keyframes fadeInOut {
  0%, 100% { opacity: 0.6; }
  50% { opacity: 1; }
}
</style>