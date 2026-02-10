# ?? MyAcademy MVC CodeFirst - Security Configuration Guide

## ?? IMPORTANT: Protecting Sensitive Information

This project uses **environment-specific configuration** to protect sensitive data like API keys and database connection strings.

---

## ?? Quick Setup (First Time)

### 1. **Create Your Local Web.config**

```bash
# Copy the example file
copy Web.config.example Web.config
```

### 2. **Configure Your Credentials**

Open `Web.config` and replace these values:

```xml
<!-- Replace with your actual Gemini API key -->
<add key="GeminiApiKey" value="YOUR_ACTUAL_API_KEY_HERE" />

<!-- Configure your database connection -->
<add name="AppDbContext" 
     connectionString="data Source=(localdb)\MSSQLLOCALDB; initial catalog=MyAcademyMVCCodeFirstDB; integrated security=true; trustServerCertificate=true" 
     providerName="System.Data.SqlClient" />
```

### 3. **Get Your Gemini API Key**

1. Visit: https://aistudio.google.com/app/apikey
2. Sign in with Google account
3. Create a new API key
4. Copy and paste into `Web.config`

---

## ?? Files in This Repository

| File | Description | Git Status |
|------|-------------|------------|
| `Web.config.example` | ? Template with placeholders | **Committed** |
| `Web.config` | ? Your actual config with secrets | **IGNORED** |
| `.gitignore` | ??? Prevents sensitive files from being committed | **Committed** |

---

## ?? Security Best Practices

### ? DO:
- Use `Web.config.example` as a template
- Keep your actual `Web.config` locally only
- Use environment variables in production
- Rotate API keys regularly
- Use Azure Key Vault for production

### ? DON'T:
- Commit `Web.config` with real credentials
- Share API keys in code or chat
- Use the same key for dev/prod
- Hard-code secrets in C# files

---

## ?? Production Deployment

### Option 1: Azure App Service (Recommended)

```bash
# Set environment variables in Azure Portal
az webapp config appsettings set --name your-app-name --resource-group your-rg --settings GeminiApiKey="YOUR_KEY"
```

### Option 2: Azure Key Vault

```csharp
// Install: Microsoft.Azure.KeyVault
var keyVaultClient = new KeyVaultClient(/* auth */);
var secret = await keyVaultClient.GetSecretAsync("https://your-vault.vault.azure.net/secrets/GeminiApiKey");
```

### Option 3: Encrypted Web.config

```powershell
# Run in Package Manager Console
aspnet_regiis -pef "appSettings" "C:\path\to\your\project"
```

---

## ?? Troubleshooting

### Problem: "API key not configured" error

**Solution:**
```csharp
// Check if Web.config exists and has the key
var apiKey = ConfigurationManager.AppSettings["GeminiApiKey"];
if (string.IsNullOrEmpty(apiKey))
{
    throw new InvalidOperationException("Gemini API key is not configured in Web.config!");
}
```

### Problem: Git keeps trying to commit Web.config

**Solution:**
```bash
# Remove from Git tracking (keeps local file)
git rm --cached Web.config

# Verify it's ignored
git status
```

---

## ?? Additional Resources

- [ASP.NET Configuration](https://docs.microsoft.com/en-us/aspnet/web-forms/overview/deployment/web-deployment-in-the-enterprise/configuring-parameters-for-web-package-deployment)
- [Azure Key Vault Documentation](https://docs.microsoft.com/en-us/azure/key-vault/)
- [Gemini API Quickstart](https://ai.google.dev/gemini-api/docs/quickstart)

---

## ?? Need Help?

If you encounter issues:

1. Check that `Web.config` exists (not just the .example file)
2. Verify your API key is valid
3. Ensure database connection string is correct
4. Check Application Insights logs in Azure (if deployed)

---

**Last Updated:** January 2025  
**Project Version:** 1.0.0
