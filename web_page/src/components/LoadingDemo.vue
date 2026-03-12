<template>
  <div>
    <!-- 加载动画组件 -->
    <LoadingAnimation v-if="isLoading" />
    
    <!-- 主要内容 -->
    <div v-else class="content">
      <h1>网站加载完成！</h1>
      <p>这是您的网站内容</p>
      <button @click="simulateLoading">重新加载</button>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import LoadingAnimation from './components/LoadingAnimation.vue'

const isLoading = ref(true)

// 模拟加载过程
const simulateLoading = () => {
  isLoading.value = true
  setTimeout(() => {
    isLoading.value = false
  }, 3000) // 3秒后隐藏加载动画
}

onMounted(() => {
  // 页面加载时显示加载动画
  setTimeout(() => {
    isLoading.value = false
  }, 2000) // 2秒后隐藏加载动画
})
</script>

<style scoped>
.content {
  text-align: center;
  padding: 50px;
}

h1 {
  color: #333;
  margin-bottom: 20px;
}

button {
  padding: 10px 20px;
  background: #007bff;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 16px;
}

button:hover {
  background: #0056b3;
}
</style>