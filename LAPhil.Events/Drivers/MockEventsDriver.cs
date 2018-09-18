using System;
using System.Linq;
using System.Drawing;
using System.Reactive.Linq;
using System.Collections.Generic;
using LAPhil.Logging;
using LAPhil.Application;


namespace LAPhil.Events
{
    public class MockEventsDriver: IEventsDriver
    {
        ILog Log = ServiceContainer.Resolve<LoggingService>().GetLogger<MockEventsDriver>();

        public MockEventsDriver()
        {
            Log.Info("Initialized MockEventsDriver");
        }

        public IObservable<Event[]> UpcomingEvents(int? daysFromNow)
        {
            var dateCurrent = DateTime.Now;
            var date12WeekAgo = dateCurrent.AddDays(daysFromNow ?? 84);    //12 Weeks ago date

            return CurrentEvents()
                .SelectMany(x => x.ToObservable())
                .Where(x => x.StartTime >= dateCurrent && x.StartTime <= date12WeekAgo)
                .Aggregate(seed: new List<Event>(), accumulator: (acc, current) =>
                {
                    acc.Add(current);
                    return acc;
                })
                .Select(x => x.ToArray());
        }

        //Harish_10M
        public IObservable<Event[]> EventsTypeList()
        {
            return null;
        }

        public IObservable<Event[]> CurrentEvents()
        {
            var today = DateTime.UtcNow;
            var first = today.AddDays(1);
            var second = today.AddDays(10);
            var third = today.AddDays(20);

            var days = new int[] { 1, 10, 20 }.Select(x => today.AddDays(x)).ToArray();

            return Observable.Return(
                new Event[] {
                    new Event
                    {
                        Url = "http://laphil-dev.herokuapp.com/api/performances/4/",
                        StartTime = new DateTimeOffset(
                            year: days[0].Year, month: days[0].Month, day: days[0].Day,
                            hour: 21, minute: 30, second: 0, offset: new TimeSpan()
                        ),
                        Program = new Program{
                            Url = "http://laphil-dev.herokuapp.com/api/programs/3/",
                            Name = "test HBP"
                        },
                        
                        Series = new Series
                        {
                            Url = "http://laphil-dev.herokuapp.com/api/series/11/",
                            Name = "testseries01",
                            Description = ""
                        },
                        
                        PerformanceTalkStartTime = new DateTime(year: 0001, month: 1, day: 1, hour: 20, minute: 30, second: 0),
                        CalendarFlag = "Sold Out",
                        GateTime = new DateTimeOffset(
                            year: days[0].Year, month: days[0].Month, day: days[0].Day,
                            hour: 20, minute: 30, second: 0, offset: new TimeSpan()
                        ),
                    },
                    
                    new Event
                    {
                        Url = "http://laphil-dev.herokuapp.com/api/performances/4/",
                        StartTime = new DateTimeOffset(
                            year: days[1].Year, month: days[1].Month, day: days[1].Day,
                            hour: 21, minute: 30, second: 0, offset: new TimeSpan()
                        ),
                        Program = new Program{
                            Url = "http://laphil-dev.herokuapp.com/api/programs/3/",
                            Name = "My Program"
                        },

                        Series = new Series
                        {
                            Url = "http://laphil-dev.herokuapp.com/api/series/11/",
                            Name = "Test series 1",
                            Description = ""
                        },

                        PerformanceTalkStartTime = new DateTime(year: 0001, month: 1, day: 1, hour: 20, minute: 30, second: 0),
                        CalendarFlag = "",
                        GateTime = new DateTimeOffset(
                            year: days[1].Year, month: days[1].Month, day: days[1].Day,
                            hour: 20, minute: 30, second: 0, offset: new TimeSpan()
                        ),
                    },

                    new Event
                    {
                        Url = "http://laphil-dev.herokuapp.com/api/performances/4/",
                        StartTime = new DateTimeOffset(
                            year: days[2].Year, month: days[2].Month, day: days[2].Day,
                            hour: 21, minute: 30, second: 0, offset: new TimeSpan()
                        ),
                        Program = new Program{
                            Url = "http://laphil-dev.herokuapp.com/api/programs/3/",
                            Name = "My Program"
                        },

                        Series = new Series
                        {
                            Url = "http://laphil-dev.herokuapp.com/api/series/11/",
                            Name = "Thursday 1",
                            Description = ""
                        },

                        PerformanceTalkStartTime = null,
                        CalendarFlag = null,
                        GateTime = null,
                    }
                });
        }

        public IObservable<Event> EventDetail(Event target)
        {
            return Observable.Return(new Event
            {
                Url = target.Url,
                StartTime = target.StartTime,

                Performers = new Performer[]{
                    new Performer{
                        Url = "http://laphil-dev.herokuapp.com/api/artists/2/",
                        Name = "Performer 1",
                        Role = "Role 1",
                        Organization = "Organization 1",
                        Title = "Title 1",
                        Website = "https://example.org",
                        Bio = "<p>This <em>is</em> my <strong>content</strong>.</p>"
                    },

                    new Performer{
                        Url = "http://laphil-dev.herokuapp.com/api/artists/2/",
                        Name = "Performer 2",
                        Role = "Role 2",
                        Organization = "Organization 2",
                        Title = "Title 2",
                        Website = "https://example.org",
                        Bio = "<p>This <em>is</em> my <strong>content</strong>.</p>"
                    },
                },

                Pieces = new Piece[] {
                    new Piece{
                        Url = "http://laphil-dev.herokuapp.com/api/pieces/1/",
                        Name = "Mozart Concerto",
                        ComposerUrl = "http://laphil-dev.herokuapp.com/api/artists/1/",
                        Description = "Lorem ipsum...",
                        Duration = "15 minutes",
                        ListenText = "Lorem ipsum dolor sit amet",
                        ListenUrl = ""
                    },

                    new Piece{
                        Url = "http://laphil-dev.herokuapp.com/api/pieces/1/",
                        Name = "Mozart Concerto #2",
                        ComposerUrl = "http://laphil-dev.herokuapp.com/api/artists/1/",
                        Description = "Lorem ipsum...",
                        Duration = "about 1 hour",
                        ListenText = "Lorem ipsum dolor sit amet",
                        ListenUrl = ""
                    },

                    new Piece{
                        Url = "http://laphil-dev.herokuapp.com/api/pieces/1/",
                        Name = "Mozart Concerto #3",
                        ComposerUrl = "http://laphil-dev.herokuapp.com/api/artists/1/",
                        Description = "Lorem ipsum...",
                        Duration = "about 5 min",
                        ListenText = "Lorem ipsum dolor sit amet",
                        ListenUrl = ""
                    }
                },

                Program = new Program
                {
                    Url = target.Program.Url,
                    Name = target.Program.Name,
                },

                Series = target.Series,
                PerformanceTalkStartTime = target.PerformanceTalkStartTime,
                CalendarFlag = target.CalendarFlag,
                GateTime = target.GateTime
            });
        }

        public IObservable<byte[]> GetEventImageBytes(Event target, Size size)
        {
            return Observable.Return(new byte[]{});
        }

        public IObservable<byte[]> GetEventImage1x1Bytes(Event target, Size size)
        {
            return Observable.Return(new byte[]{});
        }

        public IObservable<byte[]> GetEventImage3x2Bytes(Event target, Size size)
        {
            return Observable.Return(new byte[]{});
        }
    }
}
