//Getting the Login Form

//EVENT
//Event for login form submission
document.getElementById("loginForm")
    .addEventListener("submit", function (event) {

        event.preventDefault();


        const email =
            document.getElementById("loginEmail");

        const password =
            document.getElementById("loginPassword");

        const error =
            document.getElementById("loginError");


        // Clear previous custom error

        error.style.display = "none";
        error.textContent = "";


        // =========================================
        // 1. EMAIL
        // =========================================
        //Email Vaildation
        if (email.value.trim() === "") {

            error.textContent =
                "Please enter your email.";

            error.style.display = "block";

            email.focus();

            return;
        }


        // =========================================
        // 2. EMAIL FORMAT
        // =========================================

        if (!email.checkValidity()) {

            error.textContent =
                "Please enter a valid email address.";

            error.style.display = "block";

            email.focus();

            return;
        }


        // =========================================
        // 3. PASSWORD
        // =========================================
        //Password Validation
        if (password.value.trim() === "") {

            error.textContent =
                "Please enter your password.";

            error.style.display = "block";

            password.focus();

            return;
        }


        // =========================================
        // LOGIN API
        // =========================================

        $.ajax({

            url: "/api/CustomerAuthApi/login",

            type: "POST",

            contentType: "application/json",

            //Creating the request body
            data: JSON.stringify({

                email: email.value.trim(),

                password: password.value

            }),


            success: function (response) {

                 // Store access token

                 localStorage.setItem(
                       "customerAccessToken",
                        response.accessToken
                 );

                 //Store Customer ID
                if (response.customerId) {

                    localStorage.setItem(
                        "customerId",
                        response.customerId
                    );

                }


                // Go to customer home

                window.location.href =
                    "/UserHome";

            },

            
            // Runs when Ajax req fails
            error: function (xhr) {

                let message =
                    "Invalid email or password.";


                if (
                    xhr.responseJSON &&
                    xhr.responseJSON.message
                ) {

                    message =
                        xhr.responseJSON.message;

                }


                error.textContent =
                    message;

                error.style.display =
                    "block";

            }

        });

    });