
# API Endpoints

Base URL: `http://localhost:5000/api`

## Auth
- `POST /auth/register` { email, password }
- `POST /auth/login` { email, password } -> { token, role }

## Products
- `GET /products?q=&category=&sort=price_asc|price_desc`
- `GET /products/{id}`
- `POST /products` (Admin) body: Product
- `PUT /products/{id}` (Admin)
- `DELETE /products/{id}` (Admin)

## Cart
- `GET /cart` (Auth) -> list items
- `POST /cart/add` (Auth) { productId, quantity }
- `POST /cart/remove` (Auth) { productId }

## Orders
- `GET /orders` (Auth) -> my orders
- `POST /orders/checkout` (Auth)
- `GET /orders/all` (Admin)
- `POST /orders/{id}/status?status=Shipped|Delivered` (Admin)
