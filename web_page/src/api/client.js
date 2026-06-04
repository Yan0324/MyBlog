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

  let data = null
  try {
    data = await response.json()
  } catch (e) {
    data = null
  }

  if (!response.ok) {
    const message = (data && data.message) || '请求失败'
    throw new Error(message)
  }

  return data
}

// 前台获取已发布文章
export function fetchPublishedArticles(category) {
  const query = category && category !== 'all' ? `?category=${encodeURIComponent(category)}` : ''
  return request(`/articles${query}`)
}

// 后台登录
export function adminLogin(password) {
  return request('/admin/login', {
    method: 'POST',
    body: JSON.stringify({ password })
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
