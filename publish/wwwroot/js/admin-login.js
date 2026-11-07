document.addEventListener("DOMContentLoaded", function () {

    const themeToggle =
        document.getElementById("themeToggle");

    const themeIcon =
        document.getElementById("themeIcon");

    const body =
        document.body;


    /* =====================================================
       SAFETY CHECK
       ===================================================== */

    if (!themeToggle || !themeIcon) {
        return;
    }


    /* =====================================================
       APPLY SAVED ADMIN THEME
       ===================================================== */

    const savedTheme =
        localStorage.getItem("admin-theme");


    if (savedTheme === "dark") {

        body.classList.add("dark-mode");

        themeIcon.classList.remove(
            "bi-moon-fill"
        );

        themeIcon.classList.add(
            "bi-sun-fill"
        );

    }
    else {

        body.classList.remove("dark-mode");

        themeIcon.classList.remove(
            "bi-sun-fill"
        );

        themeIcon.classList.add(
            "bi-moon-fill"
        );
    }


    /* =====================================================
       TOGGLE THEME
       ===================================================== */

    themeToggle.addEventListener(
        "click",
        function () {

            body.classList.toggle("dark-mode");


            const isDark =
                body.classList.contains("dark-mode");


            if (isDark) {

                localStorage.setItem(
                    "admin-theme",
                    "dark"
                );


                themeIcon.classList.remove(
                    "bi-moon-fill"
                );

                themeIcon.classList.add(
                    "bi-sun-fill"
                );

            }
            else {

                localStorage.setItem(
                    "admin-theme",
                    "light"
                );


                themeIcon.classList.remove(
                    "bi-sun-fill"
                );

                themeIcon.classList.add(
                    "bi-moon-fill"
                );
            }

        }
    );

});