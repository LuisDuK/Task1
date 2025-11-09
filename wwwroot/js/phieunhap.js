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
    event.target.closest('.tab').classList.add('active');
}

function showDropdown(input) {
    const dropdown = input.nextElementSibling;
    dropdown.classList.add('show');
}

function filterProducts(input) {
    // Logic tìm kiếm
}

function selectProduct(item) {
    const row = item.closest('tr');
    const input = row.querySelector('.product-input');
    const detail = row.querySelector('.product-detail');
    const dropdown = row.querySelector('.autocomplete-dropdown');

    const name = item.querySelector('.item-name').textContent;
    input.value = name;

    dropdown.classList.remove('show');
    if (detail) detail.classList.add('show');
}

function deleteRow(btn) {
    if (confirm('Xóa dòng này?')) {
        btn.closest('tr').remove();
        updateRowNumbers();
    }
}

function updateRowNumbers() {
    const rows = document.querySelectorAll('#tableBody tr');
    rows.forEach((row, index) => {
        row.cells[0].textContent = index + 1;
    });
}

// Close dropdown when click outside
document.addEventListener('click', function (e) {
    if (!e.target.closest('.product-autocomplete')) {
        document.querySelectorAll('.autocomplete-dropdown').forEach(dropdown => {
            dropdown.classList.remove('show');
        });
    }
});
