$(document).ready(function () {
    $("#IsConsignment :checkbox").change(function () {
        if (this.checked) {
            $("#consignment").show();
        } else {
            $("#consignment").hide();
        }
    })



    function getCategories() {
        let url = ''
    }
});