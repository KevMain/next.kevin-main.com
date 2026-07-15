# Custom Domain & SSL Setup Guide

This guide shows you how to configure a custom domain (e.g., kevin-main.com) for your Azure-hosted portfolio site.

## 🎯 Overview

You'll configure:
- **Frontend (Static Web Apps)**: `www.kevin-main.com` and `kevin-main.com`
- **API (Container Apps)**: `api.kevin-main.com`
- **Free SSL Certificates**: Automatically managed by Azure

---

## 📋 Prerequisites

- ✅ Domain name registered (e.g., from Namecheap, GoDaddy, Google Domains, etc.)
- ✅ Access to domain registrar's DNS management
- ✅ Azure Static Web App deployed
- ✅ Azure Container App deployed

---

## 🌐 Part 1: Configure Custom Domain for Frontend (Static Web Apps)

### Step 1: Get Static Web Apps Details

```bash
# Get your Static Web App default URL
az staticwebapp show \
  --name kevinmain-frontend \
  --resource-group kevinmain-rg \
  --query defaultHostname \
  --output tsv
```

Output example: `nice-hill-0abc123.1.azurestaticapps.net`

### Step 2: Add Custom Domain in Azure

```bash
# Add apex domain (kevin-main.com)
az staticwebapp hostname set \
  --name kevinmain-frontend \
  --resource-group kevinmain-rg \
  --hostname kevin-main.com

# Add www subdomain
az staticwebapp hostname set \
  --name kevinmain-frontend \
  --resource-group kevinmain-rg \
  --hostname www.kevin-main.com
```

### Step 3: Get Validation Details

After adding the custom domain, Azure will provide validation details:

```bash
# Get validation token
az staticwebapp hostname show \
  --name kevinmain-frontend \
  --resource-group kevinmain-rg \
  --hostname kevin-main.com
```

### Step 4: Configure DNS Records

Go to your domain registrar and add these DNS records:

#### For Apex Domain (kevin-main.com):

| Type | Name | Value | TTL |
|------|------|-------|-----|
| TXT | @ | `<validation-token>` | 3600 |
| ALIAS or CNAME | @ | `nice-hill-0abc123.1.azurestaticapps.net` | 3600 |

**Note**: If your registrar doesn't support ALIAS records for apex domain, use A records:

```bash
# Get Static Web App IP address
nslookup nice-hill-0abc123.1.azurestaticapps.net
```

Then add A record pointing to that IP.

#### For WWW Subdomain (www.kevin-main.com):

| Type | Name | Value | TTL |
|------|------|-------|-----|
| TXT | www | `<validation-token>` | 3600 |
| CNAME | www | `nice-hill-0abc123.1.azurestaticapps.net` | 3600 |

### Step 5: Validate Domain

Azure will automatically validate the domain once DNS propagates (usually 5-60 minutes).

Check status:

```bash
az staticwebapp hostname show \
  --name kevinmain-frontend \
  --resource-group kevinmain-rg \
  --hostname kevin-main.com
```

### Step 6: Enable HTTPS Redirect

```bash
# Update Static Web App to redirect HTTP to HTTPS
az staticwebapp update \
  --name kevinmain-frontend \
  --resource-group kevinmain-rg \
  --set properties.allowConfigFileUpdates=true
```

Add to `staticwebapp.config.json`:

```json
{
  "routes": [
	{
	  "route": "/*",
	  "allowedRoles": ["anonymous"]
	}
  ],
  "navigationFallback": {
	"rewrite": "/index.html"
  },
  "globalHeaders": {
	"Strict-Transport-Security": "max-age=31536000"
  }
}
```

---

## 🔧 Part 2: Configure Custom Domain for API (Container Apps)

### Step 1: Get Container App Details

```bash
# Get Container App FQDN
az containerapp show \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --query properties.configuration.ingress.fqdn \
  --output tsv
```

Output example: `kevinmain-api.niceriver-12345678.uksouth.azurecontainerapps.io`

### Step 2: Add Custom Domain

```bash
# Add custom domain to Container App
az containerapp hostname add \
  --hostname api.kevin-main.com \
  --resource-group kevinmain-rg \
  --name kevinmain-api
```

### Step 3: Get Validation Details

```bash
# Get CNAME and verification token
az containerapp hostname list \
  --resource-group kevinmain-rg \
  --name kevinmain-api
```

### Step 4: Configure DNS Records

Add these DNS records to your domain registrar:

#### For API Subdomain (api.kevin-main.com):

| Type | Name | Value | TTL |
|------|------|-------|-----|
| TXT | asuid.api | `<verification-token>` | 3600 |
| CNAME | api | `kevinmain-api.niceriver-12345678.uksouth.azurecontainerapps.io` | 3600 |

### Step 5: Bind SSL Certificate

Azure will automatically provision a free managed SSL certificate once domain is validated.

```bash
# Verify certificate
az containerapp hostname list \
  --resource-group kevinmain-rg \
  --name kevinmain-api \
  --output table
```

### Step 6: Update Frontend API URL

Update your Static Web App environment variable:

```bash
# Update via Azure Portal or Azure CLI
# Static Web Apps → Configuration → Environment variables
# VITE_API_URL = https://api.kevin-main.com
```

Or rebuild with updated `.env.production`:

```bash
# frontend/.env.production
VITE_API_URL=https://api.kevin-main.com
```

### Step 7: Update CORS Configuration

Update Container App to allow your custom domain:

```bash
az containerapp update \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --set-env-vars \
	"CorsOrigins__0=https://www.kevin-main.com" \
	"CorsOrigins__1=https://kevin-main.com"
```

---

## 🔐 Part 3: SSL Certificate Management

### Free Managed Certificates

Azure provides **free SSL certificates** automatically:

- ✅ **Automatic renewal** - Azure renews before expiration
- ✅ **Zero cost** - No certificate fees
- ✅ **Wildcard support** - For Container Apps
- ✅ **TLS 1.2+ only** - Modern secure protocols

### Certificate Status

Check certificate status:

```bash
# For Static Web Apps
az staticwebapp hostname show \
  --name kevinmain-frontend \
  --resource-group kevinmain-rg \
  --hostname kevin-main.com

# For Container Apps
az containerapp hostname list \
  --resource-group kevinmain-rg \
  --name kevinmain-api
```

### Custom Certificate (Advanced)

If you want to use your own certificate:

```bash
# Upload certificate to Container App
az containerapp ssl upload \
  --hostname api.kevin-main.com \
  --resource-group kevinmain-rg \
  --name kevinmain-api \
  --certificate-file /path/to/cert.pfx \
  --certificate-password <password>
```

---

## ✅ Part 4: DNS Configuration Examples

### Example 1: Namecheap

1. Login to Namecheap
2. Go to Domain List → Manage → Advanced DNS
3. Add records:

```
Type          Host          Value                                           TTL
----------------------------------------------------------------------------------
CNAME Record  www           nice-hill-0abc123.1.azurestaticapps.net        Automatic
CNAME Record  api           kevinmain-api.niceriver-12345678...             Automatic
TXT Record    @             <static-web-app-validation-token>               1 min
TXT Record    asuid.api     <container-app-verification-token>              1 min
```

4. For apex domain (kevin-main.com), add URL Redirect:
   - Source URL: `kevin-main.com`
   - Destination URL: `https://www.kevin-main.com`
   - Type: Permanent (301)

### Example 2: Cloudflare

1. Go to DNS settings
2. Add CNAME records (proxy disabled for Azure):

```
Type    Name    Target                                      Proxy    TTL
---------------------------------------------------------------------------
CNAME   www     nice-hill-0abc123.1.azurestaticapps.net    Off      Auto
CNAME   api     kevinmain-api.niceriver-12345678...        Off      Auto
TXT     @       <validation-token>                          -        Auto
TXT     asuid.api  <verification-token>                     -        Auto
```

**Important**: Turn off Cloudflare proxy (orange cloud) for Azure domains!

### Example 3: GoDaddy

1. Go to DNS Management
2. Add records:

```
Type    Name    Value                                           TTL
------------------------------------------------------------------------
CNAME   www     nice-hill-0abc123.1.azurestaticapps.net        1 Hour
CNAME   api     kevinmain-api.niceriver-12345678...            1 Hour
TXT     @       <validation-token>                              1 Hour
TXT     asuid.api  <verification-token>                        1 Hour
```

---

## 🧪 Part 5: Testing & Verification

### Test DNS Propagation

```bash
# Check if DNS has propagated
dig kevin-main.com
dig www.kevin-main.com
dig api.kevin-main.com

# Or use online tool
# https://dnschecker.org
```

### Test SSL Certificate

```bash
# Check SSL certificate
openssl s_client -connect kevin-main.com:443 -servername kevin-main.com

# Check SSL grade
# https://www.ssllabs.com/ssltest/
```

### Test Full Flow

1. **Visit site**: `https://www.kevin-main.com`
2. **Check HTTPS**: Verify padlock icon in browser
3. **Test API**: Visit `https://api.kevin-main.com/api/cv/health`
4. **Check frontend-to-API**: Open browser console, verify API calls succeed
5. **Test contact form**: Submit a test message

### Verify Redirects

```bash
# Test HTTP to HTTPS redirect
curl -I http://www.kevin-main.com

# Should return 301 or 308 to https://www.kevin-main.com

# Test apex to www redirect (if configured)
curl -I http://kevin-main.com

# Should redirect to https://www.kevin-main.com
```

---

## 🐛 Part 6: Troubleshooting

### DNS Not Propagating

**Issue**: Domain still showing old IP or not resolving

**Solutions**:
```bash
# Clear local DNS cache
# Windows
ipconfig /flushdns

# Mac
sudo dscacheutil -flushcache

# Check TTL - wait for TTL period to expire
dig kevin-main.com | grep TTL
```

### SSL Certificate Not Provisioning

**Issue**: "NET::ERR_CERT_COMMON_NAME_INVALID"

**Solutions**:
- Wait 15-30 minutes after DNS validation
- Verify TXT records are correct
- Check domain validation status in Azure Portal
- Remove and re-add custom domain

```bash
# Remove domain
az staticwebapp hostname delete \
  --name kevinmain-frontend \
  --resource-group kevinmain-rg \
  --hostname kevin-main.com

# Re-add after 5 minutes
az staticwebapp hostname set \
  --name kevinmain-frontend \
  --resource-group kevinmain-rg \
  --hostname kevin-main.com
```

### CORS Errors with Custom Domain

**Issue**: API calls fail with CORS errors

**Solution**: Update Container App CORS settings

```bash
# Verify current CORS settings
az containerapp show \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --query properties.template.containers[0].env

# Update with your custom domains
az containerapp update \
  --name kevinmain-api \
  --resource-group kevinmain-rg \
  --set-env-vars \
	"CorsOrigins__0=https://www.kevin-main.com" \
	"CorsOrigins__1=https://kevin-main.com"
```

### Mixed Content Warnings

**Issue**: Browser shows "insecure content" warnings

**Solution**: Ensure all resources use HTTPS

1. Check `config.js` uses HTTPS for API
2. Update any hardcoded HTTP URLs
3. Add CSP header in `staticwebapp.config.json`:

```json
{
  "globalHeaders": {
	"Content-Security-Policy": "upgrade-insecure-requests"
  }
}
```

---

## 📊 Part 7: Monitoring

### Set Up Custom Domain Monitoring

```bash
# Create alert for SSL expiration (Azure Monitor)
az monitor metrics alert create \
  --name ssl-expiry-alert \
  --resource-group kevinmain-rg \
  --scopes /subscriptions/<sub-id>/resourceGroups/kevinmain-rg \
  --condition "avg SSL Certificate Expiry < 30" \
  --description "SSL certificate expires in less than 30 days"
```

### Health Checks

Create a simple monitoring script:

```bash
#!/bin/bash
# monitor.sh

echo "Checking site health..."

# Check frontend
FRONTEND_STATUS=$(curl -o /dev/null -s -w "%{http_code}\n" https://www.kevin-main.com)
echo "Frontend: $FRONTEND_STATUS"

# Check API
API_STATUS=$(curl -o /dev/null -s -w "%{http_code}\n" https://api.kevin-main.com/api/cv/health)
echo "API: $API_STATUS"

# Check SSL
SSL_EXPIRY=$(echo | openssl s_client -servername www.kevin-main.com -connect www.kevin-main.com:443 2>/dev/null | openssl x509 -noout -dates | grep notAfter | cut -d= -f2)
echo "SSL Expires: $SSL_EXPIRY"
```

Run with cron:
```bash
# Run every hour
0 * * * * /path/to/monitor.sh >> /var/log/site-monitor.log 2>&1
```

---

## 💡 Best Practices

### 1. **Use WWW Subdomain as Primary**
- Easier CNAME configuration
- Better cookie isolation
- Redirect apex (kevin-main.com) to www

### 2. **Enable HSTS**
```json
{
  "globalHeaders": {
	"Strict-Transport-Security": "max-age=31536000; includeSubDomains; preload"
  }
}
```

### 3. **Monitor SSL Expiration**
- Azure auto-renews, but monitor anyway
- Set up alerts 30 days before expiry

### 4. **DNS TTL Strategy**
- Use low TTL (300) during migration
- Increase to 3600-86400 after stable

### 5. **Test Regularly**
- Weekly SSL checks
- Monthly DNS verification
- Test from multiple locations

---

## 🎯 Complete DNS Configuration

Final DNS setup for `kevin-main.com`:

```
Type          Host          Value                                           TTL
----------------------------------------------------------------------------------
A or ALIAS    @             <static-web-app-ip> or use redirect             3600
CNAME         www           nice-hill-0abc123.1.azurestaticapps.net        3600
CNAME         api           kevinmain-api.niceriver-12345678...             3600
TXT           @             <static-web-app-validation-token>               3600
TXT           asuid.api     <container-app-verification-token>              3600
```

---

## ✅ Final Checklist

- [ ] DNS records added and propagated
- [ ] Custom domains added in Azure
- [ ] Domain validation completed
- [ ] SSL certificates provisioned and active
- [ ] HTTPS working for all domains
- [ ] HTTP redirects to HTTPS
- [ ] Apex domain handled (redirect or ALIAS)
- [ ] Frontend environment variables updated
- [ ] CORS allows custom domains
- [ ] All API calls use custom API domain
- [ ] Contact form works
- [ ] Site loads fast (CDN working)
- [ ] SSL certificate monitoring set up

---

## 🎉 Congratulations!

Your site is now live on your custom domain with HTTPS!

**Your URLs**:
- Main site: `https://www.kevin-main.com`
- API: `https://api.kevin-main.com`
- SSL: ✅ Free managed certificate

**Share your site with the world! 🌍**
