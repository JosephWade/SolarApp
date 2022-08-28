using MVVM.WPF;
using System;
using System.Text.Json.Serialization;

namespace SolarApp.Data
{
    public class CleanEntry : ModelBase
    {
        public CleanEntry(DateTime cleanDate, List<SolarEntry> relatedEntries, double solarLastDay_clean, double gridLastDay_clean, double gasLastDay_clean, double waterLastDay_clean)
        {
            CleanedDate = cleanDate;
            RelatedEntries = relatedEntries;
            SolarLastDay_clean = solarLastDay_clean;
            GridLastDay_clean = gridLastDay_clean;
            GasLastDay_clean = gasLastDay_clean;
            WaterLastDay_clean = waterLastDay_clean;
        }

        public override string ToString()
        {
            return $"Clean Date at ({CleanedDate.ToString("MM / dd / yyyy h: mm tt")}). Solar Used: {SolarLastDay_clean} | Grid Used: {GridLastDay_clean} | Gas Used: {GasLastDay_clean} | Water: {WaterLastDay_clean}";
        }

        private List<SolarEntry> relatedEntries = new List<SolarEntry>();
        public List<SolarEntry> RelatedEntries
        {
            get => relatedEntries;
            set
            {
                SetField(ref relatedEntries, value);
            }
        }

        private DateTime cleanedDate = new DateTime();
        public DateTime CleanedDate
        {
            get => cleanedDate;
            set
            {
                SetField(ref cleanedDate, value);
            }
        }

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
    }
}