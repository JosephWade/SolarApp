using SolarApp.WPF.Utilities;
using SolarApp.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace SolarApp.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Task.Delay(10);

            StartUp();
        }

        public async void StartUp()
        {
            ((MainWindowViewModel)DataContext).CommonVariables.MainWindowCodeBehind = this;

            Task.Delay(10);

            CleanDataGrid.Items.Refresh();
            CleanDataGrid.Items.MoveCurrentToLast();
            CleanDataGrid.ScrollIntoView(CleanDataGrid.Items.CurrentItem);
        }

        private void text_PrviewKeyUp(object sender, KeyEventArgs e)
        {
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateTarget();
        }

        private void DateTimePicker_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ((DateTimePicker)sender).GetBindingExpression(DateTimePicker.TextProperty).UpdateSource();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((DataGrid)sender).GetBindingExpression(DataGrid.ItemsSourceProperty).UpdateSource();
        }

        private void AddButton_IsMouseCaptureWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (((Border)sender).IsMouseOver)
            {
                ((MainWindowViewModel)DataContext).DecideWhatColorAddButtonShouldBe(0);
            }
            else
            {
                ((MainWindowViewModel)DataContext).DecideWhatColorAddButtonShouldBe(1);
            }
            
        }

        private void AddButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((MainWindowViewModel)DataContext).AddSolarEntry();
            //BindingOperations.GetBindingExpression(SolarEntriesDataGrid, DataGrid.ItemsSourceProperty).UpdateSource();
        }

        
    }
}
