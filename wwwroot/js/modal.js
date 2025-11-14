/// NotifyModal
let notifyTimer = null;
// Hàm hiển thị thông báo
function showNotifyModal({ type = "success", title = "Thành công", message = "", duration = 4000 } = {}) {
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
// Hàm tắt thông báo
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


function openConfirmDeleteModal(phieuNhapId) {
    // Lưu ID vào button
    $('#btnConfirmDelete').data('id', phieuNhapId);

    const overlay = document.getElementById('confirmDeleteOverlay');

    overlay.style.display = 'flex';
}

function closeConfirmDeleteModal() {
    document.getElementById('confirmDeleteOverlay').style.display = 'none';
}
$('#btnConfirmDelete').on('click', function () {
    const id = $(this).data('id');
    deletePhieuNhap(id);
});