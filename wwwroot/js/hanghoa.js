//console.log(nhomHangHoa);
// Biến phân trang
let currentPage = 1;
let pageSize = 10;
let filteredData = [...allData];
let selectedNhom = "";

initNhomDropdown();
renderTable();
// Khởi tạo dropdown nhóm hàng hóa
function initNhomDropdown() {
    const select = document.getElementById('filterNhom');

    nhomHangHoa.forEach(nhom => {
        const option = document.createElement('option');
        option.value = nhom.MaNhom;
        option.textContent = nhom.TenNhom;
        select.appendChild(option);
    });
}

// Hàm lọc theo nhóm
function filterByNhom() {
    selectedNhom = document.getElementById('filterNhom').value;
    applyFilters();
}
// Hàm render bảng
function renderTable() {
    const tbody = document.getElementById('tableBody');
    tbody.innerHTML = '';

    // Tính toán dữ liệu cho trang hiện tại
    const startIndex = (currentPage - 1) * pageSize;
    const endIndex = Math.min(startIndex + pageSize, filteredData.length);
    const pageData = filteredData.slice(startIndex, endIndex);

    // Render từng dòng
    pageData.forEach(item => {
        const row = document.createElement('tr');
        row.innerHTML = `
                            <td>${item.TenNhom ?? ''}</td>
                            <td>${item.TenHang ?? ''}</td>
                            <td style="text-align: center">${item.MaHang ?? ''}</td>
                            <td style="text-align: center">${item.MaDuong ?? ''}</td>
                            <td>${item.HamLuong ?? ''}</td>
                            <td>${item.HoatChat ?? ''}</td>
                            <td>${item.MaAnhXa ?? ''}</td>
                            <td>${item.Bhyt ? 'Có' : 'Không'}</td>
                            <td>${item.ThongTinThuoc ?? ''}</td>
                            <td>
                                <div class="action-icons">
                                    <button class="icon-btn" onclick="editItem('${item.MaHang}')">
                                        <i class="fas fa-edit"></i>
                                    </button>
                                    <button class="icon-btn" onclick="deleteItem('${item.MaHang}')">
                                        <i class="fas fa-trash"></i>
                                    </button>
                                </div>
                            </td>
                        `;
        tbody.appendChild(row);
    });

    // Cập nhật thông tin phân trang

    renderPaginationButtons();
}



// Hàm áp dụng tất cả bộ lọc
function applyFilters() {
    const searchTerm = document.getElementById('searchInput').value.toLowerCase();

    filteredData = allData.filter(item => {
        // Lọc theo nhóm
        const matchNhom = !selectedNhom || item.MaNhom === selectedNhom;

        // Lọc theo từ khóa tìm kiếm
        const matchSearch = !searchTerm ||
            (item.MaNhom && item.MaNhom.toLowerCase().includes(searchTerm)) ||
            (item.TenHang && item.TenHang.toLowerCase().includes(searchTerm)) ||
            (item.MaHang && item.MaHang.toLowerCase().includes(searchTerm)) ||
            (item.HoatChat && item.HoatChat.toLowerCase().includes(searchTerm));

        return matchNhom && matchSearch;
    });

    currentPage = 1; // Reset về trang 1
    renderTable();
}
// Hàm tìm kiếm
function searchData() {
    applyFilters();
}
// Hàm render nút phân trang
function renderPaginationButtons() {
    const totalPages = Math.ceil(filteredData.length / pageSize);
    const pageNumbers = document.getElementById('pageNumbers');
    pageNumbers.innerHTML = '';

    // Nút Previous
    const prevBtn = createPageButton('<i class="fas fa-chevron-left"></i>', currentPage > 1, () => {
        if (currentPage > 1) {
            currentPage--;
            renderTable();
        }
    });
    pageNumbers.appendChild(prevBtn);

    // Tính toán range hiển thị
    let startPage = Math.max(1, currentPage - 2);
    let endPage = Math.min(totalPages, currentPage + 2);

    // Điều chỉnh nếu ở đầu hoặc cuối
    if (currentPage <= 3) {
        endPage = Math.min(5, totalPages);
    }
    if (currentPage >= totalPages - 2) {
        startPage = Math.max(1, totalPages - 4);
    }

    // Nút trang đầu
    if (startPage > 1) {
        const firstBtn = createPageButton('1', true, () => goToPage(1));
        pageNumbers.appendChild(firstBtn);

        if (startPage > 2) {
            const dots = document.createElement('span');
            dots.textContent = '...';
            dots.style.padding = '6px 12px';
            dots.style.color = '#6b7280';
            pageNumbers.appendChild(dots);
        }
    }

    // Các nút trang
    for (let i = startPage; i <= endPage; i++) {
        const btn = createPageButton(i, true, () => goToPage(i), i === currentPage);
        pageNumbers.appendChild(btn);
    }

    // Nút trang cuối
    if (endPage < totalPages) {
        if (endPage < totalPages - 1) {
            const dots = document.createElement('span');
            dots.textContent = '...';
            dots.style.padding = '6px 12px';
            dots.style.color = '#6b7280';
            pageNumbers.appendChild(dots);
        }

        const lastBtn = createPageButton(totalPages, true, () => goToPage(totalPages));
        pageNumbers.appendChild(lastBtn);
    }

    // Nút Next
    const nextBtn = createPageButton('<i class="fas fa-chevron-right"></i>', currentPage < totalPages, () => {
        if (currentPage < totalPages) {
            currentPage++;
            renderTable();
        }
    });
    pageNumbers.appendChild(nextBtn);
}

// Hàm tạo nút phân trang
function createPageButton(text, enabled, onClick, isActive = false) {
    const btn = document.createElement('button');
    btn.className = 'page-btn';
    btn.innerHTML = text;
    btn.disabled = !enabled;

    if (isActive) {
        btn.classList.add('active');
    }

    if (enabled) {
        btn.onclick = onClick;
    }

    return btn;
}
// Xử lý Enter trong ô tìm kiếm
document.getElementById('searchInput').addEventListener('keypress', function (e) {
    if (e.key === 'Enter') {
        searchData();
    }
});
// Hàm chuyển trang
function goToPage(page) {
    currentPage = page;
    renderTable();
}

// Hàm thay đổi số dòng hiển thị
function changePageSize() {
    pageSize = parseInt(document.getElementById('pageSize').value);
    currentPage = 1; // Reset về trang 1
    renderTable();
}

// Hàm xóa
let maHangCanXoa = null;

function deleteItem(maHang) {
    maHangCanXoa = maHang;
    document.getElementById('deleteMaHang').value = maHangCanXoa;
    document.getElementById('deleteOverlay').style.display = 'flex';
}

function closeDeleteModal() {
    document.getElementById('deleteOverlay').style.display = 'none';
    maHangCanXoa = null;
}
/*
function confirmDelete() {
    $.ajax({
        url: '/HangHoa/XoaHangHoa',
        type: 'POST',
        data: { maHangHoa: maHangCanXoa },
        success: function (res) {
            closeDeleteModal();
            if (res.success) {
                document.querySelector(`[data-ma="${maHangCanXoa}"]`)?.closest('tr')?.remove();
                alert(res.message);

            } else {
                //console.log(res);
                alert(res.message);
            }
        },
        error: function () {
            alert('Có lỗi xảy ra khi kết nối đến server.');
        }
    });
} */
document.getElementById('deleteOverlay').addEventListener('click', function (e) {
    if (e.target === this) {
        closeDeleteModal();
    }
});
///
// ===== MODAL FUNCTIONS =====
function openModal() {
    document.getElementById('modalOverlay').classList.add('active');
    document.body.style.overflow = 'hidden';
}

function closeModal() {
    document.getElementById('modalOverlay').classList.remove('active');
    document.body.style.overflow = 'auto';
}
document.getElementById('modalOverlay').addEventListener('click', function (e) {
    if (e.target === this) {
        closeModal();
    }
});
function initModalDropdowns() {
    // Nhóm hàng hóa
    const selectNhomHH = document.querySelector('select[name="MaNhom"]');
    nhomHangHoa.forEach(item => {
        const option = document.createElement('option');
        option.value = item.Id;
        option.textContent = item.TenNhom;
        selectNhomHH.appendChild(option);
    });

    // Đơn vị tính nhập
    const selectDVTNhap = document.querySelector('select[name="DvtNhapId"]');
    donViTinh.forEach(item => {
        const option = document.createElement('option');
        option.value = item.Id;
        option.textContent = item.TenDonVi;
        selectDVTNhap.appendChild(option);
    });

    // Đơn vị tính xuất
    const selectDVTXuat = document.querySelector('select[name="DvtXuatId"]');
    donViTinh.forEach(item => {
        const option = document.createElement('option');
        option.value = item.Id;
        option.textContent = item.TenDonVi;
        selectDVTXuat.appendChild(option);
    });

    // Đường dùng
    const selectDuongDung = document.querySelector('select[name="DuongDungId"]');
    duongDung.forEach(item => {
        const option = document.createElement('option');
        option.value = item.MaDuong;
        option.textContent = item.TenDuong;
        selectDuongDung.appendChild(option);
    });

    // Nước sản xuất
    const selectNuocSX = document.querySelector('select[name="NuocId"]');
    nuocSanXuat.forEach(item => {
        const option = document.createElement('option');
        option.value = item.Id;
        option.textContent = item.TenNuoc;
        selectNuocSX.appendChild(option);
    });

    // Hãng sản xuất
    const selectHangSX = document.querySelector('select[name="HangSxId"]');
    hangSanXuat.forEach(item => {
        const option = document.createElement('option');
        option.value = item.MaHangSx;
        option.textContent = item.TenHang;
        selectHangSX.appendChild(option);

    });

    // Nhóm chi phí
    const selectNhomCP = document.querySelector('select[name="NhomChiPhiId"]');
    nhomChiPhi.forEach(item => {
        const option = document.createElement('option');
        option.value = item.Id;
        option.textContent = item.TenNhom;
        selectNhomCP.appendChild(option);
    });
    const selectNguonCT = document.querySelector('select[name="NguonChiTraId"]');
    nguonChiTra.forEach(item => {
        //console.log(item);
        const option = document.createElement('option');
        option.value = item.Id;
        option.textContent = item.MaNguon;
        selectNguonCT.appendChild(option);
    });

    // Nhà thầu
    const selectNhaThau = document.querySelector('select[name="MaNhaThau"]');
    nhaThau.forEach(item => {
        const option = document.createElement('option');
        option.value = item.MaNhaThau;
        option.textContent = item.TenNhaThau;
        selectNhaThau.appendChild(option);
    });
}
const selectDD = document.querySelector('select[name="DuongDungId"]');
const inputDD = document.querySelector('input[name="MaDuongDung"]');

// Lắng nghe sự kiện thay đổi trên select đường dùng
selectDD.addEventListener('change', function () {
    // Lấy value của option được chọn
    const selectedValue = this.value;
    // Gán giá trị đó vào ô input
    inputDD.value = selectedValue;
});

initModalDropdowns();


function setCreateMode() {
    const form = document.getElementById('productForm');
    form.reset();
    form.dataset.mode = 'create';
    form.setAttribute('action', '/HangHoa/ThemHangHoa'); // action thêm
    document.getElementById('modalTitle').textContent = 'Thêm hàng hóa';
}

/** Bấm nút Sửa trên từng dòng */
function editItem(maHang) {
    // lấy từ filteredData (đã render bảng)
    const item = filteredData.find(x => x.MaHang === maHang);
    if (!item) { alert('Không tìm thấy dữ liệu để sửa'); return; }

    // bật modal & đổi sang "Sửa"
    openModal();
    const form = document.getElementById('productForm');
    form.dataset.mode = 'edit';
    form.setAttribute('action', '/HangHoa/SuaHangHoa'); // action update
    document.getElementById('modalTitle').textContent = 'Cập nhật hàng hóa';

    // ĐIỀN DỮ LIỆU — CHÚ Ý map tên cho đúng model:
    const byName = n => form.querySelector(`[name="${n}"]`);
    const setVal = (name, val) => { const el = byName(name); if (el) el.value = (val ?? ''); };
    const setSel = setVal;
    const setChk = (name, bool) => { const el = byName(name); if (el) el.checked = !!bool; };


    // nếu item có Id, điền vào hidden để backend biết đang update
    if (typeof item.Id !== 'undefined') document.getElementById('Id').value = item.Id;

    setVal('MaHang', item.MaHang);
    setSel('MaNhom', item.IdNhom);
    setVal('TenHang', item.TenHang);
    setSel('DvtNhapId', item.DvtNhapId);
    setSel('DvtXuatId', item.DvtXuatId);
    setVal('SoLuongQuyDoi', item.SoLuongQuyDoi);

    // các tên bạn đang render trên bảng KHÁC tên model → map lại:
    setVal('MaDuongDung', item.MaDuong ?? item.MaDuongDung ?? '');

    const selectD = document.querySelector('select[name="DuongDungId"]');
    const inputD = document.querySelector('input[name="MaDuongDung"]');
    if (!inputD.value) {
        inputD.value = selectD.value;
    }

    setSel('NuocId', item.NuocId);
    setSel('HangSxId', item.HangSxId);

    setVal('HamLuong', item.HamLuong);
    setVal('HoatChatText', item.HoatChat ?? item.HoatChatText ?? '');

    setVal('DangBaoChe', item.DangBaoChe);
    setVal('QuyCachDongGoi', item.QuyCachDongGoi);
    setVal('SoDangKy', item.SoDangKy);

    setVal('MaAnhXa', item.MaAnhXa);
    setVal('MaBarcode', item.MaBarcode ?? item.MaBarCode ?? ''); // HTML đang là MaBarCode -> sửa name/id thành MaBarcode

    setVal('MaPpCheBien', item.MaPpCheBien);
    setSel('NhomChiPhiId', item.NhomChiPhiId);
    setSel('NguonChiTraId', item.NguonChiTraId);

    setChk('Bhyt', item.Bhyt);
    setVal('TiLeBHYT', item.TiLeBhyt ?? item.TiLeBHYT ?? '');
    setVal('TiLeThanhToan', item.TiLeThanhToan);

    setVal('ThongTinThau', item.ThongTinThuoc ?? item.ThongTinThau ?? '');
    setSel('MaNhaThau', item.MaNhaThau);

    setVal('GiaThau', item.GiaThau);
    setVal('SlMin', item.SlMin);
    setVal('SlMax', item.SlMax);
    setVal('SoNgayDung', item.SoNgayDung ?? 90);
}

/// Khi bấm nút Thêm mới
function onAddNewClick() {
    setCreateMode();
    openModal();
}

/// NotifyModal
let notifyTimer = null;

function showNotifyModal({ type = "success", title = "Thành công", message = "", duration = 2500 } = {}) {
    const overlay = document.getElementById("notifyOverlay");
    const titleEl = document.getElementById("notifyTitle");
    const msgEl = document.getElementById("notifyMessage");

    overlay.classList.remove("notify-success", "notify-error", "notify-info");
    overlay.classList.add(type === "error" ? "notify-error" : type === "info" ? "notify-info" : "notify-success");

    titleEl.textContent = title;
    msgEl.textContent = message;

    overlay.style.display = "flex";

    // auto close
    clearTimeout(notifyTimer);
    if (duration && duration > 0) {
        notifyTimer = setTimeout(closeNotifyModal, duration);
    }
}

function closeNotifyModal() {
    const overlay = document.getElementById("notifyOverlay");
    overlay.style.display = "none";
    clearTimeout(notifyTimer);
}

// đóng bằng ESC hoặc click ra ngoài
document.addEventListener("keydown", (e) => { if (e.key === "Escape") closeNotifyModal(); });
document.getElementById("notifyOverlay").addEventListener("click", (e) => {
    if (e.target.id === "notifyOverlay") closeNotifyModal();
});

function showSuccess(msg) { showNotifyModal({ type: "success", title: "Thành công", message: msg }); }
function showError(msg) { showNotifyModal({ type: "error", title: "Có lỗi", message: msg, duration: 4000 }); }
function showInfo(msg) { showNotifyModal({ type: "info", title: "Thông báo", message: msg }); }