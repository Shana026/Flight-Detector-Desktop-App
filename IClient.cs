﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDetector
{
    interface IClient
    {
        void connect(string ip, int port);
        void write(string command);
        void disconnect();

    }
}
