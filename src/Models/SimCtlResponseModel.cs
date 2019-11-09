﻿using System.Collections.Generic;

 namespace RemoteSimulator.Models
{

    public class SimCtlResponseModel
    {
        public List<Devicetype>                 Devicetypes { get; set; }
        public List<Runtime>                    Runtimes    { get; set; }
        public Dictionary<string, List<Device>> Devices     { get; set; }
        public Dictionary<string, Pair>         Pairs       { get; set; }
    }

    public class Device
    {
        public string State             { get; set; }
        public bool   IsAvailable       { get; set; }
        public string Name              { get; set; }
        public string Udid              { get; set; }
        public string AvailabilityError { get; set; }
    }

    public class Devicetype
    {
        public string Name       { get; set; }
        public string BundlePath { get; set; }
        public string Identifier { get; set; }
    }

    public class Pair
    {
        public Device Watch { get; set; }
        public Device Phone { get; set; }
        public string State { get; set; }
    }

    public class Runtime
    {
        public string Version      { get; set; }
        public string BundlePath   { get; set; }
        public bool?  IsAvailable  { get; set; }
        public string Name         { get; set; }
        public string Identifier   { get; set; }
        public string Buildversion { get; set; }
    }
}
