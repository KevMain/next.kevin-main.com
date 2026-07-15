# Adding Your LinkedIn Profile Photo

## Quick Setup

### Step 1: Download Your LinkedIn Photo

1. **Go to your LinkedIn profile:**
   - Visit: https://uk.linkedin.com/in/kevmain
   - Make sure you're logged in

2. **Download your profile photo:**
   - **Option A (Easy):** Right-click your profile photo → "Save image as..."
   - **Option B (Best Quality):** 
	 - Click on your profile photo to open it
	 - Right-click the larger image → "Save image as..."
   - Save as: `profile.jpg` or `profile.png`

### Step 2: Add Photo to Your Project

Place the downloaded photo here:
```
frontend/src/assets/images/profile.jpg
```

**Or via command line:**
```powershell
# Copy your downloaded photo to the project
Copy-Item "C:\Users\YourName\Downloads\profile.jpg" "C:\Git\next.kevin-main.com\frontend\src\assets\images\profile.jpg"
```

### Step 3: Test It

1. Start your frontend:
   ```powershell
   cd C:\Git\next.kevin-main.com\frontend
   npm run dev
   ```

2. Visit http://localhost:5173

3. You should see your photo on the home page! 🎉

## What I've Added

✅ **Profile image display** - Professional circular photo with gradient border
✅ **Hover effects** - Subtle scale and glow on hover
✅ **Floating animation** - Gentle up/down motion
✅ **Gradient border** - Matches your brand colors (cyan → purple)
✅ **Error handling** - Image gracefully hides if not found
✅ **Responsive design** - Looks great on all screen sizes

## Photo Specifications

### Recommended:
- **Format:** JPG or PNG
- **Size:** 400x400px or larger (square)
- **Aspect Ratio:** 1:1 (square)
- **File Size:** Under 500KB
- **Quality:** High-resolution professional headshot

### The Image Will Be:
- Displayed as a circle (200px diameter)
- Centered above your name
- Enhanced with your brand gradient border
- Animated with a subtle floating effect

## Alternative: Use a Different Image

If you want to use a different photo:

1. **Name it `profile.jpg` or `profile.png`**
2. **Place it in:** `frontend/src/assets/images/`
3. **The code will automatically use it!**

### Update the file extension (if needed):

If you use `.png` instead of `.jpg`, update `frontend/src/views/Home.vue`:

```vue
<img 
  src="@/assets/images/profile.png"   <!-- Change to .png -->
  alt="Kevin Main" 
  class="profile-image"
/>
```

## Troubleshooting

### Photo doesn't appear?
1. ✅ Check file location: `frontend/src/assets/images/profile.jpg`
2. ✅ Check file name matches exactly (case-sensitive on production servers)
3. ✅ Restart dev server: `npm run dev`
4. ✅ Hard refresh browser: `Ctrl + Shift + R`

### Photo looks stretched or cropped?
- Make sure your source image is square (1:1 aspect ratio)
- The CSS uses `object-fit: cover` to fill the circle nicely
- For best results, center your face in a square crop

### Want to adjust the size?
Edit `.profile-image-wrapper` in `frontend/src/views/Home.vue`:

```css
.profile-image-wrapper {
  width: 250px;   /* Change from 200px */
  height: 250px;  /* Change from 200px */
}
```

## Design Features

### Gradient Border
The profile photo has a 5px gradient border matching your site's brand:
- Cyan (#0ea5e9) → Purple (#a855f7)

### Shadow Effects
- **Default:** Soft cyan and purple glow
- **Hover:** Enhanced shadow for depth

### Animation
- **Float effect:** Smooth 6-second up/down motion
- **Hover scale:** Subtle 5% enlargement on hover

## Next Steps

1. ✅ Download your LinkedIn photo
2. ✅ Place it in `frontend/src/assets/images/profile.jpg`
3. ✅ Refresh your site to see it live!

Your home page will now have a professional, eye-catching profile photo that matches your personal brand! 📸✨
