// ============================================================
// USER CHECKOUT JAVASCRIPT
// ============================================================
//DOM Ready
$(document).ready(function () {

    // ========================================================
    // 1. CHECK CUSTOMER LOGIN
    // ========================================================

    const token =
        localStorage.getItem(
            "customerAccessToken"
        );

        console.log(
    "Checkout access token:",
    token
);
    if (!token) {

        window.location.href =
            "/CustomerLogin";

        return;
    }


    // ========================================================
    // 2. CHECKOUT FORM
    // ========================================================
    //Event
    //Checkout form event click
    $("#checkoutForm").on(
        "submit",
        function (event) {

            event.preventDefault();


            // ------------------------------------------------
            // GET FORM VALUES
            // ------------------------------------------------
            //Get ProductID,quantity,address,phone,paymentMethod,checkoutMessage 
            const productId =
                $("#productId").val();


            const quantityValue =
                $("#quantity").val();


            const address =
                $("#address").val();


            const phone =
                $("#phone").val();


            const paymentMethod =
                $("input[name='paymentMethod']:checked")
                    .val();


            const checkoutMessage =
                $("#checkoutMessage");


            // ------------------------------------------------
            // CLEAR PREVIOUS MESSAGE
            // ------------------------------------------------

            checkoutMessage
                .removeClass("checkout-error")
                .hide()
                .text("");


            // ------------------------------------------------
            // TRIM ADDRESS AND PHONE
            // ------------------------------------------------

            const trimmedAddress =
                (address || "").trim();


            const trimmedPhone =
                (phone || "").trim();


            // ------------------------------------------------
            // ADDRESS VALIDATION
            // ------------------------------------------------

            if (!trimmedAddress) {

                checkoutMessage
                    .text(
                        "Please enter your address."
                    )
                    .addClass("checkout-error")
                    .show();

                return;
            }


            // ------------------------------------------------
            // PHONE VALIDATION
            // ------------------------------------------------

            if (!trimmedPhone) {

                checkoutMessage
                    .text(
                        "Please enter your phone number."
                    )
                    .addClass("checkout-error")
                    .show();

                return;
            }

            //Phone number validation: must be 10 digits
            if (!/^\d{10}$/.test(trimmedPhone)) {

                checkoutMessage
                    .text(
                        "Enter the correct phone number."
                    )
                    .addClass("checkout-error")
                    .show();

                return;
            }


            // ------------------------------------------------
            // PAYMENT VALIDATION
            // ------------------------------------------------

            if (!paymentMethod) {

                $("#paymentValidation")
                    .text(
                        "Please select a payment method."
                    );

                $("#paymentValidation").show();

                return;
            }


            $("#paymentValidation")
                .text("");


            // ------------------------------------------------
            // CREATE CHECKOUT MODEL
            // ------------------------------------------------

            const model = {

                address:
                    trimmedAddress,

                phone:
                    trimmedPhone,

                paymentMethod:
                    paymentMethod
            };


            // ------------------------------------------------
            // BUY NOW
            // ------------------------------------------------

            if (productId) {

                model.productId =
                    parseInt(
                        productId
                    );


                model.quantity =
                    quantityValue
                        ? parseInt(
                            quantityValue
                        )
                        : 1;
            }


            // ------------------------------------------------
            // DISABLE PLACE ORDER BUTTON
            // ------------------------------------------------

            $("#placeOrderButton")
                .prop(
                    "disabled",
                    true
                );


            // ------------------------------------------------
            // SEND CHECKOUT REQUEST
            // ------------------------------------------------

            $.ajax({

                url:
                    "/api/UserCartApi/Checkout",

                type:
                    "POST",

                contentType:
                    "application/json",

                data:
                    JSON.stringify(model),

                success:
                    function (response) {

                        window.location.href =
                            "/Cart/OrderPlaced/" +
                            response.orderId;

                    },

                error:
                    function (xhr) {

                        let errorMessage =
                            "Unable to place order.";


                        if (
                            xhr.responseJSON &&
                            xhr.responseJSON.message
                        ) {

                            errorMessage =
                                xhr.responseJSON.message;
                        }


                        $("#checkoutMessage")
                            .text(
                                errorMessage
                            )
                            .addClass(
                                "checkout-error"
                            )
                            .show();


                        $("#placeOrderButton")
                            .prop(
                                "disabled",
                                false
                            );
                    }
            });

        }
    );

});