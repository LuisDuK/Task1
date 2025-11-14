function switchTab(tabName) {
    // Ẩn tất cả tab content
    document.querySelectorAll('.tab-content').forEach(content => {
        content.classList.remove('active');
    });

    // Bỏ active khỏi tất cả tab
    document.querySelectorAll('.tab').forEach(tab => {
        tab.classList.remove('active');
    });

    // Hiện tab được chọn
    document.getElementById(tabName).classList.add('active');

    // Thêm active vào tab tương ứng
    const button = document.querySelector(`.tab[onclick="switchTab('${tabName}')"]`);
    if (button) button.classList.add('active');

    if (tabName === 'danhsach') {
        resetPhieuNhapForm();
    }
}
function resetPhieuNhapForm() {
    $('#tableChiTiet #tableBody').empty();
    rowIndex = 0;
    addNewRow();
    updateFooterTotals();

    // Reset thông tin header nếu bạn muốn
    $('#soHoaDon').val('');
    $('#ngayHoaDon').val('');
    $('#kyHieuHoaDon').val('');
    $('#ghiChu').val('');
    $('select[name="NoiCungCap"]').val('').trigger('change');
    $('#phieuNhapId').val(''); // đang không chỉnh sửa nữa
    $('#soPhieu').val('Auto');
}
function showDropdown(input) {
    const dropdown = input.nextElementSibling;
    dropdown.classList.add('show');
}
document.addEventListener('DOMContentLoaded', function () {
    initDropdowns();
    addNewRow(); // dòng đầu tiên
    renderTable();
});
// NẠP NƠI CUNG CẤP
function initDropdowns() {
    const selectNoiCungCap = document.querySelector('select[name="NoiCungCap"]');
    if (selectNoiCungCap) {
        selectNoiCungCap.innerHTML = '<option value="">-- Nơi cung cấp --</option>';
        nhaCungCapData.forEach(ncc => {
            const option = document.createElement('option');
            option.value = ncc.NhaCungCapId;
            option.textContent = ncc.TenNhaCungCap;
            selectNoiCungCap.appendChild(option);
        });
    }
    const selectNoiCungCap2 = document.querySelector('select[name="NoiCungCap2"]');
    if (selectNoiCungCap2) {
        selectNoiCungCap2.innerHTML = '<option value="">-- Nơi cung cấp --</option>';
        nhaCungCapData.forEach(ncc => {
            const option = document.createElement('option');
            option.value = ncc.NhaCungCapId;
            option.textContent = ncc.TenNhaCungCap;
            selectNoiCungCap2.appendChild(option);
        });
    }
}

// TẠO DÒNG MỚI 
function addNewRow() {
    const newRow = `
                    <tr>
                        <td class="text-center stt">${rowIndex + 1}</td>
                        <td>
                            <select class="form-control hangHoaSelect"
                                    name="ChiTietPhieuNhap[${rowIndex}].HangHoaId"
                                    style="width:100%"></select>
                        </td>
                        <td><input type="number" class="cell-input tdSoLuong"
                                   name="ChiTietPhieuNhap[${rowIndex}].SoLuong" ></td>
                        <td class="text-center tdDvtNhap">—</td>
                        <td class="text-center tdSlqd">—</td>
                        <td><input type="number" class="cell-input text-right tdDonGia"
                                   name="ChiTietPhieuNhap[${rowIndex}].DonGiaNhap" ></td>
                                <td class="text-right tdThanhTien" style="text-align: center;
            vertical-align: middle;"></td>
                        <td><input type="number" class="cell-input text-right tdChietKhauPhanTram"
                                   name="ChiTietPhieuNhap[${rowIndex}].ChietKhauPhanTram" ></td>
                        <td><input type="number" class="cell-input text-right tdChietKhauSoTien"
                                       name="ChiTietPhieuNhap[${rowIndex}].ChietKhauSoTien" ></td>
                        
                        <td><input type="number" class="cell-input text-right tdVat"
                                                   name="ChiTietPhieuNhap[${rowIndex}].Vat" ></td>
                        <td>
                            <select class="cell-select"
                                    name="ChiTietPhieuNhap[${rowIndex}].SoLo">
                                <option>-- Số lô --</option>
                            </select>
                        </td>
                        <td><input type="date" class="cell-input"
                                   name="ChiTietPhieuNhap[${rowIndex}].NgaySanXuat"></td>
                        <td><input type="date" class="cell-input"
                                   name="ChiTietPhieuNhap[${rowIndex}].HanSuDung"></td>
                        <td><input type="text" class="cell-input"
                                   name="ChiTietPhieuNhap[${rowIndex}].GhiChuKhac"> 
                        </td>
                        <td >
                            <button type="button" class="btn-delete" onclick="deleteRow(this)">
                                <i class="fas fa-times"></i>
                            </button>
                        </td>
                    </tr>
                `;

    $('#tableChiTiet #tableBody').append(newRow);

    const $newSelect = $('#tableChiTiet #tableBody tr:last .hangHoaSelect');
    initSelect2($newSelect); // gắn select2 cho dòng mới
    rowIndex++;
}

// KHỞI TẠO SELECT2    
function initSelect2($select) {
    const data = chiChonBHYT ? hangHoaData.filter(x => x.Bhyt === true) : hangHoaData;

    $select.select2({
        data: data.map(h => ({
            id: h.Id,
            text: h.TenHang,
            ...h
        })),
        placeholder: '-- Chọn hàng hóa --',
        templateResult: formatHangHoa,
        templateSelection: formatSelection,
        dropdownAutoWidth: true,
        width: 'resolve'
    });

    // Khi chọn hàng → tự điền các thông tin
    $select.on('select2:select', function (e) {
        const item = e.params.data;
        const $row = $(this).closest('tr');

        $row.find('.tdDvtNhap').text(item.DonViTinhNhap || '');
        $row.find('.tdSlqd').text(item.SoLuongQuyDoi || '');
        $row.find('.tdDonGia').val(item.GiaThau || 0);

        calcRow($row);
        addIfLastRow($row); // tự thêm dòng kế
    });
}
//LỌC “CHỈ CHỌN BẢO HIỂM Y TẾ”
$('#toggleBHYT').on('change', function () {
    chiChonBHYT = $(this).is(':checked');

    // Reinit lại tất cả dropdowns hiện tại
    $('.hangHoaSelect').each(function () {
        $(this).empty().off('select2:select').select2('destroy');
        initSelect2($(this));
    });
});
// FORMAT SELECT2 ITEM
function formatHangHoa(item) {
    if (!item.id) return item.text;
    return $(`
                    <div class="hanghoa-item">
                        <div class="left-col">
                            <div class="tenhang">
                                <strong>${item.TenHang}</strong> 
                            </div>
                            <div class="subtext">
                                <strong>Đường dùng:</strong> ${item.DuongDung || '—'}<br/>
                                <strong>Hàm lượng:</strong> ${item.HamLuong || '—'}<br/>
                                <strong>Hoạt chất:</strong> ${item.HoatChat || '—'}<br/>
                                <strong>Số ĐK:</strong> ${item.SoDangKy || '—'}<br/>
                                <strong>TT thầu:</strong> ${item.ThongTinThau || '—'}
                            </div>
                        </div>
                        <div class="right-col">
                            <div><strong>Nhóm:</strong> ${item.TenNhom || '—'}</div>
                            <div><strong>DVT nhập:</strong> ${item.DonViTinhNhap || '—'}</div>
                            <div><strong>DVT xuất:</strong> ${item.DonViTinhXuat || '—'}</div>
                            <div><strong>SLQD:</strong> ${item.SoLuongQuyDoi || '—'}</div>
                            <div><strong>Giá thầu:</strong> ${item.GiaThau ? item.GiaThau.toLocaleString() : '—'}</div>
                        </div>
                    </div>

                `);
}

function formatSelection(item) {
    return item.TenHang || item.text;
}

// TÍNH THÀNH TIỀN
$(document).on('input', '.tdSoLuong, .tdDonGia', function () {
    const $row = $(this).closest('tr');
    calcRow($row);
});

function calcRow($row, triggerBy = null) {
    const sl = parseFloat($row.find('.tdSoLuong').val()) || 0;
    const dg = parseFloat($row.find('.tdDonGia').val()) || 0;
    const vat = parseFloat($row.find('.tdVat').val()) || 0;
    const thanhTien = sl * dg;

    let ckPhanTram = parseFloat($row.find('.tdChietKhauPhanTram').val()) || 0;
    let ckSoTien = parseFloat($row.find('.tdChietKhauSoTien').val()) || 0;

    // Đồng bộ 2 chiều giữa % và số tiền
    if (triggerBy === "phantram") {
        ckSoTien = thanhTien * ckPhanTram / 100;
        $row.find('.tdChietKhauSoTien').val(ckSoTien.toFixed(0));
    } else if (triggerBy === "sotien" && thanhTien > 0) {
        ckPhanTram = (ckSoTien / thanhTien) * 100;
        $row.find('.tdChietKhauPhanTram').val(ckPhanTram.toFixed(2));
    }

    //const sauCK = thanhTien - ckSoTien;
    //const sauVAT = sauCK * (1 + vat / 100);

    $row.find('.tdThanhTien').text(thanhTien.toLocaleString());
    updateFooterTotals();
}
$(document).on('input', '.tdSoLuong, .tdDonGia, .tdVat', function () {
    const $row = $(this).closest('tr');
    calcRow($row);
});

$(document).on('input', '.tdChietKhauPhanTram', function () {
    const $row = $(this).closest('tr');
    calcRow($row, "phantram");
});

$(document).on('input', '.tdChietKhauSoTien', function () {
    const $row = $(this).closest('tr');
    calcRow($row, "sotien");
});
// CẬP NHẬT TỔNG Ở FOOTER
function updateFooterTotals() {
    let tongTienHang = 0, tongCK = 0, tongThue = 0, tongCong = 0;

    $('#tableChiTiet #tableBody tr').each(function () {
        const sl = parseFloat($(this).find('.tdSoLuong').val()) || 0;
        const dg = parseFloat($(this).find('.tdDonGia').val()) || 0;
        const ckTien = parseFloat($(this).find('.tdChietKhauSoTien').val()) || 0;
        const vat = parseFloat($(this).find('.tdVat').val()) || 0;

        const thanhTien = sl * dg;
        const sauCK = thanhTien - ckTien;
        const tienThue = sauCK * vat / 100;
        const tong = sauCK + tienThue;

        tongTienHang += thanhTien;
        tongCK += ckTien;
        tongThue += tienThue;
        tongCong += tong;
    });

    $('.footer-value').eq(0).text(tongTienHang.toLocaleString());
    $('.footer-value').eq(1).text(tongCK.toLocaleString());
    $('.footer-value').eq(2).text(tongThue.toLocaleString());
    $('.footer-value').eq(3).text(tongCong.toLocaleString());
}
// TỰ THÊM DÒNG MỚI
function addIfLastRow($row) {
    if (isLoadingPhieu) return; // đang load dữ liệu -> không auto thêm dòng
    const $last = $('#tableChiTiet #tableBody tr:last');
    if ($row.is($last)) addNewRow();
}
// XÓA DÒNG
function deleteRow(btn) {
    const $row = $(btn).closest('tr');
    const totalRows = $('#tableChiTiet #tableBody tr').length;

    // Nếu chỉ còn 1 dòng thì không xóa hết (phải giữ lại 1 dòng trống)
    if (totalRows === 1) {
        // Xóa dữ liệu trong dòng thay vì xóa dòng
        $row.find('input[type="number"], input[type="text"], input[type="date"]').val('');
        $row.find('select').val('').trigger('change');
        $row.find('.tdDvtNhap, .tdSlqd, .tdThanhTien').text('—');
        return;
    }

    // Nếu còn nhiều dòng, xóa dòng hiện tại
    $row.remove();

    // Cập nhật lại STT và tên input (index trong form)
    updateRowIndex();

    // Cập nhật lại tổng tiền ở footer
    updateFooterTotals();
}
function updateRowIndex() {
    rowIndex = 0;
    $('#tableChiTiet #tableBody tr').each(function () {
        $(this).find('.stt').text(rowIndex + 1);
        $(this).find('select, input').each(function () {
            const name = $(this).attr('name');
            if (name) {
                const newName = name.replace(/\[\d+\]/, `[${rowIndex}]`);
                $(this).attr('name', newName);
            }
        });
        rowIndex++;
    });
}
function clearForm() {
    if (!confirm("Bạn có chắc muốn xóa trắng toàn bộ phiếu nhập không?")) return;

    // Xóa hết các dòng chi tiết trong bảng
    $('#tableChiTiet #tableBody').empty();

    // Reset chỉ số dòng
    rowIndex = 0;

    // Thêm lại 1 dòng trống đầu tiên
    addNewRow();

    // Reset lại phần footer tổng cộng
    updateFooterTotals();

}

async function savePhieuNhap() {
    const chiTiet = [];

    $('#tableChiTiet #tableBody tr').each(function () {
        const $tr = $(this);
        const hangHoaId = parseInt($(this).find('.hangHoaSelect').val());
        if (!hangHoaId) return;

        const soLuong = parseFloat($tr.find('.tdSoLuong').val()) || 0;
        const donGia = parseFloat($tr.find('.tdDonGia').val()) || 0;

        if (!hangHoaId) {
            
            return;
        }
        if (soLuong <= 0 && donGia <= 0) {
            return;
        }

        chiTiet.push({
            HangHoaId: hangHoaId,
            SoLuong: parseFloat($(this).find('.tdSoLuong').val()) || 0,
            SoLuongQuyDoi: parseFloat($(this).find('.tdSlqd').text()) || null,
            DonGiaNhap: parseFloat($(this).find('.tdDonGia').val()) || 0,
            ChietKhau: parseFloat($(this).find('.tdChietKhauPhanTram').val()) || 0,
            ChietKhauSoTien: parseFloat($(this).find('.tdChietKhauSoTien').val()) || 0,
            Vat: parseFloat($(this).find('.tdVat').val()) || 0,
            NgaySanXuat: $(this).find('input[name*="NgaySanXuat"]').val() || null,
            HanSuDung: $(this).find('input[name*="HanSuDung"]').val() || null,
            SoLo: $(this).find('select[name*="SoLo"]').val() || null,
            GhiChu: $(this).find('input[name*="GhiChu"]').val() || null
        });
    });

    if (chiTiet.length === 0) {
        alert('Chưa có hàng hóa nào được chọn!');
        return;
    }

    const phieuNhap = {
        PhieuNhapId: $('#phieuNhapId').val() ? parseInt($('#phieuNhapId').val()) : null,
        SoHoaDon: $('#soHoaDon').val(),
        NgayHoaDon: $('#ngayHoaDon').val(),
        KyHieuHoaDon: $('#kyHieuHoaDon').val(),
        NhaCungCapId: parseInt($('select[name="NoiCungCap"]').val()),
        GhiChu: $('#ghiChu').val(),
        NguoiNhap: 'Admin',
        ChiTietPhieuNhaps: chiTiet
    };

    try {
        const res = await fetch('/PhieuNhapKho/SavePhieuNhap', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(phieuNhap)
        });

        const data = await res.json();
        showSuccess(`Lưu phiếu thành công`);
        setTimeout(() => {
           location.reload();
        }, 1500);
    } catch (err) {
        showError('Lưu phiếu thất bại thất bại');
    }
}

// Tab2

const $filterHangHoa = $('#filterHangHoa');
$(document).ready(function () {
    initFilterHangHoa($filterHangHoa);
});

function initFilterHangHoa($select) {
    const data = hangHoaData;
    $select.select2({
        data: data.map(h => ({
            id: h.Id,
            text: h.TenHang,
            ...h
        })),
        placeholder: '-- Chọn hàng hóa --',
        templateResult: formatHangHoa,
        templateSelection: formatSelection,
        dropdownAutoWidth: true,
        width: 'resolve'
    });
}

// EXPORT PDF
function exportPdf(phieuNhapId) {

    fetch('/PhieuNhapKho/ExportPdf', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ id: phieuNhapId })
    })
        .then(response => {
            if (!response.ok) throw new Error("Xuất PDF thất bại");
            console.log(response);
            return response.blob();
        })
        .then(blob => {
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = `PhieuNhap_${phieuNhapId}.pdf`;
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
            document.body.removeChild(a);

            showSuccess("Đã xuất PDF thành công!");
        })
        .catch(err => {
            console.error(err);
            showError("Lỗi khi xuất PDF: " + err.message);
        });
}
// EXPORT PDF
function exportExcel(phieuNhapId) {

    fetch('/PhieuNhapKho/ExportExcel', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ id: phieuNhapId })
    })
        .then(response => {
            if (!response.ok) throw new Error("Xuất Excel thất bại");
            return response.blob();
        })
        .then(blob => {
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = `PhieuNhap_${phieuNhapId}.xlsx`;
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
            document.body.removeChild(a);

            showSuccess("Đã xuất Excel thành công!");
        })
        .catch(err => {
            console.error(err);
            showError("Lỗi khi xuất Excel: " + err.message);
        });
}

function deletePhieuNhap(phieuNhapId) {
    closeConfirmDeleteModal();
    fetch(`/PhieuNhapKho/DeletePhieuNhap`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ id: phieuNhapId })
    })
        .then(response => {
            if (!response.ok) throw new Error("Xóa thất bại");
            return response.json();
        })
        .then(data => {
            showSuccess("Đã xóa thành công!");
            // Reload hoặc remove row khỏi bảng
            //location.reload();
        })
        .catch(err => {
            console.error(err);
            alert("Lỗi: " + err.message);
        });
}

let editingPhieuId = null;
let isLoadingPhieu = false;
$(document).on('click', '.btn-edit', function () {
    const id = $(this).data('id');
    loadPhieuNhapForEdit(id);
});
async function loadPhieuNhapForEdit(id) {
    try {
        const res = await fetch(`/PhieuNhapKho/GetPhieuNhap?id=${id}`);
        if (!res.ok) {
            showError('Không tải được phiếu nhập');
            return;
        }
        const data = await res.json();
    
        // set trạng thái đang chỉnh sửa
        editingPhieuId = data.phieuNhapId || data.PhieuNhapId;
        $('#phieuNhapId').val(editingPhieuId);

        // header
        $('#soPhieu').val(data.maPhieuNhap || data.MaPhieuNhap || ''); // hiển thị mã phiếu
        $('#soHoaDon').val(data.soHoaDon || data.SoHoaDon || '');
        $('#ngayHoaDon').val(data.ngayHoaDon ? data.ngayHoaDon.substring(0, 10) : '');
        $('#kyHieuHoaDon').val(data.kyHieuHoaDon || data.KyHieuHoaDon || '');
        $('#ghiChu').val(data.ghiChu || data.GhiChu || '');
        $('select[name="NoiCungCap"]').val(data.nhaCungCapId || data.NhaCungCapId || '').trigger('change');

        // clear bảng chi tiết
        isLoadingPhieu = true;
        $('#tableChiTiet #tableBody').empty();
        rowIndex = 0;

        const chiTiet = data.chiTiet || data.ChiTiet || [];
        console.log(chiTiet);
        // tạo dòng theo chi tiết
        chiTiet.forEach((ct, idx) => {
            addNewRow();
            const $row = $('#tableChiTiet #tableBody tr').last();

            // set hàng hóa
            const hangHoaId = ct.hangHoaId || ct.HangHoaId;
            $row.find('.hangHoaSelect').val(hangHoaId).trigger('change');
            
            // ghi đè lại các giá trị số lượng, đơn giá, ck, vat, ngày...
            $row.find('.tdSoLuong').val(ct.soLuong || ct.SoLuong || 0);
            $row.find('.tdDvtNhap').text(ct.donViTinhNhap || '');
            $row.find('.tdSlqd').text(ct.soLuongQuyDoi || ct.SoLuongQuyDoi || '');
            $row.find('.tdDonGia').val(ct.donGiaNhap || ct.DonGiaNhap || 0);
            $row.find('.tdChietKhauPhanTram').val(ct.chietKhauPhanTram || ct.ChietKhauPhanTram || 0);
            $row.find('.tdChietKhauSoTien').val(ct.chietKhauSoTien || ct.ChietKhauSoTien || 0);
            $row.find('.tdVat').val(ct.vat || ct.Vat || 0);
            $row.find('input[name*="NgaySanXuat"]').val(ct.ngaySanXuat || ct.NgaySanXuat ? (ct.ngaySanXuat || ct.NgaySanXuat).substring(0, 10) : '');
            $row.find('input[name*="HanSuDung"]').val(ct.hanSuDung || ct.HanSuDung ? (ct.hanSuDung || ct.HanSuDung).substring(0, 10) : '');
            $row.find('select[name*="SoLo"]').val(ct.soLo || ct.SoLo || '');
            $row.find('input[name*="GhiChu"]').val(ct.ghiChu || ct.GhiChu || '');

            calcRow($row);
        });

        // thêm 1 dòng trống cuối cho user nhập thêm
        addNewRow();
        isLoadingPhieu = false;

        // cập nhật tổng
        updateFooterTotals();

        // chuyển qua tab Lập phiếu
        switchTab('lapphieu');
    } catch (err) {
        console.error(err);
        showError('Lỗi khi tải phiếu nhập để chỉnh sửa');
        isLoadingPhieu = false;
    }
}

let currentPage = 1;
let pageSize = 10; 
function renderTable() {
    const tbody = document.getElementById('tbodyPhieuNhap');
    tbody.innerHTML = "";

    const start = (currentPage - 1) * pageSize;
    const end = start + pageSize;

    const pageData = phieuNhapData.slice(start, end);

    pageData.forEach((p, index) => {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td class="text-center">${start + index + 1}</td>
            <td>${p.SoHoaDon}</td>
            <td>${new Date(p.NgayHoaDon).toLocaleDateString()}</td>
            <td>${new Date(p.NgayNhap).toLocaleDateString()}</td>
            <td>${p.TenNhaCungCap}</td>
            <td class="text-right">${p.TongTienHang?.toLocaleString()}</td>
            <td class="text-right">${p.TongChietKhau?.toLocaleString()}</td>
            <td class="text-right">${p.TongThue?.toLocaleString()}</td>
            <td class="text-right">${p.TongCong?.toLocaleString()}</td>
            <td>${p.NguoiNhap}</td>
            <td>${p.MaPhieuNhap}</td>
            <td class="text-center">

                <button class="action-icon-btn btn-in-pdf" onclick="exportPdf(${p.PhieuNhapId})">
                    <i class="fas fa-file-pdf"></i>
                </button>

                <button class="action-icon-btn btn-in-excel" onclick="exportExcel(${p.PhieuNhapId})">
                    <i class="fas fa-file-excel"></i>
                </button>

                <button class="action-icon-btn btn-edit" data-id="${p.PhieuNhapId}">
                    <i class="fas fa-edit"></i>
                </button>

                <button class="action-icon-btn btn-delete" onclick="openConfirmDeleteModal(${p.PhieuNhapId})">
                    <i class="fas fa-trash"></i>
                </button>
            </td>
        `;
        tbody.appendChild(tr);
    });

    renderPagination();
}
function renderPagination() {
    const totalPages = Math.ceil(phieuNhapData.length / pageSize);
    const container = document.getElementById("pageNumbers");

    container.innerHTML = "";

    // Nút Prev
    if (currentPage > 1) {
        container.innerHTML += `<button onclick="gotoPage(${currentPage - 1})">«</button>`;
    }

    for (let i = 1; i <= totalPages; i++) {
        container.innerHTML += `
            <button class="${i === currentPage ? 'active' : ''}" onclick="gotoPage(${i})">
                ${i}
            </button>
        `;
    }

    // Nút Next
    if (currentPage < totalPages) {
        container.innerHTML += `<button onclick="gotoPage(${currentPage + 1})">»</button>`;
    }
}
function gotoPage(page) {
    currentPage = page;
    renderTable();
}