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


        private double averageSolarPerHour_clean = -1;
        public double AverageSolarPerHour_clean
        {
            get => averageSolarPerHour_clean;
            set
            {
                SetField(ref averageSolarPerHour_clean, value);
            }
        }

        private double solarLastDay = -1;
        public double SolarLastDay
        {
            get => solarLastDay;
            set
            {
                SetField(ref solarLastDay, value);
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

        private double gridLastDay = -1;
        public double GridLastDay
        {
            get => gridLastDay;
            set
            {
                SetField(ref gridLastDay, value);
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

        private double gasLastDay = -1;
        public double GasLastDay
        {
            get => gasLastDay;
            set
            {
                SetField(ref gasLastDay, value);
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

        private double waterLastDay = -1;
        public double WaterLastDay
        {
            get => waterLastDay;
            set
            {
                SetField(ref waterLastDay, value);
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