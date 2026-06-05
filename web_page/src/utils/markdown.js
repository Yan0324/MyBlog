import { marked } from 'marked'
import DOMPurify from 'dompurify'

// Markdown 解析配置：GFM + 换行转 <br>
marked.setOptions({
  breaks: true,
  gfm: true
})

/**
 * 将 Markdown 转为经过消毒的 HTML，防止 XSS。
 * @param {string} source 原始 Markdown 文本
 * @returns {string} 安全 HTML
 */
export function renderMarkdown(source) {
  if (!source || !String(source).trim()) {
    return ''
  }

  const rawHtml = marked.parse(String(source))
  return DOMPurify.sanitize(rawHtml, {
    ADD_ATTR: ['target', 'rel']
  })
}
