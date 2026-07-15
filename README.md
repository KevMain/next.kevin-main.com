# next.kevin-main.com
Kevin Main's Personal Portfolio Website

A modern, professional portfolio website showcasing Kevin Main's extensive experience as a Lead Developer and Senior Software Engineer.

## Project Structure

```
├── KevinMain.API/          # .NET 10 Backend API
├── frontend/               # Vue.js 3 Frontend
└── start-dev.ps1           # PowerShell script to run both
```

## Features

- **Professional Portfolio**: Modern personal website with customizable sections
- **CV/Resume**: Complete professional CV with API-driven data
- **Project Showcase**: Featured projects with descriptions and tech stacks
- **Contact Form**: Functional email contact form with SMTP support
- **Responsive Design**: Works beautifully on desktop, tablet, and mobile
- **Modern UI**: Clean, professional design with gradient effects and code-themed headers
- **API-Driven**: Backend services for CV data and contact handling
- **Service Architecture**: Abstracted services for easy swapping of implementations

## Quick Start (Recommended)

### Run Both Frontend and Backend Together

```powershell
.\start-dev.ps1
```

This script will:
- Automatically install frontend dependencies (if needed)
- Start the .NET API backend on http://localhost:5000
- Start the Vue.js frontend on http://localhost:5173
- Display output from both servers
- Press `Ctrl+C` to stop both servers

Then open **http://localhost:5173** in your browser to view the live site.

The site includes:
- **Home**: Landing page with profile photo and introduction
- **CV**: Complete professional CV and work history
- **Projects**: Showcase of featured projects
- **Contact**: Contact form for inquiries

## Manual Setup & Run

### Backend (.NET API)

1. Navigate to the backend:
   ```powershell
   cd KevinMain.API
   ```

2. Run the API (defaults to http://localhost:5000):
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

3. Run the dev server (runs on http://localhost:5173):
   ```powershell
   npm run dev
   ```

## Using the App

1. Start both services using the `start-dev.ps1` script (recommended)
2. Or manually start backend (http://localhost:5000) then frontend (http://localhost:5173)
3. Open http://localhost:5173 in your browser
4. Navigate between Home, CV, Projects, and Contact pages

## API Endpoints

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

## Tech Stack

### Backend
- .NET 10
- ASP.NET Core Web API
- MailKit for SMTP email support
- Service-based architecture with dependency injection

### Frontend
- Vue.js 3 with Composition API
- Vue Router for navigation
- Vite for dev server and building
- Modern CSS with gradient effects

### Communication
- REST API with JSON
- CORS enabled for local development

