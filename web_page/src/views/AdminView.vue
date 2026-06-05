<template>
  <section class="subpage admin-view">
    <div class="subpage-shell">
      <!-- 未登录：显示登录表单 -->
      <div v-if="!token" class="admin-login">
        <h1 class="admin-title">文章后台</h1>
        <p class="admin-lead">登录后可发布、编辑或删除 Essay 文章。</p>
        <form class="login-form" @submit.prevent="handleLogin">
          <label class="field">
            <span>管理密码</span>
            <input v-model.trim="password" type="password" placeholder="请输入后台密码" required />
          </label>
          <p v-if="loginError" class="form-error">{{ loginError }}</p>
          <button type="submit" class="primary-btn" :disabled="loggingIn">
            {{ loggingIn ? '登录中…' : '登录' }}
          </button>
        </form>
      </div>

      <!-- 已登录：文章管理 -->
      <div v-else class="admin-panel">
        <header class="admin-toolbar">
          <div>
            <h1 class="admin-title">文章后台</h1>
            <p class="admin-lead">内容会保存到服务器，前台 Essay 页自动读取。</p>
          </div>
          <div class="toolbar-actions">
            <button type="button" class="ghost-btn" @click="startCreate">新建文章</button>
            <button type="button" class="ghost-btn" @click="handleLogout">退出登录</button>
          </div>
        </header>

        <p v-if="listError" class="form-error">{{ listError }}</p>
        <p v-else-if="loadingList" class="admin-hint">加载文章列表…</p>

        <div v-else class="admin-layout">
          <!-- 文章列表 -->
          <aside class="admin-list">
            <button
              v-for="item in articles"
              :key="item.id"
              type="button"
              class="list-item"
              :class="{ 'is-active': editingId === item.id }"
              @click="startEdit(item)"
            >
              <span class="list-title">{{ item.title }}</span>
              <span class="list-meta">{{ categoryLabel(item.category) }} · {{ item.published ? '已发布' : '草稿' }}</span>
            </button>
            <p v-if="articles.length === 0" class="admin-hint">还没有文章，点击「新建文章」开始写。</p>
          </aside>

          <!-- 编辑表单 -->
          <form v-if="showEditor" class="editor-form" @submit.prevent="handleSave">
            <h2 class="editor-heading">{{ editingId ? '编辑文章' : '新建文章' }}</h2>

            <label class="field">
              <span>分类</span>
              <select v-model="form.category" required>
                <option v-for="cat in publishCategories" :key="cat.id" :value="cat.id">
                  {{ cat.label }}
                </option>
              </select>
            </label>

            <label class="field">
              <span>角标 / 日期（kicker）</span>
              <input v-model.trim="form.kicker" type="text" placeholder="例如 2026 · 06" />
            </label>

            <label class="field">
              <span>标题</span>
              <input v-model.trim="form.title" type="text" placeholder="文章标题" required />
            </label>

            <label class="field">
              <span>摘要（列表页展示，纯文本）</span>
              <textarea v-model.trim="form.copy" rows="3" placeholder="一两句话介绍这篇文章，不使用 Markdown" />
            </label>

            <div class="field">
              <span>正文（Markdown）</span>
              <div class="md-editor">
                <textarea
                  v-model="form.content"
                  class="md-textarea"
                  rows="12"
                  placeholder="支持 Markdown：## 标题、列表、**加粗**、代码块等"
                />
                <div class="md-preview">
                  <p class="md-preview-label">预览</p>
                  <MarkdownContent :source="form.content" empty-text="输入 Markdown 后在此预览" />
                </div>
              </div>
            </div>

            <label class="field">
              <span>标签（英文逗号分隔）</span>
              <input v-model.trim="tagsInput" type="text" placeholder="Vue, 随笔, 日常" />
            </label>

            <label class="field field--inline">
              <input v-model="form.published" type="checkbox" />
              <span>立即发布（取消勾选则保存为草稿，前台不显示）</span>
            </label>

            <p v-if="saveError" class="form-error">{{ saveError }}</p>

            <div class="editor-actions">
              <button type="submit" class="primary-btn" :disabled="saving">
                {{ saving ? '保存中…' : '保存' }}
              </button>
              <button
                v-if="editingId"
                type="button"
                class="danger-btn"
                :disabled="saving"
                @click="handleDelete"
              >
                删除
              </button>
              <button type="button" class="ghost-btn" @click="cancelEditor">取消</button>
            </div>
          </form>

          <p v-else class="admin-hint editor-placeholder">从左侧选择一篇文章，或点击「新建文章」。</p>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
import {
  adminLogin,
  fetchAdminArticles,
  createArticle,
  updateArticle,
  deleteArticle
} from '../api/client'
import MarkdownContent from '../components/MarkdownContent.vue'

const TOKEN_KEY = 'blog_admin_token'

export default {
  name: 'AdminView',
  components: { MarkdownContent },
  data() {
    return {
      token: localStorage.getItem(TOKEN_KEY) || '',
      password: '',
      loggingIn: false,
      loginError: '',
      loadingList: false,
      listError: '',
      articles: [],
      showEditor: false,
      editingId: null,
      saving: false,
      saveError: '',
      tagsInput: '',
      publishCategories: [
        { id: 'tech', label: '技术' },
        { id: 'life', label: '生活' },
        { id: 'notes', label: '随笔' }
      ],
      form: {
        category: 'notes',
        kicker: '',
        title: '',
        copy: '',
        content: '',
        published: true
      }
    }
  },
  mounted() {
    if (this.token) {
      this.loadArticles()
    }
  },
  methods: {
    emptyForm() {
      return {
        category: 'notes',
        kicker: '',
        title: '',
        copy: '',
        content: '',
        published: true
      }
    },
    categoryLabel(categoryId) {
      const found = this.publishCategories.find((cat) => cat.id === categoryId)
      return found ? found.label : categoryId
    },
    async handleLogin() {
      this.loggingIn = true
      this.loginError = ''
      try {
        const data = await adminLogin(this.password)
        this.token = data.token
        localStorage.setItem(TOKEN_KEY, this.token)
        this.password = ''
        await this.loadArticles()
      } catch (err) {
        this.loginError = err.message
      } finally {
        this.loggingIn = false
      }
    },
    handleLogout() {
      this.token = ''
      localStorage.removeItem(TOKEN_KEY)
      this.articles = []
      this.cancelEditor()
    },
    async loadArticles() {
      this.loadingList = true
      this.listError = ''
      try {
        const data = await fetchAdminArticles(this.token)
        this.articles = data.articles || []
      } catch (err) {
        this.listError = err.message
        if (err.message.includes('未授权')) {
          this.handleLogout()
        }
      } finally {
        this.loadingList = false
      }
    },
    startCreate() {
      this.editingId = null
      this.form = this.emptyForm()
      this.tagsInput = ''
      this.saveError = ''
      this.showEditor = true
    },
    startEdit(item) {
      this.editingId = item.id
      this.form = {
        category: item.category,
        kicker: item.kicker || '',
        title: item.title || '',
        copy: item.copy || '',
        content: item.content || '',
        published: item.published !== false
      }
      this.tagsInput = (item.tags || []).join(', ')
      this.saveError = ''
      this.showEditor = true
    },
    cancelEditor() {
      this.showEditor = false
      this.editingId = null
      this.saveError = ''
    },
    buildPayload() {
      const tags = this.tagsInput
        .split(/[,，]/)
        .map((tag) => tag.trim())
        .filter(Boolean)

      return {
        category: this.form.category,
        kicker: this.form.kicker,
        title: this.form.title,
        copy: this.form.copy,
        content: this.form.content,
        tags,
        published: this.form.published
      }
    },
    async handleSave() {
      this.saving = true
      this.saveError = ''
      try {
        const payload = this.buildPayload()
        if (this.editingId) {
          await updateArticle(this.token, this.editingId, payload)
        } else {
          await createArticle(this.token, payload)
        }
        await this.loadArticles()
        this.cancelEditor()
      } catch (err) {
        this.saveError = err.message
      } finally {
        this.saving = false
      }
    },
    async handleDelete() {
      if (!this.editingId) return
      const ok = window.confirm('确定删除这篇文章吗？此操作不可恢复。')
      if (!ok) return

      this.saving = true
      this.saveError = ''
      try {
        await deleteArticle(this.token, this.editingId)
        await this.loadArticles()
        this.cancelEditor()
      } catch (err) {
        this.saveError = err.message
      } finally {
        this.saving = false
      }
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
  padding: clamp(2rem, 4vw, 3.2rem);
  border: 1px solid var(--border-color);
  border-radius: 34px;
  background:
    radial-gradient(circle at top left, rgba(184, 212, 227, 0.18), transparent 30%),
    linear-gradient(180deg, var(--surface-card) 0%, rgba(255, 255, 255, 0.4) 100%);
  box-shadow: 0 24px 60px rgba(0, 0, 0, 0.06);
  backdrop-filter: blur(12px);
}

.admin-title {
  font-size: clamp(1.8rem, 4vw, 2.6rem);
  line-height: 1.1;
}

.admin-lead {
  margin-top: 0.6rem;
  color: var(--text-secondary);
}

.admin-login {
  max-width: 26rem;
}

.login-form,
.editor-form {
  margin-top: 1.5rem;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.admin-toolbar {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 1rem;
  flex-wrap: wrap;
}

.toolbar-actions {
  display: flex;
  gap: 0.6rem;
  flex-wrap: wrap;
}

.admin-layout {
  display: grid;
  grid-template-columns: minmax(220px, 280px) minmax(0, 1fr);
  gap: 1.25rem;
  margin-top: 1.5rem;
}

.admin-list {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  max-height: 70vh;
  overflow: auto;
  padding-right: 0.25rem;
}

.list-item {
  text-align: left;
  padding: 0.85rem 1rem;
  border: 1px solid rgba(26, 26, 26, 0.1);
  border-radius: 16px;
  background: rgba(255, 255, 255, 0.6);
  cursor: pointer;
  transition: border-color 0.25s ease, transform 0.25s ease;
}

.list-item:hover,
.list-item.is-active {
  border-color: rgba(184, 212, 227, 0.95);
  transform: translateY(-2px);
}

.list-title {
  display: block;
  font-weight: 600;
  color: var(--text-primary);
}

.list-meta {
  display: block;
  margin-top: 0.35rem;
  font-family: var(--font-mono);
  font-size: 0.72rem;
  color: var(--text-secondary);
}

.editor-form {
  padding: 1.25rem;
  border-radius: 20px;
  border: 1px solid rgba(26, 26, 26, 0.08);
  background: rgba(255, 255, 255, 0.72);
}

.editor-heading {
  font-size: 1.2rem;
  margin-bottom: 0.25rem;
}

.field {
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
  font-size: 0.9rem;
  color: var(--text-secondary);
}

.field--inline {
  flex-direction: row;
  align-items: center;
  gap: 0.6rem;
}

.field input,
.field select,
.field textarea {
  width: 100%;
  padding: 0.65rem 0.8rem;
  border: 1px solid rgba(26, 26, 26, 0.12);
  border-radius: 12px;
  background: rgba(255, 255, 255, 0.9);
  color: var(--text-primary);
  font: inherit;
}

.field textarea {
  resize: vertical;
  min-height: 4rem;
}

/* Markdown 编辑：左侧输入、右侧预览 */
.md-editor {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 0.75rem;
  min-height: 16rem;
}

.md-textarea {
  width: 100%;
  min-height: 16rem;
  padding: 0.65rem 0.8rem;
  border: 1px solid rgba(26, 26, 26, 0.12);
  border-radius: 12px;
  background: rgba(255, 255, 255, 0.9);
  color: var(--text-primary);
  font-family: var(--font-mono);
  font-size: 0.85rem;
  line-height: 1.6;
  resize: vertical;
}

.md-preview {
  padding: 0.75rem 0.9rem;
  border: 1px solid rgba(26, 26, 26, 0.1);
  border-radius: 12px;
  background: rgba(255, 255, 255, 0.55);
  overflow: auto;
  max-height: 24rem;
}

.md-preview-label {
  margin-bottom: 0.6rem;
  font-family: var(--font-mono);
  font-size: 0.72rem;
  letter-spacing: 0.08em;
  color: var(--text-secondary);
  text-transform: uppercase;
}

.editor-actions {
  display: flex;
  flex-wrap: wrap;
  gap: 0.6rem;
}

.primary-btn,
.ghost-btn,
.danger-btn {
  padding: 0.55rem 1.1rem;
  border-radius: 999px;
  font-family: var(--font-mono);
  font-size: 0.82rem;
  cursor: pointer;
  transition: transform 0.25s ease, opacity 0.25s ease;
}

.primary-btn {
  border: none;
  background: var(--text-primary);
  color: var(--bg-color);
}

.ghost-btn {
  border: 1px solid rgba(26, 26, 26, 0.14);
  background: rgba(255, 255, 255, 0.7);
  color: var(--text-primary);
}

.danger-btn {
  border: 1px solid rgba(180, 60, 60, 0.35);
  background: rgba(255, 240, 240, 0.9);
  color: #8b2e2e;
}

.primary-btn:hover:not(:disabled),
.ghost-btn:hover:not(:disabled),
.danger-btn:hover:not(:disabled) {
  transform: translateY(-2px);
}

.primary-btn:disabled,
.ghost-btn:disabled,
.danger-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.form-error {
  color: #8b2e2e;
  font-size: 0.88rem;
}

.admin-hint,
.editor-placeholder {
  color: var(--text-secondary);
  font-size: 0.9rem;
}

@media (max-width: 900px) {
  .admin-layout {
    grid-template-columns: 1fr;
  }

  .admin-list {
    max-height: none;
  }

  .md-editor {
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
