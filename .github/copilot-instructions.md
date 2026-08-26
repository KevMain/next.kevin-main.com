# Copilot Instructions

## Project Guidelines
- Prefer minimal data models over speculative fields (YAGNI): e.g., don't add per-entity Author until multi-author is a real requirement — derive from site-level settings instead. Persist fields only when they're deliberately optimized separately from other content (e.g., MetaDescription vs article body).