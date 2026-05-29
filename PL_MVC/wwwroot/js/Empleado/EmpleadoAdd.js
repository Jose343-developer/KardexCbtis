$(document).ready(function () {

    function validateField(element, condition) {
        if (condition) {
            element.removeClass("is-invalid").addClass("is-valid");
            return true;
        } else {
            element.removeClass("is-valid").addClass("is-invalid");
            return false;
        }
    }

    var regexLetters = /^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$/;
    var regexDigits = /^\d+$/;
    var regexCurp = /^[A-Z]{4}\d{6}[HM][A-Z]{5}[A-Z\d]\d$/;
    var regexRfc = /^[A-Z&Ññ]{3,4}\d{6}[A-Z\d]{3}$/;
    var regexEmail = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

    $("#Nombre").on("input", function() {
        var val = $(this).val().trim();
        validateField($(this), val !== "" && regexLetters.test(val) && val.length <= 50);
    });

    $("#ApellidoPaterno").on("input", function() {
        var val = $(this).val().trim();
        validateField($(this), val !== "" && regexLetters.test(val) && val.length <= 50);
    });

    $("#ApellidoMaterno").on("input", function() {
        var val = $(this).val().trim();
        if (val === "") {
            $(this).removeClass("is-invalid is-valid");
        } else {
            validateField($(this), regexLetters.test(val) && val.length <= 50);
        }
    });

    $("#Curp").on("input", function() {
        var val = $(this).val().trim().toUpperCase();
        $(this).val(val);
        validateField($(this), val !== "" && regexCurp.test(val));
    });

    $("#Rfc").on("input", function() {
        var val = $(this).val().trim().toUpperCase();
        $(this).val(val);
        validateField($(this), val !== "" && regexRfc.test(val) && (val.length === 12 || val.length === 13));
    });

    // Make sure Correo has proper validation
    $("#Correo").on("input", function() {
        var val = $(this).val().trim();
        if (val === "") {
            $(this).removeClass("is-invalid is-valid");
        } else {
            validateField($(this), regexEmail.test(val) && val.length <= 100);
        }
    });

    $("#Telefono").on("input", function() {
        var val = $(this).val().trim();
        if (val === "") {
            $(this).removeClass("is-invalid is-valid");
        } else {
            validateField($(this), regexDigits.test(val) && val.length === 10);
        }
    });

    $("#Celular").on("input", function() {
        var val = $(this).val().trim();
        if (val === "") {
            $(this).removeClass("is-invalid is-valid");
        } else {
            validateField($(this), regexDigits.test(val) && val.length === 10);
        }
    });

    $("#Departamento").on("input", function() {
        var val = $(this).val().trim();
        validateField($(this), val !== "" && val.length <= 50);
    });

    $("#NombreUser").on("input", function() {
        var val = $(this).val().trim();
        validateField($(this), val !== "" && val.length >= 3 && val.length <= 100);
    });

    $("#Password").on("input", function() {
        var val = $(this).val();
        validateField($(this), val !== "" && val.length >= 4 && val.length <= 500);
    });

    $("#ddlRol").change(function () {
        validateField($(this), $(this).val() !== "");
    });

    $("#empleadoForm").submit(function (e) {
        var isValid = true;

        // Trigger inputs
        $("#Nombre").trigger("input");
        $("#ApellidoPaterno").trigger("input");
        $("#Curp").trigger("input");
        $("#Rfc").trigger("input");
        $("#Departamento").trigger("input");
        $("#NombreUser").trigger("input");
        $("#Password").trigger("input");

        if ($("#ApellidoMaterno").val().trim() !== "") $("#ApellidoMaterno").trigger("input");
        if ($("#Correo").val().trim() !== "") $("#Correo").trigger("input");
        if ($("#Telefono").val().trim() !== "") $("#Telefono").trigger("input");
        if ($("#Celular").val().trim() !== "") $("#Celular").trigger("input");

        // Validate Role
        if (!validateField($("#ddlRol"), $("#ddlRol").val() !== "")) isValid = false;

        if ($(".is-invalid").length > 0) {
            isValid = false;
        }

        if (!isValid) {
            e.preventDefault();
            $(".is-invalid").first().focus();
        }
    });
});
