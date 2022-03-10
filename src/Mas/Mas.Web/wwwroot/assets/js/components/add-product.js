$(document).ready(function () {

    $("#txtBarCode").focus();
    
    $("#consignment").hide();
    $('#txtBarCode').keypress(function (e) {
        if (e.which == '13') {
            $("#txtName").focus();
        }
    });

    $("input[type=number]").change(function () {
        if ($(this).val() < 0) {
            $(this).val(0);
        }
    });

    $("#txtTransferQuantity").change(function () {
        console.log($("#IsSplitByImportPrice").val());
        if ($("#IsSplitByImportPrice").checked) {
            let importPrice = $("#txtParentImportPrice").val();
            if (importPrice > 0) {
                let price = Math.round(importPrice / (this).val());
                $("#txtDefaultImportPrice").val(price);
            }
        }
    });

    $("#txtParentImportPrice").change(function () {
        if ($("#IsSplitByImportPrice").val()) {
            let quantity = $("#txtTransferQuantity").val();
            if (quantity > 0) {
                let price = Math.round($(this).val() / quantity);
                $("#txtDefaultImportPrice").val(price);
            }
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

    //$("#IsSplitByImportPrice").change(function () {
    //    if (this.checked) {
    //        $("#txtDefaultImportPrice").attr("disabled","disabled");
    //    } else {
    //        $("#txtDefaultImportPrice").removeAttr("disabled");
    //    }
    //});
});

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

    let defaultSell = $("#txtDefaultSellPrice").val();
    if (defaultSell < 1) {
        $("#txtDefaultSellPrice").addClass("is-invalid");
        return false;
    }

    let defaultImport = $("#txtDefaultImportPrice").val();
    if (defaultImport < 1) {
        $("#txtDefaultImportPrice").addClass("is-invalid");
        return false;
    }


}