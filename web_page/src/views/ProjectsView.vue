<template>
  <section class="subpage projects-view">
    <div class="subpage-shell">
      <header class="subpage-header">
        <span class="subpage-eyebrow">Selected Work</span>
        <h1 class="subpage-title">Projects</h1>
        <p class="subpage-lead">
          这里不再和首页、关于页连成一条长滚动页面，而是作为独立页面展示你的项目内容。
        </p>
      </header>

      <div class="project-grid">
        <article v-for="project in projects" :key="project.title" class="project-card">
          <span class="project-kicker">{{ project.kicker }}</span>
          <h2>{{ project.title }}</h2>
          <p>{{ project.copy }}</p>
          <div class="project-tags">
            <span v-for="tag in project.tags" :key="tag" class="project-tag">{{ tag }}</span>
          </div>
        </article>
      </div>
    </div>
  </section>
</template>

<script>
export default {
  name: 'ProjectsView',
  data() {
    return {
      projects: [
        {
          kicker: 'Now Building',
          title: 'MyBlog',
          copy: '把个人主页从单页滚动改成独立路由页面，让结构更清晰，也更接近真正的网站导航体验。',
          tags: ['Vue 3', 'Routing', 'UI']
        },
        {
          kicker: 'Next Slot',
          title: 'Project Archive',
          copy: '这个卡片先作为项目归档位，后面你可以放自己的作品、练习或实验页面。',
          tags: ['Portfolio', 'Case Study', 'Notes']
        },
        {
          kicker: 'In Progress',
          title: 'Motion Lab',
          copy: '这里适合放动效尝试、交互实验和视觉练习，让页面慢慢积累自己的风格。',
          tags: ['Animation', 'Interaction', 'Visual']
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
  padding-top: clamp(6rem, 10vw, 7.5rem);
}

.subpage-shell {
  position: relative;
  overflow: hidden;
  padding: clamp(2rem, 4vw, 3.2rem);
  border: 1px solid var(--border-color);
  border-radius: 34px;
  background:
    radial-gradient(circle at top left, rgba(184, 212, 227, 0.18), transparent 30%),
    radial-gradient(circle at bottom right, rgba(184, 212, 227, 0.12), transparent 28%),
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
.project-kicker,
.project-tag {
  font-family: var(--font-mono);
}

.subpage-eyebrow,
.project-kicker {
  display: inline-block;
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
  color: var(--text-secondary);
  max-width: 40rem;
}

.project-grid {
  display: grid;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: 1rem;
  margin-top: 2rem;
}

.project-card {
  min-height: 260px;
  padding: 1.5rem;
  border-radius: 24px;
  border: 1px solid rgba(26, 26, 26, 0.08);
  background: rgba(255, 255, 255, 0.72);
  box-shadow: 0 18px 45px rgba(0, 0, 0, 0.05);
  transition: transform 0.35s ease, box-shadow 0.35s ease, border-color 0.35s ease;
}

.project-card:hover {
  transform: translateY(-6px);
  border-color: rgba(184, 212, 227, 0.9);
  box-shadow: 0 24px 50px rgba(0, 0, 0, 0.08);
}

.project-card h2 {
  margin-top: 0.7rem;
  font-size: 1.35rem;
  color: var(--text-primary);
}

.project-card p {
  margin-top: 0.75rem;
  color: var(--text-secondary);
}

.project-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 0.6rem;
  margin-top: 1.2rem;
}

.project-tag {
  padding: 0.32rem 0.7rem;
  border-radius: 999px;
  background: var(--surface-soft);
  color: var(--text-primary);
  font-size: 0.74rem;
  letter-spacing: 0.04em;
}

@media (max-width: 980px) {
  .project-grid {
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
