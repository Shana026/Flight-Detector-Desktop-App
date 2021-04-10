using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDetector
{
    class joystickModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        //private double aileron;
        // private double elevator;
        //private double rudder;
        //private double throttle;

        //properties

        private FlightData flightData;

        public FlightData FlightData
        {
           get { return flightData; }
            set { }
        }

        /* public double Aileron
         {
             get { return aileron; }
             set
             {
                 aileron = value;
                 NotifyPropertyChanged("Aileron");
             }
         }
         public double Elevator
         {
             get { return elevator; }
             set
             {
                 elevator = value;
                 NotifyPropertyChanged("Elevator");
             }
         }
         public double Rudder
         {
             get { return rudder; }
             set
             {
                 rudder = value;
                 NotifyPropertyChanged("Rudder");
             }
         }
         public double Throttle
         {
             get { return throttle; }
             set
             {
                 throttle = value;
                 NotifyPropertyChanged("Throttle");
             }
         }
 */

        // constructor
        public joystickModel(FlightData data)
        {
            //aileron = -50;
            // elevator = 50;
            //throttle = 1;
            // rudder = 1;

           flightData = data;
        }

        // call the event propertyChanged when any property of class is changed
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null) //check if event is null or not, if not, we proceed.
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }


    }

}