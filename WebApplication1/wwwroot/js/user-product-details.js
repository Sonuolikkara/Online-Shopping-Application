
   //Page Ready Event
   //Event
   jQuery(document).ready(function () {

        // ============================================================
        // PRODUCT ID
        // ============================================================

         const productId =
              $("#productPage").attr("data-product-id");


        // ============================================================
        // PAGE ELEMENTS
        // ============================================================

        // Elements for loading, content, message, image, quantity input, and buttons
        const loading =
            $("#productLoading");

        const productContent =
            $("#productContent");

        const message =
            $("#productMessage");

        const productImage =
            $("#productImage");

        const productImageLabel =
            $("#productImageLabel");

        const quantityInput =
            $("#quantity");

        const addToCartButton =
            $("#addToCartButton");

        const buyNowButton =
            $("#buyNowButton");

        const wishlistButton =
            $("#wishlistButton");


        // ============================================================
        // SHOW MESSAGE
        // ============================================================
        // Displays a message to the user with a specified type (success or error)
        function showMessage(
            text,
            type
        ) {
            
            // Clear any existing classes from the message element
            message
                .removeClass(
                    "success-message error-message"
                );


            if (type === "success") {

                message.addClass(
                    "success-message"
                );

            }
            else {

                message.addClass(
                    "error-message"
                );
            }

            //Put text into the message
            message.text(text);

            message.show();


            setTimeout(function () {

                message.fadeOut();

            }, 3000);//miliseconds

        }


        // ============================================================
        // GET PRODUCT
        // ============================================================
        //EVENT
        function loadProduct() {

            $.ajax({

                url:
                    "/api/UserProductsApi/"
                    + productId,

                type:
                    "GET",

                success:
                    function (product) {

                        displayProduct(product);

                    },

                error:
                    function (xhr) {

                        loading.text(
                            "Unable to load product."
                        );


                        if (
                            xhr.status === 404
                        ) {

                            loading.text(
                                "Product not found."
                            );
                        }

                    }

            });

        }


        // ============================================================
        // DISPLAY PRODUCT
        // ============================================================
        //EVENT
        function displayProduct(
            product
        ) {

            loading.hide();

            productContent.show();


            // --------------------------------------------------------
            // PRODUCT NAME
            // --------------------------------------------------------

            $("#productName")
                .text(
                    product.productName
                );


            // --------------------------------------------------------
            // PRODUCT ID
            // --------------------------------------------------------

            $("#productId")
                .text(
                    product.productID
                );


            // --------------------------------------------------------
            // PRICE
            // --------------------------------------------------------

            const price =
                Number(product.price);


            $("#productPrice")
                .text(
                    price.toFixed(2)
                );


            // --------------------------------------------------------
            // DESCRIPTION
            // --------------------------------------------------------

            if (
                product.description &&
                product.description.trim() !== ""
            ) {

                $("#productDescription")
                    .text(
                        product.description
                    );

            }
            else {

                $("#productDescription")
                    .text(
                        "No description available for this product."
                    );

            }


            // --------------------------------------------------------
            // IMAGE
            // --------------------------------------------------------

            if (
                product.imageUrl &&
                product.imageUrl.trim() !== ""
            ) {

                productImage
                    .attr(
                        "src",
                        product.imageUrl
                    );

                productImage
                    .attr(
                        "alt",
                        product.productName
                    );

                productImage.show();

                productImageLabel.hide();

            }
            else {

                productImage.hide();

                productImageLabel.show();

            }


            // --------------------------------------------------------
            // STOCK
            // --------------------------------------------------------
            //Stock Handling
            const stock =
                Number(product.stock);


            if (stock > 0) {

                $("#stockContainer")
                    .removeClass(
                        "stock-unavailable"
                    )
                    .addClass(
                        "stock-available"
                    )
                    .html(
                        '<i class="bi bi-check-circle"></i> '
                        + stock
                        + ' items available'
                    );


                $("#quantityAndActions")
                    .show();

                $("#outOfStockContainer")
                    .hide();


                quantityInput
                    .attr(
                        "max",
                        stock
                    );

            }
            //Out of stock handling
            else {

                $("#stockContainer")
                    .removeClass(
                        "stock-available"
                    )
                    .addClass(
                        "stock-unavailable"
                    )
                    .html(
                        '<i class="bi bi-x-circle"></i> '
                        + 'Out of stock'
                    );


                $("#quantityAndActions")
                    .hide();

                $("#outOfStockContainer")
                    .show();

            }


            // --------------------------------------------------------
            // CATEGORY
            // --------------------------------------------------------

            if (
                product.categoryID !== null &&
                product.categoryID !== undefined
            ) {

                $("#productCategory")
                    .text(
                        product.categoryID
                    );

            }
            else {

                $("#productCategory")
                    .text("N/A");

            }


            // --------------------------------------------------------
            // AVAILABILITY
            // --------------------------------------------------------

            if (stock > 0) {

                $("#productAvailability")
                    .text(
                        "In Stock"
                    );

            }
            else {

                $("#productAvailability")
                    .text(
                        "Out of Stock"
                    );

            }


            // --------------------------------------------------------
            // CHECK WISHLIST
            // --------------------------------------------------------

            checkWishlistStatus();

        }


        // ============================================================
        // QUANTITY VALIDATION
        // ============================================================
        //EVENT
        function getQuantity() {

            let quantity =
                parseInt(
                    quantityInput.val()
                );

            //Check invalid quantity
            if (
                isNaN(quantity) ||
                quantity < 1
            ) {

                quantity = 1;

                quantityInput.val(
                    quantity
                );
            }


            const maxQuantity =
                parseInt(
                    quantityInput.attr("max")
                );


            if (
                !isNaN(maxQuantity) &&
                quantity > maxQuantity
            ) {

                quantity =
                    maxQuantity;

                quantityInput.val(
                    quantity
                );

            }


            return quantity;

        }


        // ============================================================
        // QUANTITY INPUT
        // ============================================================
        //Event
        // Listen for input and change events on the quantity input field
        quantityInput.on(
            "input change",
            function () {

                getQuantity();

            }
        );


        // ============================================================
        // ADD TO CART
        // ============================================================
        //Event
        //Add to Cart Event
        addToCartButton.on(
            "click",
            function () {

                const quantity =
                    getQuantity();


                if (quantity <= 0) {

                    showMessage(
                        "Quantity must be at least 1.",
                        "error"
                    );

                    return;
                }


                addToCartButton.prop(
                    "disabled",
                    true
                );

                //POST Add to Cart AJAX
                $.ajax({

                    url:
                        "/api/UserProductsApi/AddToCart",

                    type:
                        "POST",

                    data: {

                        productId:
                            productId,

                        quantity:
                            quantity

                    },

                    success:
                        function (response) {

                            showMessage(
                                response.message ||
                                "Product added to cart.",
                                "success"
                            );


                            addToCartButton
                                .prop(
                                    "disabled",
                                    false
                                );


                            // Update cart count
                            // after successful operation

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
                                "Unable to add product to cart.";


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


                            addToCartButton
                                .prop(
                                    "disabled",
                                    false
                                );

                        }

                });

            }
        );


// ============================================================
// BUY NOW
// ============================================================
//Event
//Buy Now Click Event
buyNowButton.on(
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

        //Validate quantity before proceeding to checkout
         const quantity =
            getQuantity();

        if (quantity <= 0) {

            showMessage(
                "Quantity must be at least 1.",
                "error"
            );

            return;
        }

        window.location.href =
            "/Cart/Checkout"
            + "?productId="
            + encodeURIComponent(productId)
            + "&quantity="
            + encodeURIComponent(quantity);
    }
);
       


        // ============================================================
        // CHECK WISHLIST STATUS
        // ============================================================

        function checkWishlistStatus() {

            const token =
                localStorage.getItem(
                    "customerAccessToken"
                );


            /*
             * Guest users do not have a wishlist.
             *
             * The API requires a Customer JWT.
             */

            if (!token) {

                wishlistButton
                    .removeClass("active");

                $("#wishlistIcon")
                    .removeClass(
                        "bi-heart-fill"
                    )
                    .addClass(
                        "bi-heart"
                    );

                $("#wishlistText")
                    .text(
                        "ADD TO WISHLIST"
                    );

                return;

            }

            //Wishlist Status AJAX
            $.ajax({

                url:
                    "/api/UserProductsApi/WishlistStatus/"
                    + productId,

                type:
                    "GET",

                success:
                    function (response) {

                        setWishlistState(
                            response.isInWishlist
                        );

                    },

                error:
                    function () {

                    }

            });

        }


        // ============================================================
        // SET WISHLIST STATE
        // ============================================================

        function setWishlistState(
            isInWishlist
        ) {

            if (isInWishlist) {

                wishlistButton
                    .addClass("active");


                $("#wishlistIcon")
                    .removeClass(
                        "bi-heart"
                    )
                    .addClass(
                        "bi-heart-fill"
                    );


                $("#wishlistText")
                    .text(
                        "REMOVE FROM WISHLIST"
                    );

            }
            else {

                wishlistButton
                    .removeClass("active");


                $("#wishlistIcon")
                    .removeClass(
                        "bi-heart-fill"
                    )
                    .addClass(
                        "bi-heart"
                    );


                $("#wishlistText")
                    .text(
                        "ADD TO WISHLIST"
                    );

            }

        }


        // ============================================================
        // WISHLIST BUTTON
        // ============================================================
        //EVENT
        //Wishlist Button Click Event
        wishlistButton.on(
            "click",
            function () {

                const token =
                    localStorage.getItem(
                        "customerAccessToken"
                    );


                // ----------------------------------------------------
                // CUSTOMER LOGIN REQUIRED
                // ----------------------------------------------------

                if (!token) {

                    window.location.href =
                        "/CustomerLogin";

                    return;

                }


                const isActive =
                    wishlistButton
                        .hasClass("active");


                wishlistButton
                    .prop(
                        "disabled",
                        true
                    );


                // ====================================================
                // REMOVE FROM WISHLIST
                // ====================================================

                if (isActive) {

                    $.ajax({

                        url:
                            "/api/UserProductsApi/RemoveFromWishlist"
                            + "?productId="
                            + encodeURIComponent(
                                productId
                            ),

                        type:
                            "POST",

                        success:
                            function (response) {

                                setWishlistState(
                                    false
                                );


                                showMessage(
                                    response.message ||
                                    "Product removed from wishlist.",
                                    "success"
                                );


                                wishlistButton
                                    .prop(
                                        "disabled",
                                        false
                                    );

                            },

                        error:
                            function (xhr) {

                                let errorMessage =
                                    "Unable to remove product from wishlist.";


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


                                wishlistButton
                                    .prop(
                                        "disabled",
                                        false
                                    );

                            }

                    });

                }


                // ====================================================
                // ADD TO WISHLIST
                // ====================================================

                else {

                    $.ajax({

                        url:
                            "/api/UserProductsApi/AddToWishlist"
                            + "?productId="
                            + encodeURIComponent(
                                productId
                            ),

                        type:
                            "POST",

                        success:
                            function (response) {

                                setWishlistState(
                                    true
                                );


                                showMessage(
                                    response.message ||
                                    "Product added to wishlist.",
                                    "success"
                                );


                                wishlistButton
                                    .prop(
                                        "disabled",
                                        false
                                    );

                            },

                        error:
                            function (xhr) {

                                let errorMessage =
                                    "Unable to add product to wishlist.";


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


                                wishlistButton
                                    .prop(
                                        "disabled",
                                        false
                                    );

                            }

                    });

                }

            }
        );


        // ============================================================
        // LOAD PRODUCT
        // ============================================================

        loadProduct();

    });