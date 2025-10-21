// Dữ liệu từ server


        //console.log(nhomHangHoa);
        // Biến phân trang
        let currentPage = 1;
        let pageSize = 10;
        let filteredData = [...allData];
        let selectedNhom = "";

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
                            <td>${item.MaNhom ?? ''}</td>
                            <td>${item.TenHang ?? ''}</td>
                            <td>${item.MaHang ?? ''}</td>
                            <td>${item.MaDuong ?? item.DuongDungId ?? ''}</td>
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
        function searchData() {
            applyFilters();
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
 

        // Hàm sửa
        function editItem(maHang) {
            alert('Sửa mục: ' + maHang);
            // Thêm logic sửa ở đây
        }

        // Hàm xóa
        function deleteItem(maHang) {
            if (confirm('Bạn có chắc chắn muốn xóa mục này?')) {
                alert('Xóa mục: ' + maHang);
                // Thêm logic xóa ở đây
            }
        }
initNhomDropdown();
renderTable();

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
    const selectNhomHH = document.querySelector('select[name="NhomHangHoaId"]');
    nhomHangHoa.forEach(item => {
        const option = document.createElement('option');
        option.value = item.Id;
        option.textContent = item.TenNhom;
        selectNhomHH.appendChild(option);
    });
  
    // Đơn vị tính nhập
    const selectDVTNhap = document.querySelector('select[name="DonViTinhNhap"]');
    donViTinh.forEach(item => {
        const option = document.createElement('option');
        option.value = item.Id;
        option.textContent = item.TenDonVi;
        selectDVTNhap.appendChild(option);
    });

    // Đơn vị tính xuất
    const selectDVTXuat = document.querySelector('select[name="DonViTinhXuat"]');
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
        option.value = item.Id;
        option.textContent = item.TenDuong;
        selectDuongDung.appendChild(option);
    });

    // Nước sản xuất
    const selectNuocSX = document.querySelector('select[name="NuocSanXuat"]');
    nuocSanXuat.forEach(item => {
        const option = document.createElement('option');
        option.value = item.Id;
        option.textContent = item.TenNuoc;
        selectNuocSX.appendChild(option);
    });

    // Hãng sản xuất
    const selectHangSX = document.querySelector('select[name="HangSanXuat"]');
    hangSanXuat.forEach(item => {
        const option = document.createElement('option');
        option.value = item.Id;
        option.textContent = item.TenHang;
        selectHangSX.appendChild(option);
    });

    // Nhóm chi phí
    const selectNhomCP = document.querySelector('select[name="NhomChiPhi"]');
    nhomChiPhi.forEach(item => {
        const option = document.createElement('option');
        option.value = item.Id;
        option.textContent = item.TenNhom;
        selectNhomCP.appendChild(option);
    });

    // Nhà thầu
    const selectNhaThau = document.querySelector('select[name="NhaThau"]');
    nhaThau.forEach(item => {
        const option = document.createElement('option');
        option.value = item.Id;
        option.textContent = item.TenNhaThau;
        selectNhaThau.appendChild(option);
    });
}
initModalDropdowns(); 