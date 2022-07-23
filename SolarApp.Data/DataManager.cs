using MVVM.WPF;
using System;

namespace SolarApp.Data
{
    public class DataManager : ModelBase
    {
        public string ApplicationName = "SolarApp";
        public string ApplicationVersion = "0.0.1";

        public override string ToString()
        {
            return "DataManager";
        }


    }
}