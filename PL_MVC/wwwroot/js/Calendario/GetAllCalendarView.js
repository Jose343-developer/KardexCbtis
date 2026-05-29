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

        return {
            title: item.summary,
            start: fechaInicio,
            end: fechaFin,
            classNames: [statusClass]
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
        height: 700
    });

    calendar.render();
});
