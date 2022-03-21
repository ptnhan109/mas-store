﻿$(document).ready(function () {

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

    $("#txtBarCode, #txtName, #dtpUnit, #drpCategory").change(function () {
        $(this).removeClass("is-invalid");
    });

    $("#addProduct").click(function () {
        let isValid = ValidateInput();
        if (isValid == false) {
            return;
        }

        let barcode = $("#txtBarCode").val();
        let name = $("#txtName").val();
        let category = $("#drpCategory").val();

        let unit = $("#dtpUnit").val();
        let importPrice = $("#txtDefaultImportPrice").val();
        let sellPrice = $("#txtDefaultSellPrice").val();
        let wholeSellPrice = $("#txtDefaultWholeSalePrice").val();

        var prices = [];

        $("#list-prices > tr").each(function () {
            let parentBarCode = $(this).find("input[name=TransferBarCode]").val();
            let parentUnit = $(this).find("select[name=ParentUnitId]").val();
            let parentTransfer = $(this).find("input[name=TransferQuantity]").val();
            let parentImport = $(this).find("input[name=ParentImportPrice]").val();
            let parentSell = $(this).find("input[name=ParentSellPrice]").val();
            let parentWholeSellPrice = $("input[name=ParentWholeSalePrice]").val();

            prices.push({
                ParentUnitId: +parentUnit,
                TransferQuantity: +parentTransfer,
                ParentImportPrice: +parentImport,
                ParentSellPrice: +parentSell,
                TransferBarCode: parentBarCode,
                WholeSalePrice: +parentWholeSellPrice
            });
        });

        let request = {
            BarCode: barcode,
            Name: name,
            DefaultSellPrice: +sellPrice,
            DefaultImportPrice: +importPrice,
            Inventory: 0, // TODO: manage inventory module
            CategoryId: category,
            CloseToDate: 0, // TODO:
            UnitId: +unit,
            Prices: prices,
            WholeSellPrice: +wholeSellPrice
        };

        // submit
        $.ajax({
            url: addProdUrl,
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            traditional: true,
            data: JSON.stringify(request),
            success: function (data) {
                showMessage("success", data);
            }
        });
    });

    $("#IsConsignment :checkbox").change(function () {
        if (this.checked) {
            $("#consignment").show();
        } else {
            $("#consignment").hide();
        }
    });

    $("a").click(function () {
        if ($(this).hasClass("add-transfer-session")) {
            let item = $("#item-price-html").html();
            let html = "<tr><td>" + item + "</td></tr>";
            $("#list-prices").append(html);

            $("a.remove-transfer").click(function () {

                $(this).closest("tr").remove();

            });
        }
    });
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

    let defaultWholeSellPrice = $("#txtDefaultWholeSalePrice").val();
    if (defaultWholeSellPrice < 1) {
        $("#txtDefaultWholeSalePrice").addClass("is-invalid");
        return false;
    }


}