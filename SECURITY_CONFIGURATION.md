# 🔐 Security Configuration Guide

## ✅ Safe Credential Management

Your credentials are now properly secured! Here's how it works:

### Files in Source Control (Git)

✅ **`appsettings.json`** - Contains placeholder values only
- Safe to commit to Git
- Contains structure and examples
- `Enabled: false` by default

✅ **`.gitignore`** - Updated to ignore sensitive files
- Blocks `appsettings.Development.json` from Git
- Blocks `appsettings.*.local.json` files

### Files NOT in Source Control (Local Only)

🔒 **`KevinMain.API/appsettings.Development.json`** - Your real credentials
- **Never committed to Git** (now in .gitignore)
- Contains your actual Gmail/SMTP password
- Automatically loaded in Development environment
- Each developer has their own copy with their credentials

---

## 🚀 Quick Setup

### 1. Add Your Real Credentials

Edit `KevinMain.API/appsettings.Development.json` and replace the placeholder values:

```json
{
  "SmtpSettings": {
	"Enabled": true,
	"Username": "your-actual-email@gmail.com",
	"Password": "your-actual-app-password",
	"FromEmail": "your-actual-email@gmail.com"
  }
}
```

### 2. Get Gmail App Password

1. Enable 2-Step Verification: https://myaccount.google.com/security
2. Generate App Password: https://myaccount.google.com/apppasswords
3. Copy the 16-character password into `appsettings.Development.json`

### 3. Test It

```powershell
cd KevinMain.API
dotnet run --launch-profile http
```

Visit http://localhost:5173/contact and submit a test message!

---

## 🔄 How Configuration Loading Works

ASP.NET Core automatically merges configuration files in this order:

1. **`appsettings.json`** (base settings, committed to Git)
2. **`appsettings.{Environment}.json`** (environment-specific, NOT in Git)
   - In Development: `appsettings.Development.json` 
   - In Production: `appsettings.Production.json`

Settings in later files **override** earlier ones. So your Development file only needs to specify the values that change:

```json
{
  "SmtpSettings": {
	"Enabled": true,
	"Username": "real-value",
	"Password": "real-value",
	"FromEmail": "real-value"
  }
}
```

Everything else (Host, Port, ToEmail) inherits from `appsettings.json`.

---

## 📋 Git Status Check

To verify your credentials are safe:

```powershell
# Should show the file is ignored (not tracked)
git status KevinMain.API/appsettings.Development.json

# Should return nothing (file is ignored)
# If it shows "modified" or "new file", your credentials might be exposed!
```

✅ **Expected result:** File should NOT appear in `git status`

---

## 🌐 Production Deployment

For production (Azure, AWS, etc.), **never** commit credentials. Use:

### Option 1: Environment Variables (Recommended)

```bash
# Azure App Service / Linux
export SmtpSettings__Username="email@gmail.com"
export SmtpSettings__Password="app-password"
export SmtpSettings__Enabled="true"
```

### Option 2: Azure Key Vault

```csharp
// Program.cs
builder.Configuration.AddAzureKeyVault(
	new Uri($"https://{keyVaultName}.vault.azure.net/"),
	new DefaultAzureCredential());
```

### Option 3: User Secrets (Development Alternative)

```powershell
cd KevinMain.API
dotnet user-secrets set "SmtpSettings:Username" "your-email@gmail.com"
dotnet user-secrets set "SmtpSettings:Password" "your-app-password"
dotnet user-secrets set "SmtpSettings:Enabled" "true"
```

Secrets are stored in:
- Windows: `%APPDATA%\Microsoft\UserSecrets\`
- Linux/Mac: `~/.microsoft/usersecrets/`

---

## ⚠️ IMPORTANT: What to NEVER Commit

❌ Real email passwords  
❌ App-specific passwords  
❌ API keys  
❌ Connection strings with credentials  
❌ Any token or secret  

✅ Placeholder values are fine  
✅ Structure and examples are fine  
✅ Disabled/example configuration is fine  

---

## 🛡️ Security Best Practices

1. **Use App Passwords** - Never use your main Gmail password for SMTP
2. **Rotate credentials regularly** - Change app passwords every 90 days
3. **Limit scope** - Use dedicated email accounts for applications
4. **Monitor access** - Check Google Security activity regularly
5. **Use 2FA everywhere** - Protect your Google account with 2-Step Verification

---

## 📚 More Info

- Full SMTP setup guide: See `SMTP_SETUP_GUIDE.md`
- ASP.NET Core Configuration: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/
- Safe storage of app secrets: https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets

---

## ✅ Current Status

- ✅ `.gitignore` updated to exclude sensitive files
- ✅ `appsettings.json` contains safe placeholder values (committed)
- ✅ `appsettings.Development.json` removed from Git tracking
- ✅ Ready to add your real credentials locally
- ✅ Safe to push to GitHub!
