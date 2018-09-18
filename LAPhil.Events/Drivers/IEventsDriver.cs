using System;
using System.Drawing;


namespace LAPhil.Events
{
    public interface IEventsDriver
    {
        IObservable<Event[]> UpcomingEvents(int? daysFromNow);
        IObservable<Event[]> CurrentEvents();
        IObservable<Event> EventDetail(Event target);
        IObservable<byte[]> GetEventImageBytes(Event target, Size size);
        IObservable<byte[]> GetEventImage1x1Bytes(Event target, Size size);
        IObservable<byte[]> GetEventImage3x2Bytes(Event target, Size size);

        IObservable<Event[]> EventsTypeList();

    }
}
