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

            // Rellenar modal
            document.getElementById('modalEventTitle').innerText = eventObj.title;

            // Formatear rango de fechas
            var startText = eventObj.start ? formatDateTime(eventObj.start) : "";
            var endText = eventObj.end ? formatDateTime(eventObj.end) : "";
            var timeText = startText;
            if (endText && eventObj.start.toDateString() === eventObj.end.toDateString()) {
                // Mismo día, solo mostrar hora de fin
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

            // Enlace de Google Calendar
            var googleLink = document.getElementById('modalGoogleLink');
            if (props.htmlLink) {
                googleLink.href = props.htmlLink;
                googleLink.classList.remove('d-none');
            } else {
                googleLink.classList.add('d-none');
            }

            // Mostrar el modal
            var modalEl = document.getElementById('eventDetailModal');
            var modal = new bootstrap.Modal(modalEl);
            modal.show();
        }
    });

    calendar.render();

    // Lógica de Filtros y Búsqueda
    var filterButtons = document.querySelectorAll('.filter-btn');
    var currentFilter = 'all';
    var searchText = '';

    function applyFilters() {
        var filtered = eventosGoogle.filter(function(ev) {
            var matchesSearch = !searchText || 
                ev.title.toLowerCase().includes(searchText) || 
                (ev.extendedProps.description && ev.extendedProps.description.toLowerCase().includes(searchText));
            
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
