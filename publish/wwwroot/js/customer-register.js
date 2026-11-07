// REGISTER FORM -> API

document.addEventListener("DOMContentLoaded", function () {

    const form = document.getElementById("registerForm");
    if (!form) {
        return;
    }

    form.addEventListener("submit", function (event) {

        event.preventDefault();

        const firstName = document.getElementById("firstName");
        const lastName = document.getElementById("lastName");
        const email = document.getElementById("registerEmail");
        const password = document.getElementById("registerPassword");
        const phone = document.getElementById("phone");
        const address = document.getElementById("address");
        const error = document.getElementById("registerError");

        error.style.display = "none";
        error.textContent = "";

        //Validations
        if (firstName.value.trim() === "") {
            error.textContent = "Please enter your first name.";
            error.style.display = "block";
            firstName.focus();
            return;
        }

        if (lastName.value.trim() === "") {
            error.textContent = "Please enter your last name.";
            error.style.display = "block";
            lastName.focus();
            return;
        }

        if (email.value.trim() === "") {
            error.textContent = "Please enter your email.";
            error.style.display = "block";
            email.focus();
            return;
        }

        if (!email.checkValidity()) {
            error.textContent = "Please enter a valid email address.";
            error.style.display = "block";
            email.focus();
            return;
        }

        if (password.value.trim() === "") {
            error.textContent = "Please enter your password.";
            error.style.display = "block";
            password.focus();
            return;
        }

        if (password.value.length < 6) {
            error.textContent = "Password must be at least 6 characters.";
            error.style.display = "block";
            password.focus();
            return;
        }

        if (phone.value.trim() === "") {
            error.textContent = "Please enter your phone number.";
            error.style.display = "block";
            phone.focus();
            return;
        }

        if (!phone.checkValidity()) {
            error.textContent = "Please enter a valid phone number.";
            error.style.display = "block";
            phone.focus();
            return;
        }

        if (address.value.trim() === "") {
            error.textContent = "Please enter your address.";
            error.style.display = "block";
            address.focus();
            return;
        }

        //Ajax Post
        $.ajax({
            url: "/api/CustomerAuthApi/register",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                firstName: firstName.value.trim(),
                lastName: lastName.value.trim(),
                email: email.value.trim(),
                password: password.value,
                phone: phone.value.trim(),
                address: address.value.trim()
            }),
            success: function () {
                window.location.href = "/CustomerLogin";
            },
            error: function (xhr) {
                let message = "Registration failed.";
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    message = xhr.responseJSON.message;
                }
                error.textContent = message;
                error.style.display = "block";
            }
        });

    });

});
