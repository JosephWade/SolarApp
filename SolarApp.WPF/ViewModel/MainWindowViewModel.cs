using MVVM.WPF;
using SolarApp.Data;
using SolarApp.WPF.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace SolarApp.WPF.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private BrushConverter converter = new System.Windows.Media.BrushConverter();

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
                SetField(ref listOfSolarEntries, new List<SolarEntry>(value));
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
        

        private string gasMeterTextbox = "";
        public string GasMeterTextbox
        {
            get => gasMeterTextbox;
            set
            {
                SetField(ref gasMeterTextbox, new string(value));
            }
        }

        private string waterMeterTextbox = "";
        public string WaterMeterTextbox
        {
            get => waterMeterTextbox;
            set
            {
                SetField(ref waterMeterTextbox, new string(value));
            }
        }

        private DateTime defaultRecordingTime = DateTime.Now;
        public DateTime DefaultRecordingTime
        {
            get => defaultRecordingTime;
            set
            {
                SetField(ref defaultRecordingTime, value);
            }
        }

        private DateTime timeOfRecording = DateTime.Now;
        public DateTime TimeOfRecording
        {
            get => timeOfRecording;
            set
            {
                SetField(ref timeOfRecording, value);
            }
        }


        /*public ICommand AddSolarEntryCommand
        {
            get
            {
                return new DelegateCommand(AddSolarEntry);
            }
        }*/

        public void AddSolarEntry()
        {
            int solarMeterReading = 0;
            int gridMeterReading = 0;
            int waterMeterReading = 0;
            int gasMeterReading = 0;
            Keyboard.ClearFocus();

            if (TimeOfRecording != null && Int32.TryParse((SolarMeterTextbox), out solarMeterReading) && Int32.TryParse((GridMeterTextbox), out gridMeterReading)&& Int32.TryParse((GasMeterTextbox), out gasMeterReading)&& Int32.TryParse((WaterMeterTextbox), out waterMeterReading) )
            {
                SolarEntry newEntry = new SolarEntry(solarMeterReading, gridMeterReading, TimeOfRecording, waterMeterReading, gasMeterReading);
                ListOfSolarEntries.Add(newEntry);
                ListOfSolarEntries = ListOfSolarEntries;
                WindowTitle = $"Sucessful = {TimeOfRecording} - {DefaultRecordingTime}";
                FadeAddButtonGreen();
            }
            else
            {
                WindowTitle = $"Unable to add new entry";
                FadeAddButtonRed();
            }
        }



        #region AddButtonColorOptions

        public async void DecideWhatColorAddButtonShouldBe(int option)
        {
            string color = "#FF333333";
            switch (option)
            {
                case 0:
                    color = "#FF6f648c";
                    break;
                default:
                    color = "#FF333333";
                    break;
            }
            CommonVariables.MainWindowCodeBehind.AddButton.Background = (Brush)converter.ConvertFromString(color);
        }

        public async Task FadeAddButtonHelper(List<string> colors, bool includeTextboxs=false)
        {
            colors.Reverse();

            foreach (string color in colors)
            {
                CommonVariables.MainWindowCodeBehind.AddButton.Background = (Brush)converter.ConvertFromString(color);

                if (includeTextboxs)
                {
                    if(!Int32.TryParse((SolarMeterTextbox), out _))
                    {
                        CommonVariables.MainWindowCodeBehind.SolarMeterTextbox.BorderBrush = (Brush)converter.ConvertFromString(color);
                    }
                    if(!Int32.TryParse((GridMeterTextbox), out _))
                    {
                        CommonVariables.MainWindowCodeBehind.GridMeterTextbox.BorderBrush = (Brush)converter.ConvertFromString(color);
                    }
                    if(!Int32.TryParse((GasMeterTextbox), out _))
                    {
                        CommonVariables.MainWindowCodeBehind.GasMeterTextbox.BorderBrush = (Brush)converter.ConvertFromString(color);
                    }
                    if(!Int32.TryParse((WaterMeterTextbox), out _))
                    {
                        CommonVariables.MainWindowCodeBehind.WaterMeterTextbox.BorderBrush = (Brush)converter.ConvertFromString(color);
                    }
                }
                
                await Task.Delay(30);
            }
        }
        public async void FadeAddButtonGreen()
        {
            List<string> greenFadeColors = new List<string> { "#FF57C4AD", "#FF55AC9A", "#FF539A89", "#FF4D887B", "#FF4B796D", "#FF45685F", "#FF405B53", "#FF3A4C47", "#FF333F3C", "#FF2D3331", "#FF333333" };
            await FadeAddButtonHelper(greenFadeColors);

            await Task.Delay(100);

            await FadeAddButtonHelper(greenFadeColors);

            if (CommonVariables.MainWindowCodeBehind.AddButton.IsMouseOver)
            {
                DecideWhatColorAddButtonShouldBe(0);
            }
        }
        public async void FadeAddButtonRed()
        {
            List<string> greenFadeColors = new List<string> { "#FFDB4325", "#FFC64529", "#FFB0462B", "#FF9B442C", "#FF89412E", "#FF763F2E", "#FF643A2E", "#FF54372E", "#FF45312B", "#FF362C29", "#FF333333" };
            await FadeAddButtonHelper(greenFadeColors,true);

            await Task.Delay(100);

            await FadeAddButtonHelper(greenFadeColors,true);

            if (CommonVariables.MainWindowCodeBehind.AddButton.IsMouseOver)
            {
                DecideWhatColorAddButtonShouldBe(0);
            }
        }

        #endregion

    }
}