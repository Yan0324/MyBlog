<template>
  <section id="about" class="about-section">
    <div class="about-shell">
      <span class="about-orb about-orb-a" aria-hidden="true"></span>
      <span class="about-orb about-orb-b" aria-hidden="true"></span>

      <header class="about-header reveal" ref="headerRef">
        <span class="about-eyebrow">Industrial PC developmenter</span>
        <h2 class="about-title"></h2>
        <p class="about-lead">
          Caoyan
        </p>
        <div class="about-tags">
          <span v-for="tag in tags" :key="tag" class="about-tag">{{ tag }}</span>
        </div>
      </header>

      <div class="about-top">
        <article class="about-card intro-card reveal" ref="introRef">
          <p class="card-label">Quick Intro</p>
          <h3>不是展示“会什么”，而是记录“正在变成什么样的人”。</h3>
          <ul class="intro-list">
            <li v-for="item in summaryPoints" :key="item">{{ item }}</li>
          </ul>
        </article>

        <aside class="about-note reveal" ref="statusRef">
          <p class="card-label">Now</p>
          <ul class="note-list">
            <li v-for="item in nowItems" :key="item.title">
              <span class="note-title">{{ item.title }}</span>
              <span class="note-copy">{{ item.copy }}</span>
            </li>
          </ul>
          <div class="note-signature">2026 · still building</div>
        </aside>
      </div>

      <div class="about-highlights reveal" ref="highlightsRef">
        <article
          v-for="(item, index) in highlights"
          :key="item.title"
          class="highlight-card"
          :style="{ '--delay': `${index * 0.12}s` }"
        >
          <span class="highlight-kicker">{{ item.kicker }}</span>
          <strong>{{ item.title }}</strong>
          <p>{{ item.copy }}</p>
        </article>
      </div>

      <article class="timeline-panel reveal" ref="timelineRef">
        <div class="timeline-header">
          <div>
            <p class="card-label">Journey</p>
            <h3>一条横向时间线，从出生走到工作。</h3>
          </div>
          <p class="timeline-hint">学校和公司图标的位置已经留好，后面补图片就能直接替换。</p>
        </div>

        <div class="timeline-scroll">
          <ol class="timeline-track">
            <li
              v-for="(entry, index) in timeline"
              :key="entry.key"
              class="timeline-step"
              :style="{ '--delay': `${0.18 + index * 0.12}s` }"
            >
              <span class="timeline-stage">{{ entry.stage }}</span>
              <div class="timeline-node">
                <div class="timeline-icon" :class="`timeline-icon-${entry.iconType}`" :aria-label="`${entry.stage} 图标`">
                  <svg
                    v-if="entry.iconType === 'birth'"
                    viewBox="0 0 64 64"
                    class="timeline-icon-svg"
                    aria-hidden="true"
                  >
                    <path d="M20 37c-4-4-6-9-6-15 0-10 8-18 18-18s18 8 18 18c0 6-2 11-6 15" />
                    <path d="M17 42c2-7 8-13 15-13 7 0 13 6 15 13" />
                    <path d="M24 18l4 6-6 2 6 4" />
                    <path d="M40 16l-3 8 6 1-5 4" />
                    <path d="M26 45l6-6 6 6" />
                    <path d="M14 44c4-3 9-5 18-5 9 0 14 2 18 5" />
                  </svg>
                  <img
                    v-else-if="!missingIcons[entry.key]"
                    :src="entry.iconSrc"
                    :alt="`${entry.stage} 图标`"
                    class="timeline-icon-image"
                    @error="markIconMissing(entry.key)"
                  >
                  <span v-else class="timeline-icon-fallback">{{ entry.fallback }}</span>
                </div>
              </div>
              <div class="timeline-card">
                <strong>{{ entry.title }}</strong>
                <p>{{ entry.copy }}</p>
                <span class="timeline-meta">{{ entry.meta }}</span>
              </div>
            </li>
          </ol>
        </div>
      </article>
    </div>
  </section>
</template>

<script>
export default {
  name: 'AboutSection',
  data() {
    return {
      tags: ['C#', 'Avalonia', 'Python', 'Java','...'],
      summaryPoints: [
        '喜欢把技术学习做得更有审美。',
        '在写博客，也在训练自己的表达节奏。',
        '希望作品不仅能运行，也有一点记忆点。'
      ],
      nowItems: [
        {
          title: 'Building',
          copy: '继续打磨这个博客，让它更像一个完整的个人空间。'
        },
        {
          title: 'Learning',
          copy: '补强 Vue、动画细节和更稳的前端基础。'
        },
        {
          title: 'Keeping',
          copy: '维持记录习惯，把成长过程留在页面里。'
        }
      ],
      highlights: [
        {
          kicker: 'Focus',
          title: '简洁表达',
          copy: '页面少一点解释，多一点直接感受。'
        },
        {
          kicker: 'Style',
          title: '轻一点动画',
          copy: '让内容自己出现，而不是一下子全部堆出来。'
        },
        {
          kicker: 'Goal',
          title: '持续生长',
          copy: '把博客当作长期作品，而不是一次性练习。'
        }
      ],
      timeline: [
        {
          key: 'birth',
          stage: '出生',
          title: '从这里开始',
          copy: '先把起点放在这里，用一个破壳图标表示人生时间线被轻轻点亮的时刻。',
          meta: 'Start',
          iconType: 'birth'
        },
        {
          key: 'high-school',
          stage: '高中',
          title: '第一次认真朝未来看',
          copy: '这里预留高中学校图标。后面补上校徽或校园标识后，这个节点会立刻更像你自己的经历。',
          meta: 'High School',
          iconType: 'school',
          iconSrc: '/timeline/high-school.png',
          fallback: '高中'
        },
        {
          key: 'university',
          stage: '大学',
          title: '方向感逐渐变得清晰',
          copy: '这里预留大学图标。你后续只要补一张学校 logo，这个阶段就会和整体页面更贴合。',
          meta: 'University',
          iconType: 'school',
          iconSrc: '/timeline/university.png',
          fallback: '大学'
        },
        {
          key: 'work',
          stage: '工作',
          title: '把成长带进真实场景',
          copy: '这里预留公司图标。等你补上公司 logo 后，这个节点会成为时间线里最有现实感的一站。',
          meta: 'Work',
          iconType: 'company',
          iconSrc: '/timeline/work.png',
          fallback: '工作'
        }
      ],
      missingIcons: {}
    }
  },
  mounted() {
    if (window.matchMedia('(prefers-reduced-motion: reduce)').matches) {
      this.revealAll()
      return
    }

    this.initRevealObserver()
  },
  beforeUnmount() {
    if (this._observer) {
      this._observer.disconnect()
    }
  },
  methods: {
    getRevealElements() {
      return [
        this.$refs.headerRef,
        this.$refs.introRef,
        this.$refs.statusRef,
        this.$refs.highlightsRef,
        this.$refs.timelineRef
      ].filter(Boolean)
    },
    revealAll() {
      this.getRevealElements().forEach((el) => {
        el.classList.add('is-visible')
      })
    },
    initRevealObserver() {
      const elements = this.getRevealElements()

      this._observer = new IntersectionObserver((entries) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting) {
            entry.target.classList.add('is-visible')
            this._observer.unobserve(entry.target)
          }
        })
      }, {
        threshold: 0.18
      })

      elements.forEach((el) => this._observer.observe(el))
    },
    markIconMissing(key) {
      this.missingIcons = {
        ...this.missingIcons,
        [key]: true
      }
    }
  }
}
</script>

<style scoped>
.about-section {
  position: relative;
  z-index: 10;
  width: min(1160px, calc(100% - 3rem));
  margin: 0 auto 7rem;
  padding-top: 2rem;
}

.about-shell {
  position: relative;
  overflow: hidden;
  padding: 4rem clamp(1.25rem, 3vw, 3rem);
  border: 1px solid var(--border-color);
  border-radius: 34px;
  background:
    radial-gradient(circle at top left, rgba(184, 212, 227, 0.2), transparent 32%),
    radial-gradient(circle at bottom right, rgba(184, 212, 227, 0.12), transparent 28%),
    linear-gradient(180deg, var(--surface-card) 0%, rgba(255, 255, 255, 0.38) 100%);
  box-shadow:
    0 24px 60px rgba(0, 0, 0, 0.06),
    inset 0 1px 0 rgba(255, 255, 255, 0.6);
}

.about-orb {
  position: absolute;
  border-radius: 50%;
  pointer-events: none;
  filter: blur(10px);
  opacity: 0.8;
  animation: orbFloat 8s ease-in-out infinite;
}

.about-orb-a {
  top: -48px;
  right: -36px;
  width: 180px;
  height: 180px;
  background: rgba(184, 212, 227, 0.24);
}

.about-orb-b {
  left: 4%;
  bottom: 8%;
  width: 120px;
  height: 120px;
  background: rgba(184, 212, 227, 0.16);
  animation-delay: -4s;
}

.about-header {
  position: relative;
  z-index: 1;
  max-width: 760px;
}

.about-eyebrow,
.card-label,
.highlight-kicker,
.timeline-stage,
.note-title {
  display: inline-block;
  font-family: var(--font-mono);
  font-size: 0.78rem;
  letter-spacing: 0.16em;
  text-transform: uppercase;
}

.about-eyebrow,
.card-label,
.highlight-kicker {
  color: var(--text-secondary);
}

.about-title {
  margin-top: 1rem;
  max-width: 12em;
  font-size: clamp(2.1rem, 5vw, 3.7rem);
  line-height: 1.16;
  letter-spacing: 0.02em;
  color: var(--text-primary);
}

.about-lead {
  margin-top: 1.2rem;
  max-width: 42rem;
  font-size: 1.04rem;
  color: var(--text-secondary);
}

.about-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 0.75rem;
  margin-top: 1.5rem;
}

.about-tag {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  border-radius: 999px;
  font-family: var(--font-mono);
}

.about-tag {
  padding: 0.46rem 0.85rem;
  border: 1px solid rgba(26, 26, 26, 0.08);
  background: rgba(255, 255, 255, 0.62);
  color: var(--text-secondary);
  font-size: 0.8rem;
}

.about-top {
  display: grid;
  grid-template-columns: minmax(0, 1.4fr) minmax(280px, 0.9fr);
  gap: 1.5rem;
  align-items: stretch;
  margin-top: 2rem;
}

.about-card,
.about-note,
.timeline-panel,
.highlight-card {
  position: relative;
  border: 1px solid rgba(26, 26, 26, 0.08);
  background: rgba(255, 255, 255, 0.72);
  backdrop-filter: blur(12px);
  box-shadow: 0 18px 45px rgba(0, 0, 0, 0.05);
}

.about-card,
.about-note,
.timeline-panel {
  border-radius: 28px;
}

.about-card,
.about-note {
  padding: 1.75rem;
}

.intro-card::after {
  content: '';
  position: absolute;
  top: 1.4rem;
  right: 1.4rem;
  width: 56px;
  height: 56px;
  border-radius: 18px;
  border: 1px solid rgba(184, 212, 227, 0.8);
  background: linear-gradient(135deg, rgba(184, 212, 227, 0.24), rgba(255, 255, 255, 0.12));
  transform: rotate(12deg);
}

.about-card h3,
.timeline-header h3 {
  margin-top: 0.8rem;
  color: var(--text-primary);
  line-height: 1.35;
}

.about-card h3 {
  max-width: 22rem;
  font-size: 1.56rem;
}

.intro-list,
.note-list,
.timeline-track {
  list-style: none;
}

.intro-list {
  display: grid;
  gap: 0.9rem;
  margin-top: 1.2rem;
}

.intro-list li {
  position: relative;
  padding-left: 1.1rem;
  color: var(--text-secondary);
}

.intro-list li::before {
  content: '';
  position: absolute;
  left: 0;
  top: 0.8rem;
  width: 0.42rem;
  height: 0.42rem;
  border-radius: 50%;
  background: var(--accent-cyan);
  box-shadow: 0 0 0 6px rgba(184, 212, 227, 0.12);
}

.about-note {
  background: linear-gradient(180deg, rgba(234, 243, 247, 0.92), rgba(255, 255, 255, 0.84));
  transform: rotate(1.1deg);
}

.note-list {
  display: grid;
  gap: 1rem;
  margin-top: 1rem;
}

.note-list li {
  padding-bottom: 1rem;
  border-bottom: 1px dashed rgba(26, 26, 26, 0.12);
}

.note-list li:last-child {
  padding-bottom: 0;
  border-bottom: none;
}

.note-title {
  margin-bottom: 0.35rem;
  color: var(--text-primary);
}

.note-copy,
.note-signature,
.timeline-hint,
.highlight-card p,
.timeline-card p {
  color: var(--text-secondary);
}

.note-signature {
  margin-top: 1.25rem;
  font-family: var(--font-mono);
  font-size: 0.78rem;
}

.about-highlights {
  display: grid;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: 1rem;
  margin-top: 1.5rem;
}

.highlight-card {
  overflow: hidden;
  padding: 1.3rem 1.25rem;
  border-radius: 22px;
  transform: translateY(16px);
  opacity: 0;
  transition:
    opacity 0.75s ease,
    transform 0.75s cubic-bezier(0.22, 1, 0.36, 1);
  transition-delay: var(--delay);
}

.reveal.is-visible .highlight-card {
  opacity: 1;
  transform: translateY(0);
}

.highlight-card::before {
  content: '';
  position: absolute;
  inset: 0;
  background: linear-gradient(120deg, transparent 20%, rgba(255, 255, 255, 0.5) 50%, transparent 80%);
  transform: translateX(-130%);
  animation: sweep 6s linear infinite;
  animation-delay: calc(var(--delay) + 1.2s);
}

.highlight-card strong {
  display: block;
  margin-top: 0.55rem;
  color: var(--text-primary);
  font-size: 1.1rem;
}

.highlight-card p {
  margin-top: 0.45rem;
}

.timeline-panel {
  margin-top: 1.5rem;
  padding: 1.8rem;
  overflow: hidden;
}

.timeline-header {
  display: flex;
  justify-content: space-between;
  gap: 1rem;
  align-items: end;
}

.timeline-header h3 {
  font-size: 1.48rem;
}

.timeline-hint {
  max-width: 18rem;
  text-align: right;
}

.timeline-scroll {
  position: relative;
  margin-top: 1.6rem;
  padding-bottom: 0.65rem;
  overflow-x: auto;
  overflow-y: hidden;
  scrollbar-width: thin;
  scrollbar-color: rgba(184, 212, 227, 0.8) transparent;
}

.timeline-scroll::before {
  content: none;
}

.timeline-track {
  display: grid;
  grid-auto-flow: column;
  grid-auto-columns: minmax(220px, 1fr);
  gap: 1.25rem;
  min-width: 920px;
  padding: 0.2rem 0 1rem;
}

.timeline-step {
  position: relative;
  opacity: 0;
  transform: translateY(28px);
  transition:
    opacity 0.8s ease,
    transform 0.8s cubic-bezier(0.22, 1, 0.36, 1);
  transition-delay: var(--delay);
}

.reveal.is-visible .timeline-step {
  opacity: 1;
  transform: translateY(0);
}

.timeline-stage {
  color: var(--text-primary);
}

.timeline-node {
  position: relative;
  display: flex;
  align-items: center;
  min-height: 84px;
}

.timeline-node::before,
.timeline-node::after {
  content: '';
  position: absolute;
  top: 50%;
  width: calc(50% - 44px);
  height: 2px;
  transform: translateY(-50%);
  background: linear-gradient(90deg, rgba(184, 212, 227, 0.3), rgba(184, 212, 227, 0.95));
}

.timeline-node::before {
  left: -0.65rem;
}

.timeline-node::after {
  right: -0.65rem;
}

.timeline-step:first-child .timeline-node::before,
.timeline-step:last-child .timeline-node::after {
  opacity: 0;
}

.timeline-icon {
  position: relative;
  z-index: 1;
  display: grid;
  place-items: center;
  width: 88px;
  height: 88px;
  margin: 0 auto;
  border-radius: 28px;
  border: 1px solid rgba(26, 26, 26, 0.08);
  background:
    linear-gradient(180deg, rgba(255, 255, 255, 0.94), rgba(234, 243, 247, 0.9));
  box-shadow:
    0 0 0 8px rgba(184, 212, 227, 0.12),
    0 12px 30px rgba(0, 0, 0, 0.08);
  transition:
    transform 0.35s ease,
    box-shadow 0.35s ease;
  animation: pulseDot 3s ease-in-out infinite;
}

.timeline-icon-birth {
  background:
    radial-gradient(circle at 30% 30%, rgba(255, 255, 255, 0.95), transparent 45%),
    linear-gradient(180deg, rgba(255, 250, 245, 0.98), rgba(234, 243, 247, 0.88));
}

.timeline-icon-school,
.timeline-icon-company {
  background:
    radial-gradient(circle at top, rgba(184, 212, 227, 0.32), transparent 46%),
    linear-gradient(180deg, rgba(255, 255, 255, 0.95), rgba(234, 243, 247, 0.86));
}

.timeline-icon-svg {
  width: 52px;
  height: 52px;
  fill: none;
  stroke: var(--text-primary);
  stroke-linecap: round;
  stroke-linejoin: round;
  stroke-width: 2.4;
}

.timeline-icon-image {
  width: 52px;
  height: 52px;
  object-fit: contain;
}

.timeline-icon-fallback {
  padding: 0.35rem 0.55rem;
  border-radius: 999px;
  background: rgba(184, 212, 227, 0.18);
  color: var(--text-primary);
  font-family: var(--font-mono);
  font-size: 0.76rem;
  letter-spacing: 0.08em;
}

.timeline-card {
  min-height: 176px;
  padding: 1.15rem;
  border-radius: 22px;
  border: 1px solid rgba(26, 26, 26, 0.08);
  background: rgba(255, 255, 255, 0.66);
  transition:
    transform 0.35s ease,
    border-color 0.35s ease,
    box-shadow 0.35s ease;
}

.timeline-card strong {
  display: block;
  color: var(--text-primary);
  font-size: 1rem;
}

.timeline-card p {
  margin-top: 0.45rem;
}

.timeline-meta {
  display: inline-flex;
  align-items: center;
  margin-top: 1rem;
  padding: 0.28rem 0.62rem;
  border-radius: 999px;
  background: rgba(184, 212, 227, 0.16);
  color: var(--text-secondary);
  font-family: var(--font-mono);
  font-size: 0.72rem;
  letter-spacing: 0.04em;
}

.timeline-step:hover .timeline-card {
  transform: translateY(-6px);
  border-color: rgba(184, 212, 227, 0.92);
  box-shadow: 0 18px 36px rgba(0, 0, 0, 0.06);
}

.timeline-step:hover .timeline-icon {
  transform: translateY(-4px);
  box-shadow:
    0 0 0 10px rgba(184, 212, 227, 0.14),
    0 16px 34px rgba(0, 0, 0, 0.1);
}

.reveal {
  opacity: 0;
  transform: translateY(28px);
  transition:
    opacity 0.8s ease,
    transform 0.8s cubic-bezier(0.22, 1, 0.36, 1);
}

.reveal.is-visible {
  opacity: 1;
  transform: translateY(0);
}

@keyframes orbFloat {
  0%,
  100% {
    transform: translate3d(0, 0, 0) scale(1);
  }

  50% {
    transform: translate3d(0, 14px, 0) scale(1.05);
  }
}

@keyframes pulseDot {
  0%,
  100% {
    box-shadow:
      0 0 0 8px rgba(184, 212, 227, 0.12),
      0 12px 26px rgba(0, 0, 0, 0.08);
  }

  50% {
    box-shadow:
      0 0 0 12px rgba(184, 212, 227, 0.1),
      0 16px 34px rgba(0, 0, 0, 0.1);
  }
}

@keyframes sweep {
  from {
    transform: translateX(-130%);
  }

  to {
    transform: translateX(130%);
  }
}

@media (max-width: 980px) {
  .about-top,
  .about-highlights {
    grid-template-columns: 1fr;
  }

  .about-note {
    transform: none;
  }

  .timeline-header {
    flex-direction: column;
    align-items: start;
  }

  .timeline-hint {
    max-width: none;
    text-align: left;
  }
}

@media (max-width: 768px) {
  .about-section {
    width: min(100%, calc(100% - 1.5rem));
    margin-bottom: 5rem;
  }

  .about-shell {
    padding: 2.15rem 1.1rem;
    border-radius: 24px;
  }

  .about-card,
  .about-note,
  .timeline-panel {
    padding: 1.25rem;
    border-radius: 22px;
  }

  .about-title {
    font-size: 2rem;
  }

  .about-lead {
    font-size: 0.98rem;
  }

  .timeline-track {
    grid-auto-columns: minmax(230px, 78vw);
    min-width: max-content;
  }
}

@media (max-width: 560px) {
  .about-tags {
    gap: 0.55rem;
  }

  .about-tag {
    font-size: 0.74rem;
  }

  .about-card h3,
  .timeline-header h3 {
    font-size: 1.28rem;
  }

  .timeline-card {
    min-height: 176px;
  }

  .timeline-icon {
    width: 76px;
    height: 76px;
    border-radius: 24px;
  }

  .timeline-icon-svg,
  .timeline-icon-image {
    width: 46px;
    height: 46px;
  }
}
</style>
