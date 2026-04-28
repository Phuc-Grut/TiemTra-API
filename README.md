# TiemTra API (SellAPI)

API backend cho ứng dụng TiemTra — một hệ thống thương mại điện tử (store + admin dashboard) xây dựng bằng ASP.NET Core Web API.

## Tổng quan
- Ngôn ngữ: C# (.NET 6+)
- Kiến trúc: ASP.NET Core Web API với Controllers
- DB: SQL Server (Entity Framework Core)
- Authentication: JWT Bearer
- Storage: Azure Blob Storage (để lưu ảnh)
- Tích hợp: VNPAY (thanh toán), GHN (giao hàng)
- Công cụ khác: AutoMapper, FluentValidation, Swagger/OpenAPI, BackgroundService (VoucherExpirationService)
- CORS đã cấu hình cho frontend (local & 1 Azure URL)

---

## Nội dung README này
1. Yêu cầu
2. Thiết lập môi trường (env vars)
3. Chạy ứng dụng (local / Docker)
4. Cấu trúc API chính (store & admin)
5. Authentication
6. Upload file
7. Các tích hợp bên ngoài (VNPAY, GHN, Azure Blob)
8. Mẹo debug & troubleshooting
9. Gợi ý cải thiện tài liệu

---

## 1. Yêu cầu
- .NET SDK 6.0/7.0 (phiên bản phù hợp với dự án)
- SQL Server (local hoặc container)
- Azure Storage account (nếu dùng upload ảnh thật)
- (Tùy chọn) Docker / Docker Compose để chạy container

---

## 2. Biến môi trường / cấu hình cần thiết
Bạn có thể đặt các giá trị này trong `appsettings.Development.json`, biến môi trường hệ thống, hoặc secret manager tùy môi trường.

ENV ví dụ (liệt kê tên biến/khóa trong appsettings):
- ConnectionStrings:DefaultConnection — chuỗi kết nối SQL Server
  - Ví dụ: "Server=.;Database=TiemTraDb;Trusted_Connection=True;"
- JwtSettings:
  - JwtSettings:SecretKey — khóa bí mật (ít nhất 16+ ký tự)
  - JwtSettings:Issuer
  - JwtSettings:Audience
- AzureStorage:
  - AzureStorage:ConnectionString — connection string cho Azure Blob Storage
- Vnpay:
  - Vnpay:TmnCode
  - Vnpay:HashSecret
  - Vnpay:BaseUrl
  - Vnpay:CallbackUrl
- GHN:
  - GHN:BaseUrl
  - GHN:Token
  - GHN:ShopId
- DOTNET_RUNNING_IN_CONTAINER — set "true" nếu chạy trong container để ứng dụng lắng nghe 0.0.0.0:80



---

## 3. Chạy ứng dụng

Chạy local:
1. Thiết lập biến môi trường / appsettings.
2. Tạo & migrate database (EF Core):
   - Cài dotnet-ef nếu chưa có: `dotnet tool install --global dotnet-ef`
   - Từ thư mục chứa project API (ví dụ `src/TiemTra_Api`):
     - `dotnet ef database update`
   (Lưu ý: nếu solutions/structure khác, chỉ định --project/--startup-project phù hợp.)

3. Chạy:
   - `cd src/TiemTra_Api`
   - `dotnet run`

Sau khi chạy, nếu `ASPNETCORE_ENVIRONMENT=Development`, Swagger UI sẽ bật:
- Swagger: `https://localhost:5001/swagger` hoặc `http://localhost:5000/swagger` tùy cấu hình.

Chạy bằng Docker:
- Nếu repo có Dockerfile, build & run image:
  - `docker build -t tiemtra-api .`
  - `docker run -e "ConnectionStrings__DefaultConnection=..." -e "JwtSettings__SecretKey=..." -p 80:80 tiemtra-api`
- Hoặc dùng docker-compose (nếu bạn tự tạo file compose) để kèm SQL Server.

Lưu ý: nếu chạy trong container set `DOTNET_RUNNING_IN_CONTAINER=true`, service lắng nghe `http://0.0.0.0:80`.

---

## 4. Cấu trúc API chính (tóm tắt các route đáng chú ý)

Cấu trúc phân vùng:
- Store (client-facing): prefix `/api/store/`
- Admin (dashboard): prefix `/api/admin/`
- VNPAY callbacks: controller `/Vnpay/`

Một số endpoint tiêu biểu:

Store (client)
- GET /api/store/product/get-paging-products
- GET /api/store/product/get-product-by-code/{productCode}
- GET /api/store/voucher/public
- POST /api/store/voucher/apply
- POST /api/store/profile/add-avatar-user (multipart/form-data)
- GET /api/store/profile/get-profile-by-userId
- VNPAY:
  - GET /Vnpay/api/CreatePaymentUrl?money={amount}&description={desc}
  - GET /Vnpay/api/IpnAction (callback từ VNPAY)

Admin (cần token & role Admin)
- POST /api/admin/product/create-product
- POST /api/admin/product/add-product-image (multipart/form-data)
- GET /api/admin/product/get-paging-products
- PUT /api/admin/product/update-product/{productId}
- POST /api/admin/category/add-category
- DELETE /api/admin/category/delete-category-by-ids
- POST /api/admin/brand/create-brand
- GET /api/admin/order/get-paging-orders
- POST /api/admin/order/confirm-order/{orderId}

Swagger hiển thị đầy đủ các endpoint và mẫu request/response khi chạy môi trường dev.

---

## 5. Authentication (JWT)
- API dùng JWT Bearer token.
- Để gọi các route cần bảo mật, thêm header:
  - Authorization: Bearer {token}
- Token được cấp từ các endpoint authentication (xem controller Authentication trong repo để biết các route đăng nhập/đăng ký).
- Một số endpoint Admin có attribute `[Authorize(Roles = "Admin")]` — token phải có claim Role = Admin.

---

## 6. Upload file
- Nhiều endpoint chấp nhận multipart/form-data (ví dụ upload ảnh sản phẩm, ảnh brand, avatar).
- API dùng service Azure Blob để lưu file. Endpoint trả về URL file khi upload thành công.
- Header ví dụ: Content-Type: multipart/form-data

---

## 7. Tích hợp bên ngoài
- Azure Blob Storage: dùng BlobServiceClient, cần `AzureStorage:ConnectionString`.
- VNPAY:
  - Sử dụng cấu hình `Vnpay:TmnCode`, `Vnpay:HashSecret`, `Vnpay:BaseUrl`, `Vnpay:CallbackUrl`.
  - Endpoint `/Vnpay/api/CreatePaymentUrl` trả về URL thanh toán; callback VNPAY gọi `IpnAction` để cập nhật trạng thái đơn hàng.
- GHN:
  - HttpClient đã cấu hình với BaseUrl + Token + ShopId.
  - Dùng để gọi API giao hàng của GHN.

---

## 8. Background jobs
- Có BackgroundService `VoucherExpirationService` để xử lý các voucher hết hạn định kỳ.

---

## 9. Debug / Troubleshooting
- Nếu không kết nối DB: kiểm tra `DefaultConnection` và cho phép remote connections trên SQL Server.
- Lỗi JWT: kiểm tra `JwtSettings:SecretKey`, Issuer, Audience có khớp với client.
- Lỗi upload: kiểm tra `AzureStorage:ConnectionString` và permission container.
- Nếu chạy container và không nghe được cổng: set `DOTNET_RUNNING_IN_CONTAINER=true` hoặc map port đúng khi chạy container.
- Xem logs console để thấy lỗi chi tiết; bật `ASPNETCORE_ENVIRONMENT=Development` để bật DeveloperExceptionPage.

---

## 10. Gợi ý cải thiện tài liệu (recommended)
- Bổ sung file `env.sample` liệt kê tất cả biến môi trường (không chứa giá trị thật).
- Thêm section "Database seeding" nếu repo có seed data.
- Tạo Postman / OpenAPI export để team front-end dễ sử dụng.
- Mô tả chi tiết model request/response cho các endpoint quan trọng (product create, order flow, voucher).
- Hướng dẫn setup VNPAY/GHN (các bước đăng ký, sandbox) nếu cần.

---

## 11. Liên hệ / đóng góp
- Cách đóng góp: fork -> feature branch -> PR -> review.
- Nên thêm CONTRIBUTING.md với quy chuẩn commit, PR và coding style.
