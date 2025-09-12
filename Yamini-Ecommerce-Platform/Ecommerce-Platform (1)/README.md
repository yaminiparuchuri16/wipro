
# Ecommerce-Platform

Folders:
- backend/ : ASP.NET Core API (SQLite, JWT, Swagger)
- frontend/: React + Vite + Redux Toolkit client
- docs/    : API docs, user manual, architecture diagram
- tests/   : Testing notes (Vitest + Testing Library; NUnit scaffolding)
- deployment/: Docker & CI/CD scaffolding

Quick Start (Windows/Mac/Linux)

## 1) Backend
```
cd backend/Ecommerce.Api
dotnet restore
dotnet run
```
API at http://localhost:5000 with Swagger at /swagger

## 2) Frontend
```
cd frontend
npm install
npm run dev
```
App at http://localhost:5173

Login accounts (seeded):
- Admin: admin@shop.com / Admin@123
- User : user@shop.com / User@123

## Notes
- SQLite DB file `ecommerce.db` is created automatically.
- Payment is mocked.
- For admin endpoints, login as admin and include the JWT.
