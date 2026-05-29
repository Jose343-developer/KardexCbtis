document.addEventListener("DOMContentLoaded", function () {
    const ctx = document.getElementById('academicChart');
    if (ctx) {
        const aprobadas = parseFloat(ctx.getAttribute('data-aprobadas')) || 0;
        const reprobadas = parseFloat(ctx.getAttribute('data-reprobadas')) || 0;
        new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: ['Aprobadas', 'Reprobadas'],
                datasets: [{
                    data: [aprobadas, reprobadas],
                    backgroundColor: ['#2ec4b6', '#e71d36'],
                    borderWidth: 2
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: {
                        position: 'bottom'
                    }
                }
            }
        });
    }
});
