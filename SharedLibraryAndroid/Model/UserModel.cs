using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using LAPhil.Droid;
using LAPhil.Events;
using LAPhil.User;

namespace SharedLibraryAndroid
{
    public class UserModel
    {
        private static UserModel instance;

        private UserModel() { }
        public List<Event> events;
        public List<Event> allEvents;
        public Ticket[] tickets=null;
        public Event[] ticketsEvent;
        public Event SelectedEvent;
        public Ticket selectedTickets;
        public String SelectedTab = "Concerts";
        public Boolean isFromMore=false;
        public Boolean isFromConcert = false;
        public String FilterEventType = "";
        public Boolean isFromSeeConcetFilter = false;
        public String FilterStartDate = "";
        public String FilterEndDate = "";
        public Boolean isFilterByDate = false;
        public List<String> filterEventList=new List<String>();
        public static UserModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserModel();
                }
                return instance;
            }
        }
    }
}
