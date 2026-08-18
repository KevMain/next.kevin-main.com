# Backlog: Pre-rendering / SSG for SEO

## Problem

The site is a Vue 3 SPA. The initial HTML served to crawlers contains no page content —
titles/descriptions/content are set client-side via JavaScript (including the per-route meta
added in the SEO repositioning work). Google renders JS, but:

- Indexing is slower and less reliable than static HTML
- Social media crawlers (Facebook, LinkedIn, Twitter) generally do **not** run JS, so
  per-route OG tags/preview cards never work for non-home pages
- For a small personal content site there is no reason to make crawlers do this work

## Proposed options (evaluate)

1. **vite-plugin-prerender / prerender-spa-plugin style** — snapshot each route to static
   HTML at build time. Least effort, keeps current codebase.
2. **vite-ssg** — Vue 3 + Vite native SSG. Moderate effort.
3. **Nuxt migration** — full SSR/SSG framework. Most effort, most capability
   (per-route OG tags, sitemap generation, useHead, etc.)

## Notes

- Blog (`/blog`, `/post/:slug`) is API-driven — dynamic routes need route discovery at
  build time (fetch slugs from the API) or SSR rather than pure SSG.
- Whichever option is chosen, per-route meta should move from the `router.afterEach`
  hook into the pre-rendered HTML head.

## Acceptance criteria

- `view-source:` of each route shows real content and correct per-route title/description
- OG preview works for at least `/`, `/projects`, `/cv` on LinkedIn/Twitter validators
- Lighthouse SEO score unchanged or improved; no regression in build/deploy pipeline

_Not blocking the current SEO repositioning PR._
