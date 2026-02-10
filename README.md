# ?? MyAcademy MVC CodeFirst - Insurance Management System

[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-blue)](https://dotnet.microsoft.com/download/dotnet-framework/net48)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-6.0-green)](https://docs.microsoft.com/en-us/ef/ef6/)
[![License](https://img.shields.io/badge/license-MIT-orange.svg)](LICENSE)

Modern ASP.NET MVC uygulamasý ile geliþtirilmiþ sigorta yönetim sistemi. Google Gemini API destekli yapay zeka danýþmanlýk özelliði içerir.

---

## ?? Özellikler

### ?? Sigorta Yönetim Sistemi
- Hayat Sigortasý ürün yönetimi
- Kategori yönetimi
- Hizmet paketleri
- Slider yönetimi
- Blog sistemi

### ?? Yapay Zeka Sigorta Danýþmaný
- Google Gemini API ile kiþiselleþtirilmiþ öneriler
- Yaþ, gelir ve aile durumuna göre akýllý analiz
- Gerçek zamanlý sigorta tavsiyeleri

### ?? Yönetim Paneli
- Ürün CRUD iþlemleri
- Kategori yönetimi
- Blog yönetimi
- Ekip üyesi yönetimi
- Testimonial yönetimi
- Ýletiþim mesajlarý yönetimi

### ?? Modern Arayüz
- Responsive tasarým
- Bootstrap 5
- Font Awesome icons
- Owl Carousel
- Modern UI/UX

---

## ?? Kurulum

### Gereksinimler

- Visual Studio 2019/2022
- .NET Framework 4.8
- SQL Server (LocalDB veya Express)
- Google Gemini API Key ([Buradan alýn](https://aistudio.google.com/app/apikey))

### Adým Adým Kurulum

#### 1. **Projeyi Klonlayýn**
```bash
git clone https://github.com/emreokanbaskaya1/9-MyAcademy_MVC_CodeFirst.git
cd 9-MyAcademy_MVC_CodeFirst
```

#### 2. **Yapýlandýrma Dosyasýný Oluþturun**
```bash
# Örnek dosyayý kopyalayýn
cd 9-MyAcademy_MVC_CodeFirst
copy Web.config.example Web.config
```

#### 3. **API Key'inizi Ayarlayýn**

`Web.config` dosyasýný açýn ve güncelleyin:
```xml
<appSettings>
  <!-- Gerçek API key'inizi buraya yazýn -->
  <add key="GeminiApiKey" value="YOUR_ACTUAL_API_KEY_HERE" />
</appSettings>
```

#### 4. **Veritabanýný Oluþturun**
```powershell
# Package Manager Console'da
Update-Database
```

#### 5. **Uygulamayý Çalýþtýrýn**
- Visual Studio'da `F5` tuþuna basýn
- Tarayýcýda `https://localhost:your-port` adresine gidin

---

## ?? Güvenlik Yapýlandýrmasý

?? **ÖNEMLÝ:** `Web.config` dosyasýný gerçek verilerle birlikte asla commit etmeyin!

Detaylý güvenlik yapýlandýrmasý için: [SECURITY_SETUP.md](SECURITY_SETUP.md)

**Hýzlý Kontrol:**
- ? `Web.config` dosyasý `.gitignore` içinde
- ? `Web.config.example` þablon olarak kullanýlýyor
- ? API key'ler environment variables'da (production)
- ? Azure Key Vault kullanýmý (production)

---

## ?? Proje Yapýsý

```
9-MyAcademy_MVC_CodeFirst/
??? Areas/
?   ??? Admin/                    # Admin panel
?       ??? Controllers/          # Admin kontrolcüler
?       ??? Views/                # Admin görünümler
??? Controllers/                  # MVC kontrolcüler
?   ??? HomeController.cs         # Ana sayfa + AI danýþman
??? Data/
?   ??? Context/                  # DbContext
?   ?   ??? AppDbContext.cs
?   ??? Entities/                 # Entity modeller
?       ??? Category.cs
?       ??? Product.cs
?       ??? Feature.cs
?       ??? TeamMember.cs
?       ??? Testimonial.cs
?       ??? Blog.cs
?       ??? FAQ.cs
?       ??? Contact.cs
?       ??? Slider.cs
?       ??? About.cs
??? Migrations/                   # EF migrations
??? Models/                       # View modeller
?   ??? HomeViewModel.cs
??? Services/                     # Business logic
?   ??? GeminiService.cs          # AI servis
??? UITheme/                      # Frontend assets
?   ??? LifeSure-1.0.0/
??? Views/                        # Razor views
    ??? Home/
    ??? Shared/
```

---

## ??? Kullanýlan Teknolojiler

### Backend
- **ASP.NET MVC 5** - Web framework
- **Entity Framework 6** - ORM (Code First)
- **C# 7.3** - Programlama dili
- **SQL Server** - Veritabaný

### Frontend
- **Razor View Engine** - Template engine
- **Bootstrap 5** - CSS framework
- **jQuery 3.7** - JavaScript library
- **Owl Carousel** - Slider component
- **Font Awesome** - Ýkonlar

### API & Services
- **Google Gemini API** - Yapay zeka danýþman
- **Newtonsoft.Json** - JSON iþleme

---

## ?? Veritabaný Þemasý

```sql
-- Ana tablolar
Categories (Id, Name)
Products (Id, Name, Description, ImageUrl, Price, CategoryId, IsActive)
Features (Id, Icon, Title, Description)
TeamMembers (Id, Name, Position, ImageUrl, FacebookUrl, TwitterUrl, LinkedInUrl, InstagramUrl)
Testimonials (Id, ClientName, Position, Comment, Rating, ImageUrl)
Blogs (Id, Title, Description, ImageUrl, CategoryName, Author, PublishDate, CommentCount)
FAQs (Id, Question, Answer)
Contacts (Id, Name, Email, Subject, Message, CreatedDate, IsRead)
Sliders (Id, Title, Subtitle, Description, ImageUrl, ButtonText, ButtonUrl, DisplayOrder)
Abouts (Id, Title, Subtitle, Description, ImageUrl, InsurancePolicies, AwardsWon, SkilledAgents, TeamMembers)
```

---

## ?? Temel Özellikler ve Kullaným

### 1. **Ana Sayfa**
- Dinamik slider
- Özellikler bölümü
- Hizmetler
- Hakkýmýzda
- SSS (FAQ)
- Blog yazýlarý
- Ekip üyeleri
- Müþteri yorumlarý

### 2. **AI Sigorta Danýþmaný**
```csharp
// Kullaným örneði
var service = new GeminiService();
var advice = await service.GetInsuranceAdviceAsync(
    age: 30,
    job: "Yazýlým Geliþtirici",
    income: 15000,
    family: "Evli, 1 çocuk"
);
```

### 3. **Admin Panel**
- `/Admin/Product` - Ürün yönetimi
- `/Admin/Category` - Kategori yönetimi
- `/Admin/Blog` - Blog yönetimi
- `/Admin/TeamMember` - Ekip yönetimi
- `/Admin/Testimonial` - Yorum yönetimi
- `/Admin/Contact` - Ýletiþim mesajlarý
- `/Admin/FAQ` - SSS yönetimi
- `/Admin/Slider` - Slider yönetimi

---

## ?? Test

### Uygulamayý Çalýþtýrma
```bash
# Debug modunda
dotnet build --configuration Debug

# Release modunda
dotnet build --configuration Release
```

### API Testi
```csharp
// Gemini API entegrasyonunu test et
var service = new GeminiService();
var response = await service.GetInsuranceAdviceAsync(30, "Mühendis", 15000, "Bekar");
Console.WriteLine(response);
```

---

## ?? Deployment

### Azure App Service'e Daðýtým

1. **Visual Studio'dan Yayýnlama**
   - Projeye sað týk ? Publish
   - Azure App Service seçin
   - Ayarlarý yapýlandýrýn

2. **Environment Variables Ayarlama**
   ```bash
   az webapp config appsettings set \
     --name your-app-name \
     --resource-group your-rg \
     --settings GeminiApiKey="YOUR_KEY"
   ```

3. **Connection String Yapýlandýrma**
   - Azure SQL Database kullanýn
   - App Settings'de connection string güncelleyin

### IIS'e Daðýtým

1. Projeyi Publish edin (File System)
2. IIS Manager'da yeni site oluþturun
3. Application Pool: .NET 4.8
4. `Web.config` dosyasýný production deðerleriyle güncelleyin

---

## ?? Lisans

Bu proje MIT Lisansý altýnda lisanslanmýþtýr - Detaylar için [LICENSE](LICENSE) dosyasýna bakýn.

---

## ????? Geliþtirici

**Emre Okan Baþkaya**
- GitHub: [@emreokanbaskaya1](https://github.com/emreokanbaskaya1)
- Email: emreokanbaskaya@example.com

---

## ?? Katkýda Bulunma

Katkýlar memnuniyetle karþýlanýr! Lütfen Pull Request göndermekten çekinmeyin.

1. Projeyi fork edin
2. Feature branch oluþturun (`git checkout -b feature/AmazingFeature`)
3. Deðiþikliklerinizi commit edin (`git commit -m 'Add some AmazingFeature'`)
4. Branch'inizi push edin (`git push origin feature/AmazingFeature`)
5. Pull Request açýn

---

## ?? Destek

Destek için emreokanbaskaya@example.com adresine email gönderin veya GitHub repository'de issue açýn.

---

## ?? Teþekkürler

- [LifeSure Template](https://themewagon.com/themes/free-bootstrap-insurance-website-template-lifesure/) - UI Temasý
- [Google Gemini](https://ai.google.dev/gemini-api/docs) - AI API
- [Bootstrap](https://getbootstrap.com/) - CSS Framework
- [Entity Framework](https://docs.microsoft.com/en-us/ef/) - ORM
- [Font Awesome](https://fontawesome.com/) - Ýkonlar

---

## ?? Ekran Görüntüleri

### Ana Sayfa
![Ana Sayfa](docs/screenshots/home.png)

### AI Sigorta Danýþmaný
![AI Danýþman](docs/screenshots/ai-advisor.png)

### Admin Panel
![Admin Panel](docs/screenshots/admin.png)

---

## ?? Güncellemeler

### v1.0.0 (Ocak 2025)
- ? Ýlk sürüm yayýnlandý
- ? Google Gemini AI entegrasyonu
- ? Admin panel eklendi
- ? Güvenlik yapýlandýrmasý iyileþtirildi

---

**?? ASP.NET MVC ile geliþtirilmiþtir**