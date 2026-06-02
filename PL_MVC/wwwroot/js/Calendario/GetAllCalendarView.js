document.addEventListener('DOMContentLoaded', function() {
    var calendarEl = document.getElementById('calendar');
    if (!calendarEl) return;

    var rawEvents = [];
    var eventsAttr = calendarEl.getAttribute('data-events');
    if (eventsAttr) {
        try {
            rawEvents = JSON.parse(eventsAttr) || [];
        } catch (e) {
            console.error("Error parsing calendar events data", e);
        }
    }

    var eventosGoogle = rawEvents.map(function(item) {
        // FullCalendar needs dates in ISO format (yyyy-MM-ddTHH:mm:ss)
        var fechaInicio = item.start && item.start.dateTime ? item.start.dateTime : (item.start && item.start.date ? item.start.date : "");
        var fechaFin = item.end && item.end.dateTime ? item.end.dateTime : (item.end && item.end.date ? item.end.date : "");

        var statusClass = "event-guinda"; // El color guinda para los eventos
        if (item.status === "tentative") statusClass = "event-orange";
        if (item.status === "cancelled") statusClass = "event-red";
        if (item.status === "class-schedule") statusClass = "event-blue";

        return {
            id: item.id,
            title: item.summary,
            start: fechaInicio,
            end: fechaFin,
            classNames: [statusClass],
            extendedProps: {
                description: item.description,
                status: item.status,
                location: item.location,
                htmlLink: item.htmlLink
            }
        };
    });

    var selectedEventId = null;

    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        locale: 'es',
        themeSystem: 'bootstrap5',
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
        },
        buttonText: {
            today: 'Hoy',
            month: 'Mes',
            week: 'Semana',
            day: 'Día',
            list: 'Agenda'
        },
        events: eventosGoogle,
        height: 700,
        eventClick: function(info) {
            var eventObj = info.event;
            var props = eventObj.extendedProps;
            selectedEventId = eventObj.id;

            // Rellenar modal
            document.getElementById('modalEventTitle').innerText = eventObj.title;

            // Formatear rango de fechas
            var startText = eventObj.start ? formatDateTime(eventObj.start) : "";
            var endText = eventObj.end ? formatDateTime(eventObj.end) : "";
            var timeText = startText;
            if (endText && eventObj.start.toDateString() === eventObj.end.toDateString()) {
                var endHourOptions = { hour: '2-digit', minute: '2-digit', hour12: true };
                timeText += " - " + eventObj.end.toLocaleTimeString('es-ES', endHourOptions);
            } else if (endText) {
                timeText += " a " + endText;
            }
            document.getElementById('modalEventTime').innerText = timeText;

            // Ubicación
            var locContainer = document.getElementById('modalEventLocationContainer');
            var locText = props.location || "";
            if (locText) {
                locContainer.style.display = "block";
                document.getElementById('modalEventLocation').innerText = locText;
            } else {
                locContainer.style.display = "none";
            }

            // Descripción
            var descContainer = document.getElementById('modalEventDescriptionContainer');
            var descText = props.description || "";
            if (descText) {
                descContainer.style.display = "block";
                document.getElementById('modalEventDescription').innerText = descText;
            } else {
                descContainer.style.display = "none";
            }

            // Estatus Badge
            var badge = document.getElementById('modalStatusBadge');
            badge.className = "badge rounded-pill px-3 py-2 fs-6";
            var status = props.status || "confirmed";

            if (status === "tentative") {
                badge.innerText = "Tentativo";
                badge.classList.add("bg-warning", "text-dark");
            } else if (status === "cancelled") {
                badge.innerText = "Cancelado";
                badge.classList.add("bg-danger");
            } else if (status === "class-schedule") {
                badge.innerText = "Horario de Clase";
                badge.classList.add("bg-primary");
            } else {
                badge.innerText = "Confirmado";
                badge.classList.add("bg-success");
            }

            // Mostrar/Ocultar botón de eliminar (solo habilitado para eventos creados locales que no sean clases)
            var deleteBtn = document.getElementById('btnDeleteEvent');
            if (status !== "class-schedule") {
                deleteBtn.classList.remove('d-none');
            } else {
                deleteBtn.classList.add('d-none');
            }

            // Mostrar el modal
            var modalEl = document.getElementById('eventDetailModal');
            var modal = new bootstrap.Modal(modalEl);
            modal.show();
        }
    });

    calendar.render();

    // 1. Guardar Nuevo Evento (AJAX)
    var formAddEvent = document.getElementById('formAddEvent');
    formAddEvent.addEventListener('submit', function(e) {
        e.preventDefault();
        
        if (!formAddEvent.checkValidity()) {
            formAddEvent.classList.add('was-validated');
            return;
        }

        var titulo = document.getElementById('eventTitle').value;
        var fechaInicio = document.getElementById('eventStartDate').value;
        var fechaFin = document.getElementById('eventEndDate').value;
        var ubicacion = document.getElementById('eventLocation').value;
        var estatus = document.getElementById('eventStatus').value;
        var descripcion = document.getElementById('eventDescription').value;

        // Validar que fin sea posterior a inicio
        if (new Date(fechaFin) <= new Date(fechaInicio)) {
            alert('La fecha de fin debe ser posterior a la fecha de inicio.');
            return;
        }

        var requestBody = {
            Titulo: titulo,
            FechaInicio: fechaInicio,
            FechaFin: fechaFin,
            Ubicacion: ubicacion,
            Estatus: estatus,
            Descripcion: descripcion
        };

        fetch('/Calendario/AddEvent', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(requestBody)
        })
        .then(function(res) { return res.json(); })
        .then(function(data) {
            if (data.correct) {
                // Definir clase de estado
                var statusClass = "event-guinda";
                if (estatus === "tentative") statusClass = "event-orange";
                if (estatus === "cancelled") statusClass = "event-red";

                var newEventObj = {
                    id: data.idEvento.toString(),
                    title: titulo,
                    start: fechaInicio,
                    end: fechaFin,
                    classNames: [statusClass],
                    extendedProps: {
                        description: descripcion,
                        status: estatus,
                        location: ubicacion
                    }
                };

                // Agregar al calendario
                calendar.addEvent(newEventObj);
                // Guardar en array local para filtros
                eventosGoogle.push(newEventObj);

                // Limpiar y cerrar modal
                formAddEvent.reset();
                formAddEvent.classList.remove('was-validated');
                var modalInstance = bootstrap.Modal.getInstance(document.getElementById('addEventModal'));
                modalInstance.hide();
            } else {
                alert('Error al guardar el evento: ' + data.errorMessage);
            }
        })
        .catch(function(err) {
            console.error('Error en AJAX de agregar evento:', err);
            alert('Ocurrió un error al intentar conectarse con el servidor.');
        });
    });

    // 2. Eliminar Evento (AJAX)
    var deleteBtn = document.getElementById('btnDeleteEvent');
    deleteBtn.addEventListener('click', function() {
        if (!selectedEventId) return;

        if (confirm('¿Está seguro de que desea eliminar este evento permanentemente?')) {
            var urlParams = new URLSearchParams();
            urlParams.append('idEvento', selectedEventId);

            fetch('/Calendario/DeleteEvent?' + urlParams.toString(), {
                method: 'POST'
            })
            .then(function(res) { return res.json(); })
            .then(function(data) {
                if (data.correct) {
                    // Remover de FullCalendar
                    var calEvent = calendar.getEventById(selectedEventId);
                    if (calEvent) calEvent.remove();

                    // Remover del array local
                    eventosGoogle = eventosGoogle.filter(function(ev) {
                        return ev.id !== selectedEventId;
                    });

                    // Cerrar modal
                    var modalInstance = bootstrap.Modal.getInstance(document.getElementById('eventDetailModal'));
                    modalInstance.hide();
                    selectedEventId = null;
                } else {
                    alert('Error al eliminar el evento: ' + data.errorMessage);
                }
            })
            .catch(function(err) {
                console.error('Error en AJAX de eliminar evento:', err);
                alert('Ocurrió un error al intentar comunicarse con el servidor.');
            });
        }
    });

    // Lógica de Filtros y Búsqueda
    var filterButtons = document.querySelectorAll('.filter-btn');
    var currentFilter = 'all';
    var searchText = '';

    function applyFilters() {
        var filtered = eventosGoogle.filter(function(ev) {
            var titleMatch = ev.title.toLowerCase();
            var descMatch = ev.extendedProps.description ? ev.extendedProps.description.toLowerCase() : '';
            var matchesSearch = !searchText || 
                titleMatch.includes(searchText) || 
                descMatch.includes(searchText);
            
            var matchesCategory = currentFilter === 'all' || 
                (currentFilter === 'class-schedule' && ev.extendedProps.status === 'class-schedule') ||
                (currentFilter === 'confirmed' && ev.extendedProps.status !== 'class-schedule' && ev.extendedProps.status !== 'tentative' && ev.extendedProps.status !== 'cancelled') ||
                (currentFilter === 'tentative' && ev.extendedProps.status === 'tentative');
                
            return matchesSearch && matchesCategory;
        });

        calendar.removeAllEvents();
        calendar.addEventSource(filtered);
    }

    filterButtons.forEach(function(btn) {
        btn.addEventListener('click', function() {
            filterButtons.forEach(function(b) { b.classList.remove('active'); });
            btn.classList.add('active');
            currentFilter = btn.getAttribute('data-filter');
            applyFilters();
        });
    });

    var searchInput = document.getElementById('calendarSearchInput');
    if (searchInput) {
        searchInput.addEventListener('input', function(e) {
            searchText = e.target.value.toLowerCase().trim();
            applyFilters();
        });
    }

    function formatDateTime(date) {
        var options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric', hour: '2-digit', minute: '2-digit', hour12: true };
        return date.toLocaleDateString('es-ES', options);
    }
});
