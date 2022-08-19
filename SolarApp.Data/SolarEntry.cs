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

        #region Clean Data

        private double averageSolarPerHour_clean = -1;
        public double AverageSolarPerHour_clean
        {
            get => averageSolarPerHour_clean;
            set
            {
                SetField(ref averageSolarPerHour_clean, value);
            }
        }

        private double solarLastDay_clean = -1;
        public double SolarLastDay_clean
        {
            get => solarLastDay_clean;
            set
            {
                SetField(ref solarLastDay_clean, value);
            }
        }

        private double averageGridPerHour_clean = -1;
        public double AverageGridPerHour_clean
        {
            get => averageGridPerHour_clean;
            set
            {
                SetField(ref averageGridPerHour_clean, value);
            }
        }

        private double gridLastDay_clean = -1;
        public double GridLastDay_clean
        {
            get => gridLastDay_clean;
            set
            {
                SetField(ref gridLastDay_clean, value);
            }
        }

        private double averageGasPerHour_clean = -1;
        public double AverageGasPerHour_clean
        {
            get => averageGasPerHour_clean;
            set
            {
                SetField(ref averageGasPerHour_clean, value);
            }
        }

        private double gasLastDay_clean = -1;
        public double GasLastDay_clean
        {
            get => gasLastDay_clean;
            set
            {
                SetField(ref gasLastDay_clean, value);
            }
        }

        private double averageWaterPerHour_clean = -1;
        public double AverageWaterPerHour_clean
        {
            get => averageWaterPerHour_clean;
            set
            {
                SetField(ref averageWaterPerHour_clean, value);
            }
        }

        private double waterLastDay_clean = -1;
        public double WaterLastDay_clean
        {
            get => waterLastDay_clean;
            set
            {
                SetField(ref waterLastDay_clean, value);
            }
        }

        private double powerSurplusToday = -1;
        public double PowerSurplusToday
        {
            get => powerSurplusToday;
            set
            {
                SetField(ref powerSurplusToday, value);
            }
        }

        private double powerSurplusCurrent = -1;
        public double PowerSurplusCurrent
        {
            get => powerSurplusCurrent;
            set
            {
                SetField(ref powerSurplusCurrent, value);
            }
        }


        #endregion

        //[JsonIgnore]
    }
}