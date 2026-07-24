# next.kevin-main.com
Kevin Main's Personal Portfolio Website - Live on Azure! 🚀

A modern, professional portfolio website showcasing Kevin Main's extensive experience as a Lead Developer and Senior Software Engineer.

## 🌐 Live Site

- **Website**: https://lemon-flower-011704103.7.azurestaticapps.net
- **API**: https://kevinmain-api.kindpond-04a3181a.ukwest.azurecontainerapps.io

## 🏗️ Architecture

```
Frontend (Vue.js) → Azure Static Web Apps (FREE)
    ↓ HTTPS
Backend (.NET 10 API) → Azure Container Apps ($15-30/mo)
    ↓ (Future: SQL Database)
CV Data → In-Memory (will migrate to database)
```

## Project Structure

```
├── KevinMain.API/          # .NET 10 Backend API
├── frontend/               # Vue.js 3 Frontend
├── .github/workflows/      # GitHub Actions CI/CD
├── Dockerfile              # Container image for API
└── start-dev.ps1           # Local development script
```

## Features

- **Professional Portfolio**: Modern personal website with customizable sections
- **CV/Resume**: Complete professional CV with API-driven data
- **Project Showcase**: Featured projects with descriptions and tech stacks
- **Contact Form**: Functional email contact form with SMTP support
- **Responsive Design**: Works beautifully on desktop, tablet, and mobile
- **Modern UI**: Clean, professional design with gradient effects and code-themed headers
- **API-Driven**: RESTful backend services for CV data and contact handling
- **Auto-Deploy**: Push to GitHub = automatic deployment to Azure
- **HTTPS Everywhere**: Secure by default in development and production

## Quick Start (Recommended)

### Run Both Frontend and Backend Together

```powershell
.\start-dev.ps1
```

This script will:
- Automatically install frontend dependencies (if needed)
- Trust the .NET development certificate (if needed)
- Start the .NET API backend on https://localhost:5001
- Start the Vue.js frontend on https://localhost:5173
- Display output from both servers
- Press `Ctrl+C` to stop both servers

Then open **https://localhost:5173** in your browser to view the live site.

**Note:** On first run, you may see browser security warnings about the self-signed certificates. This is normal for local development - click "Advanced" and "Proceed" to continue.

The site includes:
- **Home**: Landing page with profile photo and introduction
- **CV**: Complete professional CV and work history
- **Projects**: Showcase of featured projects
- **Contact**: Contact form for inquiries

## Prerequisites

Before running the application, ensure you have:

1. **.NET 10 SDK** - [Download here](https://dotnet.microsoft.com/download)
2. **Node.js** (v18 or later) - [Download here](https://nodejs.org/)
3. **Trusted development certificates**:
   ```powershell
   dotnet dev-certs https --trust
   ```
   This ensures your browser trusts the local HTTPS certificates.

## Manual Setup & Run

### Backend (.NET API)

1. Navigate to the backend:
   ```powershell
   cd KevinMain.API
   ```

2. Trust the development certificate (first time only):
   ```powershell
   dotnet dev-certs https --trust
   ```

3. Run the API (defaults to https://localhost:5001):
   ```powershell
   dotnet run
   ```

### Frontend (Vue.js)

1. Navigate to the frontend:
   ```powershell
   cd frontend
   ```

2. Install dependencies (first time only):
   ```powershell
   npm install
   ```

3. Run the dev server (runs on https://localhost:5173):
   ```powershell
   npm run dev
   ```

## Using the App

1. Start both services using the `start-dev.ps1` script (recommended)
2. Or manually start backend (https://localhost:5001) then frontend (https://localhost:5173)
3. Open https://localhost:5173 in your browser
4. Navigate between Home, CV, Projects, and Contact pages

**Security Note:** You may see browser warnings about self-signed certificates on first visit. This is normal for local development. Click "Advanced" and proceed to the site.

## API Endpoints

All API endpoints are served over HTTPS at `https://localhost:5001`

### CV Data
- `GET /api/cv` - Returns complete CV data in JSON format
- `GET /api/cv/health` - Health check endpoint

### Contact
- `POST /api/contact` - Submit contact form (requires Name, Email, Subject, Message)
- `GET /api/contact/health` - Health check endpoint

## Configuration

### SMTP Settings (Optional)

To enable email functionality for the contact form:

1. Copy `appsettings.Development.json.example` to `appsettings.Development.json` (this file is gitignored)
2. Update the SMTP settings:
   ```json
   {
     "SmtpSettings": {
       "Enabled": true,
       "Host": "smtp.gmail.com",
       "Port": 587,
       "UseSsl": true,
       "Username": "your-email@gmail.com",
       "Password": "your-app-password",
       "FromEmail": "your-email@gmail.com",
       "FromName": "Kevin Main Portfolio",
       "ToEmail": "your-email@gmail.com"
     }
   }
   ```

If SMTP is disabled, contact submissions are logged to the console instead.

## Updating CV Content

CV data is served by a service-based architecture for easy maintenance:

```
KevinMain.API/Services/InMemoryCVDataService.cs
```

To update your CV:
1. Edit the data in `InMemoryCVDataService.cs`
2. Restart the API server
3. Changes will be reflected immediately

The service pattern allows you to easily swap to a database or other data source by implementing the `ICVDataService` interface.

## 🚀 Production Deployment

The site is automatically deployed to Azure using GitHub Actions:

- **Push to `main`** → Triggers automatic deployment
- **Backend**: Builds Docker image, deploys to Azure Container Apps
- **Frontend**: Builds Vue app, deploys to Azure Static Web Apps

### Deployment Documentation

See these guides for deployment details:
- `README-DEPLOY.md` - Overview and live URLs
- `CI-CD-SETUP.md` - GitHub Actions configuration
- `CUSTOM-DOMAIN.md` - Custom domain setup (optional)

## Tech Stack

### Backend
- .NET 10
- ASP.NET Core Web API
- MailKit for SMTP email support
- Docker containerization
- Azure Container Apps hosting

### Frontend
- Vue.js 3 with Composition API
- Vue Router for navigation
- Vite for dev server and building
- Azure Static Web Apps hosting
- Modern CSS with gradient effects

### DevOps
- GitHub Actions CI/CD
- Azure Container Registry
- Azure Container Apps (backend)
- Azure Static Web Apps (frontend)
- Automated deployments on push

## 📝 Documentation

- **README.md** (this file) - Local development
- **README-DEPLOY.md** - Production deployment overview
- **HTTPS-SETUP.md** - Local HTTPS configuration
- **DATABASE-SETUP.md** - Future database migration guide
- **CI-CD-SETUP.md** - GitHub Actions setup
- **CUSTOM-DOMAIN.md** - Custom domain configuration

## 🔒 Security

- HTTPS enforced in development and production
- Trusted development certificates for local HTTPS
- CORS properly configured
- Secrets managed via GitHub Secrets and Azure
- Non-root Docker container user

## 📧 Contact

Questions? Use the contact form on the live site or reach out via GitHub.

