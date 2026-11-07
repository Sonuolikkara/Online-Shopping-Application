// ============================================================
// USER CART JAVASCRIPT
// ============================================================
//DOM Ready
$(document).ready(function () {

    // ========================================================
    // 1. SHOW MESSAGE
    // ========================================================

    function showMessage(text, type) {

        const message = $("#cartMessage");

        message.removeClass(
            "cart-success cart-error"
        );

        if (type === "success") {

            message.addClass("cart-success");

        }
        else {

            message.addClass("cart-error");

        }

        message.text(text);

        message.show();

        setTimeout(function () {

            message.fadeOut();

        }, 3000);
    }


    // ========================================================
    // 2. LOAD CART
    // ========================================================

    function loadCart() {

        $("#cartLoading").show();

        $("#cartContent").hide();

        $("#emptyCart").hide();

        $.ajax({

            url: "/api/UserCartApi",

            type: "GET",

            success: function (items) {
                //Hide Loading Spinner
                $("#cartLoading").hide();

                if (!items || items.length === 0) {

                    $("#emptyCart").show();

                    return;
                }

                renderCart(items);
            },

            error: function () {

                $("#cartLoading").hide();

                showMessage(
                    "Unable to load your cart.",
                    "error"
                );
            }
        });
    }


    // ========================================================
    // 3. RENDER CART
    // ========================================================

    function renderCart(items) {
       
        // Get the container for cart items
        const container = $("#cartItems");
        // Clear existing items
        container.empty();

        let totalItems = 0;

        let subtotal = 0;


        items.forEach(function (item) {

            const itemTotal =
                Number(item.price) *
                Number(item.quantity);


            totalItems +=
                Number(item.quantity);


            subtotal +=
                itemTotal;


            let imageHtml = "";


            if (
                item.imageUrl &&
                item.imageUrl.trim() !== ""
            ) {

                imageHtml =
                    '<img src="' +
                    escapeHtml(item.imageUrl) +
                    '" alt="' +
                    escapeHtml(item.productName) +
                    '">';
            }
            else {

                imageHtml =
                    "<span>Product Image</span>";
            }


            //Template literal
            const html = `

                <article class="cart-item">

                    <div class="cart-item-image">

                        ${imageHtml}

                    </div>


                    <div class="cart-item-info">

                        <div class="cart-item-name">

                            ${escapeHtml(
                                item.productName
                            )}

                        </div>


                        <div class="cart-item-id">

                            Product ID:
                            ${item.productID}

                        </div>


                        <div class="cart-item-price">

                            ₹${Number(
                                item.price
                            ).toFixed(2)}

                        </div>


                        <div class="cart-stock">

                            ${item.stock} available

                        </div>


                        <div class="quantity-area">

                            <span class="quantity-label">

                                Quantity

                            </span>


                            <input
                                type="number"
                                class="quantity-input"
                                value="${item.quantity}"
                                min="1"
                                max="${item.stock}"
                                readonly
                            />

                        </div>

                    </div>


                    <div class="cart-item-right">

                        <div class="item-total">

                            ₹${itemTotal.toFixed(2)}

                        </div>


                        <button
                            type="button"
                            class="remove-button"
                            data-cart-item-id="${
                                item.cartItemID ||
                                item.productID
                            }">

                            <i class="bi bi-trash"></i>

                            REMOVE

                        </button>

                    </div>

                </article>

            `;


            container.append(html);

        });


        $("#summaryItems")
            .text(totalItems);


        $("#summarySubtotal")
            .text(
                "₹" +
                subtotal.toFixed(2)
            );


        $("#summaryTotal")
            .text(
                "₹" +
                subtotal.toFixed(2)
            );


        $("#cartContent").show();


        updateCheckoutButtonState();
    }


    // ========================================================
    // 4. CHECKOUT BUTTON STATE
    // ========================================================

    function updateCheckoutButtonState() {

        const token =
            localStorage.getItem(
                "customerAccessToken"
            );


        if (token) {

            $("#checkoutButton")
                .text("PROCEED TO CHECKOUT");

            $("#checkoutButton").show();

        }
        else {

            $("#checkoutButton")
                .text("LOGIN TO CHECKOUT");

            $("#checkoutButton").show();
        }
    }


    // ========================================================
    // 5. REMOVE / DECREASE ITEM
    // ========================================================
    //EVENT
    //Remove OR Decrease Button click event
    $(document).on(
        "click",
        ".remove-button",
        function () {

            const button =
                $(this);


            const cartItemId =
                button.data(
                    "cart-item-id"
                );


            button.prop(
                "disabled",
                true
            );

            //POST Remove API
            $.ajax({

                url:
                    "/api/UserCartApi/Remove" +
                    "?cartItemId=" +
                    encodeURIComponent(
                        cartItemId
                    ),

                type:
                    "POST",


                success:
                    function (response) {

                        showMessage(
                            response.message ||
                            "Product quantity updated.",
                            "success"
                        );

                        //Reload cart
                        loadCart();

                        //Update header cart count
                        if (
                            typeof updateCartCount ===
                            "function"
                        ) {

                            updateCartCount();

                        }
                    },


                error:
                    function (xhr) {

                        let errorMessage =
                            "Cart item could not be updated.";


                        if (
                            xhr.responseJSON &&
                            xhr.responseJSON.message
                        ) {

                            errorMessage =
                                xhr.responseJSON.message;
                        }


                        showMessage(
                            errorMessage,
                            "error"
                        );


                        button.prop(
                            "disabled",
                            false
                        );
                    }
            });

        }
    );


    // ========================================================
    // 6. CHECKOUT
    // ========================================================
    //EVENT
    //Checkout Button click event
    $("#checkoutButton").on(
        "click",
        function () {

            const token =
                localStorage.getItem(
                    "customerAccessToken"
                );


            if (!token) {

                window.location.href =
                    "/CustomerLogin";

                return;
            }


            window.location.href =
                "/Cart/Checkout";
        }
    );


    // ========================================================
    // 7. HTML ESCAPE
    // ========================================================

    function escapeHtml(value) {

        if (
            value === null ||
            value === undefined
        ) {

            return "";
        }


        return $("<div>")
            .text(value)
            .html();
    }


    // ========================================================
    // 8. INITIAL LOAD
    // ========================================================

    updateCheckoutButtonState();

    loadCart();

});