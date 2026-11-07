// =========================================
// LOAD WISHLIST
// =========================================

$(document).ready(function () {
    if (typeof syncCustomerAuthState === "function") {

        syncCustomerAuthState()
            .always(function () {

                loadWishlist();

            });

    } else {

        loadWishlist();
    }

});


function loadWishlist() {

    $.ajax({

        url: "/api/UserWishlistApi",
        type: "GET",

        success: function (wishlist) {

            const container =
                $("#wishlistContainer");

            container.empty();


            // Wishlist is empty

            if (!wishlist ||
                wishlist.length === 0) {

                container.html(`

                    <div class="wishlist-empty">

                        <i class="bi bi-heart"></i>

                        <p>
                            Your wishlist is empty.
                        </p>

                        <a href="/UserProducts"
                           class="continue-shopping">

                            BROWSE PRODUCTS

                        </a>

                    </div>

                `);

                return;
            }


            // Display wishlist items

            wishlist.forEach(function (item) {

                let stockHtml = "";


                if (item.stock > 0) {

                    stockHtml = `

                        <span class="in-stock">

                            ${item.stock} available

                        </span>

                    `;

                }
                else {

                    stockHtml = `

                        <span class="out-stock">

                            Out of stock

                        </span>

                    `;

                }


                let cartButton = "";


                if (item.stock > 0) {

                    cartButton = `

                        <form class="wishlist-add-to-cart-form"
                              data-product-id="${item.productID}"
                              method="post">

                            <input type="hidden"
                                   name="productId"
                                   value="${item.productID}" />

                            <input type="hidden"
                                   name="quantity"
                                   value="1" />

                            <button type="submit"
                                    class="cart-button">

                                <i class="bi bi-cart-plus"></i>

                                ADD TO CART

                            </button>

                        </form>

                    `;

                }


                container.append(`

                    <div class="wishlist-item">

                        <div class="product-info">

                            <h3>
                                ${item.productName}
                            </h3>

                            <span>
                                Product ID:
                                ${item.productID}
                            </span>

                        </div>


                        <div class="product-price">

                            ₹${Number(item.price).toFixed(2)}

                        </div>


                        <div class="product-stock">

                            ${stockHtml}

                        </div>


                        <div class="wishlist-actions">

                            ${cartButton}


                            <button type="button"
                                    class="remove-button"
                                    onclick="removeFromWishlist(${item.productID})">

                                <i class="bi bi-heartbreak"></i>

                                REMOVE

                            </button>

                        </div>

                    </div>

                `);

            });

        },


        error: function (xhr) {

            // Let the shared auth handler in user-layout.js
            // retry refreshed requests or redirect to login.
            if (xhr.status === 401) {
                return;
            }


            $("#wishlistContainer").html(`

                <div class="wishlist-empty">

                    <p>
                        Unable to load wishlist.
                    </p>

                </div>

            `);

        }

    });

}


// =========================================
// SHOW MESSAGE
// =========================================

function showMessage(text, type) {

    const message =
        $("#wishlistMessage");


    message.removeClass(
        "wishlist-success wishlist-error"
    );


    if (type === "success") {

        message.addClass(
            "wishlist-success"
        );

    }
    else {

        message.addClass(
            "wishlist-error"
        );

    }


    message.text(text);

    message.stop(true, true).show();


    setTimeout(function () {

        message.fadeOut();

    }, 3000);

}


// =========================================
// ADD TO CART
// =========================================

$(document).on(
    "submit",
    ".wishlist-add-to-cart-form",
    function (e) {

        e.preventDefault();


        const form = $(this);

        const submitButton =
            form.find("button[type='submit']");


        submitButton.prop(
            "disabled",
            true
        );

        //EVENT
        //Add to cart Button Click Event
        $.ajax({

            url:
                "/api/UserProductsApi/AddToCart",

            type:
                "POST",

            data:
                form.serialize(),


            success: function (response) {

                if (
                    typeof updateCartCount ===
                    "function"
                ) {

                    updateCartCount();

                }


                showMessage(

                    response.message ||
                    "Product added to cart.",

                    "success"

                );

            },


            error: function (xhr) {

                if (xhr.status === 401) {

                    window.location.href =
                        "/CustomerLogin";

                    return;

                }


                showMessage(

                    (xhr.responseJSON &&
                     xhr.responseJSON.message) ||

                    "Unable to add product to cart.",

                    "error"

                );

            },


            complete: function () {

                submitButton.prop(
                    "disabled",
                    false
                );

            }

        });

    }
);


// =========================================
// REMOVE FROM WISHLIST
// =========================================

function removeFromWishlist(productId) {
    //Event
    //Remove Button Click Event
    $.ajax({

        url:
            "/api/UserWishlistApi/Remove?productId="
            + productId,

        type:
            "POST",


        success: function (response) {

            // Reload wishlist after removing item

            loadWishlist();

        },


        error: function (xhr) {

            if (xhr.status === 401) {

                window.location.href =
                    "/CustomerLogin";

                return;

            }


            alert(
                "Unable to remove product from wishlist."
            );

        }

    });

}