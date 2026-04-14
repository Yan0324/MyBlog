<template>
  <section class="subpage contact-view">
    <div class="subpage-shell">
      <header class="subpage-header">
        <span class="subpage-eyebrow">Keep In Touch</span>
        <h1 class="subpage-title">Contact</h1>
        <p class="subpage-lead">
          这个页面现在已经是独立路由。后面你只需要把真实联系方式替换进去，就能形成完整的站点结构。
        </p>
      </header>

      <div class="contact-layout">
        <article class="contact-card intro-card">
          <span class="card-label">Message</span>
          <h2>欢迎把合作、建议或想交流的话题放到这里。</h2>
          <p>
            现在先把联系页拆出来，之后你可以继续补邮箱、社交链接或者表单提交功能。
          </p>
        </article>

        <div class="contact-list">
          <article v-for="item in contactItems" :key="item.title" class="contact-card">
            <span class="card-label">{{ item.title }}</span>
            <strong>{{ item.value }}</strong>
            <p>{{ item.copy }}</p>
          </article>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
export default {
  name: 'ContactView',
  data() {
    return {
      contactItems: [
        {
          title: 'Email',
          value: '待补充邮箱地址',
          copy: '适合正式沟通、合作邀约，后面替换成你的真实邮箱即可。'
        },
        {
          title: 'GitHub',
          value: '待补充仓库链接',
          copy: '可以放你的项目仓库或个人主页，让联系页更完整。'
        },
        {
          title: 'Notes',
          value: '持续更新中',
          copy: '也可以把这个位置改成表单、公众号或其他你常用的联系方式。'
        }
      ]
    }
  }
}
</script>

<style scoped>
.subpage {
  position: relative;
  z-index: 10;
  width: min(1160px, calc(100% - 3rem));
  margin: 0 auto 4rem;
  padding-top: clamp(1rem, 2vw, 1.5rem);
}

.subpage-shell {
  overflow: hidden;
  padding: clamp(2rem, 4vw, 3.2rem);
  border: 1px solid var(--border-color);
  border-radius: 34px;
  background:
    radial-gradient(circle at top right, rgba(184, 212, 227, 0.22), transparent 32%),
    radial-gradient(circle at bottom left, rgba(184, 212, 227, 0.14), transparent 26%),
    linear-gradient(180deg, var(--surface-card) 0%, rgba(255, 255, 255, 0.4) 100%);
  box-shadow:
    0 24px 60px rgba(0, 0, 0, 0.06),
    inset 0 1px 0 rgba(255, 255, 255, 0.6);
  backdrop-filter: blur(12px);
}

.subpage-header {
  max-width: 42rem;
}

.subpage-eyebrow,
.card-label {
  display: inline-block;
  font-family: var(--font-mono);
  font-size: 0.78rem;
  letter-spacing: 0.16em;
  text-transform: uppercase;
  color: var(--text-secondary);
}

.subpage-title {
  margin-top: 0.8rem;
  font-size: clamp(2.4rem, 5vw, 4rem);
  line-height: 1.05;
}

.subpage-lead {
  margin-top: 1rem;
  max-width: 40rem;
  color: var(--text-secondary);
}

.contact-layout {
  display: grid;
  grid-template-columns: minmax(0, 1.1fr) minmax(0, 1fr);
  gap: 1rem;
  margin-top: 2rem;
}

.contact-list {
  display: grid;
  gap: 1rem;
}

.contact-card {
  padding: 1.5rem;
  border-radius: 24px;
  border: 1px solid rgba(26, 26, 26, 0.08);
  background: rgba(255, 255, 255, 0.72);
  box-shadow: 0 18px 45px rgba(0, 0, 0, 0.05);
}

.intro-card {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  min-height: 100%;
}

.contact-card h2,
.contact-card strong {
  display: block;
  margin-top: 0.75rem;
  color: var(--text-primary);
}

.contact-card h2 {
  font-size: 1.5rem;
  line-height: 1.3;
}

.contact-card strong {
  font-size: 1.2rem;
}

.contact-card p {
  margin-top: 0.75rem;
  color: var(--text-secondary);
}

@media (max-width: 980px) {
  .contact-layout {
    grid-template-columns: 1fr;
  }
}

@media (max-width: 768px) {
  .subpage {
    width: min(100%, calc(100% - 1.5rem));
  }

  .subpage-shell {
    padding: 1.25rem;
    border-radius: 24px;
  }
}
</style>
