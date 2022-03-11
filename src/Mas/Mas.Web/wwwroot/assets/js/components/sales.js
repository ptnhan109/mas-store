$(document).ready(function () {
    $('#productCode').focus();
    $('#productCode').on("keypress", function (e) {
        let qrCode = $('#productCode').val();
        if (qrCode !== "") {
            if (e.keyCode == 13) {
                AddProductToCart(qrCode);
                return false;
            }
        } else {
            alert("Hãy nhập từ khóa");
        }
    });
    
});
var products = [];
const currencyFractionDigits = new Intl.NumberFormat('de-DE', {
    style: 'currency',
    currency: 'VND',
}).resolvedOptions().maximumFractionDigits;

var totalMoney = 0;
function AddProductToCart(barcode) {
    let url = urlGetProd + '?barcode=' + barcode;
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
    html += '"';
    html += '><td class="text-bold-500" style="text-transform: uppercase;">';
    html += data.name;
    html += '</td><td><fieldset class="form-group position-relative"><select class="form-select" id="basicSelect">';
    data.prices.forEach((element) => {
        if (element.isDefault) {
            sellPrice = element.sellPrice;
            quantity = element.quantity;
            html += '<option selected value="';
            html += element.barCode;
            html +='">';
            html += element.unit.name;
            html += '</option>';
        } else {
            html += '<option value="';
            html += element.barCode;
            html +='">';
            html += element.unit.name;
            html += '</option>';
        }
    });
    html += '</select></fieldset></td><td class="text-bold-500"><div class="form-group position-relative"><input type="number" class="form-control" value="';
    html += quantity;
    html += '"></div></td><td product-price="';
    html += sellPrice;
    html +='"><a href="javascript:;" class="add-discount"><strong class="item-price">';
    html += sellPrice.toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits });
    html += '</strong></a></td><td class="text-primary text-bold-500"><strong class="item-price-total">';
    html += (sellPrice * quantity).toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits });
    html += '</strong></td><td><a href="javascript:;"><i class="badge-circle badge-circle-light-secondary font-medium-1" data-feather="mail"></i></a></td>';
    html += '<td><a href="javascript:;" class="product-remove" barcode="';
    html += data.barCode;
    html +='"><i class="bi bi-trash text-danger"></i></a></td></tr>';

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
        let price = $(this).closest("tr").find("td:nth-child(4)").attr("product-price");
        let barcode = $(this).closest("tr").attr("barcode");

        $("#modal-barcode").val(barcode);
        $("#modal-product-price").attr("product-price", price);
        $("#modal-product-price").val((+price).toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits }));
        $("#discount-title").html(name);
        $("#modal-discount").modal("show");
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


        $("#cart-list > tr").each(function () {
            console.log($(this).attr("barcode"));
            //if ($(this).attr("barcode") == barcode) {
            //    alert($(this).attr("barcode"));
            //    $(this).find("td:nth-child(4)").attr("product-price", newValue);
            //    let html = '<p><small class="text-danger">-' + discountValue + '</small></p>';
            //    $(this).find("td:nth-child(4)").append(html);
            //    let quantity = $(this).find("input[type=number]").val();
            //    $(this).find("strong.item-price-total").html((newValue * quantity).toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits }));
            //    updateTotalMoney();
            //    return false;
            //}
        });


        $("#modal-barcode").val();
        $("#value-discount").val(0);
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
    })
    let t = money.toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits });
    $("#order-total-money").html(t + "đ");
}