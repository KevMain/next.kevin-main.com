# start-dev.ps1
# Runs both the .NET API backend and Vue.js frontend in parallel

Write-Host "Starting Kevin Main Development Environment..." -ForegroundColor Green
Write-Host ""

# Store the root directory
$rootDir = $PSScriptRoot

# Function to cleanup processes on exit
function Stop-DevServers {
	Write-Host ""
	Write-Host "Shutting down development servers..." -ForegroundColor Yellow

	# Kill the background jobs
	Get-Job | Stop-Job
	Get-Job | Remove-Job

	Write-Host "Development servers stopped." -ForegroundColor Green
}

# Register cleanup on script exit
$null = Register-EngineEvent -SourceIdentifier PowerShell.Exiting -Action { Stop-DevServers }

# Check if npm is installed
try {
	$null = Get-Command npm -ErrorAction Stop
} catch {
	Write-Host "ERROR: npm is not installed or not in PATH" -ForegroundColor Red
	Write-Host "Please install Node.js from https://nodejs.org/" -ForegroundColor Yellow
	exit 1
}

# Check if frontend dependencies are installed
$frontendPath = Join-Path $rootDir "frontend"
$nodeModulesPath = Join-Path $frontendPath "node_modules"

if (-not (Test-Path $nodeModulesPath)) {
	Write-Host "Installing frontend dependencies..." -ForegroundColor Yellow
	Push-Location $frontendPath
	npm install
	Pop-Location
	Write-Host ""
}

# Start Backend API
Write-Host "Starting .NET API Backend (http://localhost:5000)..." -ForegroundColor Cyan
$backendPath = Join-Path $rootDir "KevinMain.API"
Start-Job -Name "Backend" -ScriptBlock {
	param($path)
	Set-Location $path
	dotnet run
} -ArgumentList $backendPath | Out-Null

# Wait a moment for backend to start
Start-Sleep -Seconds 2

# Start Frontend
Write-Host "Starting Vue.js Frontend (http://localhost:5173)..." -ForegroundColor Cyan
Start-Job -Name "Frontend" -ScriptBlock {
	param($path)
	Set-Location $path
	npm run dev
} -ArgumentList $frontendPath | Out-Null

# Wait a moment for frontend to start
Start-Sleep -Seconds 3

Write-Host ""
Write-Host "======================================" -ForegroundColor Green
Write-Host "  Development Environment Ready!" -ForegroundColor Green
Write-Host "======================================" -ForegroundColor Green
Write-Host ""
Write-Host "  Backend API:  http://localhost:5000" -ForegroundColor Cyan
Write-Host "  Frontend:     http://localhost:5173" -ForegroundColor Cyan
Write-Host ""
Write-Host "Press Ctrl+C to stop all servers" -ForegroundColor Yellow
Write-Host ""

# Monitor the jobs and display output
try {
	while ($true) {
		# Check if jobs are still running
		$jobs = Get-Job
		$runningJobs = $jobs | Where-Object { $_.State -eq "Running" }

		if ($runningJobs.Count -eq 0) {
			Write-Host "All servers have stopped." -ForegroundColor Red
			break
		}

		# Display output from jobs
		foreach ($job in $jobs) {
			$output = Receive-Job $job 2>&1
			if ($output) {
				$prefix = if ($job.Name -eq "Backend") { "[API]" } else { "[VUE]" }
				$color = if ($job.Name -eq "Backend") { "Blue" } else { "Magenta" }
				$output | ForEach-Object {
					Write-Host "$prefix $_" -ForegroundColor $color
				}
			}
		}

		Start-Sleep -Milliseconds 500
	}
}
catch {
	Write-Host "Interrupted by user." -ForegroundColor Yellow
}
finally {
	Stop-DevServers
}
