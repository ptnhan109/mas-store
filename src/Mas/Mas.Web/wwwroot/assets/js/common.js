function GetCategories(id) {
    $.ajax({
        url: categoryUrl,
        success: function (categories) {
            let html = "";
            categories.forEach((category) => {
                html += '<option value="';
                html += category.id;
                html += '"';
                if (category.id == id) {
                    html += ' selected '
                }
                html +='>';
                html += category.name;
                html += '</option>';
            });
            $("#drpCategory").append(html);
        }
    });
}

function generatePaging(currentPage, totalPages, getDataCallBack) {
    let html = '<li class="page-item pager"><a href="javascript:;" class="page-link" data-page="prev">‹</a></li>';
    if (currentPage == 1) {
        html += '<li class="page-item active"><a href="javascript:;" class="page-link" data-page="1">1</a></li>';
        if (currentPage + 1 <= totalPages) {
            html += '<li class="page-item"><a href="javascript:;" class="page-link" data-page="2">2</a></li>';
        }
        if (currentPage + 2 <= totalPages) {
            html += '<li class="page-item"><a href="javascript:;" class="page-link" data-page="3">3</a></li>';
        }

    }

    if (currentPage > 1) {
        let prev = currentPage - 1;
        html += '<li class="page-item"><a href="javascript:;" class="page-link" data-page="';
        html += prev;
        html += '">';
        html += prev;
        html += '</a></li>';

        html += '<li class="page-item active"><a href="javascript:;" class="page-link" data-page="';
        html += currentPage;
        html += '">';
        html += currentPage;
        html += '</a></li>';

        if (currentPage + 1 <= totalPages) {
            let next = currentPage + 1;
            html += '<li class="page-item active"><a href="javascript:;" class="page-link" data-page="';
            html += next;
            html += '">';
            html += next;
            html += '</a></li>';
        }
    }

    html += '<li class="page-item pager"><a href="javascript:;" class="page-link" data-page="next">›</a></li>';

    $("#mas-pagingation").html(html);

    $(".page-item a").click(function () {
        let page = $(this).attr("data-page");
        if (page == "prev") {
            if (currentPage > 1) {
                currentPage--;
                getDataCallBack(currentPage);
            }
        }
        else if (page == "next") {
            if (currentPage < totalPages) {
                currentPage++;
                getDataCallBack(currentPage);
            }
        } else {
            currentPage = +page;
            getDataCallBack(currentPage);
        }
    });
}

function showMessage(type, message) {
    switch (type) {
        case 'primary': {
            Toastify({
                text: message,
                duration: 3000,
                newWindow: true,
                close: true,
                gravity: "bottom",
                position: "right",
                stopOnFocus: false,
                style: {
                    background: "linear-gradient(to right, #0000fc, #6060f5)",
                },
                onClick: function () { }
            }).showToast();
            break;
        }
        case 'success': {
            Toastify({
                text: message,
                duration: 3000,
                newWindow: true,
                close: true,
                gravity: "bottom",
                position: "right",
                stopOnFocus: false,
                style: {
                    background: "linear-gradient(to right, #00f642, #61f789)",
                },
                onClick: function () { }
            }).showToast();
            break;
        }
        case 'danger': {
            Toastify({
                text: message,
                duration: 3000,
                newWindow: true,
                close: true,
                gravity: "bottom",
                position: "right",
                stopOnFocus: false,
                style: {
                    background: "linear-gradient(to right, #f6051e, #f55d6d)",
                },
                onClick: function () { }
            }).showToast();
            break;
        }
    }
}

function GenerateRandom(length) {
    let result = "";
    for (let i = 0; i < length; i++) {
        result += (Math.random() * 10);
    }

    return result;
}