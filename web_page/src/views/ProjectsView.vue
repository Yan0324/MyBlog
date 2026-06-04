<template>
  <section class="subpage essay-view">
    <div class="subpage-shell">
      <!-- 文章分类筛选 -->
      <p v-if="loading" class="essay-status">正在加载文章…</p>
      <p v-else-if="loadError" class="essay-status essay-status--error">
        {{ loadError }}
        <button type="button" class="retry-btn" @click="loadArticles">重试</button>
      </p>

      <nav v-else class="category-nav" aria-label="文章分类">
        <button
          v-for="cat in categories"
          :key="cat.id"
          type="button"
          class="category-btn"
          :class="{ 'is-active': activeCategory === cat.id }"
          @click="selectCategory(cat.id)"
        >
          <span class="category-label">{{ cat.label }}</span>
          <span class="category-count">{{ countByCategory(cat.id) }}</span>
        </button>
      </nav>

      <p v-if="filteredArticles.length === 0" class="essay-empty">
        该分类下暂无文章，换个分类看看，或稍后再来。
      </p>

      <template v-else>
        <p class="essay-page-hint">
          共 {{ filteredArticles.length }} 篇
          <template v-if="totalPages > 1"> · 第 {{ currentPage }} / {{ totalPages }} 页</template>
        </p>

        <div class="article-grid">
        <article
          v-for="article in paginatedArticles"
          :key="article.id"
          class="article-card"
        >
          <div class="article-meta">
            <span class="article-kicker">{{ article.kicker }}</span>
            <span class="article-category">{{ categoryLabel(article.category) }}</span>
          </div>
          <h2>{{ article.title }}</h2>
          <p>{{ article.copy }}</p>
          <div class="article-tags">
            <span v-for="tag in article.tags" :key="tag" class="article-tag">{{ tag }}</span>
          </div>
        </article>
        </div>

        <!-- 当前分类独立分页 -->
        <nav
          v-if="totalPages > 1"
          class="pagination"
          aria-label="文章分页"
        >
          <button
            type="button"
            class="page-btn page-btn--ghost"
            :disabled="currentPage <= 1"
            @click="goToPage(currentPage - 1)"
          >
            上一页
          </button>

          <div class="page-list">
            <button
              v-for="page in totalPages"
              :key="page"
              type="button"
              class="page-btn"
              :class="{ 'is-active': currentPage === page }"
              :aria-current="currentPage === page ? 'page' : undefined"
              @click="goToPage(page)"
            >
              {{ page }}
            </button>
          </div>

          <button
            type="button"
            class="page-btn page-btn--ghost"
            :disabled="currentPage >= totalPages"
            @click="goToPage(currentPage + 1)"
          >
            下一页
          </button>
        </nav>
      </template>
    </div>
  </section>
</template>

<script>
import { fetchPublishedArticles } from '../api/client'

export default {
  name: 'ProjectsView',
  data() {
    return {
      loading: true,
      loadError: '',
      // 当前选中的分类 id，all 表示全部
      activeCategory: 'all',
      // 每页展示文章数
      pageSize: 3,
      // 各分类记住各自页码，切换分类后仍保留
      categoryPages: {},
      // 分类配置：id 与文章数据的 category 字段对应
      categories: [
        { id: 'all', label: '全部' },
        { id: 'tech', label: '技术' },
        { id: 'life', label: '生活' },
        { id: 'notes', label: '随笔' }
      ],
      // 从后台 API 拉取的文章列表
      articles: []
    }
  },
  mounted() {
    this.loadArticles()
  },
  computed: {
    // 按当前分类筛选文章列表
    filteredArticles() {
      if (this.activeCategory === 'all') {
        return this.articles
      }
      return this.articles.filter((item) => item.category === this.activeCategory)
    },
    // 当前分类总页数
    totalPages() {
      return Math.max(1, Math.ceil(this.filteredArticles.length / this.pageSize))
    },
    // 当前分类页码（越界时自动收束到有效范围）
    currentPage() {
      const saved = this.categoryPages[this.activeCategory] || 1
      return Math.min(Math.max(1, saved), this.totalPages)
    },
    // 当前页要展示的文章
    paginatedArticles() {
      const start = (this.currentPage - 1) * this.pageSize
      return this.filteredArticles.slice(start, start + this.pageSize)
    }
  },
  methods: {
    // 从后台加载已发布文章
    async loadArticles() {
      this.loading = true
      this.loadError = ''
      try {
        const data = await fetchPublishedArticles()
        this.articles = data.articles || []
      } catch (err) {
        this.loadError = err.message || '加载失败，请确认 blog_api 已启动'
        this.articles = []
      } finally {
        this.loading = false
      }
    },
    // 切换分类，保留该分类上次浏览的页码
    selectCategory(categoryId) {
      this.activeCategory = categoryId
    },
    // 跳转到指定页
    goToPage(page) {
      const next = Math.min(Math.max(1, page), this.totalPages)
      this.categoryPages = {
        ...this.categoryPages,
        [this.activeCategory]: next
      }
    },
    // 根据分类 id 取展示名称
    categoryLabel(categoryId) {
      const found = this.categories.find((cat) => cat.id === categoryId)
      return found ? found.label : categoryId
    },
    // 统计某分类下的文章数量（全部则返回总数）
    countByCategory(categoryId) {
      if (categoryId === 'all') {
        return this.articles.length
      }
      return this.articles.filter((item) => item.category === categoryId).length
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

.essay-status {
  margin-top: 0.5rem;
  color: var(--text-secondary);
  font-size: 0.92rem;
}

.essay-status--error {
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

.article-kicker,
.article-tag,
.category-btn {
  font-family: var(--font-mono);
}

.article-kicker {
  display: inline-block;
  font-size: 0.78rem;
  letter-spacing: 0.16em;
  text-transform: uppercase;
  color: var(--text-secondary);
}

/* 分类导航 */
.category-nav {
  display: flex;
  flex-wrap: wrap;
  gap: 0.65rem;
  margin-top: 0;
  padding-bottom: 0.25rem;
}

.category-btn {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.55rem 1rem;
  border: 1px solid rgba(26, 26, 26, 0.1);
  border-radius: 999px;
  background: rgba(255, 255, 255, 0.55);
  color: var(--text-secondary);
  font-size: 0.8rem;
  letter-spacing: 0.06em;
  cursor: pointer;
  transition:
    color 0.3s ease,
    border-color 0.3s ease,
    background-color 0.3s ease,
    transform 0.3s ease,
    box-shadow 0.3s ease;
}

.category-btn:hover {
  color: var(--text-primary);
  border-color: rgba(184, 212, 227, 0.85);
  transform: translateY(-2px);
}

.category-btn.is-active {
  color: var(--text-primary);
  border-color: rgba(184, 212, 227, 0.95);
  background: rgba(255, 255, 255, 0.92);
  box-shadow: 0 10px 24px rgba(0, 0, 0, 0.06);
}

.category-count {
  min-width: 1.25rem;
  padding: 0.1rem 0.45rem;
  border-radius: 999px;
  background: var(--surface-soft);
  font-size: 0.72rem;
  text-align: center;
  color: var(--text-primary);
}

.category-btn.is-active .category-count {
  background: color-mix(in srgb, var(--text-primary) 12%, var(--surface-soft));
}

.essay-page-hint {
  margin-top: 1.25rem;
  font-family: var(--font-mono);
  font-size: 0.78rem;
  letter-spacing: 0.06em;
  color: var(--text-secondary);
}

.essay-empty {
  margin-top: 2rem;
  padding: 2rem 1.25rem;
  text-align: center;
  color: var(--text-secondary);
  border: 1px dashed rgba(26, 26, 26, 0.12);
  border-radius: 20px;
  background: rgba(255, 255, 255, 0.4);
}

.article-grid {
  display: grid;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: 1rem;
  margin-top: 1.5rem;
}

.article-card {
  min-height: 260px;
  padding: 1.5rem;
  border-radius: 24px;
  border: 1px solid rgba(26, 26, 26, 0.08);
  background: rgba(255, 255, 255, 0.72);
  box-shadow: 0 18px 45px rgba(0, 0, 0, 0.05);
  transition: transform 0.35s ease, box-shadow 0.35s ease, border-color 0.35s ease;
}

.article-card:hover {
  transform: translateY(-6px);
  border-color: rgba(184, 212, 227, 0.9);
  box-shadow: 0 24px 50px rgba(0, 0, 0, 0.08);
}

.article-meta {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 0.75rem;
  flex-wrap: wrap;
}

.article-category {
  font-family: var(--font-mono);
  font-size: 0.72rem;
  letter-spacing: 0.08em;
  padding: 0.28rem 0.65rem;
  border-radius: 999px;
  background: var(--surface-soft);
  color: var(--text-primary);
}

.article-card h2 {
  margin-top: 0.7rem;
  font-size: 1.35rem;
  color: var(--text-primary);
}

.article-card p {
  margin-top: 0.75rem;
  color: var(--text-secondary);
}

.article-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 0.6rem;
  margin-top: 1.2rem;
}

.article-tag {
  padding: 0.32rem 0.7rem;
  border-radius: 999px;
  background: var(--surface-soft);
  color: var(--text-primary);
  font-size: 0.74rem;
  letter-spacing: 0.04em;
}

/* 分页 */
.pagination {
  display: flex;
  align-items: center;
  justify-content: center;
  flex-wrap: wrap;
  gap: 0.75rem;
  margin-top: 2rem;
  padding-top: 0.5rem;
}

.page-list {
  display: flex;
  flex-wrap: wrap;
  gap: 0.45rem;
}

.page-btn {
  min-width: 2.4rem;
  padding: 0.5rem 0.85rem;
  border: 1px solid rgba(26, 26, 26, 0.1);
  border-radius: 999px;
  background: rgba(255, 255, 255, 0.55);
  color: var(--text-secondary);
  font-family: var(--font-mono);
  font-size: 0.78rem;
  letter-spacing: 0.04em;
  cursor: pointer;
  transition:
    color 0.3s ease,
    border-color 0.3s ease,
    background-color 0.3s ease,
    transform 0.3s ease,
    opacity 0.3s ease;
}

.page-btn:hover:not(:disabled) {
  color: var(--text-primary);
  border-color: rgba(184, 212, 227, 0.85);
  transform: translateY(-2px);
}

.page-btn.is-active {
  color: var(--text-primary);
  border-color: rgba(184, 212, 227, 0.95);
  background: rgba(255, 255, 255, 0.92);
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.06);
}

.page-btn:disabled {
  opacity: 0.38;
  cursor: not-allowed;
}

.page-btn--ghost {
  padding-inline: 1rem;
}

@media (max-width: 980px) {
  .article-grid {
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

  .category-nav {
    gap: 0.5rem;
  }

  .category-btn {
    padding: 0.5rem 0.85rem;
    font-size: 0.76rem;
  }

  .pagination {
    gap: 0.5rem;
  }

  .page-btn--ghost {
    padding-inline: 0.75rem;
    font-size: 0.74rem;
  }
}
</style>
