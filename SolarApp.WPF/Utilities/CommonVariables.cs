using MVVM.WPF;
using SolarApp.Data;
using SolarApp.WPF.ViewModel;

namespace SolarApp.WPF.Utilities
{
    public class CommonVariables : ViewModelBase
    {
        public CommonVariables()
        {

        }

        private DataManager dataMan;
        public DataManager DataMan
        {
            get => dataMan;
            set
            {
                SetField(ref dataMan, value);
            }
        }

        private MainWindowViewModel mainWindowVM;
        public MainWindowViewModel MainWindowVM
        {
            get => mainWindowVM;
            set
            {
                SetField(ref mainWindowVM, value);
            }
        }

        private MainWindow mainWindowCodeBehind;
        public MainWindow MainWindowCodeBehind
        {
            get => mainWindowCodeBehind;
            set
            {
                SetField(ref mainWindowCodeBehind, value);
            }
        }

    }
}