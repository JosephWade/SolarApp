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

            switch (field)
            {
                case 0: // solar

                    break;
                case 1: // grid

                    break;
                case 2: // gas

                    break;
                case 3: // water

                    break;
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

            // Generate the list of dates that can be cleaned.
            // This will be used as the basis for the 24 hour average.
            // Note: Disregard first and last entry as the average data can't be calculated for them
            List<DateTime> listOfDatesThatCanBeCleaned = DateRange(ListOfSolarEntries[1].TimeOfRecording, ListOfSolarEntries[ListOfSolarEntries.Count-2].TimeOfRecording);

            List<int> datesThatNeedToBeFilled = new List<int>();

            int indexOfListOfSolarEntries = 0;

            for (int dateIndex = 0; dateIndex < listOfDatesThatCanBeCleaned.Count; dateIndex++)
            {
                List<SolarEntry> solarEntriesWithinCurrentDay = new List<SolarEntry>();
                DateTime date = listOfDatesThatCanBeCleaned[dateIndex];

                // figure out which listOfSolarEntries of within this day's range += 12 hours
                for (int i = indexOfListOfSolarEntries; i < listOfSolarEntries.Count; i++)
                {
                    if (date.AddHours(-12) <= listOfSolarEntries[i].TimeOfRecording && listOfSolarEntries[i].TimeOfRecording < date.AddHours(12))
                    {
                        solarEntriesWithinCurrentDay.Add(listOfSolarEntries[i]);
                    }
                    else
                    {
                        indexOfListOfSolarEntries = i;
                        break;
                    }
                }
                
                if (solarEntriesWithinCurrentDay.Count > 0)
                {
                    CleanEntry newEntry = CalculateWeightedAveragesofList(date, solarEntriesWithinCurrentDay);
                    ListOfCleanData.Add(newEntry);
                }
                else
                {
                    // If no dates are within this day's range, extrapulate this date's time with the next one's
                    // NOTE: Based off of how this is set up, there is guarantee to be a first/last day
                    // NOTE: This will be done after all of the cleanEntries have been filled
                    ListOfCleanData.Add(new CleanEntry(date, -1, -1, -1, -1));
                    datesThatNeedToBeFilled.Add(dateIndex);
                }
            }

            for (int dateIndex = 0; dateIndex < datesThatNeedToBeFilled.Count; dateIndex++)
            {
                // To-do : Fill dates that need to be extrapolated

            }

        }

        public List<DateTime> DateRange(DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, (endDate - startDate).Days + 1).Select(d => startDate.AddDays(d)).ToList();
        }


    }
}