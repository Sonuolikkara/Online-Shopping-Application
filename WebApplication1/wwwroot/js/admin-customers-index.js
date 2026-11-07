document.addEventListener("DOMContentLoaded", function () {

    //Find delete forms    
    const deleteForms =
        document.querySelectorAll(
            'form[action*="Delete"]'
        );

        //Loop through every delete form
        deleteForms.forEach(function (form) {

        //Event
        form.addEventListener("submit", function (event) {

            const confirmed = confirm(
                "Are you sure you want to delete this customer?"
            );

            if (!confirmed) {
                event.preventDefault();
            }

        });

    });

});