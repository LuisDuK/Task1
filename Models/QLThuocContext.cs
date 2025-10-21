using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QLHangHoa.Models;

public partial class QLThuocContext : DbContext
{
    public QLThuocContext()
    {
    }

    public QLThuocContext(DbContextOptions<QLThuocContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DonViTinh> DonViTinhs { get; set; }

    public virtual DbSet<DuongDung> DuongDungs { get; set; }

    public virtual DbSet<HangHoa> HangHoas { get; set; }

    public virtual DbSet<HangSanXuat> HangSanXuats { get; set; }

    public virtual DbSet<NguonChiTra> NguonChiTras { get; set; }

    public virtual DbSet<NhomChiPhi> NhomChiPhis { get; set; }

    public virtual DbSet<NhomHangHoa> NhomHangHoas { get; set; }

    public virtual DbSet<NuocSanXuat> NuocSanXuats { get; set; }

    public virtual DbSet<NhaThau> NhaThaus { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=MSI;Initial Catalog=task1;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DonViTinh>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__don_vi_t__3213E83F085F5447");

            entity.ToTable("don_vi_tinh");

            entity.HasIndex(e => e.MaDvt, "UQ__don_vi_t__057A95F56BCE303D").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaDvt)
                .HasMaxLength(64)
                .HasColumnName("ma_dvt");
            entity.Property(e => e.TenDvt)
                .HasMaxLength(255)
                .HasColumnName("ten_dvt");
        });

        modelBuilder.Entity<DuongDung>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__duong_du__3213E83F0C798F28");

            entity.ToTable("duong_dung");

            entity.HasIndex(e => e.MaDuong, "UQ__duong_du__E401EFB36E2185D2").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaDuong)
                .HasMaxLength(64)
                .HasColumnName("ma_duong");
            entity.Property(e => e.TenDuong)
                .HasMaxLength(255)
                .HasColumnName("ten_duong");
        });

        modelBuilder.Entity<HangHoa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__hang_hoa__3213E83FA35BBABE");

            entity.ToTable("hang_hoa", tb => tb.HasTrigger("trg_hang_hoa_set_ma_hang"));

            entity.HasIndex(e => e.MaHang, "UQ__hang_hoa__6DE840427D2184E6").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bhyt).HasColumnName("bhyt");
            entity.Property(e => e.DangBaoChe)
                .HasMaxLength(255)
                .HasColumnName("dang_bao_che");
            entity.Property(e => e.DuongDungId).HasColumnName("duong_dung_id");
            entity.Property(e => e.DvtNhapId).HasColumnName("dvt_nhap_id");
            entity.Property(e => e.DvtXuatId).HasColumnName("dvt_xuat_id");
            entity.Property(e => e.GiaThau)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("gia_thau");
            entity.Property(e => e.HamLuong)
                .HasMaxLength(255)
                .HasColumnName("ham_luong");
            entity.Property(e => e.HangSxId).HasColumnName("hang_sx_id");
            entity.Property(e => e.HoatChatText)
                .HasMaxLength(500)
                .HasColumnName("hoat_chat_text");
            entity.Property(e => e.MaAnhXa)
                .HasMaxLength(128)
                .HasColumnName("ma_anh_xa");
            entity.Property(e => e.MaBarcode)
                .HasMaxLength(64)
                .HasColumnName("ma_barcode");
            entity.Property(e => e.MaDuongDung)
                .HasMaxLength(64)
                .HasColumnName("ma_duong_dung");
            entity.Property(e => e.MaHang)
                .HasMaxLength(64)
                .HasColumnName("ma_hang");
            entity.Property(e => e.MaNhom).HasColumnName("ma_nhom");
            entity.Property(e => e.MaPpCheBien)
                .HasMaxLength(64)
                .HasColumnName("ma_pp_che_bien");
            entity.Property(e => e.NguonChiTraId).HasColumnName("nguon_chi_tra_id");
            entity.Property(e => e.MaNhaThau)
                .HasMaxLength(255)
                .HasColumnName("nha_thau");
            entity.Property(e => e.NhomChiPhiId).HasColumnName("nhom_chi_phi_id");
            entity.Property(e => e.NuocId).HasColumnName("nuoc_id");
            entity.Property(e => e.QuyCachDongGoi)
                .HasMaxLength(255)
                .HasColumnName("quy_cach_dong_goi");
            entity.Property(e => e.SlMax)
                .HasColumnType("decimal(18, 3)")
                .HasColumnName("sl_max");
            entity.Property(e => e.SlMin)
                .HasColumnType("decimal(18, 3)")
                .HasColumnName("sl_min");
            entity.Property(e => e.SoDangKy)
                .HasMaxLength(128)
                .HasColumnName("so_dang_ky");
            entity.Property(e => e.SoLuongQuyDoi)
                .HasDefaultValue(1m)
                .HasColumnType("decimal(18, 6)")
                .HasColumnName("so_luong_quy_doi");
            entity.Property(e => e.SoNgayDung)
                .HasDefaultValue(90)
                .HasColumnName("so_ngay_dung");
            entity.Property(e => e.TenHang)
                .HasMaxLength(500)
                .HasColumnName("ten_hang");
            entity.Property(e => e.ThongTinThau).HasColumnName("thong_tin_thau");
            entity.Property(e => e.TiLeBhyt)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("ti_le_bhyt");
            entity.Property(e => e.TiLeThanhToan)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("ti_le_thanh_toan");

            entity.HasOne(d => d.DuongDung).WithMany(p => p.HangHoas)
                .HasForeignKey(d => d.DuongDungId)
                .HasConstraintName("FK_hh_duong");

            entity.HasOne(d => d.DvtNhap).WithMany(p => p.HangHoaDvtNhaps)
                .HasForeignKey(d => d.DvtNhapId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_hh_dvt_nhap");

            entity.HasOne(d => d.DvtXuat).WithMany(p => p.HangHoaDvtXuats)
                .HasForeignKey(d => d.DvtXuatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_hh_dvt_xuat");

            entity.HasOne(d => d.HangSx).WithMany(p => p.HangHoas)
                .HasForeignKey(d => d.HangSxId)
                .HasConstraintName("FK_hh_hsx");

            entity.HasOne(d => d.NhomHangHoa).WithMany(p => p.HangHoas)
                .HasForeignKey(d => d.MaNhom)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_hh_nhom");

            entity.HasOne(d => d.NguonChiTra).WithMany(p => p.HangHoas)
                .HasForeignKey(d => d.NguonChiTraId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_hh_nct");

            entity.HasOne(d => d.NhomChiPhi).WithMany(p => p.HangHoas)
                .HasForeignKey(d => d.NhomChiPhiId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_hh_ncp");

            entity.HasOne(d => d.Nuoc).WithMany(p => p.HangHoas)
                .HasForeignKey(d => d.NuocId)
                .HasConstraintName("FK_hh_nuoc");
        });

        modelBuilder.Entity<HangSanXuat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__hang_san__3213E83F85C37EAC");

            entity.ToTable("hang_san_xuat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NuocId).HasColumnName("nuoc_id");
            entity.Property(e => e.TenHang)
                .HasMaxLength(255)
                .HasColumnName("ten_hang");

            entity.HasOne(d => d.Nuoc).WithMany(p => p.HangSanXuats)
                .HasForeignKey(d => d.NuocId)
                .HasConstraintName("FK_hsx_nuoc");
        });

        modelBuilder.Entity<NguonChiTra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__nguon_ch__3213E83F8ECF6E12");

            entity.ToTable("nguon_chi_tra");

            entity.HasIndex(e => e.MaNguon, "UQ__nguon_ch__8BE4221E830439D2").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaNguon)
                .HasMaxLength(64)
                .HasColumnName("ma_nguon");
            entity.Property(e => e.TenNguon)
                .HasMaxLength(255)
                .HasColumnName("ten_nguon");
        });

        modelBuilder.Entity<NhomChiPhi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__nhom_chi__3213E83F0A91AA40");

            entity.ToTable("nhom_chi_phi");

            entity.HasIndex(e => e.MaNhom, "UQ__nhom_chi__9B8FD98D1D568096").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaNhom)
                .HasMaxLength(64)
                .HasColumnName("ma_nhom");
            entity.Property(e => e.TenNhom)
                .HasMaxLength(255)
                .HasColumnName("ten_nhom");
        });

        modelBuilder.Entity<NhomHangHoa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__nhom_han__3213E83F5C7B4B46");

            entity.ToTable("nhom_hang_hoa");

            entity.HasIndex(e => e.MaNhom, "UQ__nhom_han__9B8FD98D2918D0EC").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaNhom)
                .HasMaxLength(64)
                .HasColumnName("ma_nhom");
            entity.Property(e => e.TenNhom)
                .HasMaxLength(255)
                .HasColumnName("ten_nhom");
        });

        modelBuilder.Entity<NuocSanXuat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__nuoc_san__3213E83F338F36D8");

            entity.ToTable("nuoc_san_xuat");

            entity.HasIndex(e => e.MaNuoc, "UQ__nuoc_san__A1E47C7D3F29038E").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaNuoc)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("ma_nuoc");
            entity.Property(e => e.TenNuoc)
                .HasMaxLength(255)
                .HasColumnName("ten_nuoc");
        });
        modelBuilder.Entity<NhaThau>(e =>
        {
            e.ToTable("nha_thau", "dbo");
            e.HasKey(x => x.Id);

            e.Property(x => x.MaNhaThau).HasColumnName("ma_nha_thau");
            e.Property(x => x.TenNhaThau).HasColumnName("ten_nha_thau");
            e.Property(x => x.DiaChi).HasColumnName("dia_chi");
            e.Property(x => x.DienThoai).HasColumnName("dien_thoai");
            e.Property(x => x.Email).HasColumnName("email");
            e.Property(x => x.NguoiDaiDien).HasColumnName("nguoi_dai_dien");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
