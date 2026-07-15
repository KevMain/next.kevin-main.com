# CI/CD Setup Guide - GitHub Actions to Azure

This guide shows you how to set up automated deployments from GitHub to Azure using GitHub Actions.

## 🎯 Overview

We'll set up two automated workflows:

1. **API Deployment** - Builds Docker image and deploys to Azure Container Apps
2. **Frontend Deployment** - Builds Vue.js app and deploys to Azure Static Web Apps (auto-configured)

## 📋 Prerequisites

- ✅ Azure resources created (SQL Database, Container Registry, Container Apps, Static Web Apps)
- ✅ GitHub repository: `https://github.com/KevMain/next.kevin-main.com`
- ✅ Azure CLI installed locally

---

## 🔐 Part 1: Create Azure Service Principal

A service principal allows GitHub Actions to authenticate with Azure.

### Step 1: Create Service Principal

```bash
# Login to Azure
az login

# Get your subscription ID
az account show --query id --output tsv

# Create service principal (replace <subscription-id> with your actual ID)
az ad sp create-for-rbac \
  --name "github-actions-kevinmain" \
  --role contributor \
  --scopes /subscriptions/<subscription-id>/resourceGroups/kevinmain-rg \
  --sdk-auth
```

### Step 2: Save the Output

The command will output JSON like this - **SAVE IT SECURELY:**

```json
{
  "clientId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "clientSecret": "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
  "subscriptionId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "tenantId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "activeDirectoryEndpointUrl": "https://login.microsoftonline.com",
  "resourceManagerEndpointUrl": "https://management.azure.com/",
  "activeDirectoryGraphResourceId": "https://graph.windows.net/",
  "sqlManagementEndpointUrl": "https://management.core.windows.net:8443/",
  "galleryEndpointUrl": "https://gallery.azure.com/",
  "managementEndpointUrl": "https://management.core.windows.net/"
}
```

---

## 🔑 Part 2: Configure GitHub Secrets

### Step 1: Go to GitHub Repository Settings

1. Navigate to: `https://github.com/KevMain/next.kevin-main.com/settings/secrets/actions`
2. Click **"New repository secret"**

### Step 2: Add Required Secrets

Add these secrets one by one:

| Secret Name | Value | Where to Get It |
|-------------|-------|-----------------|
| `AZURE_CREDENTIALS` | Entire JSON output from service principal creation | From Step 1 above |
| `ACR_USERNAME` | Container Registry username | `az acr credential show --name kevinmainacr --query username -o tsv` |
| `ACR_PASSWORD` | Container Registry password | `az acr credential show --name kevinmainacr --query passwords[0].value -o tsv` |
| `AZURE_SQL_CONNECTION_STRING` | SQL connection string | From Azure Portal or deployment guide |
| `SMTP_USERNAME` | Your email address | Your SMTP email |
| `SMTP_PASSWORD` | App-specific password | From your email provider |

### Get ACR Credentials:

```bash
# Username
az acr credential show --name kevinmainacr --query username -o tsv

# Password
az acr credential show --name kevinmainacr --query passwords[0].value -o tsv
```

---

## 🚀 Part 3: Configure API Deployment Workflow

The workflow file is already created at `.github/workflows/deploy-api.yml`.

### How it Works:

1. **Trigger**: Runs on push to `main` branch
2. **Build**: Compiles .NET API and runs tests
3. **Docker**: Builds Docker image
4. **Push**: Pushes image to Azure Container Registry
5. **Deploy**: Updates Azure Container App with new image

### Test the Workflow:

```bash
# Make a small change and push
git add .
git commit -m "Test CI/CD workflow"
git push origin main
```

Check progress at: `https://github.com/KevMain/next.kevin-main.com/actions`

---

## 🌐 Part 4: Configure Frontend Deployment

Azure Static Web Apps automatically creates a GitHub Actions workflow when you set it up.

### Step 1: Find the Workflow File

After creating your Static Web App in Azure, a workflow file was automatically added to your repo:

```
.github/workflows/azure-static-web-apps-<random-name>.yml
```

### Step 2: Verify Workflow Configuration

The file should look like this:

```yaml
name: Azure Static Web Apps CI/CD

on:
  push:
	branches:
	  - main
  pull_request:
	types: [opened, synchronize, reopened, closed]
	branches:
	  - main

jobs:
  build_and_deploy_job:
	if: github.event_name == 'push' || (github.event_name == 'pull_request' && github.event.action != 'closed')
	runs-on: ubuntu-latest
	name: Build and Deploy Job
	steps:
	  - uses: actions/checkout@v3
		with:
		  submodules: true
	  - name: Build And Deploy
		id: builddeploy
		uses: Azure/static-web-apps-deploy@v1
		with:
		  azure_static_web_apps_api_token: ${{ secrets.AZURE_STATIC_WEB_APPS_API_TOKEN_<ID> }}
		  repo_token: ${{ secrets.GITHUB_TOKEN }}
		  action: "upload"
		  app_location: "/frontend"
		  output_location: "dist"
```

### Step 3: Add Environment Variables

Update the workflow to include the API URL:

```yaml
	  - name: Build And Deploy
		id: builddeploy
		uses: Azure/static-web-apps-deploy@v1
		with:
		  azure_static_web_apps_api_token: ${{ secrets.AZURE_STATIC_WEB_APPS_API_TOKEN_<ID> }}
		  repo_token: ${{ secrets.GITHUB_TOKEN }}
		  action: "upload"
		  app_location: "/frontend"
		  output_location: "dist"
		env:
		  VITE_API_URL: https://kevinmain-api.<your-region>.azurecontainerapps.io
```

**Or** configure it in Azure Portal:
1. Go to Static Web Apps → Configuration → Environment variables
2. Add: `VITE_API_URL` = `https://your-api-url`

---

## 🔄 Part 5: Complete Deployment Pipeline

### Full Pipeline Flow:

```
Developer pushes to GitHub
		 ↓
GitHub Actions triggered
		 ↓
	┌─────────────────┐
	│   Build Jobs    │
	├─────────────────┤
	│ 1. API Build    │
	│ 2. Frontend     │
	└────────┬────────┘
			 ↓
	┌─────────────────┐
	│  Docker Build   │
	│  & Push to ACR  │
	└────────┬────────┘
			 ↓
	┌─────────────────┐
	│ Deploy to Azure │
	├─────────────────┤
	│ • Container App │
	│ • Static Web    │
	└────────┬────────┘
			 ↓
	┌─────────────────┐
	│ Production Live │
	└─────────────────┘
```

---

## ✅ Part 6: Verify Deployment

### Check API Deployment:

```bash
# View Container App logs
az containerapp logs show \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --follow

# Check API health
curl https://kevinmain-api.<region>.azurecontainerapps.io/api/cv/health
```

### Check Frontend Deployment:

1. Go to GitHub Actions: `https://github.com/KevMain/next.kevin-main.com/actions`
2. Check the workflow run status
3. Visit your Static Web App URL

### Test End-to-End:

1. Visit your site: `https://nice-hill-<id>.azurestaticapps.net`
2. Navigate to CV page - should load from database
3. Fill out contact form - should send email
4. Check all pages work

---

## 🐛 Part 7: Troubleshooting

### Workflow Fails on Build

**Error**: "No dockerfile found"
- Check Dockerfile is in repository root
- Verify workflow `working-directory` if needed

### Authentication Failed

**Error**: "Failed to login to Azure"
- Verify `AZURE_CREDENTIALS` secret is correct
- Check service principal has correct permissions
- Try recreating the service principal

### Docker Push Failed

**Error**: "unauthorized: authentication required"
- Check `ACR_USERNAME` and `ACR_PASSWORD` secrets
- Verify Container Registry admin user is enabled

```bash
# Enable admin user
az acr update --name kevinmainacr --admin-enabled true
```

### Container App Not Updating

**Error**: Deployment succeeds but changes not visible
- Container App might be caching
- Force revision restart:

```bash
az containerapp revision restart \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --revision <revision-name>
```

### Environment Variables Not Set

- Check Container App environment variables:

```bash
az containerapp show \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --query properties.template.containers[0].env
```

---

## 🎨 Part 8: Advanced Configuration

### Add Staging Environment

Create a separate workflow for staging:

```yaml
name: Deploy to Staging

on:
  push:
	branches: [ develop ]

# ... similar to deploy-api.yml but targeting staging resources
```

### Add Database Migrations

Add migration step to workflow:

```yaml
	- name: Run Database Migrations
	  env:
		CONNECTION_STRING: ${{ secrets.AZURE_SQL_CONNECTION_STRING }}
	  run: |
		cd KevinMain.API
		dotnet tool install --global dotnet-ef
		dotnet ef database update --connection "$CONNECTION_STRING"
```

### Add Smoke Tests

```yaml
	- name: Run Smoke Tests
	  run: |
		sleep 30  # Wait for deployment
		curl -f https://kevinmain-api.<region>.azurecontainerapps.io/api/cv/health || exit 1
```

### Slack Notifications

Add Slack notifications for deployment status:

```yaml
	- name: Notify Slack
	  if: always()
	  uses: 8398a7/action-slack@v3
	  with:
		status: ${{ job.status }}
		text: 'Deployment to Azure completed'
		webhook_url: ${{ secrets.SLACK_WEBHOOK }}
```

---

## 📊 Part 9: Monitor Deployments

### View Workflow Runs

```bash
# List recent workflow runs
gh run list --repo KevMain/next.kevin-main.com

# View specific run logs
gh run view <run-id> --log
```

### Azure Monitor

Set up Application Insights for monitoring:

```bash
# Create Application Insights
az monitor app-insights component create \
  --app kevinmain-insights \
  --location uksouth \
  --resource-group kevinmain-rg \
  --kind web

# Link to Container App
az containerapp update \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --set-env-vars \
	"APPLICATIONINSIGHTS_CONNECTION_STRING=<connection-string>"
```

---

## 🎯 Best Practices

### 1. **Branch Protection**
- Require pull request reviews before merging to `main`
- Run workflows on pull requests
- Block merges if workflows fail

### 2. **Environment Secrets**
- Use GitHub Environments for different stages (dev, staging, prod)
- Require manual approval for production deploys
- Rotate secrets regularly

### 3. **Version Tagging**
```bash
# Tag releases
git tag -a v1.0.0 -m "Release v1.0.0"
git push origin v1.0.0
```

### 4. **Rollback Strategy**
- Keep previous Docker images
- Azure Container Apps supports traffic splitting
- Can revert to previous revision quickly

### 5. **Cost Optimization**
- Scale down non-production environments
- Use consumption plans
- Set up budget alerts

---

## 🔐 Security Checklist

- [ ] Service principal has minimal required permissions
- [ ] All secrets stored in GitHub Secrets (not in code)
- [ ] ACR admin access disabled in production (use managed identities)
- [ ] SQL firewall rules properly configured
- [ ] HTTPS enforced on all endpoints
- [ ] Environment variables properly secured
- [ ] Container runs as non-root user
- [ ] Dependabot enabled for security updates

---

## 📚 Useful Resources

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Azure Container Apps](https://learn.microsoft.com/azure/container-apps/)
- [Azure Static Web Apps](https://learn.microsoft.com/azure/static-web-apps/)
- [Docker Best Practices](https://docs.docker.com/develop/dev-best-practices/)

---

## 🎉 You're All Set!

Your complete CI/CD pipeline is now configured! Every push to `main` will:

1. ✅ Build and test your code
2. ✅ Create Docker images
3. ✅ Deploy to Azure automatically
4. ✅ Update both frontend and backend

**Happy deploying! 🚀**
