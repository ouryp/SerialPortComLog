using SerialPortCom;
using SerialPortComLog.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SerialPortComLog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StEvalScannerView scannerView;
        private StEvalDeviceView deviceView;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void ButtonScanner_Click(object sender, RoutedEventArgs e)
        {
            List<StEvalPeriph> stEvalPeriphs = new List<StEvalPeriph>();

            ButtonScanner.IsEnabled = false;

            BackgroundWorker backgroundWorker = new BackgroundWorker();
            // Set up the Background Worker Events 
            backgroundWorker.DoWork += _backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += _backgroundWorker_RunWorkerCompleted;
            // Run the Background Worker
            backgroundWorker.RunWorkerAsync();
            // Worker Method 
            void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
            {
                stEvalPeriphs = StEvalTools.Scanner();
            }
            // Completed Method 
            void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                scannerView = new StEvalScannerView(stEvalPeriphs);
                // TODO create MainViewModel to have one DataContext
                this.DataContext = scannerView;
                ButtonScanner.IsEnabled = true;
            }
        }

        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            StEvalPeriph periph;
            string selected = ScannerBox.SelectedItem.ToString();
            bool success = scannerView.GetPeriph(selected, out periph);
            if(success)
            {
                GroupeBoxPeriph.Header = selected;
                deviceView = new StEvalDeviceView(periph);
                // TODO create MainViewModel to have one DataContext
                this.DataContext = deviceView;
            }
        }

        private void ButtonGetter_Click(object sender, RoutedEventArgs e)
        {
            if(CheckBoxWhoIAm.IsChecked.Value)
            {
                deviceView.GetWhoIAm();
            }
            if(CheckBoxTtf.IsChecked.Value)
            {
                deviceView.GetTtf();
            }
        }
    }
}
