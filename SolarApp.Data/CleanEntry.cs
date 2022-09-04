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
            SolarLastDay = solarLastDay_clean;
            GridLastDay = gridLastDay_clean;
            GasLastDay = gasLastDay_clean;
            WaterLastDay = waterLastDay_clean;
        }

        public override string ToString()
        {
            return $"Clean Date at ({CleanedDate.ToString("MM / dd / yyyy h: mm tt")}). Solar Used: {SolarLastDay} | Grid Used: {GridLastDay} | Gas Used: {GasLastDay} | Water: {WaterLastDay}";
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
                FormattedDate = value.ToShortDateString();
            }
        }

        private string formattedDate = "";
        public string FormattedDate
        {
            get => formattedDate;
            set
            {
                SetField(ref formattedDate, value);
            }
        }


        private double averageSolarPerHour = -1;
        public double AverageSolarPerHour
        {
            get => averageSolarPerHour;
            set
            {
                SetField(ref averageSolarPerHour, value);
            }
        }

        private double solarLastDay = -1;
        public double SolarLastDay
        {
            get => solarLastDay;
            set
            {
                SetField(ref solarLastDay, value);
                AverageSolarPerHour = value / 24;
            }
        }

        private double averageGridPerHour = -1;
        public double AverageGridPerHour
        {
            get => averageGridPerHour;
            set
            {
                SetField(ref averageGridPerHour, value);
            }
        }

        private double gridLastDay = -1;
        public double GridLastDay
        {
            get => gridLastDay;
            set
            {
                SetField(ref gridLastDay, value);
                AverageGridPerHour = value / 24;
            }
        }

        private double averageGasPerHour = -1;
        public double AverageGasPerHour
        {
            get => averageGasPerHour;
            set
            {
                SetField(ref averageGasPerHour, value);
            }
        }

        private double gasLastDay = -1;
        public double GasLastDay
        {
            get => gasLastDay;
            set
            {
                SetField(ref gasLastDay, value);
                AverageGasPerHour = value / 24;
            }
        }

        private double averageWaterPerHour = -1;
        public double AverageWaterPerHour
        {
            get => averageWaterPerHour;
            set
            {
                SetField(ref averageWaterPerHour, value);
            }
        }

        private double waterLastDay = -1;
        public double WaterLastDay
        {
            get => waterLastDay;
            set
            {
                SetField(ref waterLastDay, value);
                AverageWaterPerHour = value / 24;
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