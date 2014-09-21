using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WeightWatcher
{
    public class DailyWeight : INotifyPropertyChanged
    {
        private double _weight;
        public double Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
                onPropertyChanged(this, "Weight");
            }
        }

        private DateTime _dateTime;
        public DateTime Date
        {
            get
            {
                return _dateTime;
            }
            set
            {
                _dateTime = value;
                onPropertyChanged(this, "Date");
            }
        }

        private void onPropertyChanged(object sender, string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
