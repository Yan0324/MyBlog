// API 基础地址：开发环境走 vue.config.js 代理，生产环境在 .env.production 配置
const API_BASE = process.env.VUE_APP_API_BASE || '/api'

async function request(path, options = {}) {
  const headers = {
    'Content-Type': 'application/json',
    ...(options.headers || {})
  }

  const response = await fetch(`${API_BASE}${path}`, {
    ...options,
    headers
  })

  let payload = null
  try {
    payload = await response.json()
  } catch (e) {
    payload = null
  }

  // 统一 Result 结构：{ code, message, data }
  const message = (payload && payload.message) || '请求失败'
  const code = payload && typeof payload.code === 'number' ? payload.code : null

  if (!response.ok || (code !== null && code !== 200)) {
    throw new Error(message)
  }

  // 返回 data 字段，保持业务层用法不变（data.articles / data.status 等）
  return payload && Object.prototype.hasOwnProperty.call(payload, 'data') ? payload.data : payload
}

// 前台获取已发布文章
export function fetchPublishedArticles(category) {
  const query = category && category !== 'all' ? `?category=${encodeURIComponent(category)}` : ''
  return request(`/articles${query}`)
}

// 前台获取单篇文章（详情页）
export function fetchArticleById(id) {
  return request(`/articles/${encodeURIComponent(id)}`)
}

// 前台获取首页状态
export function fetchSiteStatus() {
  return request('/status')
}

// 后台登录
export function adminLogin(password) {
  return request('/admin/login', {
    method: 'POST',
    body: JSON.stringify({ password })
  })
}

// 后台获取首页状态
export function fetchAdminStatus(token) {
  return request('/admin/status', {
    headers: { Authorization: `Bearer ${token}` }
  })
}

// 后台更新首页状态
export function updateAdminStatus(token, payload) {
  return request('/admin/status', {
    method: 'PUT',
    headers: { Authorization: `Bearer ${token}` },
    body: JSON.stringify(payload)
  })
}

// 后台获取全部文章
export function fetchAdminArticles(token) {
  return request('/admin/articles', {
    headers: { Authorization: `Bearer ${token}` }
  })
}

// 后台新建文章
export function createArticle(token, payload) {
  return request('/admin/articles', {
    method: 'POST',
    headers: { Authorization: `Bearer ${token}` },
    body: JSON.stringify(payload)
  })
}

// 后台更新文章
export function updateArticle(token, id, payload) {
  return request(`/admin/articles/${id}`, {
    method: 'PUT',
    headers: { Authorization: `Bearer ${token}` },
    body: JSON.stringify(payload)
  })
}

// 后台删除文章
export function deleteArticle(token, id) {
  return request(`/admin/articles/${id}`, {
    method: 'DELETE',
    headers: { Authorization: `Bearer ${token}` }
  })
}
