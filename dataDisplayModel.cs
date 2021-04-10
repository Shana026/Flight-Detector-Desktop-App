
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDetector
{
    class dataDisplayModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        
        private FlightData flightData;
        public FlightData FlightData
        {
            get { return flightData; }
            set { }
        }


        /*
        private double altimeter;
        private double airspeed;
        private double heading;
        private double yaw;
        private double roll;
        private double pitch;
        //properties
        public double Altimeter
        {
            get { return altimeter; }
            set
            {
                altimeter = value;
                NotifyPropertyChanged("Altimeter");
            }
        }
        public double Airspeed
        {
            get { return airspeed; }
            set
            {
                airspeed = value;
                NotifyPropertyChanged("Airspeed");
            }
        }
        public double Heading
        {
            get { return heading; }
            set
            {
                heading = value;
                NotifyPropertyChanged("Heading");
            }
        }
        public double Pitch
        {
            get { return pitch; }
            set
            {
                pitch = value;
                NotifyPropertyChanged("Pitch");
            }
        }
        public double Roll
        {
            get { return roll; }
            set
            {
                roll = value;
                NotifyPropertyChanged("Roll");
            }
        }
        public double Yaw
        {
            get { return yaw; }
            set
            {
                yaw = value;
                NotifyPropertyChanged("Yaw");
            }
        }
        */


        // constructor
        public dataDisplayModel(FlightData data)
        {
            flightData = data;
        }

        // dire que le feature propName a ete change
        // call the event propertyChanged when any property of class is changed
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null) //check if event is null or not, if not, we proceed.
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

    }

}
