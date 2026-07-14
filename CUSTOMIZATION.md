# Customizing Your Portfolio

## Changing the Hero Background Image

### Current Image
The hero section currently uses a professional coding/tech workspace image from Unsplash.

### How to Change It

**Option 1: Use Your Own Image**

1. Add your image to the `frontend/public/` folder (e.g., `hero-bg.jpg`)
2. Edit `frontend/src/App.vue` and find the `.hero` class
3. Replace the background URL:

```css
.hero {
  background: 
	linear-gradient(135deg, rgba(102, 126, 234, 0.9) 0%, rgba(118, 75, 162, 0.9) 100%),
	url('/hero-bg.jpg') center/cover;
  /* ... rest of styles */
}
```

**Option 2: Use a Different Unsplash Image**

Browse [Unsplash](https://unsplash.com/) for tech/professional images and replace the URL:

**Tech/Code themed:**
- `https://images.unsplash.com/photo-1517694712202-14dd9538aa97?w=1920&q=80` (code on laptop - current)
- `https://images.unsplash.com/photo-1498050108023-c5249f4df085?w=1920&q=80` (computer on desk)
- `https://images.unsplash.com/photo-1461749280684-dccba630e2f6?w=1920&q=80` (code close-up)

**Abstract/Modern:**
- `https://images.unsplash.com/photo-1557683316-973673baf926?w=1920&q=80` (gradient abstract)
- `https://images.unsplash.com/photo-1451187580459-43490279c0fa?w=1920&q=80` (space/tech)

**Office/Professional:**
- `https://images.unsplash.com/photo-1497366216548-37526070297c?w=1920&q=80` (office desk)
- `https://images.unsplash.com/photo-1486312338219-ce68d2c6f44d?w=1920&q=80` (laptop workspace)

### Adjusting the Overlay

To make the image lighter or darker, adjust the opacity in the gradient overlay:

```css
/* Darker overlay (more visible image) */
rgba(102, 126, 234, 0.7)  /* was 0.9 */

/* Lighter overlay (less visible image) */
rgba(102, 126, 234, 0.95) /* was 0.9 */
```

### Tips for Best Results

- Use high-resolution images (1920px width minimum)
- Choose images with space for text in the center
- Darker or blurred images work best with the overlay
- Test on mobile to ensure text remains readable

## Changing Colors

### Primary Gradient Colors

Find and replace these hex codes throughout `App.vue`:

- **Purple:** `#667eea` (primary)
- **Dark Purple:** `#764ba2` (secondary)

### Quick Color Schemes

**Blue:**
```css
background: linear-gradient(135deg, #2193b0 0%, #6dd5ed 100%);
```

**Red:**
```css
background: linear-gradient(135deg, #ee0979 0%, #ff6a00 100%);
```

**Green:**
```css
background: linear-gradient(135deg, #56ab2f 0%, #a8e063 100%);
```

**Orange:**
```css
background: linear-gradient(135deg, #f46b45 0%, #eea849 100%);
```

## Adding Your Photo

To add a profile photo to the hero section, edit `frontend/src/App.vue`:

1. Add your photo to `frontend/public/` (e.g., `profile.jpg`)
2. Add this HTML in the hero section template:

```vue
<div class="profile-photo">
  <img src="/profile.jpg" alt="Kevin Main">
</div>
```

3. Add these styles:

```css
.profile-photo {
  margin: 30px auto;
  width: 150px;
  height: 150px;
}

.profile-photo img {
  width: 100%;
  height: 100%;
  border-radius: 50%;
  object-fit: cover;
  border: 5px solid white;
  box-shadow: 0 10px 30px rgba(0,0,0,0.3);
}
```
