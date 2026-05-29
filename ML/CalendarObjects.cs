using System;

namespace ML;

public class CalendarObjects
{
public object anyoneCanAddSelf { get; set; }
        public object attachments { get; set; }
        public object attendees { get; set; }
        public object attendeesOmitted { get; set; }
        public object birthdayProperties { get; set; }
        public object colorId { get; set; }
        public object conferenceData { get; set; }
        public DateTime createdRaw { get; set; }
        public DateTime createdDateTimeOffset { get; set; }
        public DateTime created { get; set; }
        public Creator creator { get; set; }
        public object description { get; set; }
        public End end { get; set; }
        public object endTimeUnspecified { get; set; }
        public string eTag { get; set; }
        public string eventType { get; set; }
        public object extendedProperties { get; set; }
        public object focusTimeProperties { get; set; }
        public object gadget { get; set; }
        public object guestsCanInviteOthers { get; set; }
        public object guestsCanModify { get; set; }
        public object guestsCanSeeOtherGuests { get; set; }
        public object hangoutLink { get; set; }
        public string htmlLink { get; set; }
        public string iCalUID { get; set; }
        public string id { get; set; }
        public string kind { get; set; }
        public object location { get; set; }
        public object locked { get; set; }
        public Organizer organizer { get; set; }
        public object originalStartTime { get; set; }
        public object outOfOfficeProperties { get; set; }
        public object privateCopy { get; set; }
        public object recurrence { get; set; }
        public object recurringEventId { get; set; }
        public Reminders reminders { get; set; }
        public int sequence { get; set; }
        public object source { get; set; }
        public Start start { get; set; }
        public string status { get; set; }
        public string summary { get; set; }
        public string transparency { get; set; }
        public DateTime updatedRaw { get; set; }
        public DateTime updatedDateTimeOffset { get; set; }
        public DateTime updated { get; set; }
        public object visibility { get; set; }
        public object workingLocationProperties { get; set; }
    }

    public class Creator
    {
        public object displayName { get; set; }
        public string email { get; set; }
        public object id { get; set; }
        public object self { get; set; }
    }

    public class End
    {
        public string date { get; set; }
        public DateTime? dateTimeRaw { get; set; }
        public DateTime? dateTimeDateTimeOffset { get; set; }
        public DateTime? dateTime { get; set; }
        public string timeZone { get; set; }
        public object eTag { get; set; }
    }

    public class Organizer
    {
        public string displayName { get; set; }
        public string email { get; set; }
        public object id { get; set; }
        public bool self { get; set; }
    }

    public class Reminders
    {
        public object overrides { get; set; }
        public bool useDefault { get; set; }
    }

    public class Start
    {
        public string date { get; set; }
        public DateTime? dateTimeRaw { get; set; }
        public DateTime? dateTimeDateTimeOffset { get; set; }
        public DateTime? dateTime { get; set; }
        public string timeZone { get; set; }
        public object eTag { get; set; }
    }