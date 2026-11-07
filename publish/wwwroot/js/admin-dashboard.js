document.addEventListener(
    "DOMContentLoaded",
    function () {
        //Find the chart-data element
        const chartDataElement =
            document.getElementById("dashboardChartData");

        if (!chartDataElement) {
            return;
        }


        let chartData;

        try {

            chartData =
                JSON.parse(chartDataElement.textContent);

        }
        catch (error) {

            console.error(
                "Failed to read dashboard chart data:",
                error
            );

            return;
        }


        /* =========================================================
           ORDER STATUS CHART
           ========================================================= */

        const orderStatusCanvas =
            document.getElementById("orderStatusChart");


        if (orderStatusCanvas) {

            new Chart(
                orderStatusCanvas,
                {
                    type: "doughnut",

                    data: {

                        labels: [
                            "Delivered",
                            "Processing",
                            "Pending"
                        ],

                        datasets: [
                            {
                                data: [
                                    chartData.orderStatus.Delivered,
                                    chartData.orderStatus.Processing,
                                    chartData.orderStatus.Pending
                                ],

                                backgroundColor: [
                                    "#198754",
                                    "#0d6efd",
                                    "#ffc107"
                                ],

                                borderWidth: 0
                            }
                        ]
                    },

                    options: {

                        responsive: true,

                        maintainAspectRatio: false,

                        cutout: "68%",

                        plugins: {

                            legend: {
                                display: false
                            }

                        }

                    }
                }
            );

        }


        /* =========================================================
           ORDER AMOUNT CHART
           ========================================================= */

        const orderAmountCanvas =
            document.getElementById("orderAmountChart");


        if (orderAmountCanvas) {

            new Chart(
                orderAmountCanvas,
                {
                    type: "bar",

                    data: {

                        labels: chartData.orderLabels,

                        datasets: [
                            {
                                label: "Order Amount",

                                data: chartData.orderAmounts,

                                backgroundColor: "#cbd5e1",

                                borderRadius: 6,

                                barThickness: 35
                            }
                        ]
                    },

                    options: {

                        responsive: true,

                        maintainAspectRatio: false,

                        plugins: {

                            legend: {
                                display: false
                            }

                        },

                        scales: {

                            y: {

                                beginAtZero: true,

                                ticks: {

                                    callback: function (value) {

                                        return "₹" +
                                            Number(value)
                                                .toLocaleString("en-IN");

                                    }

                                }

                            },

                            x: {

                                grid: {
                                    display: false
                                }

                            }

                        }

                    }
                }
            );

        }

    }
);