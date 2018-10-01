using System.Windows.Forms;

namespace ManagerSearcherLogic
{
    public static class ManagerSearcherCommon
    {
        public static string SelectFolder()
        {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();

            string selectedFileName = string.Empty;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFileName = openFileDialog.SelectedPath;
            }
            else
            {
                selectedFileName = string.Empty;
            }
            return selectedFileName;
        }
    }
}
