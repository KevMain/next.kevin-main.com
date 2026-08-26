# Kevin Main - Portfolio Website

A modern, professional portfolio website built with Vue.js and .NET 10.

## Live Site

- **Website**: https://lemon-flower-011704103.7.azurestaticapps.net
- **API**: https://kevinmain-api.kindpond-04a3181a.ukwest.azurecontainerapps.io

## Tech Stack

- **Frontend**: Vue.js 3 + Vite
- **Backend**: .NET 10 / ASP.NET Core
- **Hosting**: Azure Static Web Apps + Azure Container Apps

## Development

### Quick Start

Run both frontend and API together:

```powershell
.\start-dev.ps1
```

This will start:
- Frontend: https://localhost:5173
- API: https://localhost:5158

### Manual Start

**Backend:**
```bash
cd KevinMain.API
dotnet run
```

**Frontend:**
```bash
cd frontend
npm install
npm run dev
```

## Configuration

Local settings are in `KevinMain.API/appsettings.Development.json` (gitignored).

## Blog Post Storage

Blog posts are stored in **Azure Table Storage** (table `blogposts`, single partition `blog`, RowKey = slug) and served through a `HybridCache` decorator. Posts change infrequently: found posts are cached for `BlogCache:PostDuration`, the published list for `BlogCache:PublishedListDuration`, and not-found lookups are negative-cached for `BlogCache:NotFoundDuration`.

### Local development

Local development uses [Azurite](https://learn.microsoft.com/azure/storage/common/storage-use-azurite) via `BlogStorage:ConnectionString` = `UseDevelopmentStorage=true` (see `appsettings.Development.json`). The table is created automatically at startup in Development only.

Start Azurite before running the API:

```powershell
azurite --silent
```

### Production deployment prerequisites

Production authenticates with a **system-assigned Managed Identity** — no storage secret is stored in configuration. The following must be provisioned by deployment/IaC (the application does **not** create the table outside Development):

1. Storage account with a `blogposts` table, e.g.:
   ```powershell
   az storage table create --name blogposts --account-name <account>
   ```
2. Container App system-assigned identity enabled.
3. Role assignment: grant the identity **Storage Table Data Contributor** on the storage account.
4. App configuration: set `BlogStorage:ServiceUri` to `https://<account>.table.core.windows.net`.

Configuration is validated at startup (`ValidateOnStart`): a missing `ServiceUri` (or `ConnectionString` in Development) fails fast with a clear error rather than at first request.

> **Future-proofing:** the code constructs `new ManagedIdentityCredential()`, which assumes a *system-assigned* identity. If the deployment ever switches to a *user-assigned* identity, add its client ID to configuration (e.g. `BlogStorage:ManagedIdentityClientId`) and pass it to `ManagedIdentityCredential(clientId)` in `Program.cs`.

## Tests

```powershell
dotnet test
```

Unit tests always run. Azurite integration tests are **explicitly opt-in** — they are skipped unless the environment variable is set, and once enabled, a broken Azurite setup is a test *failure* (no silent skipping):

```powershell
$env:RUN_AZURITE_TESTS = "true"
azurite --silent
dotnet test
```

## Features

- Professional CV/Resume
- Project showcase
- Running page with Strava integration
- Contact form
- Responsive design
## 📧 Contact

Questions? Use the contact form on the live site or reach out via GitHub.

