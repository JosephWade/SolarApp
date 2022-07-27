using MVVM.WPF;
using System;
using System.Text.Json.Serialization;

namespace SolarApp.Data
{
    public class SolarEntry : ModelBase
    {
        public SolarEntry()
        {

        }

        public SolarEntry(int solarMeterReading, int gridMeterReading, DateTime timeOfRecording)
        {
            SolarMeterReading = solarMeterReading;
            GridMeterReading = gridMeterReading;
            TimeOfRecording = timeOfRecording;
        }

        public override string ToString()
        {
            return $"Solar Entry recorded at ({TimeOfRecording.ToString("MM / dd / yyyy h: mm tt")}). Solar: {SolarMeterReading} | Grid: {GridMeterReading}";
        }


        private int solarMeterReading = -1;
        public int SolarMeterReading
        {
            get => solarMeterReading;
            set
            {
                SetField(ref solarMeterReading, value);
            }
        }

        private int gridMeterReading = -1;
        public int GridMeterReading
        {
            get => gridMeterReading;
            set
            {
                SetField(ref gridMeterReading, value);
            }
        }

        private DateTime timeOfRecording;
        public DateTime TimeOfRecording
        {
            get => timeOfRecording;
            set
            {
                SetField(ref timeOfRecording, value);
            }
        }

        //[JsonIgnore]


    }
}