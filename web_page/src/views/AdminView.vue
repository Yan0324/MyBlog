<template>
  <!-- ==================== 登录态 ==================== -->
  <div v-show="!token" class="admin-login-overlay">
    <div class="login-card-wrapper">
      <el-card shadow="always">
        <template #header>
          <div class="login-header">文章后台</div>
        </template>
        <el-form label-position="top" size="large" @submit.prevent="handleLogin">
          <el-form-item>
            <el-input
              v-model.trim="username"
              placeholder="用户名"
              @keyup.enter="handleLogin"
            />
          </el-form-item>
          <el-form-item>
            <el-input
              v-model.trim="password"
              type="password"
              placeholder="密码"
              show-password
              @keyup.enter="handleLogin"
            />
          </el-form-item>
          <el-alert
            v-if="loginError"
            :title="loginError"
            type="error"
            show-icon
            :closable="false"
            style="margin-bottom: 1rem"
          />
          <el-form-item>
            <el-button
              type="primary"
              :loading="loggingIn"
              style="width: 100%"
              @click="handleLogin"
            >
              {{ loggingIn ? '登录中…' : '登录' }}
            </el-button>
          </el-form-item>
        </el-form>
      </el-card>
    </div>
  </div>

  <!-- ==================== 后台态 ==================== -->
  <div v-show="token" class="admin-dashboard">
    <el-container style="height: 100vh">
      <!-- ── 侧边栏 ── -->
      <el-aside :width="isCollapse ? '64px' : '220px'" class="admin-aside">
        <div class="aside-brand">
          <span v-show="!isCollapse" class="brand-text">文章后台</span>
          <el-button class="collapse-btn" text @click="isCollapse = !isCollapse">
            <el-icon :size="18">
              <Fold v-if="!isCollapse" />
              <Expand v-else />
            </el-icon>
          </el-button>
        </div>
        <el-menu
          :default-active="activeMenu"
          :collapse="isCollapse"
          :collapse-transition="false"
          background-color="#304156"
          text-color="#bfcbd9"
          active-text-color="#409EFF"
          @select="handleMenuSelect"
        >
          <el-menu-item index="status">
            <el-icon><Monitor /></el-icon>
            <template #title>首页状态</template>
          </el-menu-item>
          <el-menu-item index="articles">
            <el-icon><Document /></el-icon>
            <template #title>文章管理</template>
          </el-menu-item>
        </el-menu>
      </el-aside>

      <!-- ── 右侧：Header + Main ── -->
      <el-container>
        <el-header class="admin-header">
          <div class="header-left">
            <el-breadcrumb separator="/">
              <el-breadcrumb-item>后台管理</el-breadcrumb-item>
              <el-breadcrumb-item>
                {{ activeMenu === 'status' ? '首页状态' : '文章管理' }}
              </el-breadcrumb-item>
            </el-breadcrumb>
          </div>
          <div class="header-right">
            <el-button size="small" @click="handleLogout">退出登录</el-button>
          </div>
        </el-header>

        <el-main class="admin-main">
          <!-- 错误 / 加载 -->
          <el-alert
            v-if="listError"
            :title="listError"
            type="error"
            show-icon
            :closable="false"
            style="margin-bottom: 1rem"
          />
          <div v-if="loadingList" class="loading-center">
            <el-icon class="is-loading" :size="28"><Loading /></el-icon>
            <p>加载中…</p>
          </div>

          <!-- ═══════ 首页状态 ═══════ -->
          <div v-if="activeMenu === 'status' && !loadingList" class="view-panel">
            <el-card>
              <template #header>
                <span class="card-heading">首页状态</span>
              </template>
              <p class="card-desc">管理首页头像下方的年度关键词与状态行，保存后前台立即生效。</p>
              <el-form
                label-position="top"
                style="margin-top: 1.25rem; max-width: 480px"
                @submit.prevent="handleSaveStatus"
              >
                <el-form-item label="年度关键词">
                  <el-input v-model.trim="statusForm.keyword" placeholder="例如 Be Rich" />
                </el-form-item>
                <el-form-item label="状态行">
                  <el-input v-model.trim="statusForm.statusLine" placeholder="例如 2026 · 平静" />
                </el-form-item>
                <el-alert
                  v-if="statusError"
                  :title="statusError"
                  type="error"
                  show-icon
                  :closable="false"
                  style="margin-bottom: 1rem"
                />
                <el-alert
                  v-if="statusSavedHint"
                  :title="statusSavedHint"
                  type="success"
                  show-icon
                  :closable="false"
                  style="margin-bottom: 1rem"
                />
                <el-form-item>
                  <el-button type="primary" :loading="savingStatus" @click="handleSaveStatus">
                    {{ savingStatus ? '保存中…' : '保存状态' }}
                  </el-button>
                </el-form-item>
              </el-form>
            </el-card>
          </div>

          <!-- ═══════ 文章管理 ═══════ -->
          <div v-if="activeMenu === 'articles' && !loadingList" class="articles-layout">
            <!-- 文章列表 -->
            <div class="article-list-panel">
              <div class="list-toolbar">
                <span class="card-heading">文章列表</span>
                <el-button type="primary" size="small" @click="startCreate">
                  <el-icon><Plus /></el-icon> 新建
                </el-button>
              </div>
              <div class="article-list-scroll">
                <div
                  v-for="item in articles"
                  :key="item.id"
                  class="article-list-item"
                  :class="{ 'is-active': editingId === item.id }"
                  @click="startEdit(item)"
                >
                  <div class="item-title">{{ item.title }}</div>
                  <div class="item-meta">
                    <el-tag size="small" type="info">{{ categoryLabel(item.category) }}</el-tag>
                    <el-tag
                      size="small"
                      :type="item.published ? 'success' : 'warning'"
                      style="margin-left: 0.35rem"
                    >
                      {{ item.published ? '已发布' : '草稿' }}
                    </el-tag>
                  </div>
                </div>
                <el-empty v-if="articles.length === 0" description="还没有文章" :image-size="80" />
              </div>
            </div>

            <!-- 编辑器 -->
            <div class="editor-panel">
              <el-card v-if="showEditor">
                <template #header>
                  <span class="card-heading">{{ editingId ? '编辑文章' : '新建文章' }}</span>
                </template>
                <el-form label-position="top" @submit.prevent="handleSave">
                  <el-row :gutter="16">
                    <el-col :span="12">
                      <el-form-item label="分类">
                        <el-select v-model="form.category" style="width: 100%">
                          <el-option
                            v-for="cat in publishCategories"
                            :key="cat.id"
                            :label="cat.label"
                            :value="cat.id"
                          />
                        </el-select>
                      </el-form-item>
                    </el-col>
                    <el-col :span="12">
                      <el-form-item label="角标 / 日期">
                        <el-input v-model.trim="form.kicker" placeholder="例如 2026 · 06" />
                      </el-form-item>
                    </el-col>
                  </el-row>

                  <el-form-item label="标题">
                    <el-input v-model.trim="form.title" placeholder="文章标题" />
                  </el-form-item>

                  <el-form-item label="摘要（列表页展示，纯文本）">
                    <el-input
                      v-model.trim="form.copy"
                      type="textarea"
                      :rows="2"
                      placeholder="一两句话介绍这篇文章，不使用 Markdown"
                    />
                  </el-form-item>

                  <el-form-item label="正文（Markdown）">
                    <div class="md-editor">
                      <el-input
                        v-model="form.content"
                        type="textarea"
                        :rows="14"
                        placeholder="支持 Markdown：## 标题、列表、**加粗**、代码块等"
                        class="md-textarea"
                      />
                      <div class="md-preview">
                        <div class="md-preview-label">预览</div>
                        <MarkdownContent :source="form.content" empty-text="输入 Markdown 后在此预览" />
                      </div>
                    </div>
                  </el-form-item>

                  <el-form-item label="标签（英文逗号分隔）">
                    <el-input v-model.trim="tagsInput" placeholder="Vue, 随笔, 日常" />
                  </el-form-item>

                  <el-form-item>
                    <el-checkbox v-model="form.published" label="立即发布（取消勾选则保存为草稿，前台不显示）" />
                  </el-form-item>

                  <el-alert
                    v-if="saveError"
                    :title="saveError"
                    type="error"
                    show-icon
                    :closable="false"
                    style="margin-bottom: 1rem"
                  />

                  <el-form-item>
                    <el-button type="primary" :loading="saving" @click="handleSave">
                      {{ saving ? '保存中…' : '保存' }}
                    </el-button>
                    <el-button
                      v-if="editingId"
                      type="danger"
                      :disabled="saving"
                      @click="handleDelete"
                    >
                      删除
                    </el-button>
                    <el-button @click="cancelEditor">取消</el-button>
                  </el-form-item>
                </el-form>
              </el-card>

              <el-empty
                v-else
                description="从左侧选择一篇文章，或点击「新建」"
                :image-size="100"
                style="margin-top: 3rem"
              />
            </div>
          </div>
        </el-main>
      </el-container>
    </el-container>
  </div>
</template>

<script>
import {
  adminLogin,
  fetchAdminArticles,
  fetchAdminStatus,
  updateAdminStatus,
  createArticle,
  updateArticle,
  deleteArticle
} from '../api/client'
import MarkdownContent from '../components/MarkdownContent.vue'
import { Monitor, Document, Fold, Expand, Loading, Plus } from '@element-plus/icons-vue'

const TOKEN_KEY = 'blog_admin_token'

export default {
  name: 'AdminView',
  components: {
    MarkdownContent,
    Monitor,
    Document,
    Fold,
    Expand,
    Loading,
    Plus
  },
  data() {
    return {
      // Auth
      token: localStorage.getItem(TOKEN_KEY) || '',
      username: '',
      password: '',
      loggingIn: false,
      loginError: '',

      // Layout
      isCollapse: false,
      activeMenu: 'status',

      // Data loading
      loadingList: false,
      listError: '',

      // Articles
      articles: [],
      showEditor: false,
      editingId: null,
      saving: false,
      saveError: '',
      tagsInput: '',

      // Status
      statusForm: {
        keyword: '',
        statusLine: ''
      },
      savingStatus: false,
      statusError: '',
      statusSavedHint: '',

      // Constants
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
    /* ── 工具 ── */
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
    categoryLabel(id) {
      const found = this.publishCategories.find((cat) => cat.id === id)
      return found ? found.label : id
    },
    handleMenuSelect(index) {
      this.activeMenu = index
    },

    /* ── 登录 ── */
    async handleLogin() {
      this.loggingIn = true
      this.loginError = ''
      try {
        const data = await adminLogin(this.username, this.password)
        this.token = data.token
        localStorage.setItem(TOKEN_KEY, this.token)
        this.username = ''
        this.password = ''
      } catch (err) {
        // 登录失败：错误显示在登录表单上
        this.loginError = err.message
        this.loggingIn = false
        return
      }
      this.loggingIn = false

      // 等待 Vue 完成 v-if→v-else DOM 切换（登录表单隐藏 → dashboard 显示），
      // Element Plus 的 el-container 等组件需要挂载到 DOM 后才能正确计算布局
      await this.$nextTick()

      // 登录成功后再加载数据；loadArticles 自己处理错误，错误会显示在 dashboard 的 listError 上
      await this.loadArticles()
    },
    handleLogout() {
      this.token = ''
      localStorage.removeItem(TOKEN_KEY)
      this.articles = []
      this.cancelEditor()
    },

    /* ── 数据加载 ── */
    async loadArticles() {
      this.loadingList = true
      this.listError = ''
      try {
        const [articlesData, statusData] = await Promise.all([
          fetchAdminArticles(this.token),
          fetchAdminStatus(this.token)
        ])
        this.articles = articlesData.articles || []
        const status = statusData.status || {}
        this.statusForm = {
          keyword: status.keyword || '',
          statusLine: status.statusLine || ''
        }
      } catch (err) {
        this.listError = err.message
        if (err.message.includes('未授权')) {
          this.handleLogout()
        }
      } finally {
        this.loadingList = false
      }
    },

    /* ── 状态管理 ── */
    async handleSaveStatus() {
      this.savingStatus = true
      this.statusError = ''
      this.statusSavedHint = ''
      try {
        const data = await updateAdminStatus(this.token, {
          keyword: this.statusForm.keyword,
          statusLine: this.statusForm.statusLine
        })
        const status = data.status || {}
        this.statusForm = {
          keyword: status.keyword || '',
          statusLine: status.statusLine || ''
        }
        this.statusSavedHint = '状态已保存'
      } catch (err) {
        this.statusError = err.message
      } finally {
        this.savingStatus = false
      }
    },

    /* ── 文章编辑器 ── */
    startCreate() {
      this.activeMenu = 'articles'
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
      try {
        await this.$confirm('确定删除这篇文章吗？此操作不可恢复。', '警告', {
          confirmButtonText: '确定删除',
          cancelButtonText: '取消',
          type: 'warning'
        })
      } catch {
        return
      }

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
/* ═══════════════════════════════════════════
   登录态
   ═══════════════════════════════════════════ */
.admin-login-overlay {
  position: fixed;
  inset: 0;
  z-index: 200;
  display: flex;
  align-items: center;
  justify-content: center;
  background: var(--bg-color);
}

.login-card-wrapper {
  width: min(28rem, calc(100% - 2rem));
}

.login-header {
  font-size: 1.5rem;
  font-weight: 600;
  text-align: center;
}



/* ═══════════════════════════════════════════
   后台态 — 全屏 Dashboard
   ═══════════════════════════════════════════ */
.admin-dashboard {
  position: fixed;
  inset: 0;
  z-index: 200;
}

/* ── 侧边栏 ── */
.admin-aside {
  background-color: #304156;
  overflow: hidden;
  transition: width 0.28s ease;
}

.aside-brand {
  height: 60px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 1rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.08);
}

.brand-text {
  color: #fff;
  font-size: 1.05rem;
  font-weight: 600;
  white-space: nowrap;
  overflow: hidden;
}

.collapse-btn {
  color: #bfcbd9 !important;
  flex-shrink: 0;
}

/* 去除菜单右侧边框 */
.admin-aside .el-menu {
  border-right: none;
}

/* ── Header ── */
.admin-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  background: #fff;
  border-bottom: 1px solid #e6e6e6;
  padding: 0 1.25rem;
  height: 56px;
  flex-shrink: 0;
}

.header-left {
  display: flex;
  align-items: center;
}

.header-right {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

/* ── Main ── */
.admin-main {
  background: #f0f2f5;
  padding: 1.25rem;
  overflow-y: auto;
}

.loading-center {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 3rem;
  color: #909399;
}

.loading-center p {
  margin-top: 0.5rem;
}

/* ── 卡片通用 ── */
.card-heading {
  font-size: 1.05rem;
  font-weight: 600;
}

.card-desc {
  color: #909399;
  font-size: 0.85rem;
  margin-top: 0.25rem;
}

/* ═══════════════════════════════════════════
   文章管理布局
   ═══════════════════════════════════════════ */
.articles-layout {
  display: grid;
  grid-template-columns: 300px 1fr;
  gap: 1.25rem;
  align-items: start;
  height: calc(100vh - 56px - 2.5rem); /* 减去 header + main padding */
}

/* ── 文章列表面板 ── */
.article-list-panel {
  display: flex;
  flex-direction: column;
  background: #fff;
  border-radius: 4px;
  overflow: hidden;
  height: 100%;
}

.list-toolbar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0.85rem 1rem;
  border-bottom: 1px solid #ebeef5;
  flex-shrink: 0;
}

.article-list-scroll {
  flex: 1;
  overflow-y: auto;
  padding: 0.5rem;
}

.article-list-item {
  padding: 0.75rem 0.85rem;
  border-radius: 6px;
  cursor: pointer;
  transition: background 0.2s ease;
  margin-bottom: 2px;
}

.article-list-item:hover {
  background: #f5f7fa;
}

.article-list-item.is-active {
  background: #ecf5ff;
}

.item-title {
  font-size: 0.9rem;
  font-weight: 500;
  color: #303133;
  margin-bottom: 0.35rem;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.item-meta {
  display: flex;
  align-items: center;
  gap: 0.35rem;
}

/* ── 编辑器面板 ── */
.editor-panel {
  overflow-y: auto;
  height: 100%;
}

/* Markdown 编辑区 */
.md-editor {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 0.75rem;
}

.md-textarea :deep(textarea) {
  font-family: var(--font-mono);
  font-size: 0.85rem;
  line-height: 1.6;
}

.md-preview {
  padding: 0.65rem 0.8rem;
  border: 1px solid #dcdfe6;
  border-radius: 4px;
  background: #fafafa;
  overflow: auto;
  max-height: 24rem;
}

.md-preview-label {
  margin-bottom: 0.5rem;
  font-family: var(--font-mono);
  font-size: 0.72rem;
  letter-spacing: 0.08em;
  color: #909399;
  text-transform: uppercase;
}

/* ═══════════════════════════════════════════
   响应式
   ═══════════════════════════════════════════ */
@media (max-width: 900px) {
  .articles-layout {
    grid-template-columns: 1fr;
    height: auto;
  }

  .article-list-panel {
    max-height: 40vh;
  }

  .md-editor {
    grid-template-columns: 1fr;
  }

  .admin-aside {
    position: absolute;
    height: 100%;
    z-index: 10;
  }
}

@media (max-width: 640px) {
  .admin-main {
    padding: 0.75rem;
  }
}
</style>
