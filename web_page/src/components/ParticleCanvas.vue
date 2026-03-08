<template>
  <canvas ref="canvas" class="particle-overlay"></canvas>
</template>

<script>
export default {
  name: 'ParticleCanvas',
  data() {
    return {
      animFrameId: null
    }
  },
  mounted() {
    if (window.matchMedia('(prefers-reduced-motion: reduce)').matches) return

    const canvas = this.$refs.canvas
    const ctx = canvas.getContext('2d')
    let particles = []
    const particleCount = 50

    const resizeCanvas = () => {
      canvas.width = window.innerWidth
      canvas.height = window.innerHeight
    }

    this._resizeHandler = resizeCanvas
    window.addEventListener('resize', this._resizeHandler)
    resizeCanvas()

    class Particle {
      constructor() {
        this.x = Math.random() * canvas.width
        this.y = Math.random() * canvas.height
        this.size = Math.random() * 2 + 0.5
        this.speedX = Math.random() * 0.5 - 0.25
        this.speedY = Math.random() * 0.5 - 0.25
        this.opacity = Math.random() * 0.5 + 0.1
      }

      update() {
        this.x += this.speedX
        this.y += this.speedY
        if (this.x > canvas.width) this.x = 0
        if (this.x < 0) this.x = canvas.width
        if (this.y > canvas.height) this.y = 0
        if (this.y < 0) this.y = canvas.height
      }

      draw() {
        ctx.fillStyle = `rgba(100, 100, 100, ${this.opacity})`
        ctx.beginPath()
        ctx.arc(this.x, this.y, this.size, 0, Math.PI * 2)
        ctx.fill()
      }
    }

    for (let i = 0; i < particleCount; i++) {
      particles.push(new Particle())
    }

    const animate = () => {
      ctx.clearRect(0, 0, canvas.width, canvas.height)
      particles.forEach(p => { p.update(); p.draw() })
      this.animFrameId = requestAnimationFrame(animate)
    }

    animate()
  },
  beforeUnmount() {
    if (this.animFrameId) cancelAnimationFrame(this.animFrameId)
    if (this._resizeHandler) window.removeEventListener('resize', this._resizeHandler)
  }
}
</script>

<style>
.particle-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  pointer-events: none;
  z-index: 0;
}
</style>
