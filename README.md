# next.kevin-main.com
New personal website

## Project Structure

```
├── KevinMain.API/          # .NET 10 Backend API
├── frontend/               # Vue.js 3 Frontend
└── start-dev.ps1           # PowerShell script to run both
```

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

1. Start the backend API first (http://localhost:5000)
2. Start the frontend dev server (http://localhost:5173)
3. Open http://localhost:5173 in your browser
4. Click "Get Message from API" to fetch data from the backend

## API Endpoints

- `GET http://localhost:5000/api/hello` - Returns a hello world message

## Tech Stack

- **Backend**: .NET 10, ASP.NET Core Web API
- **Frontend**: Vue.js 3, Vite
- **Communication**: REST API with CORS enabled

