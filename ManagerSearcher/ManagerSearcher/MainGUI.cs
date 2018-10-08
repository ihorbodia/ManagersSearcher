using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManagerSearcher.Common;
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
            Text = "Manager searcher v2.8";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            ChoosenPathLabel.AutoEllipsis = true;
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
            ProcessFilesButton.BeginInvoke((MethodInvoker)delegate () { ProcessFilesButton.Enabled = false; });
            SearchByMiddleNamesCB.BeginInvoke((MethodInvoker)delegate () { SearchByMiddleNamesCB.Enabled = false; });
            try
            {
                new Task(() =>
                {
                    Thread t = new Thread(RunProgram);
                    t.Start();
                    t.Join();
                    StatusLabelText.BeginInvoke((MethodInvoker)delegate () { StatusLabelText.Text = "Finish"; });
                    ProcessFilesButton.BeginInvoke((MethodInvoker)delegate () { ProcessFilesButton.Enabled = true; });
                    SearchByMiddleNamesCB.BeginInvoke((MethodInvoker)delegate () { SearchByMiddleNamesCB.Enabled = true; });
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
            ManagerSearcherProcessorAP ms = new ManagerSearcherProcessorAP(chosenPath, SearchByMiddleNamesCB.Checked);
            ms.ProcessFileByEpp();
            try
            {
                ms.SaveFile();
            }
            catch (InvalidOperationException ex)
            {
                DialogResult result = MessageBox.Show("Try to close excel file and click OK.", "Error while saving file", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    ms.SaveFile();
                }
            }
        }
    }
}
