# 🚀 Production Deployment - Kevin Main Portfolio

## ✅ Live URLs

Your site is **live and deployed** on Azure! 🎉

- **Website**: https://lemon-flower-011704103.7.azurestaticapps.net
- **API**: https://kevinmain-api.kindpond-04a3181a.ukwest.azurecontainerapps.io

## 🏗️ What's Deployed

### Frontend (Azure Static Web Apps - FREE)
- Vue.js SPA
- Global CDN
- Automatic HTTPS/SSL
- Auto-deploy on Git push

### Backend (Azure Container Apps - $15-30/mo)
- .NET 10 API in Docker container
- Auto-scaling (scales to zero when idle)
- HTTPS endpoints
- Auto-deploy on Git push

### Data Storage (Currently In-Memory - FREE)
- CV data served from InMemoryCVDataService
- Future: Will migrate to Azure SQL Database

## 💰 Current Monthly Cost

| Service | Cost |
|---------|------|
| Frontend (Static Web Apps) | **$0** (Free tier) |
| Backend (Container Apps) | **$15-30** (scales to zero) |
| Container Registry | **$5** (Basic tier) |
| **TOTAL** | **$20-35/month** |

## 🔄 How Deployment Works

### Automatic CI/CD via GitHub Actions

Every push to `main` branch triggers:

1. **Backend Deployment** (`.github/workflows/deploy-api.yml`):
   - Build .NET project
   - Run tests
   - Build Docker image
   - Push to Azure Container Registry
   - Deploy to Container Apps
   - Takes ~3-5 minutes

2. **Frontend Deployment** (auto-generated workflow):
   - Build Vue.js app
   - Deploy to Static Web Apps
   - Takes ~2-3 minutes

**To deploy changes**: Just `git push origin main` - that's it! 🚀

```bash
# Step 1: Create Azure resources (10 min)
# Step 2: Deploy API (10 min)
# Step 3: Deploy Frontend (5 min)
# Step 4: Configure secrets (5 min)
```

### 3. Set Up Database (Optional - 20 min)

Follow `DATABASE-SETUP.md` to:
- Add Entity Framework Core
- Create database models

## 📚 Available Documentation

- **README.md** - Local development setup
- **README-DEPLOY.md** (this file) - Production deployment info
- **HTTPS-SETUP.md** - Local HTTPS configuration
- **DATABASE-SETUP.md** - Future database migration guide
- **CI-CD-SETUP.md** - GitHub Actions configuration details
- **CUSTOM-DOMAIN.md** - Custom domain setup

## 🔧 Managing Your Deployment

### View Logs

```powershell
# Backend API logs
az containerapp logs show --name kevinmain-api --resource-group kevinmain-rg --follow

# Check deployment status
az containerapp show --name kevinmain-api --resource-group kevinmain-rg
```

### Update Configuration

Backend environment variables are set in the Container App. To update:

```powershell
az containerapp update `
  --name kevinmain-api `
  --resource-group kevinmain-rg `
  --set-env-vars "KEY=value"
```

### Manual Redeploy

Usually not needed (auto-deploys on push), but to manually trigger:

```powershell
# Trigger GitHub Actions
git commit --allow-empty -m "Trigger deployment"
git push origin main
```

## 🎯 Next Steps (Optional)

### 1. Custom Domain

See `CUSTOM-DOMAIN.md` to set up:
- `www.kevin-main.com` → Frontend
- `api.kevin-main.com` → Backend API

### 2. Add Database

When Azure SQL capacity is available, follow `DATABASE-SETUP.md` to migrate from in-memory to database-backed CV data.

### 3. Configure SMTP

Add email credentials to Container App environment variables for working contact form:

```powershell
az containerapp update `
  --name kevinmain-api `
  --resource-group kevinmain-rg `
  --set-env-vars `
	"SmtpSettings__Enabled=true" `
	"SmtpSettings__Username=your-email@gmail.com" `
	"SmtpSettings__Password=your-app-password"
```

## 🆘 Troubleshooting

### Site Not Loading

1. Check GitHub Actions - any failed deployments?
2. Check Container App logs (command above)
3. Verify CORS settings include your frontend URL

### API Errors

```powershell
# Check container status
az containerapp show --name kevinmain-api --resource-group kevinmain-rg

# Restart container
az containerapp revision restart --name kevinmain-api --resource-group kevinmain-rg
```

### Frontend Not Updating

- Static Web Apps deployment takes 2-3 minutes
- Check GitHub Actions for build status
- Hard refresh browser (Ctrl+Shift+R)

## 💡 Cost Optimization Tips

1. **Container Apps**: Set `--min-replicas 0` to scale to zero when idle
2. **Monitor costs**: Set up budget alerts in Azure Portal
3. **Use free tiers**: Static Web Apps and SQL Database have free options

## 🎉 Success!

Your portfolio is live and automatically deploying. Every push to `main` updates your site!

**Happy coding!** 🚀
- ✅ Responsive design
- ✅ Profile photo
- ✅ Code-themed headers
- ✅ Environment-based configuration

### Backend API
- ✅ .NET 10 Web API
- ✅ CV data endpoint
- ✅ Contact form endpoint
- ✅ SMTP email support (MailKit)
- ✅ Service-based architecture
- ✅ Database support ready
- ✅ Health check endpoints
- ✅ CORS configured

### Infrastructure
- ✅ Docker containerization
- ✅ Azure-ready configuration
- ✅ Environment variables support
- ✅ Production settings
- ✅ Security headers
- ✅ HTTPS enforced

### DevOps
- ✅ GitHub Actions workflow
- ✅ Automated builds
- ✅ Automated deployments
- ✅ Container registry integration

---

## 🔧 Local Development Still Works!

All your local development commands still work:

```bash
# Start both services
.\start-dev.ps1

# Or manually:
cd KevinMain.API
dotnet run

cd frontend
npm run dev
```

Visit: `https://localhost:5173`

---

## 🚦 Deployment Checklist

### Before Deploying
- [ ] Azure account created
- [ ] Azure CLI installed
- [ ] GitHub repository up to date
- [ ] SMTP credentials ready (for contact form)
- [ ] Review cost estimates

### Deployment Steps
- [ ] Create Azure resource group
- [ ] Create SQL Server & Database
- [ ] Create Container Registry
- [ ] Create Container Apps environment
- [ ] Build and push Docker image
- [ ] Deploy Container App
- [ ] Create Static Web App
- [ ] Configure environment variables
- [ ] Set up secrets

### Post-Deployment
- [ ] Test API endpoints
- [ ] Test frontend loads
- [ ] Test contact form
- [ ] Verify CV data loads
- [ ] Check CORS working
- [ ] Monitor first few days

### Optional
- [ ] Set up database (Entity Framework)
- [ ] Configure CI/CD (GitHub Actions)
- [ ] Add custom domain
- [ ] Enable Application Insights
- [ ] Set up budget alerts

---

## 🐛 Troubleshooting Quick Links

| Problem | Solution Document | Section |
|---------|-------------------|---------|
| API not responding | DEPLOYMENT-SUMMARY.md | Common Issues |
| CORS errors | AZURE-DEPLOYMENT.md | Troubleshooting |
| Database connection | DATABASE-SETUP.md | Testing |
| DNS not working | CUSTOM-DOMAIN.md | DNS Propagation |
| SSL certificate | CUSTOM-DOMAIN.md | SSL Certificate |
| GitHub Actions | CI-CD-SETUP.md | Troubleshooting |
| High costs | DEPLOYMENT-SUMMARY.md | Cost Management |

---

## 💡 Pro Tips

1. **Start Simple**: Deploy without database first, add it later
2. **Use Free Tiers**: Azure has generous free tiers for small sites
3. **Monitor Costs**: Set up budget alert from day one
4. **Test Locally First**: Use Docker to test the container locally
5. **Keep Secrets Safe**: Never commit secrets to Git
6. **Read the Logs**: Container App logs are your friend
7. **Scale Smart**: Start with 0 min replicas to save costs

---

## 📊 What You'll Have

After following the guides, your live site will have:

- ✅ **Public URL**: `https://<your-app>.azurestaticapps.net`
- ✅ **Custom domain**: `https://www.kevin-main.com` (optional)
- ✅ **API endpoint**: `https://kevinmain-api.<region>.azurecontainerapps.io`
- ✅ **Database**: Azure SQL with CV data
- ✅ **Email**: Working contact form
- ✅ **SSL**: Free managed certificates
- ✅ **CDN**: Global content delivery
- ✅ **Auto-deploy**: Push to GitHub = live in minutes
- ✅ **Monitoring**: Application Insights (optional)
- ✅ **Backups**: Automatic database backups

---

## 🎓 Learning Resources

- [.NET on Azure](https://learn.microsoft.com/aspnet/core/host-and-deploy/azure-apps/)
- [Vue.js Production Deployment](https://vuejs.org/guide/best-practices/production-deployment.html)
- [Docker Best Practices](https://docs.docker.com/develop/dev-best-practices/)
- [Azure Architecture Center](https://learn.microsoft.com/azure/architecture/)

---

## 🆘 Need Help?

1. **Check the docs**: Start with DEPLOYMENT-SUMMARY.md
2. **Review logs**: `az containerapp logs show ...`
3. **Test endpoints**: Use curl or Postman
4. **Azure support**: Portal has chat support
5. **Community**: Stack Overflow with [azure] tag

---

## 🎉 Ready to Deploy!

Everything is prepared and ready to go. Your next step is:

1. **Read**: `DEPLOYMENT-SUMMARY.md` - Quick Start section
2. **Run**: The Azure CLI commands to create resources
3. **Deploy**: Follow the step-by-step guide
4. **Test**: Verify everything works
5. **Share**: Tell the world about your site! 🌍

---

## 📝 Notes

- All documentation uses `kevinmain-*` naming convention
- UK South region is used (change if needed)
- SMTP configured for Gmail (change if using different provider)
- Database models match your current CV structure
- CI/CD assumes main branch for production

---

**Time to deploy**: 30-45 minutes  
**Difficulty**: Intermediate  
**Monthly cost**: $15-40  
**Result**: Professional portfolio site on Azure! 🚀

---

**Good luck with your deployment! You've got this! 💪**

Questions? Check the detailed docs or reach out for help.

