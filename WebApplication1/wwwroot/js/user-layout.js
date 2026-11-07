// ============================================================
// USER LAYOUT JAVASCRIPT
// ============================================================


// ============================================================
// 1. JWT AUTHORIZATION FOR AJAX REQUESTS
// ============================================================

//Configure default behavior for jQuery AJAX requests.
//EVENT
$.ajaxSetup({
    beforeSend: function (xhr) {

        //Reading the token
        const token =
            localStorage.getItem(
                "customerAccessToken"
            );

        //Setting the Header
        //JWT gets attached to the HTTP request
        if (token) {

            xhr.setRequestHeader(
                "Authorization",
                "Bearer " + token
            );
        }
    }
});


// ============================================================
// 2. REFRESH ACCESS TOKEN
// ============================================================

function refreshAccessToken() {

    //Event
    return $.ajax({
        url: "/api/CustomerAuthApi/refresh",
        type: "POST"
    })
        .then(function (response) {

            if (!response.accessToken) {
                throw new Error(
                    "Access token was not returned."
                );
            }

            localStorage.setItem(
                "customerAccessToken",
                response.accessToken
            );

            if (response.customerId) {

                localStorage.setItem(
                    "customerId",
                    response.customerId
                );
            }

            return response.accessToken;
        })
        .catch(function (xhr) {

            console.error(
                "Token refresh failed:",
                xhr.status,
                xhr.responseText
            );

            throw xhr;
        });
}


// ============================================================
// 2b. CUSTOMER AUTH UI STATE
// ============================================================

function setCustomerAuthUi(isAuthenticated) {

    if (isAuthenticated) {

        $("#customerLoginLink")
            .hide();

        $("#customerLogoutButton")
            .show();

    } else {

        $("#customerLoginLink")
            .show();

        $("#customerLogoutButton")
            .hide();
    }
}


function clearCustomerAuthState() {

    localStorage.removeItem(
        "customerAccessToken"
    );

    localStorage.removeItem(
        "customerId"
    );

    localStorage.removeItem(
        "customerEmail"
    );
}


//Checking customer's authentication is still valid
function syncCustomerAuthState() {

    return refreshAccessToken()
        .then(function () {

            setCustomerAuthUi(true);

            return true;
        })
        .catch(function () {

            clearCustomerAuthState();

            setCustomerAuthUi(false);

            return false;
        });
}


// ============================================================
// 3. HANDLE EXPIRED JWT
// ============================================================

let isRefreshingToken = false;

let refreshPromise = null;


//Handles 401 Unauthorized responses and refreshes the access token if needed
function handleUnauthorizedRequest(
    xhr,
    settings
) {

    if (xhr.status !== 401) {
        return false;
    }

    // Prevent refresh endpoint from refreshing itself
    if (
        settings.url.includes(
            "/api/CustomerAuthApi/refresh"
        )
    ) {
        return false;
    }

    if (!isRefreshingToken) {

        isRefreshingToken = true;

        //Start the refresh
        refreshPromise =
            refreshAccessToken()
                .finally(function () {

                    isRefreshingToken = false;
                    refreshPromise = null;

                });
    }

    return refreshPromise
        .then(function () {

            // Retry the original request
            return $.ajax(settings);//Sending Failed Ajaz req again after refreshing the token

        })
        .catch(function () {

            // Refresh failed
            localStorage.removeItem(
                "customerAccessToken"
            );

            localStorage.removeItem(
                "customerId"
            );

            window.location.href =
                "/CustomerLogin";
        });
}


// ============================================================
// 4. GLOBAL AJAX ERROR HANDLING
// ============================================================

$(document).ajaxError(
    function (event, xhr, settings) {

        handleUnauthorizedRequest(
            xhr,
            settings
        );

    }
);


// ============================================================
// 5. UPDATE CART COUNT
// ============================================================

//Defines a function
function updateCartCount() {

    $.ajax({
        url: "/api/UserCartApi",
        type: "GET",

        success: function (cartItems) {

            let count = 0;

            //checks api response array or not
            if (Array.isArray(cartItems)) {

                cartItems.forEach(function (item) {

                    count +=
                        Number(item.quantity || 0);

                });
            }

            //Update the cart count in the UI
            const cartCount =
                $("#cartCount");

            if (!cartCount.length) {
                return;
            }

            cartCount.text(count);

            if (count > 0) {

                cartCount.show();

            } else {

                cartCount.hide();
            }
        },

        //Runs when Ajax req fails
        error: function (xhr) {

            console.error(
                "Failed to load cart count:",
                xhr.status,
                xhr.responseText
            );
        }
    });
}


// ============================================================
// 6. CUSTOMER LOGIN / LOGOUT UI
// ============================================================

//EVENT
$(document).ready(function () {

    const token =
        localStorage.getItem(
            "customerAccessToken"
        );

    // --------------------------------------------------------
    // LOGIN STATE
    // --------------------------------------------------------

    if (token) {

        //Update UI
        $("#customerLoginLink")
            .hide();

        $("#customerLogoutButton")
            .show();

    } else {

        $("#customerLoginLink")
            .show();

        $("#customerLogoutButton")
            .hide();
    }


    // --------------------------------------------------------
    // UPDATE CART COUNT
    // --------------------------------------------------------

    updateCartCount();


    // --------------------------------------------------------
    // LOGOUT Event
    // --------------------------------------------------------

    // Event listener for the logout button
    $("#customerLogoutButton").on(
        "click",
        function () {

            $.ajax({
                url:
                    "/api/CustomerAuthApi/logout",
                type: "POST",

                complete: function () {

                    clearCustomerAuthState();

                    setCustomerAuthUi(false);

                    // Redirect
                    window.location.href =
                        "/CustomerLogin";
                }
            });

        }
    );

});


// ============================================================
// 7. BACK/FORWARD CACHE REFRESH
// ============================================================

window.addEventListener(
    "pageshow",
    function () {

        syncCustomerAuthState()
            .always(function () {
                updateCartCount();
            });

    }
);