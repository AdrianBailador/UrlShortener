# 🔗 UrlShortener

A simple web application to shorten URLs, built with **ASP.NET Core 8** and **Entity Framework Core**. This project serves as a clean and minimal base for learning about routing, input validation, database access, and REST API development.

## ✨ Features

- Shorten long URLs into reusable short codes.
- Automatic redirection via short code.
- Input validation for safe and valid URLs.
- Persistent storage using EF Core and SQLite.
- Minimal API architecture with clean, maintainable code.

## 🚀 Tech Stack

- [.NET 8](https://dotnet.microsoft.com/)
- [ASP.NET Core Minimal APIs](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [SQLite](https://www.sqlite.org/index.html)

## 📦 Getting Started

1. **Clone the repository**

```bash
git clone https://github.com/AdrianBailador/UrlShortener.git
cd UrlShortener
````

2. **Restore dependencies**

```bash
dotnet restore
```

3. **Apply database migrations**

```bash
dotnet ef database update
```

4. **Run the application**

```bash
dotnet run
```

The API will be available at `https://localhost:5001` or `http://localhost:5000`.

## 📚 API Endpoints

| Method | Route          | Description                        |
| ------ | -------------- | ---------------------------------- |
| POST   | `/shorten`     | Shortens a given URL               |
| GET    | `/{shortCode}` | Redirects to the original long URL |

### Example – POST `/shorten`

```json
POST /shorten
Content-Type: application/json

{
  "url": "https://example.com"
}
```

**Response:**

```json
{
  "shortUrl": "https://localhost:5001/abc123"
}
```

## 🗂 Project Structure

```
UrlShortener/
├── Models/             # Data models
├── Data/               # EF Core DbContext
├── Program.cs          # App configuration and routes
└── README.md
```

## ✅ Potential Improvements

* Frontend UI for generating short links.
* Expiry dates for shortened URLs.
* Click statistics tracking.
* Authentication for private URLs.

## 📄 License

This project is licensed under the [MIT License](LICENSE).

---

> Developed by [Adrián Bailador]([https://github.com/AdrianBailador](https://adrianbailador.github.io/) 🇮🇪

---

