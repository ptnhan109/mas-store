const currencyFractionDigits = new Intl.NumberFormat('de-DE', {
    style: 'currency',
    currency: 'VND',
}).resolvedOptions().maximumFractionDigits;

var totalMoney = 0;
var customerId = "";
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

    $('#productCode').on("keypress", function (e) {
        let qrCode = $('#productCode').val();
        if (e.keyCode == 13) {
            if (qrCode !== "") {
                AddProductToCart(qrCode);
                return false;
            } else {
                alert("Hãy nhập từ khóa");
            }
        }

        if (e.keyCode == 8) {
            DisplaySuggestion(qrCode);
        }
    });

    $('#customer-name').on("keypress", function (e) {
        let customer = $("#customer-name").val();
        if (e.keyCode == 8) {
            DisplayCustomerSuggestion(customer);
        }
    });

    $('#customer-name').keyup(function () {
        var productCode = $('#customer-name').val();
        DisplayCustomerSuggestion(productCode);
    });

    $("#btn-checkout").click(function () {
        AddInvoices();
        CleanOrder();
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
var products = [];

function AddProductToCart(barcode) {
    let isWholeSale = $("#saleType").val();
    let url = urlGetProd + '?barcode=' + barcode + '&isWholeSale=' + isWholeSale;

    $.ajax({
        url: url,
        success: function (data) {
            let isExsit = false;
            $("#cart-list > tr").each(function () {
                if ($(this).attr("barcode") == data.barCode) {
                    let total;
                    isExsit = true;
                    let quantity = $(this).find('input[type=number]').val();
                    quantity++;

                    $(this).find('input[type=number]').val(quantity);
                    data.prices.forEach(function (element) {

                        if (element.isDefault) {
                            let newPrice = element.sellPrice * quantity;
                            total = newPrice.toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits });
                            totalMoney += element.sellPrice;
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
            $("#productCode").val("");
        }
    });
}

function AppendProductHtml(data) {

    let sellPrice;
    let quantity;
    let html = '<tr barcode="';
    html += data.barCode;
    html += '" ';
    html += 'prod-id="';
    html += data.id;
    html += '" ';
    html += '><td class="text-bold-500" style="text-transform: uppercase;">';
    html += data.name;
    html += '</td><td><fieldset class="form-group position-relative"><select class="form-select" id="basicSelect">';
    data.prices.forEach((element) => {
        if (element.isDefault) {
            sellPrice = element.sellPrice;
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
    html += sellPrice;
    html += '"><a href="javascript:;" class="add-discount"><strong class="item-price">';
    html += sellPrice.toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits });
    html += '</strong></a></td><td class="text-primary text-bold-500"><strong class="item-price-total">';
    html += (sellPrice * quantity).toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits });
    html += '</strong></td><td><a href="javascript:;"><i class="badge-circle badge-circle-light-secondary font-medium-1" data-feather="mail"></i></a></td>';
    html += '<td><a href="javascript:;" class="product-remove" barcode="';
    html += data.barCode;
    html += '"><i class="bi bi-trash text-danger"></i></a></td>';
    html += '<td class="d-none" product-price="';
    html += sellPrice;
    html += '"></td></tr>';

    $("#cart-list").append(html);
    totalMoney += sellPrice;
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
        $(this).closest("tr").find("td:nth-child(4)").attr("product-price", priceChange.sellPrice);
        $(this).closest("tr").find("td:nth-child(8)").attr("product-price", priceChange.sellPrice);
        $(this).closest("tr").find("strong.item-price").html(priceChange.sellPrice.toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits }));

        let quantity = $(this).closest("tr").find("td:nth-child(3)").find("input[type=number]").val();
        let money = (priceChange.sellPrice * quantity).toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits });

        $(this).closest("tr").find("strong.item-price-total").html(money);
        updateTotalMoney();
    });

    $("a.product-remove").click(function () {
        let barcode = $(this).attr("barcode");
        let remove = "tr[barcode=" + barcode + "]";
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

function DisplaySuggestion(keyword) {
    console.log("keyword: " + keyword);
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
                    AddProductToCart(barcode);
                    $('#product-search-suggestion').html("");
                });
            }
        });
    }
}

function DisplayCustomerSuggestion(keyword) {
    let html = "";
    if (keyword == "") {
        $("#customer-search-suggestion").html("");
    } else {
        $.ajax({
            url: urlSearchCustomer,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            traditional: true,
            data: {
                keyword: keyword
            },
            success: function (data) {
                data.items.forEach(function (cust) {
                    html += '<li class="list-group-item"><a customer-id="';
                    html += cust.id;
                    html += '" class="suggestion-item" href="javascript:;"><strong>';
                    html += cust.name.toUpperCase();
                    html += " - ";
                    html += cust.province;
                    html += '</strong></a></li>';
                });

                $('#customer-search-suggestion').html(html);
                $("a.suggestion-item").click(function () {
                    customerId = $(this).attr("customer-id");
                    $("#customer-name").val($(this).text().split('-')[0]);
                    $('#customer-search-suggestion').html("");
                });
            }
        });
    }
}
// ===========================================================================================================================

function AddInvoices() {
    let items = [];
    let customer = $("#customer-name").val();
    if (customer == "" || customer == undefined) {
        customer = "Khách lẻ";
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
        items.forEach(function (item) {
            totalAmount += item.CurrentPrice * item.Quantity;
        });
    });

    let request = {
        CustomerName: customer,
        Note: note,
        Discount: +discount,
        InvoiceDetails: items
    };

    if (customerId != "") {
        request.CustomerId = customerId
    }

    if ($('#is-print-invoice').is(":checked")) {
        $.ajax({
            url: invoiceUrl,
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            traditional: true,
            data: JSON.stringify(request),
            success: function (data) {
                PrintInvoice(data);
            }
        });
    }

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

function PrintInvoice(html) {
    var frame1 = document.createElement('iframe');
    frame1.name = "frame1";
    frame1.style.position = "absolute";
    frame1.style.top = "-1000000px";
    document.body.appendChild(frame1);
    var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
    frameDoc.document.open();
    frameDoc.document.write(html);
    frameDoc.document.close();
    setTimeout(function () {
        window.frames["frame1"].focus();
        window.frames["frame1"].print();
        document.body.removeChild(frame1);
    }, 200);

    return false;
}

function CleanOrder() {
    $("#cart-list").html();
    $("#order-value-discount").val(0);
    updateTotalMoney();
}