const currencyFractionDigits = new Intl.NumberFormat('de-DE', {
    style: 'currency',
    currency: 'VND',
}).resolvedOptions().maximumFractionDigits;

var totalMoney = 0;
var manufatureId = "";
var products = [];
var manufactureId = "";
$(document).ready(function () {
    $("#product-search-suggestion").css({
        'width': ($("#productCode").width() + 'px')
    });
    $("#customer-search-suggestion").css({
        'width': ($("#customer-name").width() + 'px')
    });

    $('#productCode').focus();

    $('#productCode').keyup(function () {
        var productCode = $('#productCode').val();
        DisplaySuggestion(productCode);
    });

    $('#btn-cancel').click(function () {
        window.location.href = cancel;
    });

    $('#productCode').on("keypress", function (e) {
        let qrCode = $('#productCode').val();
        if (e.keyCode == 13) {
            if (qrCode !== "") {
                AddProductToImportInvoice(qrCode);
                return false;
            } else {
                alert("Hãy nhập từ khóa");
            }
        }

        if (e.keyCode == 8) {
            DisplaySuggestion(qrCode);
        }
    });

    $('#manufacture-name').on("keypress", function (e) {
        let manufacture = $("#manufacture-name").val();
        if (e.keyCode == 8) {
            DisplayManufactureSuggestion(manufacture);
        }
    });

    $('#manufacture-name').keyup(function () {
        var productCode = $('#manufacture-name').val();
        DisplayManufactureSuggestion(productCode);
    });

    $("#btn-checkout").click(function () {
        AddImportInvoices();
        CleanImportInvoices();
    });

    $("#appendDiscount").click(function () {
        let barcode = $("#modal-barcode").val();
        let value = $("#value-discount").val();
        let currentPrice = $("#modal-product-price").attr("product-price");
        let newValue;
        let discountValue;
        let isPercent = $("input[name=isPercent]:checked").val();
        if (isPercent == "1") {
            discountValue = Math.round(currentPrice * value / 100);
            newValue = currentPrice - discountValue;
        } else {
            newValue = currentPrice - value;
            discountValue = value;
        }

        $("#cart-list").find("tr").each(function (index, row) {
            let barcodeItem = $(row).attr("barcode");

            if (barcodeItem === barcode) {
                $(row).find("td:nth-child(4)").attr("product-price", newValue);
                let html = '<p><small class="text-danger">-' + discountValue + '</small></p>';
                $(row).find("td:nth-child(4) > p").remove();
                if (discountValue > 0) {
                    $(row).find("td:nth-child(4)").append(html);
                }

                let quantity = $(this).find("input[type=number]").val();
                $(row).find("strong.item-price-total").html((newValue * quantity).toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits }));
                updateTotalMoney();
                $("#modal-discount").modal("hide");
                $("#modal-barcode").val();
                $("#value-discount").val(0);
                return false;
            }
        });
    });

    $("#btn-order-discount").click(function () {
        $("#modal-order-discount").modal("show");
    });

    $("#order-append-discount").click(function () {
        DiscountOrder();
        $("#modal-order-discount").modal("hide");
    });
});

function DisplaySuggestion(keyword) {
    let html = "";
    if (keyword == "") {
        $('#product-search-suggestion').html("");
    } else {
        $.ajax({
            url: urlSearchProd,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            traditional: true,
            data: {
                keyword: keyword
            },
            success: function (data) {
                console.log(data);
                data.items.forEach(function (prod) {
                    html += '<li class="list-group-item"><a prod-barcode="';
                    html += prod.barCode;
                    html += '" class="suggestion-item" href="javascript:;"><strong>';
                    html += prod.name.toUpperCase();
                    html += " - ";
                    html += prod.defaultSell;
                    html += '</strong></a></li>';
                });

                $('#product-search-suggestion').html(html);
                $("a.suggestion-item").click(function () {
                    let barcode = $(this).attr("prod-barcode");
                    AddProductToImportInvoice(barcode);
                    $('#product-search-suggestion').html("");
                });
            }
        })
    }
}

function AddProductToImportInvoice(barcode) {
    let url = urlGetProd + '?barcode=' + barcode;

    $.ajax({
        url: url,
        success: function (data) {
            let isExsit = false;
            let isSplit = $('#isSplit').is(":checked");
            console.log(isSplit);
            // no split row
            if (!isSplit) {
                $("#cart-list > tr").each(function () {
                    if ($(this).attr("barcode") == data.barCode) {
                        let total;
                        isExsit = true;
                        let quantity = $(this).find('input[type=number]').val();
                        quantity++;

                        $(this).find('input[type=number]').val(quantity);
                        data.prices.forEach(function (element) {

                            if (element.isDefault) {
                                let newPrice = element.importPrice * quantity;
                                total = newPrice.toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits });
                                totalMoney += element.importPrice;
                            }
                        });

                        $(this).find('strong.item-price-total').html(total);
                        updateTotalMoney();
                    }
                });

                if (!isExsit) {
                    AppendProductHtml(data);
                    products.push(data);
                }
            } else {
                AppendProductHtml(data);
                products.push(data);
            }
            
            $("#productCode").val("");
        }
    });
}

function AppendProductHtml(data) {

    let importPrice;
    let quantity;
    let html = '<tr barcode="';
    let rowId = GenerateRandom(10);
    html += data.barCode;
    html += '" ';
    html += 'prod-id="';
    html += data.id;
    html += '" ';
    html += 'row-id="';
    html += rowId;
    html += '"';
    html += '><td class="text-bold-500" style="text-transform: uppercase;">';
    html += data.name;
    html += '</td><td><fieldset class="form-group position-relative"><select class="form-select" id="basicSelect">';
    data.prices.forEach((element) => {
        if (element.isDefault) {
            importPrice = element.importPrice;
            quantity = element.quantity;
            html += '<option selected value="';
            html += element.barCode;
            html += '">';
            html += element.unit.name;
            html += '</option>';
        } else {
            html += '<option value="';
            html += element.barCode;
            html += '">';
            html += element.unit.name;
            html += '</option>';
        }
    });
    html += '</select></fieldset></td><td class="text-bold-500"><div class="form-group position-relative"><input type="number" class="form-control" value="';
    html += quantity;
    html += '"></div></td><td product-price="';
    html += importPrice;
    html += '"><a href="javascript:;" class="add-discount"><strong class="item-price">';
    html += importPrice.toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits });
    html += '</strong></a></td><td class="text-primary text-bold-500"><strong class="item-price-total">';
    html += (importPrice * quantity).toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits });
    html += '</strong></td><td><a href="javascript:;"><i class="badge-circle badge-circle-light-secondary font-medium-1" data-feather="mail"></i></a></td>';
    html += '<td><a href="javascript:;" class="product-remove" barcode="';
    html += data.barCode;
    html += '" row-id="';
    html += rowId;
    html += '"><i class="bi bi-trash text-danger"></i></a></td>';
    html += '<td class="d-none" product-price="';
    html += importPrice;
    html += '"></td></tr>';

    $("#cart-list").append(html);
    totalMoney += importPrice;
    updateTotalMoney();

    $("input[type=number]").change(function () {
        let price = $(this).closest("tr").find("td:nth-child(4)").attr("product-price");
        let quantity = $(this).val();
        let money = (price * quantity).toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits });
        $(this).closest("tr").find("strong.item-price-total").html(money);
        updateTotalMoney();
    });

    $("a.add-discount").click(function () {
        let name = $(this).closest("tr").find("td:nth-child(1)").html();
        let price = $(this).closest("tr").find("td:nth-child(8)").attr("product-price");
        let barcode = $(this).closest("tr").attr("barcode");

        $("#modal-barcode").val(barcode);
        $("#modal-product-price").attr("product-price", price);
        $("#modal-product-price").val((+price).toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits }));
        $("#discount-title").html(name);
        $("#modal-discount").modal("show");
    });



    $("select.form-select").change(function () {
        let currentBarCode = $(this).val();
        let priceChange;
        products.forEach(function (product) {
            product.prices.forEach(function (price) {
                if (price.barCode == currentBarCode) {
                    priceChange = price;
                }
            });
        });
        $(this).closest("tr").attr("barcode", priceChange.barCode);
        $(this).closest("tr").find("td:nth-child(4)").attr("product-price", priceChange.importPrice);
        $(this).closest("tr").find("td:nth-child(8)").attr("product-price", priceChange.importPrice);
        $(this).closest("tr").find("strong.item-price").html(priceChange.importPrice.toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits }));

        let quantity = $(this).closest("tr").find("td:nth-child(3)").find("input[type=number]").val();
        let money = (priceChange.importPrice * quantity).toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits });

        $(this).closest("tr").find("strong.item-price-total").html(money);
        updateTotalMoney();
    });

    $("a.product-remove").click(function () {
        let barcode = $(this).attr("row-id");
        let remove = "tr[row-id=" + barcode + "]";
        $(remove).remove();
        updateTotalMoney();
    });
}

function updateTotalMoney() {
    let money = 0;
    $("#cart-list > tr").each(function () {
        let quantity = $(this).find("td:nth-child(3)").find("input[type=number]").val();
        let price = $(this).find("td:nth-child(4)").attr("product-price");
        money += quantity * price;
    });
    let t = money.toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits });
    $("#order-total-money").html(t + "đ");
    $("#order-total-money").attr("total-money", money);
    DiscountOrder();
}

function DiscountOrder() {
    let current = $("#order-total-money").attr("total-money");
    let isPercent = $("input[name=isPercent-order]:checked").val();
    let value = $("#order-value-discount").val();
    let discount;
    let newValue;
    if (isPercent == 1) {
        discount = Math.round(current * value / 100);
    } else {
        discount = value;
    }
    newValue = current - discount;

    $("#order-discount-money").html(discount.toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits }));
    $("#order-discount-money").attr("order-discount", discount);
    $("#order-total-money-checkout").attr("total-money", newValue);
    $("#order-total-money-checkout").html(newValue.toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits }));
}

function GenerateRandom(length) {
    let result = "";
    for (let i = 0; i < length; i++) {
        result += Math.floor(Math.random() * 10);
    }

    return result;
}

function AddImportInvoices() {
    let items = [];
    let manufactureName = $("#manufacture-name").val();
    if (manufactureName == "" || manufactureName == undefined) {
        showMessage("danger", "Vui lòng chọn nhà phân phối");

        return;
    }

    let note = $("#order-note").val();
    let discount = $("#order-discount-money").attr("order-discount");
    $("#cart-list > tr").each(function () {
        let barcode = $(this).attr("barcode");
        let id = $(this).attr("prod-id");
        let name = $(this).find("td:nth-child(1)").text();
        let unit = $(this).find("td:nth-child(2)").find("select.form-select option:selected").text();
        let quantity = $(this).find("td:nth-child(3)").find("input[type=number]").val();
        let price = $(this).find("td:nth-child(8)").attr("product-price");
        let afterDiscount = $(this).find("td:nth-child(4)").attr("product-price");
        let discount = price - afterDiscount;

        items.push({
            Name: name,
            UnitName: unit,
            BarCode: barcode,
            CurrentPrice: +price,
            Discount: +discount,
            Quantity: +quantity,
            ProductId: id
        });

        let totalAmount = 0;
        let totalDiscount = 0;
    });

    let request = {
        Note: note,
        Discount: +discount,
        Items: items,
        ManufactureId: manufactureId,
        Code: "",
        CreatedBy: ""
    };

    console.log(request);
    //if ($('#is-print-invoice').is(":checked")) {
    //    $.ajax({
    //        url: invoiceUrl,
    //        type: "POST",
    //        dataType: "json",
    //        contentType: "application/json; charset=utf-8",
    //        traditional: true,
    //        data: JSON.stringify(request),
    //        success: function (data) {
    //            PrintInvoice(data);
    //        }
    //    });
    //}

    $.ajax({
        url: addInvoiceUrl,
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        traditional: true,
        data: JSON.stringify(request),
        success: function (data) {
            showMessage("success", data);
        }
    });

}

function CleanImportInvoices() {
    $("#cart-list").html();
    $("#order-value-discount").val(0);
    updateTotalMoney();
}

function DisplayManufactureSuggestion(keyword) {
    let html = "";
    if (keyword == "") {
        $("#customer-search-suggestion").html("");
    } else {
        $.ajax({
            url: urlSearchManufacture,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            traditional: true,
            data: {
                keyword: keyword
            },
            success: function (data) {
                data.items.forEach(function (manufacture) {
                    html += '<li class="list-group-item"><a manufacture-id="';
                    html += manufacture.id;
                    html += '" class="suggestion-item" href="javascript:;"><strong>';
                    html += manufacture.name.toUpperCase();
                    html += " - ";
                    html += manufacture.group;
                    html += '</strong></a></li>';
                });

                $('#customer-search-suggestion').html(html);
                $("a.suggestion-item").click(function () {
                    manufactureId = $(this).attr("manufacture-id");
                    $("#manufacture-name").val($(this).text().split('-')[0]);
                    $('#customer-search-suggestion').html("");
                });
            }
        });
    }
}