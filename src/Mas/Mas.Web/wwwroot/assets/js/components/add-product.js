$(document).ready(function () {

    $("#txtBarCode").focus();
    GetCategories();

    $('#txtBarCode').keypress(function (e) {
        if (e.which == '13') {
            $("#txtName").focus();
        }
    });

    $("#txtBarCode, #txtName, #dtpUnit, #drpCategory, #TransferQuantity").change(function () {
        $(this).removeClass("is-invalid");
    });

    $("#addProduct").click(function () {
        let isValid = ValidateInput();
        if (isValid == false) {
            return;
        }
        $("#addProductForm").submit();
    });

    $("#IsConsignment :checkbox").change(function () {
        if (this.checked) {
            $("#consignment").show();
        } else {
            $("#consignment").hide();
        }
    });

    $("#IsSplitByImportPrice").change(function () {
        if (this.checked) {
            $("#txtDefaultImportPrice").attr("disabled");
        } else {
            $("#txtDefaultImportPrice").removeAttr("disabled");
        }
    })
});

function GetCategories() {
    $.ajax({
        url: categoryUrl,
        success: function (categories) {
            console.log(categories);
            let html = "";
            categories.forEach((category) => {
                html += '<option value="';
                html += category.id;
                html += '">';
                html += category.name;
                html += '</option>';
            });
            $("#drpCategory").append(html);
        }
    });
}

function ValidateInput() {
    let barCode = $("#txtBarCode").val();
    if (barCode == '') {
        $("#txtBarCode").addClass("is-invalid");
        return false;
    }

    let name = $("#txtName").val();
    if (name == '') {
        $("#txtName").addClass("is-invalid");
        return false;
    }
    let unitId = $("#dtpUnit").val();
    if (unitId == '') {
        $("#dtpUnit").addClass("is-invalid");
        return false;
    }

    let category = $("#drpCategory").val();

    if (category == '') {
        $("#drpCategory").addClass("is-invalid");
        return false;
    }

    let parentUnit = $("#txtParentUnitId").val();
    if (parentUnit != '') {
        let tranfer = $("#TransferQuantity").val();
        if (tranfer < 1) {
            return false;
        }
    }
}