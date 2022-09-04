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
        public string ImportFile = $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}ImportData.csv";

        public const int TotalMinutesInADay = 1440;

        public DataManager()
        {
            ImportData();

            ImportCsvData();

            GenerateCleanData();
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
        public List<CleanEntry> CleanData
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

            
        }

        public void ImportCsvData()
        {
            List<SolarEntry> entriesToImport = new List<SolarEntry>();
            DateTime.TryParse("01 / 01 / 0001 12: 00 AM", out DateTime zeroDate);

            if (!File.Exists(ImportFile))
            {
                return;
            }

            try
            {
                using (var rd = new StreamReader(ImportFile))
                {
                    rd.ReadLine();
                    while (!rd.EndOfStream)
                    {
                        List<string> splits = rd.ReadLine().Split(',').ToList();

                        DateTime.TryParse(splits[0], out DateTime date);

                        // NOTE: Some lines in CSV can be interpreted as all-zeros, so this will prevent this line from being added
                        if (!date.Equals(zeroDate))
                        {
                            double.TryParse(splits[1], out double grid);
                            double.TryParse(splits[2], out double solar);
                            double.TryParse(splits[3], out double water);
                            double.TryParse(splits[4], out double gas);
                            entriesToImport.Add(new SolarEntry(solar, grid, date, water, gas));
                        }
                        
                    }
                }
            }
            catch(Exception e)
            {
                WriteLog($"There was a problem importing data from the importdata.cvs file ({ImportFile}). Please make sure the file is closed. Error: {e.Message}");
            }
            
            ListOfSolarEntries.AddRange(entriesToImport);

        }

       
        public void GenerateCleanData()
        {
            // We need at least three elements to get the last 24 hours
            if(ListOfSolarEntries == null || ListOfSolarEntries.Count < 3)
            {
                return;
            }

            if (ListOfSolarEntries.Count() < 3)
            {
                return;
            }

            ListOfSolarEntries.Sort((x, y) => x.TimeOfRecording.CompareTo(y.TimeOfRecording));

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

            int mostRecentIndex = 0;

            foreach (DateTime date in DatesThatCanBeCleaned)
            {
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
                        //mostRecentIndex++;
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

                    currentlySpanned += weightedTimeSpans[mostRecentIndex].Item2;
                }

                CleanData.Add(new CleanEntry(date, relatedEntries, averageSolarSlope, averageGridSlope, averageGasSlope, averageWaterSlope));
                if(relatedEntries.Count > 1)
                {
                    mostRecentIndex -= 1;
                }

            }
            
        }

        public List<DateTime> DateRange(DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, (endDate - startDate).Days + 1).Select(d => startDate.AddDays(d)).ToList();
        }


    }
}