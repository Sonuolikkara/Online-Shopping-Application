# User Page Execution Flow Study Guide

This document maps the user-facing shopping flow in the application so it can be studied with breakpoints in Visual Studio.

## 1) Application startup flow

### File
- `WebApplication1/Program.cs:15-232`

### What happens
1. Services are registered:
   - MVC controllers/views
   - API controllers
   - JWT authentication
   - Session
   - DbContexts
   - `IGuestCartService`
   - cart, wishlist, product, auth services
2. Middleware pipeline is configured:
   - exception handling
   - correlation id
   - authentication
   - authorization
   - Hangfire dashboard
   - controller routes
3. Endpoint mapping:
   - `app.MapControllers()`
   - `app.MapControllerRoute(...)`

### Breakpoints
- `builder.Services.AddAuthentication(...)`
- `builder.Services.AddControllersWithViews()`
- `app.UseAuthentication()`
- `app.UseAuthorization()`
- `app.MapControllers()`
- `app.MapControllerRoute(...)`

---

## 2) Shared user layout flow

### File
- `WebApplication1/Views/Shared/_UserLayout.cshtml:1-21, 112-239`

### What happens
This layout is loaded by all user pages.

It displays:
- Home link
- Products link
- Search bar
- Wishlist link
- Cart link
- Login / Logout area

It also loads:
- jQuery
- `wwwroot/js/customer-api.js`

### Breakpoints
- Inspect links and script inclusion:
  - `asp-controller="UserHome"`
  - `asp-controller="UserProducts"`
  - `asp-controller="UserWishlist"`
  - `asp-controller="UserCart"`
  - `~/js/customer-api.js`

---

## 3) Home page flow

### Files
- `WebApplication1/Controllers/UserHomeController.cs:5-12`
- `WebApplication1/Views/UserHome/Index.cshtml:1-4, 496-514`

### Execution flow
1. Browser requests `/UserHome`
2. `UserHomeController.Index()` returns the view
3. Razor page renders inside `_UserLayout`

### Breakpoints
- `UserHomeController.Index()`

---

## 4) Products page flow

### Files
- `WebApplication1/Controllers/UserProductsController.cs:5-20`
- `WebApplication1/Views/UserProducts/Index.cshtml:1-5, 796-1233`
- `WebApplication1/Controllers/UserProductsApiController.cs:1-67`
- `WebApplication1/Services/ProductService/ProductService.Get.cs:9-38`

### Execution flow
#### A. User clicks Products
1. Browser requests `/UserProducts`
2. `UserProductsController.Index()` returns the Razor view
3. The view loads and its JavaScript calls:
   - `GET /api/UserProductsApi`

#### B. API returns product list
1. `UserProductsApiController.GetProducts()`
2. Calls `_productService.GetProducts()`
3. `ProductService.GetProducts()` queries the database
4. Product JSON is returned

#### C. Browser renders cards
The view script renders:
- featured product
- small product cards
- “View Product” links

### Breakpoints
- `UserProductsController.Index()`
- `UserProductsApiController.GetProducts()`
- `ProductService.GetProducts()`
- Product-loading JS in `Views/UserProducts/Index.cshtml`

---

## 5) Product details flow

### Files
- `WebApplication1/Controllers/UserProductsController.cs:15-20`
- `WebApplication1/Controllers/UserProductsApiController.cs:51-67`
- `WebApplication1/Views/UserProducts/Details.cshtml:1-9, 620-712, 800-1127, 1196-1678`

### Execution flow
#### A. User clicks View Product
1. Browser navigates to `/UserProducts/Details/{id}`
2. `UserProductsController.Details(id)` returns the Razor view
3. The page JS calls:
   - `GET /api/UserProductsApi/{id}`

#### B. API returns product data
1. `UserProductsApiController.GetProduct(id)`
2. `_productService.GetProductById(id)`
3. Product details are returned to the browser

#### C. Page renders
- Name
- Price
- Description
- Stock
- Wishlist state

### Breakpoints
- `UserProductsController.Details(int id)`
- `UserProductsApiController.GetProduct(int id)`
- `loadProduct()` in `Details.cshtml`
- `displayProduct(product)`
- `checkWishlistStatus()`

---

## 6) Add to Cart flow from product details

### Files
- `WebApplication1/Views/UserProducts/Details.cshtml:1196-1360`
- `WebApplication1/Controllers/UserProductsApiController.cs:75-186`
- `WebApplication1/Services/CartItemService/CartItemService.Get.cs:9-39`
- `WebApplication1/Services/CartItemService/CartItemService.Create.cs:9-31`
- `WebApplication1/Services/CartItemService/CartItemService.Update.cs:9-31`
- `WebApplication1/Services/GuestCartService.cs:18-52`

### Execution flow
#### Guest user
1. User clicks Add to Cart
2. JS sends `POST /api/UserProductsApi/AddToCart`
3. API checks JWT
4. No JWT means guest path
5. `GuestCartService.GetCart()` reads session cart
6. Item is added or updated
7. `GuestCartService.SaveCart(...)`
8. JSON response returned

#### Logged-in user
1. Same API call is made
2. API reads customer ID from JWT
3. `_cartItemService.GetCartIdByCustomerId(customerId)`
4. `_cartItemService.GetCartItemByProductId(...)`
5. Existing item is updated or a new one is inserted
6. JSON response returned

### Breakpoints
- Add to Cart click handler in `Details.cshtml`
- `UserProductsApiController.AddToCart(...)`
- `CartItemService.GetCartIdByCustomerId(...)`
- `CartItemService.GetCartItemByProductId(...)`
- `CartItemService.AddCartItem(...)`
- `CartItemService.UpdateCartItem(...)`
- `GuestCartService.GetCart()`
- `GuestCartService.SaveCart(...)`

---

## 7) Buy Now flow from product details

### Files
- `WebApplication1/Views/UserProducts/Details.cshtml:1310-1339`
- `WebApplication1/Views/UserCart/Checkout.cshtml:1-10, 309-551`

### Execution flow
#### Logged-in customer
1. User clicks Buy Now
2. JS redirects to:
   - `/Cart/Checkout?productId=...&quantity=...`
3. `UserCartController.Checkout(...)` returns the checkout Razor view
4. Checkout JS submits order to:
   - `POST /api/UserCartApi/Checkout`

#### Guest user
1. Buy Now checks `customerAccessToken`
2. If missing, user is redirected to `/CustomerLogin`
3. Checkout is not opened first

### Breakpoints
- Buy Now handler in `Details.cshtml`
- `UserCartController.Checkout(...)`
- Checkout form submit handler in `Checkout.cshtml`

---

## 8) Wishlist flow

### Files
- `WebApplication1/Controllers/UserWishlistController.cs:5-15`
- `WebApplication1/Views/UserWishlist/Index.cshtml:1-4, 254-515`
- `WebApplication1/Controllers/UserWishlistApiController.cs:8-126`
- `WebApplication1/Services/WishlistService/WishlistService.Get.cs:9-62`
- `WebApplication1/Services/WishlistService/WishlistService.Add.cs:9-43`
- `WebApplication1/Services/WishlistService/WishlistService.Remove.cs:8-31`

### Execution flow
#### A. Open wishlist page
1. Browser requests `/UserWishlist`
2. `UserWishlistController.Index()` returns the Razor view
3. View JS calls `GET /api/UserWishlistApi`

#### B. API returns wishlist items
1. `UserWishlistApiController.Index()`
2. JWT customer ID is read from claims
3. `_wishlistService.GetWishlistByCustomerId(customerId)`
4. Wishlist JSON returned

#### C. Add / remove wishlist items
- `POST /api/UserWishlistApi/Add`
- `POST /api/UserWishlistApi/Remove`

#### D. Add to cart from wishlist
- Wishlist Add to Cart posts to:
  - `POST /api/UserProductsApi/AddToCart`

### Breakpoints
- `UserWishlistController.Index()`
- `UserWishlistApiController.Index()`
- `UserWishlistApiController.Add(...)`
- `UserWishlistApiController.Remove(...)`
- `loadWishlist()` in `UserWishlist/Index.cshtml`
- `removeFromWishlist(productId)`
- wishlist Add to Cart form submit handler

---

## 9) Cart page flow

### Files
- `WebApplication1/Controllers/UserCartController.cs:5-31`
- `WebApplication1/Views/UserCart/Index.cshtml:1-4, 587-1008`
- `WebApplication1/Controllers/UserCartApiController.cs:117-188, 196-600`
- `WebApplication1/Services/CartItemService/CartItemService.ClearCart.cs:9-30`
- `WebApplication1/Services/OrderService/OrderService.Create.cs:10-17`
- `WebApplication1/Services/OrderItemService/OrderItemService.Create.cs:10-17`
- `WebApplication1/Services/PaymentService/PaymentService.Create.cs:10-17`

### Execution flow
#### A. Open cart page
1. Browser requests `/Cart`
2. `UserCartController.Index()` returns cart Razor view
3. View JS calls `GET /api/UserCartApi`

#### B. Cart API chooses guest or logged-in flow
1. `UserCartApiController.GetCart()`
2. `GetCustomerId()` checks JWT
3. Guest:
   - reads session cart from `GuestCartService`
   - resolves product details
4. Logged in:
   - reads DB cart items through `CartItemService`

#### C. Remove item
1. User clicks remove
2. JS sends `POST /api/UserCartApi/Remove?cartItemId=...`
3. API removes/decreases item
4. JS reloads cart and updates badge

#### D. Checkout button click
1. Guest:
   - redirected to `/CustomerLogin`
2. Logged in:
   - goes to `/Cart/Checkout`

### Breakpoints
- `UserCartController.Index()`
- `UserCartApiController.GetCart()`
- `UserCartApiController.Remove(...)`
- `GetCustomerId()`
- `CartItemService.DecreaseCartItemQuantity(...)`
- `CartItemService.ClearCartByCustomerId(...)`
- `loadCart()` in `UserCart/Index.cshtml`
- remove button click handler
- checkout button click handler

---

## 10) Checkout page flow

### Files
- `WebApplication1/Controllers/UserCartController.cs:15-22`
- `WebApplication1/Views/UserCart/Checkout.cshtml:1-10, 309-551`
- `WebApplication1/Controllers/UserCartApiController.cs:196-600`

### Execution flow
#### A. Page access
1. Browser requests `/Cart/Checkout`
2. `UserCartController.Checkout(productId, quantity)` returns the Razor view
3. JS checks `customerAccessToken`
4. If missing, user is redirected to `/CustomerLogin`

#### B. Place order
1. User fills address, phone, payment method
2. JS validates fields
3. JS sends `POST /api/UserCartApi/Checkout`
4. API validates JWT and model
5. API creates:
   - `Order`
   - `OrderItem`
   - `Payment`
6. For normal cart checkout:
   - `ClearCartByCustomerId(...)`
7. Returns `orderId`
8. Browser redirects to `/Cart/OrderPlaced/{id}`

### Breakpoints
- `UserCartController.Checkout(...)`
- checkout submit handler in `Checkout.cshtml`
- `UserCartApiController.Checkout(...)`
- `OrderService.AddOrder(...)`
- `OrderItemService.AddOrderItem(...)`
- `PaymentService.AddPayment(...)`
- `CartItemService.ClearCartByCustomerId(...)`

---

## 11) Login flow

### Files
- `WebApplication1/Controllers/CustomerLoginController.cs:5-22`
- `WebApplication1/Views/CustomerLogin/Index.cshtml:1-6, 229-495`
- `WebApplication1/Controllers/CustomerAuthApiController.cs:9-210`
- `WebApplication1/Services/CustomerAuthService.cs:30-170`

### Execution flow
#### A. Open login page
1. Browser requests `/CustomerLogin`
2. `CustomerLoginController.Index()` returns the Razor view

#### B. Submit login
1. Login form JS sends `POST /api/CustomerAuthApi/login`
2. `CustomerAuthApiController.Login(...)`
3. `CustomerAuthService.Login(...)`
4. Password is verified
5. JWT is generated
6. API returns:
   - `accessToken`
   - `customerId`

#### C. Browser stores auth state
- `sessionStorage.customerAccessToken`
- `sessionStorage.customerId`

#### D. After login
- Redirects to `/UserHome`

#### E. Guest cart merge
- Guest session cart is merged into the customer cart
- Guest cart session is cleared

### Breakpoints
- `CustomerLoginController.Index()`
- Login form submit handler in `CustomerLogin/Index.cshtml`
- `CustomerAuthApiController.Login(...)`
- `CustomerAuthService.Login(...)`

---

## 12) Logout flow

### File
- `WebApplication1/wwwroot/js/customer-api.js:84-151`

### Execution flow
1. User clicks Logout
2. JS clears:
   - `customerAccessToken`
   - `customerId`
   - `customerEmail`
3. Redirects to `/CustomerLogin`

### Breakpoint
- Logout click handler in `customer-api.js`

---

## 13) Guest cart architecture

### Files
- `WebApplication1/Services/GuestCartService.cs:18-52`
- `WebApplication1/Program.cs:132-139, 143`

### How it works
- Guest cart is stored in session
- `GuestCartService` uses `IHttpContextAccessor`
- Cart is serialized to JSON under the session key `GuestCart`

### Breakpoints
- `GuestCartService.GetCart()`
- `GuestCartService.SaveCart(...)`
- `GuestCartService.ClearCart()`

---

## 14) JWT auth architecture

### Files
- `WebApplication1/Program.cs:57-128`
- `WebApplication1/Services/CustomerAuthService.cs:127-170`
- API controllers

### How it works
- JWT is the default auth for API operations
- Login returns an access token
- JS stores token in sessionStorage
- jQuery `ajaxSetup` adds `Authorization: Bearer <token>`
- API controllers read claims from the JWT

### Breakpoints
- JWT config in `Program.cs`
- JWT generation in `CustomerAuthService.Login()`
- `User.FindFirst(ClaimTypes.NameIdentifier)` in API controllers

---

## 15) Best Visual Studio breakpoint order

1. `Program.cs`
2. `CustomerLoginController`
3. `CustomerAuthApiController`
4. `UserHomeController`
5. `UserProductsController`
6. `UserProductsApiController`
7. `UserCartController`
8. `UserCartApiController`
9. `UserWishlistController`
10. `UserWishlistApiController`
11. `GuestCartService`
12. `CustomerAuthService`
13. `CartItemService` methods
14. `WishlistService` methods

---

## 16) Recommended study scenarios

### Scenario 1: guest browsing
- Open Home
- Open Products
- Open Details
- Add to Cart
- Open Cart
- Click Checkout
- Confirm redirect to Login

### Scenario 2: guest login merge
- Add items to guest cart
- Login
- Confirm cart merge
- Confirm guest cart clears

### Scenario 3: logged-in checkout
- Add to cart
- Open cart
- Checkout
- Place order
- Confirm order/item/payment creation

### Scenario 4: wishlist
- Add to wishlist
- Open wishlist
- Remove item
- Add wishlist item to cart

---

## 17) Mental model

### MVC layer
Used only for returning Razor views.

Examples:
- `UserHomeController`
- `UserProductsController`
- `UserCartController`
- `UserWishlistController`
- `CustomerLoginController`

### Web API layer
Used for operations, auth, cart, wishlist, checkout, and order creation.

Examples:
- `UserProductsApiController`
- `UserCartApiController`
- `UserWishlistApiController`
- `CustomerAuthApiController`

---

## 18) Suggested next study format

If you want to study deeper, create a breakpoint trace in this order:

- Open login page
- Login
- Add product to cart
- Open cart
- Remove item
- Checkout
- Place order
- Open wishlist
- Add/remove wishlist item

Each of these paths has a clean controller → API → service flow.
