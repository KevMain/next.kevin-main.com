# Azure Deployment - Quick Start Summary

**Complete guide to deploying Kevin Main's portfolio to Azure with database backend.**

---

## 🎯 What You're Building

A production-ready full-stack portfolio site with:
- ✅ Vue.js frontend (Static Web Apps)
- ✅ .NET 10 API (Container Apps)
- ✅ SQL Database for CV data
- ✅ Email contact form (SMTP)
- ✅ Custom domain support
- ✅ Free SSL certificates
- ✅ Automatic deployments (CI/CD)

---

## 💰 Monthly Cost Breakdown

| Service | Tier | Cost/Month |
|---------|------|------------|
| **Azure Static Web Apps** | Free | **$0** |
| **Azure Container Apps** | Consumption (0.5 vCPU, 1GB RAM) | **$15-30** |
| **Azure SQL Database** | Free tier or Basic | **$0-5** |
| **Azure Container Registry** | Basic | **$5** |
| **Bandwidth** | First 100GB free | **$0** |
| **Total Estimated** | | **$20-40/month** |

### 💡 Cost Optimization Tips:
- Use Azure SQL **Free tier** (32MB) → **Save $5/month**
- Scale Container Apps to 0 replicas when idle → **Save ~$10/month**
- Use Static Web Apps free tier → **Already free!**

**Optimized Cost: As low as $10-15/month!**

---

## 📚 Documentation Index

| Document | Purpose |
|----------|---------|
| **AZURE-DEPLOYMENT.md** | Complete step-by-step Azure setup |
| **DATABASE-SETUP.md** | Entity Framework, migrations, seeding |
| **CI-CD-SETUP.md** | GitHub Actions automation |
| **CUSTOM-DOMAIN.md** | Custom domain & SSL configuration |
| **THIS FILE** | Quick reference and summary |

---

## 🚀 Quick Start (30 Minutes)

### Prerequisites
```bash
# Install Azure CLI
winget install Microsoft.AzureCLI

# Login
az login

# Install Docker Desktop (optional, for local testing)
# Download from: https://www.docker.com/products/docker-desktop
```

### Step 1: Azure Resources (10 min)

```bash
# Create resource group
az group create --name kevinmain-rg --location uksouth

# Create SQL Server & Database (FREE tier!)
az sql server create \
  --name kevinmain-sql \
  --resource-group kevinmain-rg \
  --location uksouth \
  --admin-user sqladmin \
  --admin-password <YourPassword123!>

az sql db create \
  --resource-group kevinmain-rg \
  --server kevinmain-sql \
  --name kevinmain-db \
  --edition GeneralPurpose \
  --compute-model Serverless \
  --family Gen5 \
  --capacity 1 \
  --use-free-limit true

# Create Container Registry
az acr create \
  --resource-group kevinmain-rg \
  --name kevinmainacr \
  --sku Basic \
  --admin-enabled true

# Create Container Apps environment
az containerapp env create \
  --name kevinmain-env \
  --resource-group kevinmain-rg \
  --location uksouth
```

### Step 2: Deploy API (10 min)

```bash
# Build and push Docker image
az acr build \
  --registry kevinmainacr \
  --image kevinmain-api:latest \
  --file Dockerfile \
  .

# Get ACR credentials
ACR_USER=$(az acr credential show --name kevinmainacr --query username -o tsv)
ACR_PASS=$(az acr credential show --name kevinmainacr --query passwords[0].value -o tsv)

# Deploy Container App
az containerapp create \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --environment kevinmain-env \
  --image kevinmainacr.azurecr.io/kevinmain-api:latest \
  --target-port 8080 \
  --ingress external \
  --registry-server kevinmainacr.azurecr.io \
  --registry-username $ACR_USER \
  --registry-password $ACR_PASS \
  --cpu 0.5 \
  --memory 1.0Gi \
  --min-replicas 0 \
  --max-replicas 2

# Get API URL
az containerapp show \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --query properties.configuration.ingress.fqdn \
  --output tsv
```

### Step 3: Deploy Frontend (5 min)

**Option A: Via Azure Portal (Recommended for first time)**

1. Go to [Azure Portal](https://portal.azure.com)
2. Create "Static Web App"
   - Name: `kevinmain-frontend`
   - Region: UK South
   - Source: GitHub
   - Repository: `KevMain/next.kevin-main.com`
   - Branch: `main`
   - App location: `/frontend`
   - Output location: `dist`

3. After creation:
   - Go to Configuration → Environment variables
   - Add: `VITE_API_URL` = `https://<your-api-url>`

**Option B: Via Azure CLI**

```bash
# Install Static Web Apps CLI extension
az extension add --name staticwebapp

# Create Static Web App with GitHub integration
az staticwebapp create \
  --name kevinmain-frontend \
  --resource-group kevinmain-rg \
  --location uksouth \
  --source https://github.com/KevMain/next.kevin-main.com \
  --branch main \
  --app-location "/frontend" \
  --output-location "dist"
```

### Step 4: Configure Secrets (5 min)

```bash
# Get SQL connection string
SQL_CONN="Server=tcp:kevinmain-sql.database.windows.net,1433;Database=kevinmain-db;User ID=sqladmin;Password=<YourPassword>;Encrypt=true;"

# Set Container App secrets
az containerapp secret set \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --secrets \
	"dbconnection=$SQL_CONN" \
	"smtpuser=<your-email>" \
	"smtppass=<your-app-password>"

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
	"SmtpSettings__Username=secretref:smtpuser" \
	"SmtpSettings__Password=secretref:smtppass" \
	"SmtpSettings__FromEmail=secretref:smtpuser" \
	"SmtpSettings__FromName=Kevin Main Portfolio" \
	"SmtpSettings__ToEmail=kevin.main@live.co.uk" \
	"DataSource__UseDatabase=true" \
	"DataSource__Provider=SqlServer"
```

---

## ✅ Verification

```bash
# Test API
curl https://<your-api-url>/api/cv/health

# Should return: {"status":"healthy"}

# Test frontend
# Visit your Static Web App URL in browser
# Check CV page loads
# Test contact form
```

---

## 📁 Project Structure

```
next.kevin-main.com/
├── KevinMain.API/                 # .NET API Backend
│   ├── Controllers/
│   ├── Services/                  # CV & Contact services
│   ├── Data/                      # EF Core DbContext & Models
│   ├── Models/                    # API models
│   ├── appsettings.json           # Base config
│   └── appsettings.Production.json # Production config
├── frontend/                      # Vue.js Frontend
│   ├── src/
│   │   ├── views/                 # Home, CV, Projects, Contact
│   │   ├── components/            # Shared components
│   │   └── config.js              # API URL configuration
│   ├── .env.development          # Local env vars
│   ├── .env.production           # Production env vars
│   └── staticwebapp.config.json  # Azure SWA config
├── Dockerfile                     # Container image definition
├── .dockerignore                  # Docker build exclusions
├── .github/
│   └── workflows/
│       └── deploy-api.yml         # CI/CD workflow
└── Documentation/
	├── AZURE-DEPLOYMENT.md        # Full Azure guide
	├── DATABASE-SETUP.md          # EF Core & SQL setup
	├── CI-CD-SETUP.md            # GitHub Actions guide
	├── CUSTOM-DOMAIN.md          # Domain & SSL setup
	└── DEPLOYMENT-SUMMARY.md     # This file
```

---

## 🔄 CI/CD Pipeline

Once set up, every push to `main` branch automatically:

1. ✅ Builds .NET API
2. ✅ Runs tests
3. ✅ Creates Docker image
4. ✅ Pushes to Azure Container Registry
5. ✅ Deploys to Container Apps
6. ✅ Builds Vue.js frontend
7. ✅ Deploys to Static Web Apps

**Setup Guide**: See `CI-CD-SETUP.md`

---

## 🌐 Custom Domain Setup

To use your own domain (e.g., kevin-main.com):

1. **Add custom domain in Azure Portal**
2. **Update DNS records at your registrar:**
   ```
   CNAME  www     → <static-web-app-url>
   CNAME  api     → <container-app-url>
   TXT    @       → <validation-token>
   TXT    asuid.api → <verification-token>
   ```
3. **Wait for validation** (5-60 minutes)
4. **Azure auto-provisions SSL** (free!)

**Detailed Guide**: See `CUSTOM-DOMAIN.md`

---

## 🗄️ Database Setup

To add Entity Framework Core and database support:

```bash
# Add EF Core packages
cd KevinMain.API
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design

# Create initial migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update

# Or deploy to Azure SQL
dotnet ef migrations script --output migration.sql
# Then run migration.sql in Azure Portal Query Editor
```

**Complete Guide**: See `DATABASE-SETUP.md`

---

## 🐛 Common Issues & Solutions

### API not responding
```bash
# Check logs
az containerapp logs show \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --follow
```

### Frontend shows CORS errors
- Verify `VITE_API_URL` environment variable
- Check Container App CORS settings allow your domain

### Database connection failed
- Verify SQL firewall allows Azure services
- Check connection string is correct
- Ensure database is running

### Docker build fails
- Check Dockerfile syntax
- Ensure .dockerignore doesn't exclude needed files
- Verify .NET 10 SDK is available in build image

---

## 📊 Cost Management

### View Current Costs

```bash
# Get cost analysis
az consumption usage list \
  --start-date $(date -d "30 days ago" +%Y-%m-%d) \
  --end-date $(date +%Y-%m-%d) \
  --resource-group kevinmain-rg
```

### Set Budget Alert

```bash
# Create budget alert at $50/month
az consumption budget create \
  --budget-name monthly-budget \
  --amount 50 \
  --time-grain Monthly \
  --start-date $(date +%Y-%m-01) \
  --end-date 2025-12-31 \
  --resource-group kevinmain-rg
```

### Cost Optimization

1. **Scale to zero**: Container Apps scale to 0 when idle
   ```bash
   az containerapp update \
	 --name kevinmain-api \
	 --resource-group kevinmain-rg \
	 --min-replicas 0
   ```

2. **Use SQL free tier**: 32MB storage, 100 DTU/month free

3. **Clean up unused resources**:
   ```bash
   # List all resources
   az resource list --resource-group kevinmain-rg --output table

   # Delete unused resources
   az resource delete --ids <resource-id>
   ```

---

## 🔐 Security Checklist

- [ ] SQL firewall configured (no public access)
- [ ] Secrets stored in Key Vault or Container App secrets
- [ ] HTTPS enforced on all endpoints
- [ ] CORS restricted to your domains only
- [ ] Container runs as non-root user
- [ ] Application Insights logging enabled
- [ ] Regular security updates via Dependabot
- [ ] Strong SQL admin password
- [ ] API rate limiting enabled

---

## 📈 Monitoring & Logging

### Enable Application Insights

```bash
# Create Application Insights
az monitor app-insights component create \
  --app kevinmain-insights \
  --location uksouth \
  --resource-group kevinmain-rg

# Get connection string
INSIGHTS_CONN=$(az monitor app-insights component show \
  --app kevinmain-insights \
  --resource-group kevinmain-rg \
  --query connectionString -o tsv)

# Link to Container App
az containerapp update \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --set-env-vars \
	"APPLICATIONINSIGHTS_CONNECTION_STRING=$INSIGHTS_CONN"
```

### View Logs

```bash
# Real-time API logs
az containerapp logs show \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --follow

# SQL query performance
az sql db show-query-stats \
  --resource-group kevinmain-rg \
  --server kevinmain-sql \
  --database kevinmain-db
```

---

## 🎯 Next Steps After Deployment

1. ✅ **Test thoroughly** - All pages, contact form, CV loading
2. ✅ **Set up custom domain** - See `CUSTOM-DOMAIN.md`
3. ✅ **Configure CI/CD** - See `CI-CD-SETUP.md`
4. ✅ **Add monitoring** - Application Insights
5. ✅ **Enable backups** - SQL automatic backups
6. ✅ **Set up alerts** - Budget, availability, errors
7. ✅ **Review costs** - Monitor first month usage
8. ⭐ **Share your site!**

---

## 🆘 Getting Help

### Documentation
- [Azure Static Web Apps Docs](https://learn.microsoft.com/azure/static-web-apps/)
- [Azure Container Apps Docs](https://learn.microsoft.com/azure/container-apps/)
- [Azure SQL Database Docs](https://learn.microsoft.com/azure/sql-database/)

### Troubleshooting
- Check logs: `az containerapp logs show`
- Verify environment variables
- Test API directly: `curl` commands
- Review GitHub Actions workflow logs

### Support
- Azure Portal chat support
- Stack Overflow: [azure] tag
- GitHub Discussions
- Azure Community Forums

---

## 🎉 Your Site is Live!

**Congratulations!** You've deployed a production-ready portfolio site with:

- ✅ Modern cloud architecture
- ✅ Automatic scaling
- ✅ Database backend
- ✅ CI/CD automation
- ✅ Free SSL certificates
- ✅ Global CDN
- ✅ Professional hosting

**Default URLs:**
- Frontend: `https://<your-app>.azurestaticapps.net`
- API: `https://kevinmain-api.<region>.azurecontainerapps.io`

**Total time to deploy**: ~30-45 minutes  
**Monthly cost**: $15-40 (or as low as $10 with optimization)

---

## 📝 Maintenance Tasks

### Weekly
- [ ] Check site is loading correctly
- [ ] Test contact form
- [ ] Review logs for errors

### Monthly
- [ ] Review costs in Azure Portal
- [ ] Check for .NET/npm package updates
- [ ] Verify SSL certificates are valid
- [ ] Review Application Insights metrics

### Quarterly
- [ ] Update CV data in database
- [ ] Review and optimize costs
- [ ] Security audit
- [ ] Performance testing

---

**Ready to go live? Start with Step 1 above! 🚀**

For detailed instructions, see:
- **AZURE-DEPLOYMENT.md** - Complete setup guide
- **DATABASE-SETUP.md** - Database configuration
- **CI-CD-SETUP.md** - Automation setup
- **CUSTOM-DOMAIN.md** - Custom domain & SSL
