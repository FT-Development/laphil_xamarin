using Android.Content;
using Android.Views;
using Android.Widget;

namespace MonoDroid.TimesSquare
{
    public class MonthAdapter : BaseAdapter<MonthDescriptor>
    {
        private readonly LayoutInflater _inflater;
        private readonly CalendarPickerView _calendar;
        private readonly Context context;
        public MonthAdapter(Context context, CalendarPickerView calendar)
        {
            _calendar = calendar;
            _inflater = LayoutInflater.From(context);
            this.context = context;
        }

        public override MonthDescriptor this[int position]
        {
            get { return _calendar.Months[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override bool IsEnabled(int position)
        {
            return false;
        }

        public override int Count
        {
            get { return _calendar.Months.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var c = _calendar;
            var monthView = (MonthView) convertView ??
                            MonthView.Create(parent, _inflater, c.WeekdayNameFormat, c.Today,
                                c.ClickHandler, c.DividerColor, c.DayBackgroundResID, c.DayTextColorResID,
                                             c.TitleTextColor, c.HeaderTextColor,context);
            monthView.Init(_calendar.Months[position], _calendar.Cells[position],context);
            return monthView;
        }
    }
}