# HTTPS Configuration Summary

This document summarizes the HTTPS configuration changes made to the Kevin Main portfolio site.

## Changes Made

### Backend (.NET API)

1. **launchSettings.json**
   - HTTPS profile now default (listed first)
   - HTTPS endpoint: `https://localhost:5001`
   - HTTP profile still available for fallback

2. **Program.cs**
   - Updated CORS to allow both HTTP and HTTPS origins from localhost:5173
   - Enabled HTTPS redirection middleware

### Frontend (Vue.js)

1. **vite.config.js**
   - Added `@vitejs/plugin-basic-ssl` plugin
   - Enabled HTTPS on dev server (port 5173)

2. **API Calls Updated**
   - `Contact.vue`: Changed fetch URL to `https://localhost:5001/api/contact`
   - `CV.vue`: Changed fetch URL to `https://localhost:5001/api/cv`

3. **Package Added**
   - Installed `@vitejs/plugin-basic-ssl` for automatic SSL certificate generation

### Scripts & Documentation

1. **start-dev.ps1**
   - Updated to use HTTPS launch profile for backend
   - Display HTTPS URLs in output
   - Backend: `https://localhost:5001`
   - Frontend: `https://localhost:5173`

2. **README.md**
   - Updated all URLs to HTTPS
   - Added Prerequisites section with certificate trust instructions
   - Added security notes about browser warnings for self-signed certificates
   - Updated API endpoint documentation

## Running the Application

### Quick Start (Recommended)
```powershell
.\start-dev.ps1
```

### Manual Start

**Backend:**
```powershell
cd KevinMain.API
dotnet run
```
This will start at `https://localhost:5001`

**Frontend:**
```powershell
cd frontend
npm run dev
```
This will start at `https://localhost:5173`

## Certificate Trust

### First-Time Setup
Run this command to trust the .NET development certificate:
```powershell
dotnet dev-certs https --trust
```

### Browser Warnings
On first visit, you may see browser security warnings about self-signed certificates. This is expected for local development:

1. Click "Advanced" or "Show Details"
2. Click "Proceed to localhost" or "Accept the Risk"
3. This only needs to be done once per browser

## Security Notes

- The HTTPS certificates used are self-signed and for **local development only**
- The `@vitejs/plugin-basic-ssl` generates a basic SSL certificate for Vite
- .NET uses the official .NET development certificate
- **Never** use these certificates in production
- Both certificates are not trusted by default until you explicitly trust them

## Benefits of HTTPS in Development

1. **Matches production environment** - Most production sites use HTTPS
2. **Service workers** - Some browser features require HTTPS
3. **Security testing** - Test security headers and HTTPS-only features
4. **Modern browser features** - Some APIs only work over HTTPS
5. **Mixed content debugging** - Catch mixed content issues early

## Troubleshooting

### Certificate Not Trusted
If you see certificate errors after trusting:
```powershell
# Clean and regenerate certificate
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

### Port Already in Use
If port 5001 or 5173 is in use:
- Close other applications using these ports
- Or modify the ports in `launchSettings.json` and `vite.config.js`

### CORS Errors
If you see CORS errors:
- Ensure both frontend and backend are running
- Check that CORS allows the HTTPS origin in `Program.cs`
- Verify API calls use the correct HTTPS URL

## Reverting to HTTP

If you need to revert to HTTP for any reason:

1. **Backend**: Use `dotnet run --launch-profile http`
2. **Frontend**: Remove `https: true` from `vite.config.js`
3. **API Calls**: Change URLs back to `http://localhost:5000`
4. **CORS**: Update `Program.cs` to allow HTTP origin only
