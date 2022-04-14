const currencyFractionDigits = new Intl.NumberFormat('de-DE', {
    style: 'currency',
    currency: 'VND',
}).resolvedOptions().maximumFractionDigits;
var totalMoney = 0;
var customerId = "";
var products = [];
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
        DisplayDestructionSuggestion(productCode);
    });

    $('#productCode').on("keypress", function (e) {
        let qrCode = $('#productCode').val();
        if (e.keyCode == 13) {
            if (qrCode !== "") {
                AddProductToDestructionInvoice(qrCode);
                return false;
            } else {
                alert("Hãy nhập từ khóa");
            }
        }

        if (e.keyCode == 8) {
            DisplayDestructionSuggestion(qrCode);
        }
    });

    $("#add-destruction-button").click(function () {
        AddInvoices();
    });
});

function AddProductToDestructionInvoice(barcode) {
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
                            let newPrice = element.importPrice * quantity;
                            total = newPrice.toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits });
                            totalMoney += element.importPrice;
                        }
                    });

                    //$(this).find('strong.item-price-total').html(total);
                    //updateTotalMoney();
                }
            });
            if (!isExsit) {
                AppendProductHtml(data);
                products.push(data);
            }

            $("#productCode").val("");
        }
    });
    $('#product-search-suggestion').html("");
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
    html += '"><i class="bi bi-trash text-danger"></i></a></td>';
    html += '<td class="d-none" product-price="';
    html += importPrice;
    html += '"></td></tr>';

    $("#cart-list").append(html);
    totalMoney += importPrice;

    $("input[type=number]").change(function () {
        let price = $(this).closest("tr").find("td:nth-child(4)").attr("product-price");
        let quantity = $(this).val();
        let money = (price * quantity).toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits });
        $(this).closest("tr").find("strong.item-price-total").html(money);
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
    });

    $("a.product-remove").click(function () {
        let barcode = $(this).attr("barcode");
        let remove = "tr[barcode=" + barcode + "]";
        $(remove).remove();
    });
}

function DisplayDestructionSuggestion(keyword) {
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
                    AddProductToDestructionInvoice(barcode);
                    $('#product-search-suggestion').html("");
                    $("#productCode").val("");
                });
            }
        });
    }
}

function AddInvoices() {
    let items = [];

    let code = $("#destruction-code").val();
    let description = $("#destruction-description").val();
    $("#cart-list > tr").each(function () {
        let barcode = $(this).attr("barcode");
        let id = $(this).attr("prod-id");
        let name = $(this).find("td:nth-child(1)").text();
        let unit = $(this).find("td:nth-child(2)").find("select.form-select option:selected").text();
        let quantity = $(this).find("td:nth-child(3)").find("input[type=number]").val();
        let price = $(this).find("td:nth-child(8)").attr("product-price");

        items.push({
            Name: name,
            UnitName: unit,
            BarCode: barcode,
            CurrentPrice: +price,
            Quantity: +quantity,
            ProductId: id
        });
    });

    let request = {
        Code: code,
        Description: description,
        CreatedBy: user,
        Items: items
    };

    $.ajax({
        url: addDestructionUrl,
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        traditional: true,
        data: JSON.stringify(request),
        success: function (data) {
            $("#cart-list").html("");
            showMessage("success", data);
        }
    });

}