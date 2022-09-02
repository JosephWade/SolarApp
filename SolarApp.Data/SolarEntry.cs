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

        public SolarEntry(double solarMeterReading, double gridMeterReading, DateTime timeOfRecording, double waterMeterReading, double gasMeterReading)
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


        private double solarMeterReading = -1;
        public double SolarMeterReading
        {
            get => solarMeterReading;
            set
            {
                SetField(ref solarMeterReading, value);
            }
        }

        private double gridMeterReading = -1;
        public double GridMeterReading
        {
            get => gridMeterReading;
            set
            {
                SetField(ref gridMeterReading, value);
            }
        }

        private double gasMeterReading = -1;
        public double GasMeterReading
        {
            get => gasMeterReading;
            set
            {
                SetField(ref gasMeterReading, value);
            }
        }
        

        private double waterMeterReading = -1;
        public double WaterMeterReading
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