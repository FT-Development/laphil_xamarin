using System;
using System.Drawing;
using LAPhil.Cache;


namespace LAPhil.Events
{
    public class EventService
    {
        public readonly IEventsDriver Driver;

        public EventService(IEventsDriver driver)
        {
            Driver = driver;
        }

        public IObservable<Event[]> UpcomingEvents(int daysFromNow = 84)
        {
            return Driver.UpcomingEvents(daysFromNow);
        }

        //Harish_10M
        public IObservable<Event[]> EventsTypeList()
        {
            return Driver.EventsTypeList();
        }


        public IObservable<Event[]> CurrentEvents()
        {
            return Driver.CurrentEvents();
        }

        public IObservable<Event> EventDetail(Event target)
        {
            return Driver.EventDetail(target);
        }

        public IObservable<byte[]> GetEventImage1x1Bytes(Event target, Size size)
        {
            return Driver.GetEventImage1x1Bytes(target: target, size: size);
        }

        public IObservable<byte[]> GetEventImage3x2Bytes(Event target, Size size)
        {
            return Driver.GetEventImage3x2Bytes(target: target, size: size);
        }

        public IObservable<byte[]> GetEventImageBytes(Event target, Size size)
        {
            return Driver.GetEventImageBytes(target: target, size: size);
        }
    }
}
