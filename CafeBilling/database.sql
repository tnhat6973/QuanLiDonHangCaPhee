-- Tạo database
CREATE DATABASE CafeBilling;
GO
USE CafeBilling;
GO



-- Tạo bảng khách hàng
CREATE TABLE KHACHHANG(
  MAKH INT IDENTITY(1,1) PRIMARY KEY,  -- MA KH tu tang
  TenKH NVARCHAR(100) NOT NULL,
  SDT   NVARCHAR(20) NULL
  );



  --Bang nhan vien
CREATE TABLE NHANVIEN(
   MaNV INT IDENTITY(1,1) PRIMARY KEY,
   TenNV NVARCHAR(100) NOT NULL,
   ChucVu NVARCHAR(50) NULL
   );



   -- Bang Mon
CREATE TABLE MON(
   MaMon INT IDENTITY(1,1) PRIMARY KEY,
   TenMon NVARCHAR(100) NOT NULL,
   Gia DECIMAL(12,2) NOT NULL
   );



   --Bang hoa don
CREATE TABLE HOADON(
   MaHD INT IDENTITY(1,1) PRIMARY KEY,
   NgayLap DATETIME DEFAULT GETDATE(),
   MaKH INT NULL,
   MANV INT NULL
   );



   --BANG chi tiet hoa don
CREATE TABLE CHITIETHOADON(
   MaHD INT NOT NULL,
   MaMon INT NOT NULL,
   SoLuong INT NOT NULL DEFAULT 1,
   PRIMARY KEY (MaHD,MaMon),  --Khoa chinh ghep doi
   FOREIGN KEY (MaHD) REFERENCES HOADON(MaHD),
   FOREIGN KEY (MaMon) REFERENCES MON(MaMon)
   );



   -- THEM DU LIEU MAU
   INSERT INTO KHACHHANG(TenKH,SDT) VALUES('Trinh Tran Phuong Tuan','0374389745'), ('Tran Long Nhat','0378904563');
   INSERT INTO NHANVIEN(TenNV,ChucVu) VALUES('Tran Thi Hanh','0356436798'), ('Nguyen Dinh Phuong Thuy','0323443567');
   INSERT INTO MON(TenMon,Gia) VALUES ('Cà phê sữa',35000),('Cà phê đen',30000),('Matcha Phanxipang',55000),('Bò Húc Matcha',40000),('Hồng Trà Sữa',30000),('Bạc Xỉu',30000),('Americano: Espresso',60000),('Latte',30000),('Cold Brew',50000);








