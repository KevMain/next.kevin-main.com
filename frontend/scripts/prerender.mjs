// Build-time prerender of blog article pages.
//
// Runs after `vite build`. Fetches published posts from the API and writes
// dist/blog/{slug}/index.html for each one, based on the built SPA shell
// (dist/index.html) so the pages load the same JS/CSS bundles and hydrate
// into the Vue app after first paint.
//
// Each page gets a unique <title>, meta description, canonical URL,
// Open Graph / article tags, BlogPosting JSON-LD and the full article HTML
// rendered into #app so crawlers receive real content without JavaScript.
//
// Env:
//   VITE_API_URL        API base URL (default: https://api.kevin-main.com)
//   SITE_BASE_URL       Public site base URL (default: https://www.kevin-main.com)
//   PRERENDER_OPTIONAL  'true' to warn instead of fail when the API is unreachable

import { readFile, writeFile, mkdir } from 'node:fs/promises'
import { fileURLToPath } from 'node:url'
import path from 'node:path'
import { marked } from 'marked'

const __dirname = path.dirname(fileURLToPath(import.meta.url))
const distDir = path.resolve(__dirname, '..', 'dist')

const apiBaseUrl = (process.env.VITE_API_URL || 'https://api.kevin-main.com').replace(/\/+$/, '')
const siteBaseUrl = (process.env.SITE_BASE_URL || 'https://www.kevin-main.com').replace(/\/+$/, '')
const siteName = 'Kevin Main'
const defaultAuthor = 'Kevin Main'
const optional = process.env.PRERENDER_OPTIONAL === 'true'

function escapeHtml(value) {
  return String(value)
    .replaceAll('&', '&amp;')
    .replaceAll('<', '&lt;')
    .replaceAll('>', '&gt;')
    .replaceAll('"', '&quot;')
}

// Match the API's Markdig DisableHtml() behaviour: raw HTML embedded in
// Markdown is escaped rather than emitted, so content cannot inject markup.
marked.use({
  renderer: {
    html(token) {
      return escapeHtml(token.text ?? token.raw ?? '')
    }
  }
})

function excerpt(markdown, maxLength = 160) {
  const plain = markdown
    .replace(/```[\s\S]*?```/g, ' ')
    .replace(/^>\s?/gm, '')
    .replace(/^#{1,6}\s+/gm, '')
    .replace(/[*_`~]/g, '')
    .replace(/\[([^\]]*)\]\([^)]*\)/g, '$1')
    .replace(/\s+/g, ' ')
    .trim()
  if (plain.length <= maxLength) return plain
  const cut = plain.lastIndexOf(' ', maxLength)
  return plain.slice(0, cut > 0 ? cut : maxLength) + '…'
}

function formatDate(iso) {
  return new Date(iso).toLocaleDateString('en-GB', { day: 'numeric', month: 'long', year: 'numeric' })
}

function renderArticlePage(template, post) {
  const canonicalUrl = `${siteBaseUrl}/blog/${encodeURIComponent(post.slug)}`
  const author = defaultAuthor
  const description = post.metaDescription?.trim() || excerpt(post.content)
  const pageTitle = `${post.title} | ${siteName}`
  const publishedIso = post.publishedAt ?? null
  const updatedIso = post.updatedAt ?? post.publishedAt ?? null
  const bodyHtml = marked.parse(post.content)

  const jsonLd = JSON.stringify({
    '@context': 'https://schema.org',
    '@type': 'BlogPosting',
    mainEntityOfPage: { '@type': 'WebPage', '@id': canonicalUrl },
    headline: post.title,
    description,
    url: canonicalUrl,
    ...(publishedIso ? { datePublished: publishedIso } : {}),
    ...(updatedIso ? { dateModified: updatedIso } : {}),
    author: { '@type': 'Person', name: author, url: `${siteBaseUrl}/` },
    publisher: { '@type': 'Person', name: siteName }
  })

  const headTags = [
    `<meta name="description" content="${escapeHtml(description)}">`,
    `<link rel="canonical" href="${escapeHtml(canonicalUrl)}">`,
    `<meta property="og:type" content="article">`,
    `<meta property="og:title" content="${escapeHtml(post.title)}">`,
    `<meta property="og:description" content="${escapeHtml(description)}">`,
    `<meta property="og:url" content="${escapeHtml(canonicalUrl)}">`,
    `<meta property="og:site_name" content="${escapeHtml(siteName)}">`,
    publishedIso ? `<meta property="article:published_time" content="${publishedIso}">` : null,
    updatedIso ? `<meta property="article:modified_time" content="${updatedIso}">` : null,
    `<meta property="article:author" content="${escapeHtml(author)}">`,
    `<meta name="twitter:card" content="summary">`,
    `<meta name="twitter:title" content="${escapeHtml(post.title)}">`,
    `<meta name="twitter:description" content="${escapeHtml(description)}">`,
    `<script type="application/ld+json">${jsonLd}</script>`
  ].filter(Boolean).join('\n    ')

  const appHtml = `
      <div class="post-page">
        <section class="post-header hero">
          <div class="container">
            <div class="hero-content">
              <h1 class="page-title">${escapeHtml(post.title)}</h1>
              <p class="post-byline">By ${escapeHtml(author)}</p>
              ${publishedIso ? `<time class="post-date" datetime="${publishedIso}">${formatDate(publishedIso)}</time>` : ''}
              ${post.updatedAt ? `<time class="post-updated" datetime="${post.updatedAt}">Updated ${formatDate(post.updatedAt)}</time>` : ''}
            </div>
          </div>
        </section>
        <section class="post-section">
          <div class="container">
            <article class="post-content-card">
              <div class="post-content">${bodyHtml}</div>
            </article>
            <div class="back-link-wrap">
              <a href="/blog" class="back-link">&larr; Back to Blog</a>
            </div>
          </div>
        </section>
      </div>`

  let html = template

  // Unique title
  html = html.replace(/<title>[\s\S]*?<\/title>/, `<title>${escapeHtml(pageTitle)}</title>`)

  // Remove the SPA shell's homepage-specific head tags that would conflict
  html = html
    .replace(/^\s*<meta name="description"[^>]*>\s*$/m, '')
    .replace(/^\s*<link rel="canonical"[^>]*>\s*$/m, '')
    .replace(/^\s*<meta property="og:(type|url|title|description)"[^>]*>\s*$/gm, '')
    .replace(/^\s*<meta property="twitter:[^"]*"[^>]*>\s*$/gm, '')

  // Inject per-article head tags
  html = html.replace('</head>', `    ${headTags}\n  </head>`)

  // Prerendered article content shown until the Vue app mounts and takes over
  html = html.replace(/<div id="app">\s*<\/div>/, `<div id="app">${appHtml}\n    </div>`)

  return html
}

async function main() {
  const templatePath = path.join(distDir, 'index.html')
  const template = await readFile(templatePath, 'utf8')

  console.log(`Prerendering blog articles from ${apiBaseUrl} ...`)
  const listResponse = await fetch(`${apiBaseUrl}/api/blog`)
  if (!listResponse.ok) {
    throw new Error(`Failed to list blog posts: HTTP ${listResponse.status}`)
  }
  // Defence in depth: the API list endpoint only returns published posts,
  // but never prerender anything not explicitly marked as published.
  const posts = (await listResponse.json()).filter((p) => p.isPublished === true)

  // /blog/* is excluded from the SWA navigationFallback so unknown slugs
  // return a real 404. Emit the SPA shell at /blog/index.html so the blog
  // listing route itself still serves the Vue app as a static file.
  const blogDir = path.join(distDir, 'blog')
  await mkdir(blogDir, { recursive: true })
  await writeFile(path.join(blogDir, 'index.html'), template, 'utf8')

  for (const post of posts) {
    const outDir = path.join(distDir, 'blog', post.slug)
    await mkdir(outDir, { recursive: true })
    await writeFile(path.join(outDir, 'index.html'), renderArticlePage(template, post), 'utf8')
    console.log(`  ✓ /blog/${post.slug}`)
  }

  // Emit staticwebapp.config.json into dist with an explicit server-side 301
  // for each known legacy /post/{slug} URL. SWA route rules cannot capture
  // wildcard segments, but per-slug routes give real 301s on the public host
  // (the Vue /post/:slug redirect remains as a client-side fallback).
  const configSourcePath = path.resolve(__dirname, '..', 'staticwebapp.config.json')
  const swaConfig = JSON.parse(await readFile(configSourcePath, 'utf8'))
  const redirectRoutes = posts.map((post) => ({
    route: `/post/${post.slug}`,
    redirect: `/blog/${post.slug}`,
    statusCode: 301
  }))
  swaConfig.routes = [...redirectRoutes, ...(swaConfig.routes ?? [])]
  await writeFile(path.join(distDir, 'staticwebapp.config.json'), JSON.stringify(swaConfig, null, 2), 'utf8')
  console.log(`  ✓ staticwebapp.config.json with ${redirectRoutes.length} legacy /post 301 redirect(s)`)

  console.log(`Prerendered ${posts.length} article page(s).`)
}

main().catch((err) => {
  if (optional) {
    console.warn(`Prerender skipped (PRERENDER_OPTIONAL=true): ${err.message}`)
    process.exit(0)
  }
  console.error(`Prerender failed: ${err.message}`)
  process.exit(1)
})
