using ManagerSearcher.Logic;
using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace ManagersSearcher.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string chosenPath = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
            InitializeComponent();
            StatusLabelText.Content = "Choose file";
            this.Title = "Manager searcher v1.0";
            this.ResizeMode = ResizeMode.NoResize;
        }

        private void ChooseFirstFolderButton_Click(object sender, System.EventArgs e)
        {
            chosenPath = ManagerSearcherCommon.SelectFile();
            if (!string.IsNullOrEmpty(chosenPath.Trim()))
            {
                StatusLabelText.Content = "Start process";
                ChoosenPathLabel.Content = chosenPath;
            }
        }

        private void ProcessFilesButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(chosenPath.Trim()))
            {
                return;
            }
            StatusLabelText.Content = "Processing";
            try
            {
                new Task(() =>
                {
                    Thread t = new Thread(RunProgram);
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();
                    t.Join();
                    StatusLabelText.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate () { StatusLabelText.Content = "Finish"; });
                    Console.WriteLine("Finish");
                }).Start();
            }
            catch (Exception)
            {
                StatusLabelText.Content = "Something wrong";
            }
        }
        [STAThread]
        private void RunProgram()
        {
            ManagerSearcherProcessor ms = new ManagerSearcherProcessor(chosenPath);
            ms.ProcessFile();
            ms.SaveFile();
        }
    }
}
