var CartController = function () {
    this.initialize = function () {        
        
        loadData();
        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '.btn-plus', function (e) {
            e.preventDefault(); //Để ngăn event nhảy lại đầu trang khi click
            const productId = $(this).data('id');
            const quantity = parseInt($('#txt_quantity_' + productId).val()) + 1;
            updateCart(productId, quantity);
            
        });
        $('body').on('click', '.btn-minus', function (e) {
            e.preventDefault(); //Để ngăn event nhảy lại đầu trang khi click
            const productId = $(this).data('id');
            const quantity = parseInt($('#txt_quantity_' + productId).val()) - 1;
            updateCart(productId, quantity);

        });
        $('body').on('click', '.btn-remove', function (e) {
            e.preventDefault(); //Để ngăn event nhảy lại đầu trang khi click
            const productId = $(this).data('id');
            const quantity = 0;
            updateCart(productId, quantity);

        });
    }

    function updateCart(productId, quantity) {
        const culture = $('#hidculture').val();
        $.ajax({
            type: "POST",
            url: '/' + culture + '/Cart/UpdateCart',
            dataType: 'json',
            data: { id: productId, quantity: quantity },
            success: function (res) {
                if (res.length == 0) {
                    $('#tbl_cart').hide();
                }
                $('#txt_quantity_' + productId).val(quantity);
                $('#lbl_number_of_items').text(res.length);                
                loadData();
            }

        });
    }

    function loadData() {
        const culture = $('#hidculture').val();
        $.ajax({
            type: "GET",
            url: "/" + culture + '/Cart/GetListItems',
            success: function (res) {
                var html = '';
                var total = 0;
                $.each(res, function (i, item) {
                    var amount = item.price * item.quantity;
                    html += "<tr>"
                        + "<td> <img width=\"60\" src=\"" + $('#hidBaseAddress').val()+ item.image + "\" alt=\"\" /></td>"
                        + "<td>" + item.description + "</td>"
                        + "<td><div class=\"input-append\"><input class=\"span1\" style=\"max-width: 34px\" placeholder=\"1\" id=\"txt_quantity_" + item.productId + "\" value=\"" + item.quantity + "\" size=\"16\" type=\"text\">"
                        + "<button class=\"btn btn-minus\" data-id=\"" + item.productId + "\" type=\"button\"> <i class=\"icon-minus\"></i></button>"
                        + "<button class=\"btn btn-plus\"  data-id=\"" + item.productId + "\" type=\"button\"><i class=\"icon-plus\"></i></button>"
                        + "<button class=\"btn btn-danger btn-remove\" type=\"button\" data-id=\"" + item.productId + "\"><i class=\"icon-remove icon-white\"></i></button>"
                        + "</div>"
                        + "</td>"
                        + "<td>" + numberWithCommas(item.price) + "</td>"
                        + "<td>" + numberWithCommas(amount) + "</td>"
                        + "</tr>";
                    total += item.price * item.quantity;
                });
                $('#cart_body').html(html);
                $('#lbl_number_of_items_lst').text(res.length);               
                $('#lbl_total').text(numberWithCommas(total));

            }
        });
    }
}