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
      <p class="quote-source">{{ quoteSourceText }}</p>
    </div>

    <div class="social-section" ref="socialSection">
      <a
        class="social-link"
        href="https://github.com/Yan0324"
        target="_blank"
        rel="noreferrer"
        aria-label="GitHub"
        title="GitHub"
      >
        <img src="../assets/github.png" alt="" class="social-icon" />
      </a>
      <a
        class="social-link"
        href="https://space.bilibili.com/2135587869?spm_id_from=333.1007.0.0"
        target="_blank"
        rel="noreferrer"
        aria-label="哔哩哔哩"
        title="哔哩哔哩"
      >
        <img src="../assets/bilibili.png" alt="" class="social-icon" />
      </a>
    </div>
  </div>
</template>

<script>
const FALLBACK_POEM = {
  content: '「如何得与凉风约，不共尘沙一并来！」',
  source: '—— 《中牟道中》'
}

const JINRISHICI_SDK_SRC = 'https://sdk.jinrishici.com/v2/browser/jinrishici.js'
const REVEAL_EASE = 'cubic-bezier(0.22, 1, 0.36, 1)'
const REVEAL_DURATION = 760
const TYPE_INTERVAL = 58

export default {
  name: 'HeroSection',
  data() {
    return {
      quoteTargetText: FALLBACK_POEM.content,
      quoteSourceText: FALLBACK_POEM.source,
      hasStartedTyping: false,
      typingTimer: null,
      typingRunId: 0,
      pendingTimers: []
    }
  },
  mounted() {
    this._isUnmounted = false
    this.startAnimations()
    this.initTiltEffect()
    this.loadDailyPoem()
  },
  beforeUnmount() {
    this.clearPendingTimers()
    if (this.typingTimer) {
      clearTimeout(this.typingTimer)
    }
    if (this._avatarPointerEnterHandler && this.$refs.avatarContainer) {
      this.$refs.avatarContainer.removeEventListener('pointerenter', this._avatarPointerEnterHandler)
    }
    if (this._avatarPointerMoveHandler && this.$refs.avatarContainer) {
      this.$refs.avatarContainer.removeEventListener('pointermove', this._avatarPointerMoveHandler)
    }
    if (this._avatarPointerLeaveHandler && this.$refs.avatarContainer) {
      this.$refs.avatarContainer.removeEventListener('pointerleave', this._avatarPointerLeaveHandler)
    }
    this.typingRunId += 1
    this._isUnmounted = true
  },
  methods: {
    prefersReducedMotion() {
      return window.matchMedia('(prefers-reduced-motion: reduce)').matches
    },

    queueTimer(callback, delay) {
      const timer = window.setTimeout(() => {
        this.pendingTimers = this.pendingTimers.filter((id) => id !== timer)
        if (!this._isUnmounted) {
          callback()
        }
      }, delay)

      this.pendingTimers.push(timer)
      return timer
    },

    clearPendingTimers() {
      this.pendingTimers.forEach((timer) => clearTimeout(timer))
      this.pendingTimers = []
    },

    animateIn(el, delay) {
      if (!el) return
      this.queueTimer(() => {
        el.style.transition = `opacity ${REVEAL_DURATION}ms ${REVEAL_EASE}, transform ${REVEAL_DURATION}ms ${REVEAL_EASE}`
        el.style.opacity = '1'
        el.style.transform = 'translate3d(0, 0, 0) scale(1)'
        el.classList.add('visible')
      }, delay)
    },

    revealImmediately() {
      const sections = [
        this.$refs.logoSection,
        this.$refs.avatarSection,
        this.$refs.statusSection,
        this.$refs.quoteSection,
        this.$refs.socialSection
      ]

      sections.forEach((el) => {
        if (!el) return
        el.style.opacity = '1'
        el.style.transform = 'translate3d(0, 0, 0) scale(1)'
        el.classList.add('visible')
      })

      if (this.$refs.separator) {
        this.$refs.separator.classList.add('visible')
      }

      if (this.$refs.quoteText) {
        this.$refs.quoteText.textContent = this.quoteTargetText
        this.$refs.quoteText.classList.add('done')
      }

      this.hasStartedTyping = true
    },

    startAnimations() {
      if (this.prefersReducedMotion()) {
        this.revealImmediately()
        return
      }

      const introStart = 100

      this.animateIn(this.$refs.logoSection, introStart)

      this.queueTimer(() => {
        const avatar = this.$refs.avatarSection
        if (!avatar) return

        avatar.style.transition = `opacity 620ms ${REVEAL_EASE}, transform 700ms cubic-bezier(0.34, 1.56, 0.64, 1)`
        avatar.style.opacity = '1'
        avatar.style.transform = 'translate3d(0, 0, 0) scale(1)'
      }, introStart + 150)

      this.animateIn(this.$refs.statusSection, introStart + 340)

      this.queueTimer(() => {
        if (this.$refs.separator) this.$refs.separator.classList.add('visible')
      }, introStart + 500)

      this.animateIn(this.$refs.quoteSection, introStart + 580)

      this.animateIn(this.$refs.socialSection, introStart + 860)

      this.queueTimer(() => {
        this.hasStartedTyping = true
        this.startTypewriter()
      }, introStart + 690)
    },

    startTypewriter() {
      const el = this.$refs.quoteText
      if (!el) return

      if (this.typingTimer) {
        clearTimeout(this.typingTimer)
      }

      this.typingRunId += 1
      const currentRunId = this.typingRunId
      const text = this.quoteTargetText

      el.textContent = ''
      el.classList.remove('done')
      let i = 0

      const type = () => {
        if (this._isUnmounted || currentRunId !== this.typingRunId) return

        if (i < text.length) {
          el.textContent += text.charAt(i)
          i++
          this.typingTimer = window.setTimeout(type, TYPE_INTERVAL)
        } else {
          el.classList.add('done')
          if (this.$refs.quoteSection) this.$refs.quoteSection.classList.add('visible')
        }
      }
      type()
    },

    normalizeQuoteText(text) {
      if (!text) return FALLBACK_POEM.content

      const trimmed = text.trim()
      if (!trimmed) return FALLBACK_POEM.content
      if (/^[「『“"].*[」』”"]$/.test(trimmed)) return trimmed

      return `「${trimmed}」`
    },

    formatPoemSource(origin = {}) {
      const meta = [origin.dynasty, origin.author].filter(Boolean).join('·')
      const title = origin.title ? `《${origin.title}》` : ''

      if (meta && title) return `—— ${meta}${title}`
      if (title) return `—— ${title}`
      if (meta) return `—— ${meta}`

      return FALLBACK_POEM.source
    },

    applyPoemResult(result) {
      const content = result?.data?.content
      if (!content) return

      this.quoteTargetText = this.normalizeQuoteText(content)
      this.quoteSourceText = this.formatPoemSource(result?.data?.origin)

      if (this.hasStartedTyping) {
        this.startTypewriter()
      }
    },

    loadJinrishiciSdk() {
      if (window.jinrishici?.load) {
        return Promise.resolve(window.jinrishici)
      }

      if (window.__jinrishiciSdkPromise) {
        return window.__jinrishiciSdkPromise
      }

      window.__jinrishiciSdkPromise = new Promise((resolve, reject) => {
        const existingScript = document.querySelector('script[data-jinrishici-sdk="true"]')
        if (existingScript) {
          existingScript.addEventListener('load', () => resolve(window.jinrishici), { once: true })
          existingScript.addEventListener('error', () => reject(new Error('Failed to load Jinrishici SDK.')), { once: true })
          return
        }

        const script = document.createElement('script')
        script.src = JINRISHICI_SDK_SRC
        script.charset = 'utf-8'
        script.async = true
        script.dataset.jinrishiciSdk = 'true'
        script.onload = () => resolve(window.jinrishici)
        script.onerror = () => reject(new Error('Failed to load Jinrishici SDK.'))
        document.head.appendChild(script)
      })

      return window.__jinrishiciSdkPromise
    },

    loadDailyPoem() {
      this.loadJinrishiciSdk()
        .then((sdk) => {
          if (this._isUnmounted || !sdk?.load) return

          sdk.load((result) => {
            this.applyPoemResult(result)
          })
        })
        .catch((error) => {
          console.warn('Jinrishici SDK load failed:', error)
        })
    },

    initTiltEffect() {
      if (this.prefersReducedMotion()) return
      const container = this.$refs.avatarContainer
      if (!container) return

      this._avatarPointerEnterHandler = () => {
        container.style.transition = 'transform 0.18s ease-out, box-shadow 0.28s ease, border-color 0.28s ease'
      }

      this._avatarPointerMoveHandler = (event) => {
        const rect = container.getBoundingClientRect()
        const offsetX = event.clientX - rect.left
        const offsetY = event.clientY - rect.top
        const rotateY = ((offsetX / rect.width) - 0.5) * 12
        const rotateX = (0.5 - (offsetY / rect.height)) * 12
        container.style.transform = `perspective(960px) rotateX(${rotateX.toFixed(2)}deg) rotateY(${rotateY.toFixed(2)}deg) scale(1.03)`
      }

      this._avatarPointerLeaveHandler = () => {
        container.style.transition = 'transform 0.45s cubic-bezier(0.22, 1, 0.36, 1), box-shadow 0.28s ease, border-color 0.28s ease'
        container.style.transform = 'perspective(1000px) rotateX(0) rotateY(0) scale(1)'
      }

      container.addEventListener('pointerenter', this._avatarPointerEnterHandler)
      container.addEventListener('pointermove', this._avatarPointerMoveHandler)
      container.addEventListener('pointerleave', this._avatarPointerLeaveHandler)
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
  width: 100%;
  min-height: calc(100vh - var(--nav-offset));
  min-height: calc(100svh - var(--nav-offset));
  padding: clamp(1.25rem, 3vw, 2rem) 1.5rem clamp(1.75rem, 4vw, 3rem);
  z-index: 10;
  overflow: hidden;
}

.content-wrapper {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  text-align: center;
  gap: clamp(1rem, 2.5vh, 1.75rem);
  max-width: 800px;
  min-height: 100%;
  padding: 0 2rem;
  width: 100%;
}

/* Logo Section */
.logo-section {
  opacity: 0;
  transform: translate3d(0, -28px, 0);
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
  text-shadow: 6px 6px 0px var(--accent-cyan);
  transform: translateY(-4px) rotate(-1deg);
}

.brand-subtitle {
  font-family: var(--font-mono);
  font-size: 0.8rem;
  text-transform: uppercase;
  letter-spacing: 0.3em;
  color: var(--text-primary);
  margin-bottom: 0;
  opacity: 0.9;
}

/* Avatar Section */
.avatar-section {
  opacity: 0;
  transform: translate3d(0, 18px, 0) scale(0.9);
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
  will-change: transform;
}

.avatar-container:hover {
  border-color: var(--accent-cyan);
  box-shadow: 0 16px 36px rgba(184, 212, 227, 0.35);
}

.avatar-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.5s ease;
}

.avatar-container:hover .avatar-img {
  transform: scale(1.05);
}

/* Status Section */
.status-section {
  opacity: 0;
  transform: translate3d(0, 24px, 0);
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
  margin: 0 auto;
  width: 60px;
  height: 2px;
  position: relative;
  display: flex;
  justify-content: center;
  opacity: 0.4;
  transition: opacity 0.5s ease;
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

.separator.visible {
  opacity: 1;
}

/* Quote Section */
.quote-section {
  position: relative;
  opacity: 0;
  width: min(100%, 44rem);
  transform: translate3d(0, 20px, 0);
}

.quote-text {
  font-family: var(--font-serif);
  font-style: italic;
  font-size: 1.2rem;
  color: var(--text-secondary);
  margin-bottom: 0.75rem;
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
  opacity: 0;
  transform: translateY(10px);
  transition: opacity 0.5s ease, transform 0.5s ease;
}

.quote-section.visible .quote-source {
  opacity: 0.7;
  transform: translateY(0);
}

.social-section {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.9rem;
  margin-top: clamp(1.2rem, 4vh, 2.2rem);
  opacity: 0;
  transform: translate3d(0, 18px, 0);
}

.social-link {
  width: 2.8rem;
  height: 2.8rem;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  border-radius: 999px;
  border: 1px solid rgba(102, 102, 102, 0.15);
  background: rgba(255, 255, 255, 0.55);
  box-shadow: 0 10px 24px rgba(0, 0, 0, 0.06);
  transition: transform 0.28s ease, box-shadow 0.28s ease, border-color 0.28s ease, background-color 0.28s ease;
}

.social-link:hover {
  transform: translateY(-4px);
  border-color: rgba(184, 212, 227, 0.8);
  background: rgba(255, 255, 255, 0.82);
  box-shadow: 0 14px 28px rgba(0, 0, 0, 0.1);
}

.social-icon {
  width: 1.3rem;
  height: 1.3rem;
  display: block;
  object-fit: contain;
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
    padding: 1rem 1rem 1.5rem;
  }

  .content-wrapper {
    gap: 0.85rem;
    padding: 0 0.5rem;
  }

  .status-section {
    margin-top: 0.25rem;
  }

  .quote-text {
    font-size: 1rem;
    line-height: 1.8;
  }

  .social-section {
    margin-top: 1.1rem;
  }

  .social-link {
    width: 2.5rem;
    height: 2.5rem;
  }

  .social-icon {
    width: 1.15rem;
    height: 1.15rem;
  }
}
</style>
