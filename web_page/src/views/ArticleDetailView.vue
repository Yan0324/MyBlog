<template>
  <section class="subpage article-detail">
    <div class="subpage-shell">
      <router-link to="/essay" class="back-link">← 返回 Essay</router-link>

      <p v-if="loading" class="detail-status">正在加载文章…</p>
      <p v-else-if="loadError" class="detail-status detail-status--error">
        {{ loadError }}
        <button type="button" class="retry-btn" @click="loadArticle">重试</button>
      </p>

      <article v-else-if="article" class="detail-article">
        <header class="detail-header">
          <div class="detail-meta">
            <span class="detail-kicker">{{ article.kicker }}</span>
            <span class="detail-category">{{ categoryLabel(article.category) }}</span>
          </div>
          <h1 class="detail-title">{{ article.title }}</h1>
          <p v-if="article.copy" class="detail-lead">{{ article.copy }}</p>
          <div v-if="article.tags && article.tags.length" class="detail-tags">
            <span v-for="tag in article.tags" :key="tag" class="detail-tag">{{ tag }}</span>
          </div>
        </header>

        <div class="detail-body">
          <MarkdownContent
            v-if="article.content"
            :source="article.content"
            empty-text="正文为空"
          />
          <p v-else class="detail-fallback">{{ article.copy || '暂无正文' }}</p>
        </div>
      </article>
    </div>
  </section>
</template>

<script>
import { fetchArticleById } from '../api/client'
import MarkdownContent from '../components/MarkdownContent.vue'

export default {
  name: 'ArticleDetailView',
  components: { MarkdownContent },
  data() {
    return {
      loading: true,
      loadError: '',
      article: null,
      categories: [
        { id: 'tech', label: '技术' },
        { id: 'life', label: '生活' },
        { id: 'notes', label: '随笔' }
      ]
    }
  },
  watch: {
    '$route.params.id'() {
      this.loadArticle()
    }
  },
  mounted() {
    this.loadArticle()
  },
  methods: {
    async loadArticle() {
      const id = this.$route.params.id
      if (!id) {
        this.loadError = '文章不存在'
        this.loading = false
        return
      }

      this.loading = true
      this.loadError = ''
      this.article = null

      try {
        const data = await fetchArticleById(id)
        this.article = data.article
      } catch (err) {
        this.loadError = err.message || '加载失败'
      } finally {
        this.loading = false
      }
    },
    categoryLabel(categoryId) {
      const found = this.categories.find((cat) => cat.id === categoryId)
      return found ? found.label : categoryId
    }
  }
}
</script>

<style scoped>
.subpage {
  position: relative;
  z-index: 10;
  width: min(860px, calc(100% - 3rem));
  margin: 0 auto 4rem;
  padding-top: clamp(1rem, 2vw, 1.5rem);
}

.subpage-shell {
  padding: clamp(2rem, 4vw, 3.2rem);
  border: 1px solid var(--border-color);
  border-radius: 34px;
  background:
    radial-gradient(circle at top left, rgba(184, 212, 227, 0.18), transparent 30%),
    linear-gradient(180deg, var(--surface-card) 0%, rgba(255, 255, 255, 0.4) 100%);
  box-shadow: 0 24px 60px rgba(0, 0, 0, 0.06);
  backdrop-filter: blur(12px);
}

.back-link {
  display: inline-block;
  margin-bottom: 1.25rem;
  font-family: var(--font-mono);
  font-size: 0.82rem;
  letter-spacing: 0.04em;
  color: var(--text-secondary);
  text-decoration: none;
  transition: color 0.25s ease;
}

.back-link:hover {
  color: var(--text-primary);
}

.detail-status {
  color: var(--text-secondary);
  font-size: 0.92rem;
}

.detail-status--error {
  color: #8b2e2e;
}

.retry-btn {
  margin-left: 0.5rem;
  padding: 0.25rem 0.65rem;
  border: 1px solid rgba(26, 26, 26, 0.14);
  border-radius: 999px;
  background: rgba(255, 255, 255, 0.7);
  cursor: pointer;
  font-family: var(--font-mono);
  font-size: 0.78rem;
}

.detail-meta {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  flex-wrap: wrap;
}

.detail-kicker {
  font-family: var(--font-mono);
  font-size: 0.78rem;
  letter-spacing: 0.16em;
  text-transform: uppercase;
  color: var(--text-secondary);
}

.detail-category {
  font-family: var(--font-mono);
  font-size: 0.72rem;
  letter-spacing: 0.08em;
  padding: 0.28rem 0.65rem;
  border-radius: 999px;
  background: var(--surface-soft);
  color: var(--text-primary);
}

.detail-title {
  margin-top: 0.85rem;
  font-size: clamp(1.8rem, 4vw, 2.4rem);
  line-height: 1.25;
  color: var(--text-primary);
}

.detail-lead {
  margin-top: 0.85rem;
  color: var(--text-secondary);
  font-size: 1.05rem;
}

.detail-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  margin-top: 1rem;
}

.detail-tag {
  padding: 0.32rem 0.7rem;
  border-radius: 999px;
  background: var(--surface-soft);
  font-family: var(--font-mono);
  font-size: 0.74rem;
  color: var(--text-primary);
}

.detail-body {
  margin-top: 2rem;
  padding-top: 1.5rem;
  border-top: 1px solid rgba(26, 26, 26, 0.08);
}

.detail-fallback {
  color: var(--text-secondary);
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
