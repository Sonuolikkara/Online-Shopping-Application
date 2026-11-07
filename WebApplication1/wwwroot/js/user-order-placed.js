// ============================================================
// USER ORDER PLACED JAVASCRIPT
// ============================================================

$(document).ready(function () {

    // ========================================================
    // 1. GET ORDER ID
    // ========================================================

    const orderId =
        $("#orderPage").attr("data-order-id");


    // ========================================================
    // 2. LOAD ORDER
    // ========================================================

    $.ajax({

        url:
            "/api/UserCartApi/OrderPlaced/"
            + orderId,

        type:
            "GET",

        success:
            function (order) {

                $("#orderLoading")
                    .hide();


                $("#orderId")
                    .text(
                        "#" +
                        order.orderID
                    );


                const orderDate =
                    new Date(
                        order.orderDate
                    );


                $("#orderDate")
                    .text(
                        orderDate.toLocaleDateString(
                            "en-GB",
                            {
                                day: "2-digit",
                                month: "short",
                                year: "numeric"
                            }
                        )
                    );


                $("#orderStatus")
                    .text(
                        order.status
                    );


                if (
                    order.totalAmount !== null &&
                    order.totalAmount !== undefined
                ) {

                    $("#orderTotal")
                        .text(
                            "₹" +
                            Number(
                                order.totalAmount
                            ).toFixed(2)
                        );

                }
                else {

                    $("#orderTotal")
                        .text(
                            "₹0.00"
                        );
                }


                $("#orderDetails")
                    .show();


                $("#orderActions")
                    .show();

            },

        error:
            function (xhr) {

                $("#orderLoading")
                    .text(
                        "Unable to load order details."
                    );

            }

    });

});