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



    }
}