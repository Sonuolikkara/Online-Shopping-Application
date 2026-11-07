
   // PRODUCTS PAGE JAVASCRIPT

     let allProducts = [];


    // =========================================================
    // LOAD PRODUCTS FROM WEB API
    // =========================================================

    function loadProducts() {

        //AJAX request
        $.ajax({
            url: "/api/UserProductsApi",
            type: "GET",

            success: function (products) {

                allProducts = products || [];

                const initialSearch =
                    getInitialSearchTerm();

                if (initialSearch) {
                    $("#productSearch").val(initialSearch);
                    applySearch(initialSearch);
                    return;
                }

                displayProducts(allProducts, {
                    showSmallProducts: true,
                    searchMode: false
                });
            },

            error: function (xhr) {

                console.error(
                    "Failed to load products:",
                    xhr
                );

                //Hide Product Layout
                $("#productLayout").hide();

                $("#emptyProducts").show();

                $("#emptyTitle").text(
                    "Unable to load products."
                );

                $("#emptyMessage").text(
                    "Please try again later."
                );
            }
        });
    }


    function getInitialSearchTerm() {

        const urlParams =
            new URLSearchParams(window.location.search);

        const searchFromUrl =
            urlParams.get("search") || "";

        const searchFromInput =
            $("#productSearch").val() || "";

        return (searchFromUrl || searchFromInput).trim();

    }


    //EVENT
    //Search Form Button Event
    function applySearch(search) {

        const normalizedSearch =
            (search || "")
                .trim()
                .toLowerCase();

        updateSearchUrl(search);

        //Empty search check
        if (!normalizedSearch) {

             $("#searchResultMessage").hide();
            displayProducts(allProducts, {
                showSmallProducts: true,
                searchMode: false
            });
            return;
        }


        //Filter Products
        const filteredProducts =
            allProducts.filter(function (product) {

                return (
                    product.productName &&
                    product.productName
                        .toLowerCase()
                        .includes(normalizedSearch)
                );
            });


        // Update the search result message
        $("#searchResultMessage")
            .text(
                "Showing results for \"" +
                search.trim() +
                "\""
            )
            .show();


        displayProducts(filteredProducts, {
            showSmallProducts: false,
            searchMode: true
        });

    }


    // =========================================================
    // DISPLAY PRODUCTS
    // =========================================================

    function displayProducts(products, options) {

        const showSmallProducts = !options || options.showSmallProducts !== false;
        const searchMode = !!(options && options.searchMode);

        $("#productCount").text(
            products.length +
            (products.length === 1
                ? " product"
                : " products")
        );


        // No products

        if (products.length === 0) {

            $("#productLayout").hide();

            $("#emptyProducts").show();

            //Search-specific empty state
            //No Product Found Situation
            const search =
                $("#productSearch").val().trim();

            // Check if there is a search term
                if (search) {

                $("#emptyTitle").text(
                    "No products found."
                );

                $("#emptyMessage").html(
                    'No products match "<strong>' +
                    escapeHtml(search) +
                    '</strong>".'
                );

                $("#viewAllProducts").show();

            } else {

                $("#emptyTitle").text(
                    "No products are currently available."
                );

                $("#emptyMessage").text("");

                $("#viewAllProducts").hide();
            }

            return;
        }


        // Products Exist
        $("#emptyProducts").hide();

        $("#productLayout")
            .toggleClass("search-results", searchMode)
            .show();


        // Featured product

        const featuredProduct =
            products[0];

        displayFeaturedProduct(
            featuredProduct
        );


        // Small products
        const smallProducts =
            products.slice(1, 5);

        const smallProductsContainer =
            $("#smallProducts");

        smallProductsContainer.empty();

        if (showSmallProducts) {

            smallProducts.forEach(function (product) {

                smallProductsContainer.append(
                    createSmallProductCard(product)
                );

            });

            smallProductsContainer.show();

        } else {

            smallProductsContainer.hide();
        }
    }


    // =========================================================
    // FEATURED PRODUCT
    // =========================================================

    function displayFeaturedProduct(product) {

        const imageContainer =
            $("#featuredImage");

        imageContainer.empty();


        if (product.imageUrl) {

           //Dynamic image
           imageContainer.append(
                $("<img>", {
                    src: product.imageUrl,
                    alt: product.productName || "Product"
                })
            );

        } else {

            // Fallback image
            imageContainer.append(
                $("<span>", {
                    class: "image-label",
                    text: "Product Image"
                })
            );
        }


        $("#featuredName").text(
            product.productName || ""
        );


        $("#featuredDescription").text(
            product.description ||
            "Quality product available for your shopping needs."
        );


        //Dynamic price
        $("#featuredPrice").text(
            "₹" +
            Number(product.price || 0)
                .toLocaleString(
                    "en-IN",
                    {
                        minimumFractionDigits: 2,
                        maximumFractionDigits: 2
                    }
                )
        );


        const stockElement =
            $("#featuredStock");

        //Dynamic Stock
        if (product.stock > 0) {

            stockElement
                .removeClass("out-of-stock")
                .text(
                    product.stock +
                    " available"
                );

        } else {

            stockElement
                .addClass("out-of-stock")
                .text(
                    "Out of stock"
                );
        }

        //Event
        //Product Detlis Link
        $("#featuredViewProduct")
            .attr(
                "href",
                "/UserProducts/Details/" +
                product.productID
            );
    }


    // =========================================================
    // SMALL PRODUCT CARD
    // =========================================================

    function createSmallProductCard(product) {

        const card =
            $("<article>", {
                class: "product-card"
            });

        const image =
            $("<div>", {
                class: "product-image"
            });

        if (product.imageUrl) {

            image.append(
                $("<img>", {
                    src: product.imageUrl,
                    alt: product.productName || "Product"
                })
            );

        } else {

            image.append(
                $("<span>", {
                    class: "image-label",
                    text: "Product Image"
                })
            );
        }

        const info =
            $("<div>", {
                class: "product-info"
            });

        const name =
            $("<h2>", {
                class: "product-name",
                text: product.productName || ""
            });

        const description =
            $("<p>", {
                class: "product-description",
                text: product.description || "Quality product."
            });

        const meta =
            $("<div>", {
                class: "product-meta"
            });

        const price =
            $("<span>", {
                class: "product-price",
                text:
                    "₹" +
                    Number(product.price || 0).toLocaleString(
                        "en-IN",
                        {
                            minimumFractionDigits: 2,
                            maximumFractionDigits: 2
                        }
                    )
            });

        const stock =
            $("<span>", {
                class: "product-stock"
            });

        if (product.stock > 0) {

            stock.text(product.stock + " available");

        } else {

            stock
                .addClass("out-of-stock")
                .text("Out of stock");
        }

        meta.append(price, stock);

        const viewProduct =
            $("<a>", {
                class: "view-product",
                text: "VIEW PRODUCT",
                href: "/UserProducts/Details/" + product.productID
            });

        info.append(name, description, meta, viewProduct);

        card.append(image, info);

        return card;
    }


    // =========================================================
    // SEARCH
    // =========================================================
    //EVENT
    // Event listener for the search form
    $("#productSearchForm").on(
        "submit",
        function (event) {

            event.preventDefault();

            applySearch(
                $("#productSearch").val()
            );
        }
    );


    // =========================================================
    // ESCAPE HTML
    // =========================================================

    function escapeHtml(value) {

        return $("<div>")
            .text(value)
            .html();
    }


    function updateSearchUrl(search) {

        const url = new URL(window.location.href);
        const trimmedSearch = (search || "").trim();

        if (trimmedSearch) {
            url.searchParams.set("search", trimmedSearch);
        } else {
            url.searchParams.delete("search");
        }

        const nextUrl = url.pathname + url.search + url.hash;
        window.history.replaceState({}, "", nextUrl);
    }


    // =========================================================
    // LOAD WHEN PAGE OPENS
    // =========================================================
    //EVENT
    $(document).ready(function () {

        loadProducts();

    });