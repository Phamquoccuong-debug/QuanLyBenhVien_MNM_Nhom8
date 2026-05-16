CREATE DATABASE QuanLyBenhVien;
GO

USE QuanLyBenhVien;
GO

CREATE TABLE KHOA (
    MaKhoa INT PRIMARY KEY IDENTITY,
    TenKhoa NVARCHAR(100) NOT NULL
);

-- 1. BẢNG BẢO HIỂM Y TẾ
CREATE TABLE BHYT (
    MaBHYT NVARCHAR(50) PRIMARY KEY,
    NgayCap DATE,
    NgayHetHan DATE,
    MucHuong FLOAT CHECK (MucHuong BETWEEN 0 AND 1)
);

-- 2. BẢNG BỆNH NHÂN (Đã bỏ MaBHYT để chuyển sang bảng KHAMBENH)
CREATE TABLE BENHNHAN (
    MaBN INT PRIMARY KEY IDENTITY,
    HotenBN NVARCHAR(100) NOT NULL,
    Ngaysinh DATE NOT NULL,
    Gioitinh NVARCHAR(10) NOT NULL CHECK (Gioitinh IN (N'Nam', N'Nữ')),
    Diachi NVARCHAR(200),
    Dienthoai NVARCHAR(15)
);

-- 3. BẢNG NGƯỜI NHÀ
CREATE TABLE NGUOINHA (
    MaNN INT PRIMARY KEY IDENTITY,
    HotenNN NVARCHAR(100) NOT NULL,
    QuanHe NVARCHAR(50),
    Diachi NVARCHAR(200),
    Dienthoai NVARCHAR(15),
    MaBN INT NOT NULL,
    FOREIGN KEY (MaBN) REFERENCES BENHNHAN(MaBN)
);

-- 4. BẢNG NHÂN VIÊN
CREATE TABLE NHANVIEN (
    MaNV INT PRIMARY KEY IDENTITY,
    HotenNV NVARCHAR(100) NOT NULL,
    Chucdanh NVARCHAR(50) NOT NULL,
    MaKhoa INT NOT NULL,
    FOREIGN KEY (MaKhoa) REFERENCES KHOA(MaKhoa)
);

-- 5. BẢNG TÀI KHOẢN
CREATE TABLE TAIKHOAN (
    Username NVARCHAR(50) PRIMARY KEY,
    Password NVARCHAR(100) NOT NULL,
    MaNV INT,
    Role INT NOT NULL DEFAULT 0 CHECK (Role IN (0,1)),
    FOREIGN KEY (MaNV) REFERENCES NHANVIEN(MaNV)
);

-- 6. BẢNG PHÒNG CHỨC NĂNG (Khám bệnh, cận lâm sàng...)
CREATE TABLE PHONG (
    MaPhong INT PRIMARY KEY IDENTITY,
    TenPhong NVARCHAR(100) NOT NULL,
    LoaiPhong NVARCHAR(50),
    MaKhoa INT NOT NULL,
    FOREIGN KEY (MaKhoa) REFERENCES KHOA(MaKhoa)
);

-- 7. BẢNG PHÒNG BỆNH (Nội trú)
CREATE TABLE PHONGBENH (
    MaPhongBenh INT PRIMARY KEY IDENTITY,
    TenPhong NVARCHAR(100) NOT NULL,
    SoGiuongToiDa INT NOT NULL CHECK (SoGiuongToiDa > 0),
    GiaNgay FLOAT NOT NULL DEFAULT 0 CHECK (GiaNgay >= 0), -- Giá tiền giường/ngày của phòng này
    MaKhoa INT NOT NULL,
    FOREIGN KEY (MaKhoa) REFERENCES KHOA(MaKhoa)
);

-- 8. BẢNG GIƯỜNG BỆNH
CREATE TABLE GIUONGBENH (
    MaGiuong INT PRIMARY KEY IDENTITY,
    MaPhongBenh INT NOT NULL,
    TrangThai NVARCHAR(50) NOT NULL DEFAULT N'Trống' CHECK (TrangThai IN (N'Trống', N'Có khách', N'Đang dọn')),
    FOREIGN KEY (MaPhongBenh) REFERENCES PHONGBENH(MaPhongBenh)
);
-- 9. BẢNG KHÁM BỆNH (Đã thêm MaBHYT và sửa lỗi chính tả ChanDoan)
CREATE TABLE KHAMBENH (
    MaKB INT PRIMARY KEY IDENTITY,
    MaBN INT NOT NULL,
    MaNV INT NOT NULL,
    MaPhong INT NOT NULL,
    LoaiKham NVARCHAR(20) NOT NULL CHECK (LoaiKham IN (N'Nội trú', N'Ngoại trú')),
    MaGiuong INT NULL,
    MaBHYT NVARCHAR(50) NULL, -- Gắn thẻ BHYT vào từng lần khám cụ thể
    NgayVao DATE,
    NgayRa DATE,
    NgayKham DATETIME NOT NULL DEFAULT GETDATE(),
    TrieuChung NVARCHAR(200),
    ChanDoan NVARCHAR(200), 
    SoThuTu INT,
    FOREIGN KEY (MaBN) REFERENCES BENHNHAN(MaBN),
    FOREIGN KEY (MaNV) REFERENCES NHANVIEN(MaNV),
    FOREIGN KEY (MaPhong) REFERENCES PHONG(MaPhong),
    FOREIGN KEY (MaGiuong) REFERENCES GIUONGBENH(MaGiuong),
    FOREIGN KEY (MaBHYT) REFERENCES BHYT(MaBHYT)
);

-- 10. BẢNG THUỐC
CREATE TABLE THUOC (
    MaThuoc INT PRIMARY KEY IDENTITY,
    TenThuoc NVARCHAR(100) NOT NULL,
    DonViTinh NVARCHAR(50),
    GiaHienTai FLOAT NOT NULL CHECK (GiaHienTai >= 0)
);

-- 11. BẢNG ĐƠN THUỐC
CREATE TABLE DONTHUOC (
    MaDonThuoc INT PRIMARY KEY IDENTITY,
    MaKB INT NOT NULL,
    NgayLap DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (MaKB) REFERENCES KHAMBENH(MaKB)
);

-- 12. BẢNG CHI TIẾT ĐƠN THUỐC (Đã thêm DonGiaBan để lưu vết giá)
CREATE TABLE CT_DONTHUOC (
    MaDonThuoc INT NOT NULL,
    MaThuoc INT NOT NULL,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGiaBan FLOAT NOT NULL CHECK (DonGiaBan >= 0),
    CachDung NVARCHAR(200),
    PRIMARY KEY (MaDonThuoc, MaThuoc),
    FOREIGN KEY (MaDonThuoc) REFERENCES DONTHUOC(MaDonThuoc),
    FOREIGN KEY (MaThuoc) REFERENCES THUOC(MaThuoc)
);

-- 13. BẢNG DỊCH VỤ
CREATE TABLE DICHVU (
    MaDV INT PRIMARY KEY IDENTITY,
    TenDV NVARCHAR(200) NOT NULL,
    DonGia FLOAT NOT NULL CHECK (DonGia >= 0)
);

-- 14. BẢNG CHỈ ĐỊNH DỊCH VỤ KHÁM BỆNH (Đã thêm DonGiaBan để lưu vết giá)
CREATE TABLE KHAMBENH_DICHVU (
    MaKB INT NOT NULL,
    MaDV INT NOT NULL,
    SoLuong INT NOT NULL DEFAULT 1 CHECK (SoLuong > 0),
    DonGiaBan FLOAT NOT NULL CHECK (DonGiaBan >= 0),
    PRIMARY KEY (MaKB, MaDV),
    FOREIGN KEY (MaKB) REFERENCES KHAMBENH(MaKB),
    FOREIGN KEY (MaDV) REFERENCES DICHVU(MaDV)
);

-- 15. BẢNG HÓA ĐƠN (Đã bỏ các bảng chi tiết hóa đơn thừa thãi)
CREATE TABLE HOADON (
    MaHD INT PRIMARY KEY IDENTITY,
    MaKB INT NOT NULL UNIQUE, -- Ràng buộc 1 lần khám chỉ xuất 1 hóa đơn tổng
    NgayLap DATETIME NOT NULL DEFAULT GETDATE(),
    TongTien FLOAT NOT NULL DEFAULT 0,
    TrangThai NVARCHAR(30) NOT NULL 
        DEFAULT N'Chưa thanh toán' 
        CHECK (TrangThai IN (N'Chưa thanh toán', N'Đã thanh toán', N'Đã hủy')),
    FOREIGN KEY (MaKB) REFERENCES KHAMBENH(MaKB)
);

-- 16. BẢNG XÉT NGHIỆM
CREATE TABLE XETNGHIEM (
    MaXN INT PRIMARY KEY IDENTITY,
    TenXN NVARCHAR(100) NOT NULL
);

-- 17. BẢNG PHIẾU XÉT NGHIỆM (Chuyển DATE thành DATETIME)
CREATE TABLE PHIEUXN (
    MaPXN INT PRIMARY KEY IDENTITY,
    MaKB INT,
    MaXN INT,
    NgayYeuCau DATETIME, 
    NgayThucHien DATETIME, 
    KetQua NVARCHAR(200),
    FOREIGN KEY (MaKB) REFERENCES KHAMBENH(MaKB),
    FOREIGN KEY (MaXN) REFERENCES XETNGHIEM(MaXN)
);

drop table KHAMBENH_DICHVU
drop table DICHVU
drop table PHIEUXN
drop table XETNGHIEM

-- 1. XÓA BẢNG XETNGHIEM CŨ (Xóa bảng PHIEUXN trước vì có khóa ngoại)
DROP TABLE PHIEUXN;
DROP TABLE XETNGHIEM;

-- 2. CẢI TIẾN BẢNG DỊCH VỤ (Thêm cột LoaiDV)
CREATE TABLE DICHVU (
    MaDV INT PRIMARY KEY IDENTITY,
    TenDV NVARCHAR(200) NOT NULL,
    LoaiDV NVARCHAR(50) NOT NULL, -- Phân loại: 'Khám bệnh', 'Xét nghiệm', 'Siêu âm', 'X-Quang'...
    DonGia FLOAT NOT NULL CHECK (DonGia >= 0)
);

-- Bảng KHAMBENH_DICHVU giữ nguyên (Dùng để tính tiền và theo dõi chỉ định)
-- ...

-- 3. TẠO LẠI BẢNG KẾT QUẢ (Thay thế PHIEUXN, dùng chung cho cả Xét nghiệm, Siêu âm, X-Quang)
CREATE TABLE KETQUA_LAMSANG (
    MaKetQua INT PRIMARY KEY IDENTITY,
    MaKB INT NOT NULL,
    MaDV INT NOT NULL, -- Trỏ thẳng đến Dịch vụ đã được chỉ định
    NgayYeuCau DATETIME NOT NULL DEFAULT GETDATE(),
    NgayThucHien DATETIME,
    KetQua NVARCHAR(MAX), -- Dùng NVARCHAR(MAX) vì kết quả siêu âm/X-Quang có thể mô tả rất dài
    NguoiThucHien INT, -- (Tùy chọn) ID của Kỹ thuật viên nhập kết quả
    FOREIGN KEY (MaKB) REFERENCES KHAMBENH(MaKB),
    FOREIGN KEY (MaDV) REFERENCES DICHVU(MaDV)
);
alter table DICHVU
add NoiDung nvarchar(max)