# Azure Deployment Guide - Kevin Main Portfolio

This guide will walk you through deploying your full-stack portfolio to Azure with a database backend.

## 🏗️ Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                         Internet                             │
└────────────────────┬────────────────────────────────────────┘
					 │
					 ▼
┌─────────────────────────────────────────────────────────────┐
│         Azure Static Web Apps (Frontend - FREE)              │
│  • Vue.js SPA                                                │
│  • Global CDN                                                │
│  • Auto SSL/TLS                                              │
│  • Custom domain support                                     │
└────────────────────┬────────────────────────────────────────┘
					 │ HTTPS
					 ▼
┌─────────────────────────────────────────────────────────────┐
│       Azure Container Apps (Backend - $15-30/mo)             │
│  • .NET 10 API                                               │
│  • Auto-scaling                                              │
│  • HTTPS endpoints                                           │
│  • Environment variables                                     │
└────────────────────┬────────────────────────────────────────┘
					 │ SQL Connection
					 ▼
┌─────────────────────────────────────────────────────────────┐
│    Azure SQL Database (CV Data - FREE or $5/mo)              │
│  • Structured CV data                                        │
│  • Automatic backups                                         │
│  • Point-in-time restore                                     │
│  • Query editor                                              │
└─────────────────────────────────────────────────────────────┘
```

## 💰 Cost Estimate

| Service | Tier | Monthly Cost |
|---------|------|--------------|
| Azure Static Web Apps | Free | **$0** |
| Azure Container Apps | Consumption | **$15-30** |
| Azure SQL Database | Free/Basic | **$0-$5** |
| **TOTAL** | | **$15-35/month** |

## 📋 Prerequisites

Before you begin, ensure you have:

- ✅ Azure account ([Create free account](https://azure.microsoft.com/free/))
- ✅ Azure CLI installed ([Download](https://docs.microsoft.com/cli/azure/install-azure-cli))
- ✅ GitHub account (for CI/CD)
- ✅ Docker Desktop (for local testing - optional)
- ✅ Your GitHub repository pushed to `https://github.com/KevMain/next.kevin-main.com`

## 🚀 Part 1: Azure SQL Database Setup

### Step 1: Create Resource Group

```bash
# Login to Azure
az login

# Create resource group
az group create \
  --name kevinmain-rg \
  --location uksouth
```

### Step 2: Create SQL Server

```bash
# Create SQL Server (replace <your-password> with a strong password)
az sql server create \
  --name kevinmain-sql \
  --resource-group kevinmain-rg \
  --location uksouth \
  --admin-user sqladmin \
  --admin-password <YourStrongPassword123!>
```

### Step 3: Configure Firewall

```bash
# Allow Azure services
az sql server firewall-rule create \
  --resource-group kevinmain-rg \
  --server kevinmain-sql \
  --name AllowAzureServices \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0

# Allow your IP (for local management)
az sql server firewall-rule create \
  --resource-group kevinmain-rg \
  --server kevinmain-sql \
  --name AllowMyIP \
  --start-ip-address <YourIPAddress> \
  --end-ip-address <YourIPAddress>
```

### Step 4: Create Database

**Option A: FREE Tier (32MB, 100 DTUs/month)**
```bash
az sql db create \
  --resource-group kevinmain-rg \
  --server kevinmain-sql \
  --name kevinmain-db \
  --edition GeneralPurpose \
  --compute-model Serverless \
  --family Gen5 \
  --capacity 1 \
  --auto-pause-delay 60 \
  --use-free-limit true
```

**Option B: Basic Tier ($5/month, 2GB)**
```bash
az sql db create \
  --resource-group kevinmain-rg \
  --server kevinmain-sql \
  --name kevinmain-db \
  --service-objective Basic
```

### Step 5: Get Connection String

```bash
# Get connection string
az sql db show-connection-string \
  --client ado.net \
  --name kevinmain-db \
  --server kevinmain-sql
```

**Save this connection string - you'll need it later!**

Example output:
```
Server=tcp:kevinmain-sql.database.windows.net,1433;Database=kevinmain-db;User ID=<username>;Password=<password>;Encrypt=true;Connection Timeout=30;
```

---

## 🐳 Part 2: Azure Container Registry & Container Apps

### Step 1: Create Container Registry

```bash
# Create Azure Container Registry
az acr create \
  --resource-group kevinmain-rg \
  --name kevinmainacr \
  --sku Basic \
  --admin-enabled true
```

### Step 2: Build and Push Docker Image

```bash
# Login to ACR
az acr login --name kevinmainacr

# Build and push image
az acr build \
  --registry kevinmainacr \
  --image kevinmain-api:latest \
  --file Dockerfile \
  .
```

### Step 3: Create Container Apps Environment

```bash
# Create Container Apps environment
az containerapp env create \
  --name kevinmain-env \
  --resource-group kevinmain-rg \
  --location uksouth
```

### Step 4: Get ACR Credentials

```bash
# Get ACR credentials
az acr credential show --name kevinmainacr
```

Save the `username` and one of the `passwords`.

### Step 5: Deploy Container App

```bash
# Deploy the API
az containerapp create \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --environment kevinmain-env \
  --image kevinmainacr.azurecr.io/kevinmain-api:latest \
  --target-port 8080 \
  --ingress external \
  --registry-server kevinmainacr.azurecr.io \
  --registry-username <ACR_USERNAME> \
  --registry-password <ACR_PASSWORD> \
  --cpu 0.5 \
  --memory 1.0Gi \
  --min-replicas 0 \
  --max-replicas 2
```

### Step 6: Configure Environment Variables

```bash
# Set connection string (replace with your actual connection string)
az containerapp secret set \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --secrets "dbconnection=<YOUR_SQL_CONNECTION_STRING>"

# Set environment variables
az containerapp update \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --set-env-vars \
	"ConnectionStrings__DefaultConnection=secretref:dbconnection" \
	"SmtpSettings__Enabled=true" \
	"SmtpSettings__Host=smtp.gmail.com" \
	"SmtpSettings__Port=587" \
	"SmtpSettings__UseSsl=true" \
	"SmtpSettings__FromName=Kevin Main Portfolio" \
	"SmtpSettings__ToEmail=kevin.main@live.co.uk" \
	"DataSource__UseDatabase=true" \
	"DataSource__Provider=SqlServer"

# Set SMTP secrets (replace with your credentials)
az containerapp secret set \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --secrets \
	"smtpuser=<YOUR_EMAIL>" \
	"smtppass=<YOUR_APP_PASSWORD>"

az containerapp update \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --set-env-vars \
	"SmtpSettings__Username=secretref:smtpuser" \
	"SmtpSettings__Password=secretref:smtppass" \
	"SmtpSettings__FromEmail=secretref:smtpuser"
```

### Step 7: Get API URL

```bash
# Get the API URL
az containerapp show \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --query properties.configuration.ingress.fqdn \
  --output tsv
```

**Save this URL** - you'll use it as `VITE_API_URL` for the frontend.

Example: `kevinmain-api.niceriver-12345678.uksouth.azurecontainerapps.io`

---

## 🌐 Part 3: Azure Static Web Apps (Frontend)

### Option A: Deploy via Azure Portal (Recommended for first time)

1. **Go to Azure Portal** → Create a resource → Search "Static Web Apps"

2. **Configure basics:**
   - Subscription: Your subscription
   - Resource Group: `kevinmain-rg`
   - Name: `kevinmain-frontend`
   - Plan type: Free
   - Region: UK South
   - Source: GitHub

3. **Configure build:**
   - Organization: Your GitHub username
   - Repository: `next.kevin-main.com`
   - Branch: `main`
   - Build Presets: Custom
   - App location: `/frontend`
   - Output location: `dist`

4. Click **Review + Create** → **Create**

5. **Configure environment variable:**
   - Go to your Static Web App → Settings → Environment variables
   - Add: `VITE_API_URL` = `https://kevinmain-api.niceriver-12345678.uksouth.azurecontainerapps.io`

6. **Update CORS in Container App:**
```bash
# Get your Static Web Apps URL from the portal (e.g., https://nice-hill-0abc123.1.azurestaticapps.net)

# Update API environment to allow your frontend
az containerapp update \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --set-env-vars \
	"CorsOrigins__0=https://<your-static-web-app-url>"
```

### Option B: Deploy via Azure CLI

```bash
# This will be set up automatically with GitHub Actions
# See the generated workflow in .github/workflows/azure-static-web-apps-*.yml
```

---

## 🗄️ Part 4: Initialize Database

### Option 1: Using Azure Data Studio (Recommended)

1. Download [Azure Data Studio](https://docs.microsoft.com/sql/azure-data-studio/download)
2. Connect using your SQL Server connection string
3. Run the migration scripts (we'll create these next)

### Option 2: Using Azure Portal Query Editor

1. Go to Azure Portal → SQL databases → `kevinmain-db`
2. Click "Query editor"
3. Login with your SQL credentials
4. Run the migration scripts

### Migration Scripts Coming Next!

We'll create Entity Framework Core migrations in the next steps to:
- Create CV tables
- Seed initial data
- Set up indexes

---

## ✅ Verification Checklist

After deployment, verify:

- [ ] SQL Database is running and accessible
- [ ] Container App API is running (visit `https://<api-url>/api/cv/health`)
- [ ] Static Web App is deployed (visit your site URL)
- [ ] Frontend can call backend API
- [ ] CORS is properly configured
- [ ] Contact form works
- [ ] CV data loads from database

---

## 🔧 Troubleshooting

### API not responding
```bash
# Check container logs
az containerapp logs show \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --follow
```

### CORS errors
- Verify `CorsOrigins__0` environment variable in Container App
- Check browser console for the exact CORS error
- Ensure frontend uses HTTPS URL for API

### Database connection failed
- Verify firewall rules allow Azure services
- Check connection string is correct
- Ensure secrets are properly configured

### Frontend build failed
- Check GitHub Actions workflow logs
- Verify `VITE_API_URL` environment variable is set
- Ensure `frontend` app location is correct

---

## 📊 Monitoring & Management

### View Container App Metrics
```bash
az containerapp show \
  --name kevinmain-api \
  --resource-group kevinmain-rg
```

### View SQL Database Metrics
```bash
az sql db show \
  --resource-group kevinmain-rg \
  --server kevinmain-sql \
  --name kevinmain-db
```

### Scale Container App
```bash
# Scale up
az containerapp update \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --min-replicas 1 \
  --max-replicas 5
```

---

## 🎯 Next Steps

1. ✅ Complete database migration (see `DATABASE-SETUP.md`)
2. ✅ Set up custom domain (see `CUSTOM-DOMAIN.md`)
3. ✅ Configure GitHub Actions for automatic deployment
4. ✅ Set up Application Insights for monitoring (optional)
5. ✅ Create admin API for CV updates (optional)

---

## 💡 Useful Commands

```bash
# Restart API
az containerapp revision restart \
  --name kevinmain-api \
  --resource-group kevinmain-rg

# View environment variables
az containerapp show \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --query properties.template.containers[0].env

# Delete everything (cleanup)
az group delete --name kevinmain-rg --yes
```

---

**🎉 Congratulations!** Your portfolio is now running on Azure!

Visit your site and share it with the world! 🌍
