using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManagerSearcher.Logic;
using ManagerSearcherLogic;

namespace ManagerSearcherMainGUI
{
    public partial class MainGUI : Form
    {
        string chosenPath = string.Empty;
        public MainGUI()
        {
            InitializeComponent();
            StatusLabelText.Text = "Choose folder";
            this.Text = "Manager searcher v1.0";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void ChooseFirstFolderButton_Click(object sender, System.EventArgs e)
        {
            chosenPath = ManagerSearcherCommon.SelectFolder();
            if (!string.IsNullOrEmpty(chosenPath.Trim()))
            {
                StatusLabelText.Text = "Start process";
                ChoosenPathLabel.Text = chosenPath;
            }
        }

        private void ProcessFilesButton_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(chosenPath.Trim()))
            {
                return;
            }

            StatusLabelText.Text = "Processing";
            try
            {
                new Task(() =>
                {
                    Thread t = new Thread(RunProgram);
                    t.Start();
                    t.Join();
                    StatusLabelText.BeginInvoke((MethodInvoker)delegate () { StatusLabelText.Text = "Finish"; });
                    Console.WriteLine("Finish");
                }).Start();
            }
            catch (Exception)
            {
                StatusLabelText.Text = "Something wrong";
            }

        }
        private void RunProgram()
        {
            ManagerSearcherProcessor ms = new ManagerSearcherProcessor(chosenPath);
        }
    }
}
