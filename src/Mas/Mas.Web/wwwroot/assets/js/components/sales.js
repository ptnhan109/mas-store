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
            html += '<option selected>';
            html += element.unit.name;
            html += '</option>';
        } else {
            html += '<option>';
            html += element.unit.name;
            html += '</option>';
        }
    });
    html += '</select></fieldset></td><td class="text-bold-500"><div class="form-group position-relative"><input type="number" class="form-control" value="';
    html += quantity;
    html += '"></div></td><td product-price="';
    html += sellPrice;
    html +='"><strong class="item-price">';
    html += sellPrice.toLocaleString('it-IT', { maximumFractionDigits: currencyFractionDigits });
    html += '</strong></td><td class="text-primary text-bold-500"><strong class="item-price-total">';
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
        console.log(money);
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

/*function remove*/