using MVVM.WPF;
using SolarApp.Data;
using SolarApp.WPF.Utilities;

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

    }
}