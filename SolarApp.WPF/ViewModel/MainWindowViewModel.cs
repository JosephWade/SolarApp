using MVVM.WPF;
using SolarApp.Data;
using SolarApp.WPF.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace SolarApp.WPF.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            // This will be run on startup
            StartUp();
        }

        public void StartUp()
        {
            CommonVariables.DataMan = new DataManager();
            CommonVariables.MainWindowVM = this;

            WindowTitle = "";
        }

        private string windowTitle = "App Starting Up";
        public string WindowTitle
        {
            get => windowTitle;
            set
            {
                SetField(ref windowTitle, new string($"{CommonVariables.DataMan.ApplicationName} {CommonVariables.DataMan.ApplicationVersion} {(string.IsNullOrWhiteSpace(value) ? "" : " - "+value)}"));
            }
        }

        private CommonVariables commonVariables = new CommonVariables();
        public CommonVariables CommonVariables
        {
            get => commonVariables;
            set
            {
                SetField(ref commonVariables, value);
            }
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

        private string solarMeterTextbox = "";
        public string SolarMeterTextbox
        {
            get => solarMeterTextbox;
            set
            {
                SetField(ref solarMeterTextbox, new string(value));
            }
        }

        private string gridMeterTextbox = "";
        public string GridMeterTextbox
        {
            get => gridMeterTextbox;
            set
            {
                SetField(ref gridMeterTextbox, new string(value));
            }
        }

        private DateTime timeOfRecording = new DateTime();
        public DateTime TimeOfRecording
        {
            get => timeOfRecording;
            set
            {
                SetField(ref timeOfRecording, value);
            }
        }


        public ICommand AddSolarEntryCommand
        {
            get
            {
                return new DelegateCommand(AddSolarEntry);
            }
        }

        public void AddSolarEntry()
        {

        }

    }
}