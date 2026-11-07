document.addEventListener("DOMContentLoaded", function () {

    /* =====================================================
       GET ELEMENTS
       ===================================================== */

    const selectAll =
        document.getElementById("selectAll");

    const productCheckboxes =
        document.querySelectorAll(".product-checkbox");

    const deleteSelectedButton =
        document.getElementById("deleteSelectedButton");


    /* =====================================================
       SAFETY CHECK
       ===================================================== */

    if (!selectAll ||
        !deleteSelectedButton ||
        productCheckboxes.length === 0) {

        return;
    }


    /* =====================================================
       UPDATE DELETE BUTTON
       ===================================================== */

    function updateDeleteButton() {

        const selected =
            document.querySelectorAll(
                ".product-checkbox:checked"
            );

        deleteSelectedButton.disabled =
            selected.length === 0;
    }


    /* =====================================================
       SELECT ALL
       ===================================================== */

    selectAll.addEventListener(
        "change",
        function () {

            productCheckboxes.forEach(
                function (checkbox) {

                    checkbox.checked =
                        selectAll.checked;

                }
            );

            updateDeleteButton();

        }
    );


    /* =====================================================
       INDIVIDUAL CHECKBOXES
       ===================================================== */

    productCheckboxes.forEach(
        function (checkbox) {

            checkbox.addEventListener(
                "change",
                function () {

                    const selectedCount =
                        document.querySelectorAll(
                            ".product-checkbox:checked"
                        ).length;


                    if (
                        selectedCount ===
                        productCheckboxes.length
                    ) {

                        selectAll.checked = true;

                    }
                    else {

                        selectAll.checked = false;

                    }


                    updateDeleteButton();

                }
            );

        }
    );


    /* =====================================================
       INITIAL STATE
       ===================================================== */

    updateDeleteButton();

});