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

        public SolarEntry(int solarMeterReading, int gridMeterReading, DateTime timeOfRecording, int waterMeterReading, int gasMeterReading)
        {
            SolarMeterReading = solarMeterReading;
            GridMeterReading = gridMeterReading;
            TimeOfRecording = timeOfRecording;
            WaterMeterReading = waterMeterReading;
            GasMeterReading = gasMeterReading;
        }

        public override string ToString()
        {
            return $"Solar Entry recorded at ({TimeOfRecording.ToString("MM / dd / yyyy h: mm tt")}). Solar: {SolarMeterReading} | Grid: {GridMeterReading} | Gas: {GasMeterReading} | Water: {WaterMeterReading}";
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

        private int gasMeterReading = -1;
        public int GasMeterReading
        {
            get => gasMeterReading;
            set
            {
                SetField(ref gasMeterReading, value);
            }
        }
        

        private int waterMeterReading = -1;
        public int WaterMeterReading
        {
            get => waterMeterReading;
            set
            {
                SetField(ref waterMeterReading, value);
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