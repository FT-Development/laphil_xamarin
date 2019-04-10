using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using LAPhil.Events;
using LAPhil.User;

namespace HollywoodBowl.Droid
{
    public class UserModel
    {
        private static UserModel instance;

        private UserModel() { }
        public List<Event> events;
        public List<Event> allEvents;
        public Ticket[] tickets = null;
        public Event[] ticketsEvent;
        public Event SelectedEvent;
        public Ticket selectedTickets;
        public String SelectedTab= "Concerts";
        public String FilterEventType = "";
        public Boolean isFromMore=false;
        public Boolean isFromConcert = false;
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
