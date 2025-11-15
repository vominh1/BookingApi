<!-- Tiêu đề -->
<h1 align="center" style="font-size: 36px; font-weight: bold; margin-bottom: 10px;">
  Booking Management System
</h1>

<!-- Subtitle -->
<p align="center" style="font-size: 18px; color: #555;">
  Hệ thống quản lý đặt phòng & dịch vụ – API Backend
</p>

<hr/>

<h2>📌 Giới thiệu</h2>
<p>
  Dự án này mô phỏng hệ thống <b>Quản lý đặt phòng & dịch vụ</b> cho khách hàng và admin.
  Mục tiêu: xây dựng API backend với đầy đủ nghiệp vụ cơ bản, quản trị, hệ thống và báo cáo.
</p>

<h2>🚀 Các nhóm nghiệp vụ</h2>

<h3>1️⃣ Quản lý Booking (Đặt chỗ)</h3>
<ul>
  <li>Tạo, xem, hủy và xác nhận đơn đặt phòng</li>
  <li>API: <code>GET /api/bookings</code>, <code>POST /api/bookings</code></li>
  <li>Thực hành: Stored Procedure <code>CreateBooking</code></li>
</ul>

<h3>2️⃣ Quản lý Booking Items (Phòng & Thời gian)</h3>
<ul>
  <li>Chọn phòng, chọn giờ, kiểm tra trùng lịch</li>
  <li>Thực hành: validate thời gian, xử lý conflict</li>
</ul>

<h3>3️⃣ Quản lý Services (Dịch vụ thêm)</h3>
<ul>
  <li>Cho phép khách chọn thêm dịch vụ</li>
  <li>Tính tổng tiền booking</li>
</ul>

<h3>4️⃣ Quản lý Resources & Venues</h3>
<ul>
  <li>CRUD phòng & địa điểm</li>
  <li>Quan hệ 1-n (venue → resource)</li>
</ul>

<h3>5️⃣ Quản lý Availability Overrides (Khóa phòng)</h3>
<ul>
  <li>Admin chặn phòng trong khoảng thời gian bảo trì</li>
  <li>Kiểm tra conflict với booking hiện có</li>
</ul>

<h3>6️⃣ Quản lý Payments (Thanh toán)</h3>
<ul>
  <li>Ghi nhận thanh toán, xem lịch sử, thống kê doanh thu</li>
</ul>

<h3>7️⃣ Quản lý Users & Roles</h3>
<ul>
  <li>Đăng nhập, phân quyền, quản lý tài khoản</li>
  <li>JWT + hash mật khẩu + mapping UserRoles</li>
</ul>

<h3>8️⃣ Báo cáo</h3>
<ul>
  <li>Số lượng đặt chỗ theo tháng</li>
  <li>Doanh thu theo Venue</li>
  <li>Top phòng được đặt nhiều nhất</li>
</ul>

<h2>🛠️ Công nghệ sử dụng</h2>
<ul>
  <li><b>Backend:</b> ASP.NET Core / Node.js</li>
  <li><b>Database:</b> SQL Server / MySQL</li>
  <li><b>Authentication:</b> JWT</li>
</ul>

## 📂 Cấu trúc dự án
![Booking List](./Images/bookingList.jpg)

<div align="center" style="
    border: 1px solid #ddd;
    border-radius: 12px;
    padding: 16px;
    width: 70%;
    background: #fafafa;
    box-shadow: 0 0 10px rgba(0,0,0,0.05);
">
  <img src="./Images/bookingList.jpg" 
       width="450"
       style="border-radius:10px; margin-bottom: 5px;" />
  <p style="font-size: 14px; color: #666;">Hình ảnh cấu trúc dự án</p>
</div>
