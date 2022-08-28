using MVVM.WPF;
using System;
using Newtonsoft.Json;

namespace SolarApp.Data
{
    public class DataManager : ModelBase
    {
        public string ApplicationName = "SolarApp";
        public string ApplicationVersion = "0.0.1";
        public string DatabaseFile = $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}SolarApp.json";

        public const int TotalMinutesInADay = 1440;

        public DataManager()
        {
            ImportData();

        }

        public override string ToString()
        {
            return $"DataManager. SolarEntries: {ListOfSolarEntries.Count}";
        }

        private List<SolarEntry> listOfSolarEntries = new List<SolarEntry>();
        public List<SolarEntry> ListOfSolarEntries
        {
            get => listOfSolarEntries;
            set
            {
                SetField(ref listOfSolarEntries, value);
            }
        }

        private List<CleanEntry> listOfCleanData= new List<CleanEntry>();
        public List<CleanEntry> ListOfCleanData
        {
            get => listOfCleanData;
            set
            {
                SetField(ref listOfCleanData, value);
            }
        }

        public void WriteLog(string message)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("MM/dd/yy hh:mm:ss tt")}][{(new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name}] {message}");
        }

        public void CreateDirectoryIfNeeded()
        {
            // Create Directory if it doesn't already exist
            if (!Directory.Exists(Path.GetDirectoryName(DatabaseFile)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(DatabaseFile));
            }
        }

        public void ExportData()
        {
            CreateDirectoryIfNeeded();

            // Take a backup of the current database file
            if (File.Exists(DatabaseFile))
            {
                // Delete backup file if it already exists
                if (File.Exists(DatabaseFile+".backup"))
                {
                    File.Delete(DatabaseFile + ".backup");
                }
                File.Copy(DatabaseFile, DatabaseFile + ".backup");
                File.Delete(DatabaseFile);
            }

            try
            {
                File.WriteAllText(DatabaseFile, JsonConvert.SerializeObject(ListOfSolarEntries));
            }
            catch (Exception e)
            {
                WriteLog($"There was a problem serialzing the database file to ({DatabaseFile}). Error: {e.Message}");
            }

        }

        public void ImportData()
        {
            CreateDirectoryIfNeeded();

            if (File.Exists(DatabaseFile))
            {
                if(new FileInfo(DatabaseFile).Length != 0)
                {
                    try
                    {
                        ListOfSolarEntries = JsonConvert.DeserializeObject<List<SolarEntry>>(File.ReadAllText(DatabaseFile));
                    }
                    catch(Exception e)
                    {
                        WriteLog($"There was a problem deserialzing the database file from ({DatabaseFile}). Error: {e.Message}");
                    }
                }
            }
            else if (File.Exists(DatabaseFile+".backup"))
            {
                if (new FileInfo(DatabaseFile+".backup").Length != 0)
                {
                    WriteLog($"Backup File does not exist, so the backup database file was used ({DatabaseFile}.backup).");
                    try
                    {
                        ListOfSolarEntries = JsonConvert.DeserializeObject<List<SolarEntry>>(File.ReadAllText(DatabaseFile +".backup"));
                    }
                    catch (Exception e)
                    {
                        WriteLog($"There was a problem deserialzing the database backup file from ({DatabaseFile}.backup). Error: {e.Message}");
                    }
                }
            }

            CalculateAverages();
        }



        public double CalculateWeightedAveragesofListHelper(List<SolarEntry> listOfEntriesToAverage, int field)
        {
            // TO DO: Take weighted average of list of entries 
            List<DateTime> dates = listOfEntriesToAverage.Select(entry => entry.TimeOfRecording).ToList();
            List<int> entries = new List<int>();
            double slopeForEntry = 0;


            switch (field)
            {
                case 0: // solar
                    entries = listOfEntriesToAverage.Select(entry => entry.SolarMeterReading).ToList();
                    break;
                case 1: // grid
                    entries = listOfEntriesToAverage.Select(entry => entry.GridMeterReading).ToList();
                    break;
                case 2: // gas
                    entries = listOfEntriesToAverage.Select(entry => entry.GasMeterReading).ToList();
                    break;
                case 3: // water
                    entries = listOfEntriesToAverage.Select(entry => entry.WaterMeterReading).ToList();
                    break;
                default:
                    WriteLog($"Unable to find field ({field}) in SolarEntries. Weighted Average was unable to be found.");
                    return -1;
            }

            // Create a list of weighted slopes
            for (int entry=0; entry < listOfEntriesToAverage.Count -1; entry++)
            {
                double minutesAwayFromNextEntry = dates[entry].Subtract(dates[entry+1]).TotalMinutes;

                double portionOfDayForSlope = minutesAwayFromNextEntry / TotalMinutesInADay;

                slopeForEntry += (entries[entry+1] - entries[entry]) / minutesAwayFromNextEntry;
            }

            return -1;
        }
        
        public CleanEntry CalculateWeightedAveragesofList(DateTime date, List<SolarEntry> listOfEntriesToAverage)
        {
            double solarAverage = CalculateWeightedAveragesofListHelper(listOfEntriesToAverage, 0);
            double gridAverage = CalculateWeightedAveragesofListHelper(listOfEntriesToAverage, 1);
            double gasAverage = CalculateWeightedAveragesofListHelper(listOfEntriesToAverage, 2);
            double waterAverage = CalculateWeightedAveragesofListHelper(listOfEntriesToAverage, 3);

            return new CleanEntry(date,solarAverage,gridAverage,gasAverage,waterAverage);
        }

        public void CalculateAverages()
        {
            // We need at least three elements to get the last 24 hours
            if(ListOfSolarEntries == null || ListOfSolarEntries.Count < 3)
            {
                return;
            }

            ListOfSolarEntries = listOfSolarEntries.OrderBy(entry => entry.TimeOfRecording).ToList();

            if (ListOfSolarEntries.Count() < 3)
            {
                return;
            }

            // Generate the list of dates that can be cleaned.
            // This will be used as the basis for the 24 hour average.
            // Note: Disregard first and last entry as the average data can't be calculated for them
            List<DateTime> DatesThatCanBeCleaned = DateRange(ListOfSolarEntries[1].TimeOfRecording, ListOfSolarEntries[ListOfSolarEntries.Count-2].TimeOfRecording).Select(dateTime => new DateTime(dateTime.Year,dateTime.Month,dateTime.Day,0,0,0)).ToList();

            if (DatesThatCanBeCleaned.Count() < 3)
            {
                return;
            }


            List<int> datesThatNeedToBeFilled = new List<int>();

            int indexOfListOfSolarEntries = 0;

            List<SolarEntry> solarEntriesWithinCurrentDay = new List<SolarEntry>();
            //DateTime date = listOfDatesThatCanBeCleaned[dateIndex];

            List<Tuple<DateTime, TimeSpan, double, double,double,double>> weightedTimeSpans = new List<Tuple<DateTime, TimeSpan, double, double, double, double>>();

            // Generate a list of <DateStart,TimeSpans,slope>
            for (int dateIndex = 0; dateIndex < ListOfSolarEntries.Count-1; dateIndex++)
            {
                SolarEntry currentEntry = ListOfSolarEntries[dateIndex];
                SolarEntry nextEntry = ListOfSolarEntries[dateIndex+1];

                TimeSpan timeSpan = nextEntry.TimeOfRecording.Subtract(currentEntry.TimeOfRecording);
                double solarDifference = nextEntry.SolarMeterReading - currentEntry.SolarMeterReading;
                double gridDifference = nextEntry.GridMeterReading - currentEntry.GridMeterReading;
                double gasDifference = nextEntry.GasMeterReading - currentEntry.GasMeterReading;
                double waterDifference = nextEntry.WaterMeterReading - currentEntry.WaterMeterReading;
                weightedTimeSpans.Add(new Tuple<DateTime, TimeSpan, double, double, double, double>(currentEntry.TimeOfRecording, timeSpan, solarDifference, gridDifference, gasDifference,waterDifference));
            }

            int mostRecentIndex = 1;

            foreach (DateTime date in DatesThatCanBeCleaned)
            {
                mostRecentIndex -= 1;

                List<SolarEntry> relatedEntries = new List<SolarEntry>();
                TimeSpan currentlySpanned = new TimeSpan(0, 0, 0);
                double averageSolarSlope = 0;
                double averageGridSlope = 0;
                double averageGasSlope = 0;
                double averageWaterSlope = 0;

                for (; mostRecentIndex < weightedTimeSpans.Count; mostRecentIndex++)
                {
                    if(currentlySpanned.TotalMinutes >= TotalMinutesInADay)
                    {
                        break;
                    }
                    double currentMinutes = weightedTimeSpans[mostRecentIndex].Item2.TotalMinutes;

                    // Only take up to 24 hours, such that currentlySpanned + currentMinutes < 24
                    if (currentMinutes + currentlySpanned.TotalMinutes > TotalMinutesInADay)
                    {
                        currentMinutes = TotalMinutesInADay - currentlySpanned.TotalMinutes;
                    }
                    relatedEntries.Add(ListOfSolarEntries[mostRecentIndex]);

                    averageSolarSlope = (averageSolarSlope * currentlySpanned.TotalMinutes + weightedTimeSpans[mostRecentIndex].Item3 * currentMinutes) / (currentlySpanned.TotalMinutes + currentMinutes);
                    averageGridSlope = (averageGridSlope * currentlySpanned.TotalMinutes + weightedTimeSpans[mostRecentIndex].Item4 * currentMinutes) / (currentlySpanned.TotalMinutes + currentMinutes);
                    averageGasSlope = (averageGasSlope * currentlySpanned.TotalMinutes + weightedTimeSpans[mostRecentIndex].Item5 * currentMinutes) / (currentlySpanned.TotalMinutes + currentMinutes);
                    averageWaterSlope = (averageWaterSlope * currentlySpanned.TotalMinutes + weightedTimeSpans[mostRecentIndex].Item6 * currentMinutes) / (currentlySpanned.TotalMinutes + currentMinutes);

                    currentlySpanned = currentlySpanned + weightedTimeSpans[mostRecentIndex].Item2;

                    ListOfCleanData.Add(new CleanEntry(date,relatedEntries,averageSolarSlope,averageGridSlope,averageGasSlope,averageWaterSlope));
                }


            }


            /*
            for (int dateIndex = 0; dateIndex < datesThatNeedToBeFilled.Count; dateIndex++)
            {
                // To-do : Fill dates that need to be extrapolated

            }
            */

        }

        public List<DateTime> DateRange(DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, (endDate - startDate).Days + 1).Select(d => startDate.AddDays(d)).ToList();
        }


    }
}