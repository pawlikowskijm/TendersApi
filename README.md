# Tenders API

This project is a RESTful API template that integrates with the [Tenders Guru API](https://tenders.guru/pl/api), providing local access, filtering, sorting, and caching of public tender data.

⚠️ **Note:** This is not a complete production-ready application — it serves as a **template** or **starting point** for future development. Unit tests and full error handling are intentionally minimal.

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (optional for containerized deployment)
- Visual Studio 2022+ or any modern IDE

---

### Running the API (Locally)

1. Clone the repository:
   ```bash
   git clone https://github.com/pawlikowskijm/TendersApi.git
   cd TendersApi
   ```

2. Set up config files:
   Update `appsettings.json` or use environment variables if you wish to adjust API host, page limits, and cache times.

3. Run the API:
   ```bash
   dotnet run --project TendersApi
   ```

4. Use TendersApi.http file for local endpoint testing. Be aware of setting correct port in `TendersApi_HostAddress` variable.

---

## 🔧 Configuration

The application uses two primary configuration sections from `appsettings.{environment}.json`:

### `TendersGuruApiOptions`

```json
"TendersGuruApiOptions": {
  "Host": "https://tenders.guru",
  "TendersResource": "/api/pl/tenders",
  "TendersDataCacheInMinutes": 30
}
```

- `Host`: Base URL of the Tenders Guru API.
- `TendersResource`: Resource path for tenders.
- `TendersDataCacheInMinutes`: How long fetched tenders are cached in memory/distributed cache.

### `TendersQueryingOptions`

```json
"TendersQueryingOptions": {
  "MinPage": 1,
  "MaxPage": 100,
  "PageSize": 100
}
```

- `MinPage` / `MaxPage`: Defines which pages to pull from the external API.
- `PageSize`: Number of items per page in API responses.

---

## ✅ Features

- Caching via `IDistributedCache` (in-memory or distributed).
- Filtering by:
  - Tender ID
  - Supplier ID
  - Date range
  - Amount in EUR
- Sorting by:
  - Date
  - Amount in EUR
- Pagination
- Background refresh on app startup (via `IHostedService`)
- Extensible architecture using DI, MediatR, AutoMapper, and validators

---

## 🧪 Testing

Unit tests are **intentionally not included** in this template. The focus is on providing a clean structure and modular design so that test cases can be easily added based on actual implementation needs.

---

## 📁 Project Structure (Simplified)

```
TendersApi/
│
├── Application/             # Queries, Models, Interfaces
├── Infrastructure/          # API integration, cache services
├── Controllers/             # HTTP endpoints
├── Program.cs               # Entry point
├── appsettings.json         # Configuration
```

---

## 📌 Endpoints

# Endpoint: `GET /tenders`

Retrieves a list of public tenders from the local cache. This endpoint supports filtering, sorting, and pagination.

⚠️ **Note:** You have to know, that correct usage of this endpoint requires some warmup from fresh started API (max. 5 minutes) — it has to fetch data from source API which is not available at start of API service.

---

## 🔄 Request

```http
GET /tenders
```

### ✅ Query Parameters

| Name            | Type       | Description                                                                 |
|-----------------|------------|-----------------------------------------------------------------------------|
| `tenderId`      | `int?`     | Filters results to a specific tender ID.                                    |
| `supplierId`    | `int?`     | Filters tenders where the supplier with this ID is listed.                  |
| `minDate`       | `date?`    | Filters tenders from this date (inclusive). Format: `YYYY-MM-DD`.           |
| `maxDate`       | `date?`    | Filters tenders until this date (inclusive). Format: `YYYY-MM-DD`.          |
| `minAmountInEur`| `decimal?` | Filters tenders with an amount in EUR greater than or equal to this value.  |
| `maxAmountInEur`| `decimal?` | Filters tenders with an amount in EUR less than or equal to this value.     |
| `page`          | `int?`     | Returns the specified page, based on server-side pagination (default 1).    |
| `sort`          | `string?`  | Sorting format: `field direction`, e.g. `Date asc` or `AmountInEur desc`.   |

> 🧠 Both `field` and `direction` are optional, but if used, `direction` must be `asc` or `desc`.
> Supported fields: `Date`, `AmountInEur`

---

## 📦 Response

### 🟢 200 OK

Returns a paginated list of tenders matching the query parameters.

```json
{
  "page": 1,
  "totalPages": 10,
  "items": [
    {
      "id": 12345,
      "date": "2025-06-18",
      "title": "Construction services for school buildings",
      "description": "Framework contract for regional development support",
      "amountInEur": 95420.50,
      "suppliers": [
        {
          "id": 678,
          "name": "Supplier Ltd."
        }
      ]
    },
    ...
  ]
}
```

### 🔴 400 Bad Request

Returned when an invalid parameter is passed (e.g. unsupported sort direction or unknown field).

### 🔴 500 Internal Server Error

Returned when cached data is unavailable or another unexpected error occurs.

---

## 🧪 Examples

```http
GET /tenders

GET /tenders?tenderId=579782

GET /tenders?supplierId=15097

GET /tenders?minAmountInEur=10000&maxAmountInEur=50000&minDate=2021-01-01&maxDate=2022-01-01&page=1&sort=amountineur+desc
```

---

## 📌 Notes

- All data is cached and updated periodically in the background.
- This endpoint does **not call the external API directly** — it works entirely on the cached dataset.



---

## 📝 License

MIT — feel free to use, fork, and modify.