//console.log(nhomHangHoa);
// Biến phân trang
let currentPage = 1;
let pageSize = 10;
let filteredData = [...allData];
let selectedNhom = "";



// Hiển thị menu
function toggleColumnMenu() {
    const menu = document.getElementById('columnMenu');
    menu.classList.toggle('active');
}

// Close menu when click outside
document.addEventListener('click', function (e) {
    const menu = document.getElementById('columnMenu');
    const menuBtn = e.target.closest('.btn-menu');

    if (!menu.contains(e.target) && !menuBtn) {
        menu.classList.remove('active');
    }
});

// Handle column visibility - GẮN SAU KHI DOM LOADED
document.addEventListener('DOMContentLoaded', function () {
    // Khởi tạo event listeners cho checkboxes
    document.querySelectorAll('.column-checkbox input').forEach(checkbox => {
        checkbox.addEventListener('change', function () {
            const columnName = this.dataset.column;
            const isChecked = this.checked;

            toggleTableColumn(columnName, isChecked);
          //  saveColumnSettings();
        });
    });

    // Load settings đã lưu
    //loadColumnSettings();
});

// Hàm ẩn/hiện cột - DÙNG data-column thay vì index
function toggleTableColumn(columnName, show) {
    const table = document.querySelector('table');

    // Tìm header theo data-column
    const header = table.querySelector(`thead th[data-column="${columnName}"]`);
    if (!header) {
        console.warn(`Không tìm thấy header với data-column="${columnName}"`);
        return;
    }

    // Lấy index của header
    const headers = Array.from(table.querySelectorAll('thead th'));
    const headerIndex = headers.indexOf(header);

    if (headerIndex === -1) return;

    // Ẩn/hiện header
    header.style.display = show ? '' : 'none';

    // Ẩn/hiện cells trong body
    const rows = table.querySelectorAll('tbody tr');
    rows.forEach(row => {
        const cell = row.querySelectorAll('td')[headerIndex];
        if (cell) {
            cell.style.display = show ? '' : 'none';
        }
    });
}

// Lưu settings - không dùng trong ví dụ này
function saveColumnSettings() {
    const settings = {};
    document.querySelectorAll('.column-checkbox input').forEach(checkbox => {
        settings[checkbox.dataset.column] = checkbox.checked;
    });
    localStorage.setItem('columnSettings', JSON.stringify(settings));
}

// Load settings - không dùng trong ví dụ này
function loadColumnSettings() {
    const saved = localStorage.getItem('columnSettings');
    if (!saved) return;

    try {
        const settings = JSON.parse(saved);
        Object.keys(settings).forEach(columnName => {
            const checkbox = document.querySelector(`input[data-column="${columnName}"]`);
            if (checkbox) {
                checkbox.checked = settings[columnName];
                toggleTableColumn(columnName, settings[columnName]);
            }
        });
    } catch (e) {
        console.error('Error loading column settings:', e);
    }
}

// Khởi tạo khi page load
document.addEventListener('DOMContentLoaded', function () {
    loadColumnSettings();
});

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

    const startIndex = (currentPage - 1) * pageSize;
    const endIndex = Math.min(startIndex + pageSize, filteredData.length);
    const pageData = filteredData.slice(startIndex, endIndex);

    const currentPath = window.location.pathname.toLowerCase();

    pageData.forEach(item => {
        const row = document.createElement('tr');

        let actionHtml = '';
        if (currentPath.includes('/khoiphuc')) {
            actionHtml = `
                <div class="action-icons">
                    <button class="icon-btn" onclick="openActionModal('${item.MaHang}', 'restore')">
                        <i class="fas fa-undo"></i> 
                    </button>
                </div>`;
        } else {
            actionHtml = `
                <div class="action-icons">
                    <button class="icon-btn" onclick="editItem('${item.MaHang}')">
                        <i class="fas fa-edit"></i>
                    </button>
                    <button class="icon-btn" onclick="openActionModal('${item.MaHang}', 'delete')">
                        <i class="fas fa-trash"></i>
                    </button>
                </div>`;
        }
       
        // Render 
        row.innerHTML = `
            <td>${item.TenNhom ?? ''}</td>
            <td>${item.TenHang ?? ''}</td>
            <td>${item.MaHang ?? ''}</td>
            <td style="display: none;">${item.DonViTinhNhap ?? ''}</td>
            <td style="display: none;">${item.DonViTinhXuat ?? ''}</td>
            <td style="display: none;">${item.SoLuongQuyDoi ?? ''}</td>
            <td>${item.MaDuong ?? ''}</td>
            <td style="display: none;">${item.DuongDung ?? ''}</td>
            <td style="display: none;">${item.NuocSanXuat ?? ''}</td>
            <td style="display: none;">${item.HangSanXuat ?? ''}</td>
            <td>${item.HamLuong ?? ''}</td>
            <td>${item.HoatChat ?? ''}</td>
            <td>${item.MaAnhXa ?? ''}</td>
            <td style="display: none;">${item.MaPpCheBien ?? ''}</td>
            <td style="display: none;">${item.SlMin ?? ''}</td>
            <td style="display: none;">${item.SlMax ?? ''}</td>
            <td style="display: none;">${item.SoNgayDung ?? ''}</td>
            <td style="display: none;">${item.MaNhomChiPhi ?? ''}</td>
            <td style="display: none;">${item.NguonChiTra ?? ''}</td>
            <td style="display: none;">${item.DangBaoChe ?? ''}</td>
            <td>${item.Bhyt ? 'Có' : 'Không'}</td>
            <td style="display: none;">${item.MaBarCode ?? ''}</td>
            <td style="display: none;">${item.SoDangKy ?? ''}</td>
            <td style="display: none;">${item.QuyCachDongGoi ?? ''}</td>
            <td>${item.ThongTinThau ?? ''}</td>
            <td style="display: none;">${item.NhaThau ?? ''}</td>
            <td style="display: none;">${item.GiaThau ?? ''}</td>
            <td style="display: none;">${item.TiLeBHYT ?? ''}</td>
            <td style="display: none;">${item.TiLeThanhToan ?? ''}</td>
            <td>${actionHtml}</td>
        `;

        tbody.appendChild(row);
    });

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
let maHangHienTai = null;
let hanhDongHienTai = null; // 'delete' hoặc 'restore'

// Hiển thị modal xác nhận
function openActionModal(maHang, actionType) {
    maHangHienTai = maHang;
    hanhDongHienTai = actionType;

    const overlay = document.getElementById('actionOverlay');
    const form = document.getElementById('actionForm');
    const title = document.getElementById('modalTitle');
    const message = document.getElementById('modalMessage');
    const btn = document.getElementById('actionSubmitBtn');
    overlay.style.display = 'flex';
    console.log(overlay);

    document.getElementById('actionMaHang').value = maHang;

    if (actionType === 'delete') {
        title.textContent = 'Xác nhận xóa';
        message.textContent = 'Bạn có chắc chắn muốn xóa hàng hóa này không?';
        form.action = '/HangHoa/XoaHangHoa';
        btn.textContent = 'Xóa';
        btn.className = 'btn btn-danger';
    } else if (actionType === 'restore') {
        title.textContent = 'Khôi phục hàng hóa';
        message.textContent = 'Bạn có chắc chắn muốn khôi phục hàng hóa này không?';
        form.action = '/HangHoa/KhoiPhucHangHoa';
        btn.textContent = 'Khôi phục';
        btn.className = 'btn btn-primary';
    }

    overlay.style.display = 'flex';
}

// Đóng modal
function closeActionModal() {
    document.getElementById('actionOverlay').style.display = 'none';
    maHangHienTai = null;
    hanhDongHienTai = null;
}

// Cho phép click ngoài modal để đóng
document.getElementById('actionOverlay').addEventListener('click', function (e) {
    if (e.target === this) {
        closeActionModal();
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
        option.value = item.Id;
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

    if (typeof item.Id !== 'undefined') document.getElementById('Id').value = item.Id;

    setVal('MaHang', item.MaHang);
    setSel('MaNhom', item.IdNhom);
    setVal('TenHang', item.TenHang);
    setSel('DvtNhapId', item.DvtNhapId);
    setSel('DvtXuatId', item.DvtXuatId);
    setVal('SoLuongQuyDoi', item.SoLuongQuyDoi);
    
    setVal('MaDuongDung', item.MaDuong);
    setSel('DuongDungId', item.MaDuong);

    setSel('NuocId', item.NuocId);
    setSel('HangSxId', item.HangSxId);

    setVal('HamLuong', item.HamLuong);
    setVal('HoatChatText', item.HoatChat ?? item.HoatChatText ?? '');

    setVal('DangBaoChe', item.DangBaoChe);
    setVal('QuyCachDongGoi', item.QuyCachDongGoi);
    setVal('SoDangKy', item.SoDangKy);

    setVal('MaAnhXa', item.MaAnhXa);
    setVal('MaBarcode', item.MaBarcode ?? item.MaBarCode ?? '');

    setVal('MaPpCheBien', item.MaPpCheBien);
    setSel('NhomChiPhiId', item.NhomChiPhiId);
    setSel('NguonChiTraId', item.NguonChiTraId);

    setChk('Bhyt', item.Bhyt);
    setVal('TiLeBHYT', item.TiLeBhyt ?? item.TiLeBHYT ?? '');
    setVal('TiLeThanhToan', item.TiLeThanhToan);

    setVal('ThongTinThau', item.ThongTinThuoc ?? item.ThongTinThau ?? '');
    setSel('MaNhaThau', item.IdNhaThau);

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