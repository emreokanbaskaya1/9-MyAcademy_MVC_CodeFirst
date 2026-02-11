# LifeSure — Insurance Management Platform

[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-512BD4)](https://dotnet.microsoft.com/download/dotnet-framework/net48)
[![ASP.NET MVC](https://img.shields.io/badge/ASP.NET%20MVC-5.2-0078D4)](https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-6.5-68217A)](https://docs.microsoft.com/en-us/ef/ef6/)
[![Gemini AI](https://img.shields.io/badge/Google%20Gemini-2.0%20Flash-4285F4)](https://ai.google.dev/)
[![HuggingFace](https://img.shields.io/badge/HuggingFace-BART--MNLI-FFD21E)](https://huggingface.co/)

A full-featured insurance management system built with ASP.NET MVC 5 and Entity Framework 6 (Code First). The platform includes a public-facing insurance website, a comprehensive admin panel with analytics dashboards, and multiple AI-powered features: a **Google Gemini**-driven insurance advisor, **Hugging Face** zero-shot message classification, **Tavily** real-time web search integration, and a custom **Linear Regression** revenue forecasting engine.

---

## Screenshots

| Public Website | Admin Dashboard |
|:-:|:-:|
| ![Home](docs/screenshots/home.png) | ![Dashboard](docs/screenshots/admin.png) |

| AI Insurance Advisor | Revenue Forecast |
|:-:|:-:|
| ![AI Advisor](docs/screenshots/ai-advisor.png) | ![Forecast](docs/screenshots/forecast.png) |

---

## Key Features

### Public Website
- **Dynamic Carousel** — Admin-managed homepage sliders
- **Services Catalog** — Insurance products with categories and pricing
- **About Section** — Company stats with animated counters
- **Blog System** — Articles with author, date, and category
- **Team Page** — Team members with social media links
- **Testimonials** — Client reviews with star ratings in an Owl Carousel
- **FAQ Accordion** — Frequently asked questions
- **Contact Form** — With email notification support (MailKit)
- **AI Insurance Advisor** — Personalized recommendations via Google Gemini

### Admin Panel (Tailwind CSS + Dark Mode)
- Full **CRUD** for all content entities (Products, Categories, Blogs, Sliders, Team Members, Testimonials, FAQs, About, Contact Info)
- **Policy Sales Management** — Create, edit, track policy sales with customer info, payment status, and date-based auto-expiration
- **Contact Messages** — View incoming messages with AI-powered auto-classification (Hugging Face) and auto-reply generation (Gemini)
- **Web Search** — Tavily-powered real-time web search from within the admin panel
- **Analytics Dashboard** — Interactive Chart.js visualizations:
  - Monthly sales trend (line chart)
  - Top 5 products by sales (horizontal bar)
  - Payment status distribution (doughnut)
  - Policy status distribution (doughnut)
  - Recent sales table
- **AI Revenue Forecast** — Next month's revenue & sales count prediction using OLS Linear Regression with 95% confidence intervals, R² score, and trend indicators

### AI & ML Integrations

| Service | Purpose | Provider |
|---------|---------|----------|
| **Insurance Advisor** | Personalized policy recommendations based on age, income, occupation, family status | Google Gemini 2.0 Flash |
| **Contact Auto-Reply** | AI-generated professional responses to customer messages | Google Gemini 2.0 Flash |
| **Message Classification** | Zero-shot categorization of contact messages (complaint, inquiry, urgent, etc.) | Hugging Face BART-MNLI |
| **Real-time Search** | Current market data & pricing enrichment for AI responses | Tavily Search API |
| **Revenue Forecast** | Next-month revenue prediction with linear regression (OLS) | Custom implementation |

---

## Tech Stack

### Backend
| Technology | Version | Usage |
|-----------|---------|-------|
| ASP.NET MVC | 5.2.9 | Web framework |
| Entity Framework | 6.5.1 | ORM (Code First approach) |
| C# | 7.3 | Language |
| .NET Framework | 4.8 | Runtime |
| MailKit | 3.6.0 | SMTP email sending |
| Newtonsoft.Json | 13.0.3 | JSON serialization |

### Frontend
| Technology | Usage |
|-----------|-------|
| Razor View Engine | Server-side templating |
| Bootstrap 5 | Public website styling |
| Tailwind CSS (CDN) | Admin panel styling with dark mode |
| Chart.js 4.4 | Dashboard visualizations |
| jQuery 3.7 | DOM manipulation |
| Owl Carousel | Testimonial/slider carousels |
| Font Awesome | Icon library |
| Material Icons | Admin panel icons |

### AI & External APIs
| API | Model/Version | Usage |
|-----|--------------|-------|
| Google Gemini | 2.0 Flash | Insurance advice & auto-replies |
| Hugging Face | facebook/bart-large-mnli | Message zero-shot classification |
| Tavily Search | Advanced | Real-time web data enrichment |

---

## Admin Panel Routes

| Route | Description |
|-------|-------------|
| `/Admin/Dashboard` | Analytics dashboard with charts and AI forecast |
| `/Admin/PolicySale` | Policy sales management (CRUD) |
| `/Admin/Product` | Insurance products |
| `/Admin/Category` | Product categories |
| `/Admin/Blog` | Blog articles |
| `/Admin/Slider` | Homepage carousel sliders |
| `/Admin/Feature` | Homepage features section |
| `/Admin/TeamMember` | Team members |
| `/Admin/Testimonial` | Client testimonials |
| `/Admin/FAQ` | Frequently asked questions |
| `/Admin/About` | About section content |
| `/Admin/Contact` | Contact info settings |
| `/Admin/ContactMessage` | Customer messages (with AI classification) |
| `/Admin/Search` | Tavily web search tool |

---

## Developer

**Emre Okan Baskaya**
- GitHub: [@emreokanbaskaya1](https://github.com/emreokanbaskaya1)

---

## License

This project is licensed under the MIT License.

---
