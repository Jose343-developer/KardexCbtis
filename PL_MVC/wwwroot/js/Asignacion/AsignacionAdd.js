document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("frmAsignacion");
    const timeInicio = document.getElementById("timeInicio");
    const timeFin = document.getElementById("timeFin");

    if (form) {
        form.addEventListener("submit", function (e) {
            let isValid = true;
            
            // Clear custom states
            timeFin.classList.remove("is-invalid");
            
            // Native validation
            if (!form.checkValidity()) {
                e.preventDefault();
                e.stopPropagation();
                isValid = false;
            }

            // Custom validation: HoraInicio < HoraFin
            if (timeInicio.value && timeFin.value) {
                const start = timeInicio.value;
                const end = timeFin.value;

                if (start >= end) {
                    e.preventDefault();
                    e.stopPropagation();
                    timeFin.classList.add("is-invalid");
                    document.getElementById("valFin").innerText = "La hora de fin debe ser posterior a la hora de inicio.";
                    isValid = false;
                }
            }

            form.classList.add("was-validated");
        });
    }

    // Interactive checks
    if (timeInicio && timeFin) {
        timeInicio.addEventListener("change", checkHours);
        timeFin.addEventListener("change", checkHours);
    }

    function checkHours() {
        if (timeInicio.value && timeFin.value) {
            if (timeInicio.value >= timeFin.value) {
                timeFin.classList.add("is-invalid");
                const valFin = document.getElementById("valFin");
                if (valFin) {
                    valFin.innerText = "La hora de fin debe ser posterior.";
                }
            } else {
                timeFin.classList.remove("is-invalid");
                timeFin.classList.add("is-valid");
            }
        }
    }
});
