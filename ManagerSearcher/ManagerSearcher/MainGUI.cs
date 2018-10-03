using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManagerSearcher.Common;
using ManagerSearcher.Logic;
using ManagerSearcher.Logic.AgilityPack;

namespace ManagerSearcherMainGUI
{
    public partial class MainGUI : Form
    {
        string chosenPath = string.Empty;
        public MainGUI()
        {
            InitializeComponent();
            StatusLabelText.Text = "Choose folder";
            Text = "Manager searcher v1.0";
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void ChooseFirstFolderButton_Click(object sender, System.EventArgs e)
        {
            chosenPath = ManagerSearcherCommon.SelectFile();
            if (!string.IsNullOrEmpty(chosenPath.Trim()))
            {
                StatusLabelText.Text = "Start process";
                ChoosenPathLabel.Text = chosenPath;
            }
        }

        private void ProcessFilesButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(chosenPath.Trim()))
            {
                return;
            }
            StatusLabelText.Text = "Processing";
            ProcessFilesButton.Enabled = false;
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
            ProcessFilesButton.Enabled = true;
        }
        private void RunProgram()
        {
            ManagerSearcherProcessorAP ms = new ManagerSearcherProcessorAP(chosenPath);
            ms.ProcessFileByEpp();
        }
    }
}
