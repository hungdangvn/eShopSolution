// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var SiteController = function () {
    this.initialize = function () {        
        registerEvents();
        loadCart();
    }

    function loadCart() {
        const culture = $('#hidculture').val();
        $.ajax({
            type: "GET",
            url: "/" + culture + '/Cart/GetListItems',
            success: function (res) {
                $('#lbl_number_of_items_header').text(res.length)
            }
        });
    }

    function registerEvents() {
        $('body').on('click', '.btn-add-cart', function (e) {
            e.preventDefault(); //Để ngăn event nhảy lại đầu trang khi click
            const productId = $(this).data('product_id');
            //alert(productId);
            const culture = $('#hidculture').val();
            $.ajax({
                type: "POST",
                url: '/' + culture + '/Cart/AddToCart',
                dataType: 'json',
                data: { id: productId, languageId: culture },
                success: function (res) {
                    $('#lbl_number_of_items').text(res.length)
                }

            });
        });
    }
}



function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
