<template>
  <div v-if="html" class="markdown-body" v-html="html" />
  <p v-else class="markdown-empty">{{ emptyText }}</p>
</template>

<script>
import { renderMarkdown } from '../utils/markdown'

export default {
  name: 'MarkdownContent',
  props: {
    // 原始 Markdown 文本
    source: {
      type: String,
      default: ''
    },
    emptyText: {
      type: String,
      default: '暂无内容'
    }
  },
  computed: {
    html() {
      return renderMarkdown(this.source)
    }
  }
}
</script>

<style scoped>
.markdown-empty {
  color: var(--text-secondary);
  font-size: 0.92rem;
}
</style>

<!-- v-html 内容需全局样式，不能 scoped -->
<style>
.markdown-body {
  color: var(--text-primary);
  line-height: 1.85;
  word-break: break-word;
}

.markdown-body > :first-child {
  margin-top: 0;
}

.markdown-body > :last-child {
  margin-bottom: 0;
}

.markdown-body h1,
.markdown-body h2,
.markdown-body h3,
.markdown-body h4 {
  margin: 1.6rem 0 0.75rem;
  line-height: 1.35;
  color: var(--text-primary);
}

.markdown-body h1 { font-size: 1.75rem; }
.markdown-body h2 { font-size: 1.45rem; }
.markdown-body h3 { font-size: 1.2rem; }

.markdown-body p,
.markdown-body ul,
.markdown-body ol,
.markdown-body blockquote,
.markdown-body pre {
  margin: 0.85rem 0;
}

.markdown-body ul,
.markdown-body ol {
  padding-left: 1.4rem;
}

.markdown-body li {
  margin: 0.35rem 0;
}

.markdown-body blockquote {
  padding: 0.5rem 1rem;
  border-left: 3px solid var(--accent-cyan);
  background: var(--surface-soft);
  color: var(--text-secondary);
  border-radius: 0 8px 8px 0;
}

.markdown-body a {
  color: color-mix(in srgb, var(--accent-cyan) 55%, var(--text-primary));
  text-decoration: underline;
  text-underline-offset: 3px;
}

.markdown-body code {
  padding: 0.15rem 0.4rem;
  border-radius: 6px;
  background: var(--surface-soft);
  font-family: var(--font-mono);
  font-size: 0.88em;
}

.markdown-body pre {
  padding: 1rem 1.1rem;
  border-radius: 14px;
  background: rgba(26, 26, 26, 0.92);
  overflow-x: auto;
}

.markdown-body pre code {
  padding: 0;
  background: transparent;
  color: #f5f5f5;
  font-size: 0.85rem;
}

.markdown-body hr {
  margin: 1.5rem 0;
  border: none;
  border-top: 1px solid var(--border-color);
}

.markdown-body img {
  max-width: 100%;
  border-radius: 12px;
}

.markdown-body table {
  width: 100%;
  border-collapse: collapse;
  margin: 1rem 0;
  font-size: 0.92rem;
}

.markdown-body th,
.markdown-body td {
  padding: 0.5rem 0.75rem;
  border: 1px solid var(--border-color);
}
</style>
