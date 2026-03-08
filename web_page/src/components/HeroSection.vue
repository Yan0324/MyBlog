<template>
  <div class="content-wrapper">
    <!-- Logo 区域 -->
    <div class="logo-section" ref="logoSection">
      <h1 class="brand-logo">Cyan</h1>
      <p class="brand-subtitle">A better day when night belongs to dawn</p>
    </div>

    <!-- 头像区域 -->
    <div class="avatar-section" ref="avatarSection">
      <div class="avatar-container" ref="avatarContainer">
        <img src="../assets/logo.png" alt="Avatar" class="avatar-img" />
      </div>
    </div>

    <!-- 年度关键词与状态 -->
    <div class="status-section" ref="statusSection">
      <div class="keyword">Be Rich</div>
      <div class="status-line">2026 &middot; 平静</div>
    </div>

    <!-- 分隔线 -->
    <div class="separator" ref="separator">
      <div class="line-draw"></div>
    </div>

    <!-- 引用区 -->
    <div class="quote-section" ref="quoteSection">
      <p class="quote-text" ref="quoteText"></p>
      <p class="quote-source">&mdash; 「中牟道中」</p>
    </div>
  </div>
</template>

<script>
const QUOTE_TEXT = '「如何得与凉风约，不共尘沙一并来！」'

export default {
  name: 'HeroSection',
  mounted() {
    this.startAnimations()
    this.initTiltEffect()
  },
  beforeUnmount() {
    if (this._mouseMoveHandler) {
      document.removeEventListener('mousemove', this._mouseMoveHandler)
    }
  },
  methods: {
    animateIn(el, delay) {
      if (!el) return
      setTimeout(() => {
        el.style.transition = 'all 0.8s cubic-bezier(0.25, 0.46, 0.45, 0.94)'
        el.style.opacity = '1'
        el.style.transform = 'translate(0, 0) scale(1)'
        el.classList.add('visible')
      }, delay)
    },

    startAnimations() {
      setTimeout(() => {
        // Logo: 从上滑入
        this.animateIn(this.$refs.logoSection, 100)

        // 头像: 弹性弹出
        setTimeout(() => {
          const avatar = this.$refs.avatarSection
          if (avatar) {
            avatar.style.transition = 'opacity 0.5s ease, transform 0.8s cubic-bezier(0.68, -0.55, 0.265, 1.55)'
            avatar.style.opacity = '1'
            avatar.style.transform = 'scale(1)'
          }
        }, 300)

        // 状态: 从左滑入
        this.animateIn(this.$refs.statusSection, 500)

        // 分隔线
        setTimeout(() => {
          if (this.$refs.separator) this.$refs.separator.classList.add('visible')
        }, 650)

        // 引用区淡入
        this.animateIn(this.$refs.quoteSection, 800)

        // 打字机效果
        setTimeout(() => this.startTypewriter(), 800)
      }, 100)
    },

    startTypewriter() {
      const el = this.$refs.quoteText
      if (!el) return
      el.textContent = ''
      let i = 0
      const type = () => {
        if (i < QUOTE_TEXT.length) {
          el.textContent += QUOTE_TEXT.charAt(i)
          i++
          setTimeout(type, 100)
        } else {
          el.classList.add('done')
          if (this.$refs.quoteSection) this.$refs.quoteSection.classList.add('visible')
        }
      }
      type()
    },

    initTiltEffect() {
      if (window.matchMedia('(prefers-reduced-motion: reduce)').matches) return
      const container = this.$refs.avatarContainer
      if (!container) return

      this._mouseMoveHandler = (e) => {
        const rotateX = (e.clientY / window.innerHeight) * -10
        const rotateY = (e.clientX / window.innerWidth) * 10
        container.style.transform = `perspective(1000px) rotateX(${rotateX}deg) rotateY(${rotateY}deg) scale(1)`
      }

      container.addEventListener('mouseenter', () => {
        container.style.transition = 'transform 0.1s ease'
      })

      container.addEventListener('mouseleave', () => {
        container.style.transition = 'transform 0.5s ease'
        container.style.transform = 'perspective(1000px) rotateX(0) rotateY(0) scale(1)'
      })

      document.addEventListener('mousemove', this._mouseMoveHandler)
    }
  }
}
</script>

<style>
/* Main Content */
.main-content {
  flex: 1;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding-top: 10vh;
  padding-bottom: 5vh;
  z-index: 10;
  min-height: 90vh;
}

.content-wrapper {
  text-align: center;
  max-width: 800px;
  padding: 0 2rem;
  width: 100%;
}

/* Logo Section */
.logo-section {
  opacity: 0;
  transform: translateY(-30px);
}

.brand-logo {
  font-family: var(--font-hand);
  font-size: clamp(4rem, 8vw, 6rem);
  color: var(--logo-fill);
  -webkit-text-stroke: 3px var(--stroke-color);
  text-shadow: 5px 5px 0px var(--accent-cyan);
  line-height: 1.2;
  margin-bottom: 0.5rem;
  transform: rotate(-2deg);
  display: inline-block;
  transition: text-shadow 0.3s ease, transform 0.3s ease;
}

.brand-logo:hover {
  text-shadow: 7px 7px 0px var(--accent-cyan);
  animation: wobble 1s ease-in-out infinite;
}

@keyframes wobble {
  0%, 100% { transform: rotate(-2deg); }
  50%       { transform: rotate(2deg); }
}

.brand-subtitle {
  font-family: var(--font-mono);
  font-size: 0.8rem;
  text-transform: uppercase;
  letter-spacing: 0.3em;
  color: var(--text-primary);
  margin-bottom: 2rem;
  opacity: 0.9;
}

/* Avatar Section */
.avatar-section {
  margin-bottom: 2.5rem;
  opacity: 0;
  transform: scale(0);
  display: flex;
  justify-content: center;
}

.avatar-container {
  width: 150px;
  height: 150px;
  border-radius: 50%;
  border: 3px solid var(--bg-color);
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
  overflow: hidden;
  transition: all 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
  cursor: pointer;
  background-color: #fff;
}

.avatar-container:hover {
  border-color: var(--accent-cyan);
  box-shadow: 0 10px 30px rgba(184, 212, 227, 0.4);
}

.avatar-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.5s ease;
}

/* Status Section */
.status-section {
  margin-bottom: 3rem;
  opacity: 0;
  transform: translateX(-50px);
}

.keyword {
  font-size: 2rem;
  font-weight: 700;
  color: var(--text-primary);
  margin-bottom: 0.5rem;
  font-family: var(--font-serif);
}

.status-line {
  font-size: 0.9rem;
  color: var(--text-secondary);
  font-family: var(--font-serif);
}

/* Separator */
.separator {
  margin: 3rem auto;
  width: 60px;
  height: 2px;
  position: relative;
  display: flex;
  justify-content: center;
}

.line-draw {
  width: 0%;
  height: 1px;
  background-color: var(--text-secondary);
  opacity: 0.5;
  transition: width 1s ease-out;
}

.separator.visible .line-draw {
  width: 100%;
}

/* Quote Section */
.quote-section {
  margin-top: 2rem;
  position: relative;
  opacity: 0;
}

.quote-text {
  font-family: var(--font-serif);
  font-style: italic;
  font-size: 1.2rem;
  color: var(--text-secondary);
  margin-bottom: 1rem;
  line-height: 2;
  min-height: 2.4em;
}

.quote-text::after {
  content: '|';
  animation: blink 1s infinite;
  opacity: 1;
}

.quote-text.done::after {
  display: none;
}

@keyframes blink {
  0%, 100% { opacity: 1; }
  50%       { opacity: 0; }
}

.quote-source {
  font-family: var(--font-serif);
  font-size: 0.9rem;
  color: var(--text-secondary);
  text-align: right;
  margin-right: 2rem;
  opacity: 0;
  transform: translateY(10px);
  transition: opacity 0.5s ease, transform 0.5s ease;
}

.quote-section.visible .quote-source {
  opacity: 0.7;
  transform: translateY(0);
}

/* Footer */
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

/* Mobile Responsive */
@media (max-width: 768px) {
  .brand-logo {
    -webkit-text-stroke: 2px var(--stroke-color);
    text-shadow: 3px 3px 0px var(--accent-cyan);
    font-size: 3.5rem;
  }

  .brand-subtitle {
    font-size: 0.7rem;
    letter-spacing: 0.15em;
  }

  .avatar-container {
    width: 120px;
    height: 120px;
  }

  .main-content {
    padding-top: 5vh;
  }
}
</style>
