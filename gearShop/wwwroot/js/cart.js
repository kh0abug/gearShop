var cart = {
    init: function () {
        cart.regEvents();
    },
    regEvents: function () {
        $('#btnContinue').on('click', function () {
            window.location.href = "/Customer/Product/Index";
        });

        $('#btnUpdate').on('click', function (e) {
            var listP = $('.quantity');
            var cartList = [];
            $.each(listP, function (i, item) {
                cartList.push({
                    Quantity: $(item).val(),
                    product: {
                        Id: $(item).data('id')
                    }
                });
            });
            e.preventDefault();
            $.ajax({
                url: '/Customer/Cart/Update',
                data: {
                    cartModel: JSON.stringify(cartList)
                },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status) {
                        window.location.href = "/Customer/Cart/Index";
                    }
                },
            });
        });

        $('#btnDelete').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                url: '/Customer/Cart/RemoveAll',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status) {
                        window.location.href = "/Customer/Cart/Index";
                    }
                },
            });
        });

        $('#btnBuy').on('click', function () {
            window.location.href = "/Customer/Cart/CheckOut";
        });

        $('.delete').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                data: { id: $(this).data('id') },
                url: '/Customer/Cart/Remove',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status) {
                        window.location.href = "/Customer/Cart/Index";
                    }
                },
            });
        });
    }
};
cart.init();


