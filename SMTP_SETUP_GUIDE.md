# SMTP Email Configuration Guide

## Quick Start (Gmail Example)

### 1. Enable SMTP in appsettings.json

```json
"SmtpSettings": {
  "Enabled": true,
  "Host": "smtp.gmail.com",
  "Port": 587,
  "UseSsl": true,
  "Username": "your-email@gmail.com",
  "Password": "your-app-password-here",
  "FromEmail": "your-email@gmail.com",
  "FromName": "Kevin Main Portfolio",
  "ToEmail": "kevin.main@live.co.uk"
}
```

### 2. Get a Gmail App Password

**Important:** Gmail requires an App Password (not your regular password) for SMTP access.

#### Steps to get a Gmail App Password:

1. Enable 2-Step Verification on your Google Account:
   - Go to https://myaccount.google.com/security
   - Click "2-Step Verification" and follow the setup

2. Generate an App Password:
   - Go to https://myaccount.google.com/apppasswords
   - Select "Mail" and your device
   - Click "Generate"
   - Copy the 16-character password (format: xxxx xxxx xxxx xxxx)

3. Use the App Password in appsettings.json:
   ```json
   "Password": "abcd efgh ijkl mnop"
   ```
   (You can include or omit the spaces - both work)

### 3. Start the Backend

```powershell
cd KevinMain.API
dotnet run --launch-profile http
```

### 4. Test the Contact Form

1. Visit http://localhost:5173/contact
2. Fill in the form:
   - Name: Test User
   - Email: test@example.com
   - Subject: Test Contact Form
   - Message: This is a test message from the new SMTP implementation.
3. Click "Send Message"
4. Check your inbox at kevin.main@live.co.uk

## Alternative SMTP Providers

### Outlook / Office365

```json
"SmtpSettings": {
  "Enabled": true,
  "Host": "smtp-mail.outlook.com",
  "Port": 587,
  "UseSsl": true,
  "Username": "your-email@outlook.com",
  "Password": "your-password",
  "FromEmail": "your-email@outlook.com",
  "FromName": "Kevin Main Portfolio",
  "ToEmail": "kevin.main@live.co.uk"
}
```

**Note:** Outlook/Office365 typically works with your regular account password (no app password needed).

### Custom SMTP Server

```json
"SmtpSettings": {
  "Enabled": true,
  "Host": "mail.your-domain.com",
  "Port": 587,
  "UseSsl": true,
  "Username": "your-username",
  "Password": "your-password",
  "FromEmail": "noreply@your-domain.com",
  "FromName": "Kevin Main Portfolio",
  "ToEmail": "kevin.main@live.co.uk"
}
```

## Toggle Between SMTP and Logging

To switch back to logging-only mode (no real emails):

```json
"SmtpSettings": {
  "Enabled": false,
  ...
}
```

The application automatically uses:
- **SmtpContactService** when `Enabled: true` → Sends real emails
- **LoggingContactService** when `Enabled: false` → Only logs to console

No code changes required!

## Troubleshooting

### "Authentication failed"
- Double-check your username and password
- For Gmail: Make sure you're using an App Password, not your Google account password
- Verify 2-Step Verification is enabled on your Google account

### "Connection refused" or timeout
- Check firewall/antivirus settings
- Verify the Host and Port are correct
- Try Port 465 with SSL instead of 587 with TLS:
  ```json
  "Port": 465,
  "UseSsl": true
  ```

### "Emails not arriving"
- Check spam/junk folders
- Verify the ToEmail address is correct
- Check backend logs for any error messages
- Test with a different email provider

### Frontend shows "Failed to send message"
- Check browser console (F12) for detailed error info
- Check backend console logs for SMTP connection details
- Verify CORS is working (backend should show the OPTIONS preflight request)

## Security Best Practices

### 1. Use Environment Variables (Production)

**Never commit real credentials to Git!**

For production, use environment variables or Azure Key Vault:

```json
"SmtpSettings": {
  "Enabled": true,
  "Host": "smtp.gmail.com",
  "Port": 587,
  "UseSsl": true,
  "Username": "${SMTP_USERNAME}",
  "Password": "${SMTP_PASSWORD}",
  "FromEmail": "${SMTP_FROM_EMAIL}",
  "FromName": "Kevin Main Portfolio",
  "ToEmail": "kevin.main@live.co.uk"
}
```

Set environment variables:
```powershell
$env:SMTP_USERNAME="your-email@gmail.com"
$env:SMTP_PASSWORD="your-app-password"
$env:SMTP_FROM_EMAIL="your-email@gmail.com"
```

### 2. Use User Secrets (Development)

```powershell
cd KevinMain.API
dotnet user-secrets init
dotnet user-secrets set "SmtpSettings:Username" "your-email@gmail.com"
dotnet user-secrets set "SmtpSettings:Password" "your-app-password"
dotnet user-secrets set "SmtpSettings:FromEmail" "your-email@gmail.com"
```

### 3. Add appsettings.Development.json to .gitignore

Create `KevinMain.API/appsettings.Development.json` with your real credentials:

```json
{
  "SmtpSettings": {
	"Enabled": true,
	"Username": "your-real-email@gmail.com",
	"Password": "your-real-app-password",
	"FromEmail": "your-real-email@gmail.com"
  }
}
```

Then add to `.gitignore`:
```
**/appsettings.Development.json
```

## Email Features

The emails sent include:

✅ **Plain text version** (for email clients that don't support HTML)  
✅ **Styled HTML version** with your brand gradient colors  
✅ **Contact details** (name, email, subject, timestamp)  
✅ **Full message** from the contact form  
✅ **Reply-to link** for easy responses  
✅ **Professional formatting** matching your site's design

## Next Steps

1. Configure your SMTP credentials in `appsettings.json`
2. Set `Enabled: true`
3. Test the contact form
4. Once working, move credentials to user secrets or environment variables
5. Deploy to production with secure credential management

## Testing Checklist

- [ ] SMTP credentials configured
- [ ] Backend build successful
- [ ] Backend running on http://localhost:5000
- [ ] Frontend running on http://localhost:5173
- [ ] Contact form submits without errors
- [ ] Email received at ToEmail address
- [ ] Email formatting looks good (check both HTML and plain text)
- [ ] Reply-to link works correctly
- [ ] Error handling works (try with Enabled: false)
- [ ] Toggle back to LoggingContactService works
