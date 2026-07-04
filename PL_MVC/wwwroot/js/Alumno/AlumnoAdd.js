$(document).ready(function () {
    
    // 0. Ocultamos todos los grupos de la lista maestra al inicio
    $("#ddlGrupo option[data-semestre]").hide();

    // ==========================================
    // PASO 1: Cuando el usuario cambia el SEMESTRE
    // ==========================================
    $("#ddlsemestre").change(function () {
        var semestreElegido = $(this).val();
        
        var ddlTurno = $("#ddlTurno");
        var ddlGrupo = $("#ddlGrupo");

        // Reseteamos y bloqueamos los siguientes pasos
        ddlTurno.empty();
        ddlGrupo.val("").prop("disabled", true);
        $("#ddlGrupo option[data-semestre]").hide();

        if (semestreElegido != "") {
            // Quitamos el candado al Turno
            ddlTurno.prop("disabled", false);
            ddlTurno.append('<option value="">Seleccione un Turno</option>');

            // Buscamos qué turnos existen para este semestre
            var turnosEncontrados = [];
            
            $("#ddlGrupo option[data-semestre='" + semestreElegido + "']").each(function () {
                var turno = $(this).attr("data-turno");
                
                // Si no hemos agregado este turno a la lista, lo agregamos
                if ($.inArray(turno, turnosEncontrados) === -1) {
                    turnosEncontrados.push(turno);
                    ddlTurno.append('<option value="' + turno + '">' + turno + '</option>');
                }
            });
        } else {
            ddlTurno.prop("disabled", true);
            ddlTurno.append('<option value="">Primero seleccione Semestre</option>');
        }
        validateField($(this), $(this).val() !== "");
    });

    // ==========================================
    // PASO 2: Cuando el usuario cambia el TURNO
    // ==========================================
    $("#ddlTurno").change(function () {
        var turnoElegido = $(this).val();
        var semestreElegido = $("#ddlsemestre").val();
        var ddlGrupo = $("#ddlGrupo");

        // Escondemos las opciones y limpiamos el valor actual
        $("#ddlGrupo option[data-semestre]").hide();
        ddlGrupo.val("");

        if (turnoElegido != "") {
            // Quitamos el candado al Grupo
            ddlGrupo.prop("disabled", false);
            
            // Mostramos SOLO el grupo que coincida con el Semestre Y el Turno
            $("#ddlGrupo option[data-semestre='" + semestreElegido + "'][data-turno='" + turnoElegido + "']").show();
        } else {
            ddlGrupo.prop("disabled", true);
        }
        validateField($(this), $(this).val() !== "");
    });

    $("#ddlGrupo").change(function () {
        validateField($(this), $(this).val() !== "");
    });

    $("#ddlEspecialidad").change(function () {
        validateField($(this), $(this).val() !== "");
    });

    // ==========================================
    // VALIDACIÓN DE CAMPOS EN TIEMPO REAL
    // ==========================================
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

    $("#Matricula").on("input", function() {
        var val = $(this).val().trim();
        validateField($(this), val !== "" && regexDigits.test(val) && val.length <= 20);
    });

    $("#Curp").on("input", function() {
        var val = $(this).val().trim().toUpperCase();
        $(this).val(val);
        validateField($(this), val !== "" && regexCurp.test(val));
    });

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

    $("#NombreUser").on("input", function() {
        var val = $(this).val().trim();
        validateField($(this), val !== "" && val.length <= 100);
    });

    $("#Password").on("input", function() {
        var val = $(this).val();
        validateField($(this), val !== "" && val.length >= 4 && val.length <= 500);
    });

    // Validar en el submit del formulario
    $("#alumnoForm").submit(function (e) {
        var isValid = true;

        // Trigger input validation
        $("#Nombre").trigger("input");
        $("#ApellidoPaterno").trigger("input");
        $("#Matricula").trigger("input");
        $("#Curp").trigger("input");
        $("#NombreUser").trigger("input");
        $("#Password").trigger("input");
        
        if ($("#ApellidoMaterno").val().trim() !== "") $("#ApellidoMaterno").trigger("input");
        if ($("#Correo").val().trim() !== "") $("#Correo").trigger("input");
        if ($("#Telefono").val().trim() !== "") $("#Telefono").trigger("input");
        if ($("#Celular").val().trim() !== "") $("#Celular").trigger("input");

        // Validate select lists
        if (!validateField($("#ddlsemestre"), $("#ddlsemestre").val() !== "")) isValid = false;
        if (!validateField($("#ddlTurno"), $("#ddlTurno").val() !== "" && $("#ddlTurno").val() !== null)) isValid = false;
        if (!validateField($("#ddlGrupo"), $("#ddlGrupo").val() !== "")) isValid = false;
        if (!validateField($("#ddlEspecialidad"), $("#ddlEspecialidad").val() !== "")) isValid = false;

        // Check if any element has class is-invalid
        if ($(".is-invalid").length > 0) {
            isValid = false;
        }

        if (!isValid) {
            e.preventDefault();
            // Focus the first invalid element
            $(".is-invalid").first().focus();
        }
    });

    // ==========================================
    // BUSCADOR Y ELIMINADOR DE ALUMNOS (ADMIN)
    // ==========================================
    $("#btnSearchAlumno").click(function () {
        var term = $("#searchInput").val().trim();
        if (term === "") {
            alert("Por favor ingresa un término de búsqueda.");
            return;
        }

        // Show loading state if needed
        var btn = $(this);
        var originalText = btn.html();
        btn.html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Buscando...');
        btn.prop("disabled", true);

        $.ajax({
            url: '/Alumno/SearchAlumnos',
            type: 'GET',
            data: { term: term },
            success: function (res) {
                btn.html(originalText);
                btn.prop("disabled", false);

                var tbody = $("#tableAlumnosResults tbody");
                tbody.empty();

                if (res.correct && res.objects && res.objects.length > 0) {
                    $("#noResultsMsg").hide();
                    $("#tableAlumnosResults").show();

                    $.each(res.objects, function (i, item) {
                        var nombreCompleto = item.nombre + " " + item.apellidoPaterno + (item.apellidoMaterno ? " " + item.apellidoMaterno : "");
                        var correo = item.correo || "N/A";
                        var usuario = (item.usuario && item.usuario.nombreUser) ? item.usuario.nombreUser : "Sin Usuario";
                        var idUsuario = item.usuario ? item.usuario.idUsuario : 0;
                        var grupo = item.grupo ? (item.grupo.semestre + item.grupo.letra + " " + item.grupo.turno) : "N/A";

                        var tr = $("<tr>");
                        tr.append($("<td>").text(item.matricula));
                        tr.append($("<td>").text(nombreCompleto));
                        tr.append($("<td>").text(correo));
                        tr.append($("<td>").text(grupo));
                        tr.append($("<td>").text(usuario));
                        
                        var btnDelete = $("<button>")
                            .addClass("btn btn-danger btn-sm")
                            .html('<i class="bi bi-trash"></i>')
                            .attr("title", "Eliminar Alumno")
                            .click(function () {
                                if (confirm("¿Estás seguro de que deseas ELIMINAR PERMANENTEMENTE a este alumno y su usuario? Esta acción borrará todas sus calificaciones y no se puede deshacer.")) {
                                    deleteAlumnoFull(item.idAlumno, idUsuario, tr);
                                }
                            });
                        
                        tr.append($('<td class="text-center">').append(btnDelete));
                        tbody.append(tr);
                    });
                } else {
                    $("#tableAlumnosResults").hide();
                    $("#noResultsMsg").text(res.errorMessage || "No se encontraron alumnos.").show();
                }
            },
            error: function () {
                btn.html(originalText);
                btn.prop("disabled", false);
                alert("Ocurrió un error al buscar.");
            }
        });
    });

    // Enter key para buscar
    $("#searchInput").keypress(function (e) {
        if (e.which == 13) {
            e.preventDefault();
            $("#btnSearchAlumno").click();
        }
    });

    function deleteAlumnoFull(idAlumno, idUsuario, rowElement) {
        $.ajax({
            url: '/Alumno/DeleteFull',
            type: 'POST',
            data: { idAlumno: idAlumno, idUsuario: idUsuario },
            success: function (res) {
                if (res.correct) {
                    rowElement.fadeOut(400, function() { $(this).remove(); });
                    alert("Alumno eliminado correctamente.");
                } else {
                    alert("Error al eliminar: " + res.errorMessage);
                }
            },
            error: function () {
                alert("Ocurrió un error en el servidor al intentar eliminar.");
            }
        });
    }
});
