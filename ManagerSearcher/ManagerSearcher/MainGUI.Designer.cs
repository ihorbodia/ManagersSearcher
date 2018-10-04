namespace ManagerSearcherMainGUI
{
    partial class MainGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainGUI));
            this.ChoosenPathLabel = new System.Windows.Forms.Label();
            this.FolderChosenPath = new System.Windows.Forms.Label();
            this.StatusLabelText = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ProcessFilesButton = new System.Windows.Forms.Button();
            this.ChooseFirstFolderButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ChoosenPathLabel
            // 
            this.ChoosenPathLabel.AutoSize = true;
            this.ChoosenPathLabel.Location = new System.Drawing.Point(10, 31);
            this.ChoosenPathLabel.Name = "ChoosenPathLabel";
            this.ChoosenPathLabel.Size = new System.Drawing.Size(0, 13);
            this.ChoosenPathLabel.TabIndex = 12;
            this.ChoosenPathLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // FolderChosenPath
            // 
            this.FolderChosenPath.AutoSize = true;
            this.FolderChosenPath.Location = new System.Drawing.Point(10, 7);
            this.FolderChosenPath.Name = "FolderChosenPath";
            this.FolderChosenPath.Size = new System.Drawing.Size(63, 13);
            this.FolderChosenPath.TabIndex = 11;
            this.FolderChosenPath.Text = "Folder path:";
            // 
            // StatusLabelText
            // 
            this.StatusLabelText.AutoSize = true;
            this.StatusLabelText.Location = new System.Drawing.Point(12, 137);
            this.StatusLabelText.Name = "StatusLabelText";
            this.StatusLabelText.Size = new System.Drawing.Size(0, 13);
            this.StatusLabelText.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Status:";
            // 
            // ProcessFilesButton
            // 
            this.ProcessFilesButton.Location = new System.Drawing.Point(265, 176);
            this.ProcessFilesButton.Name = "ProcessFilesButton";
            this.ProcessFilesButton.Size = new System.Drawing.Size(107, 23);
            this.ProcessFilesButton.TabIndex = 8;
            this.ProcessFilesButton.Text = "Process files";
            this.ProcessFilesButton.UseVisualStyleBackColor = true;
            this.ProcessFilesButton.Click += new System.EventHandler(this.ProcessFilesButton_Click);
            // 
            // ChooseFirstFolderButton
            // 
            this.ChooseFirstFolderButton.Location = new System.Drawing.Point(12, 176);
            this.ChooseFirstFolderButton.Name = "ChooseFirstFolderButton";
            this.ChooseFirstFolderButton.Size = new System.Drawing.Size(107, 23);
            this.ChooseFirstFolderButton.TabIndex = 7;
            this.ChooseFirstFolderButton.Text = "Choose file";
            this.ChooseFirstFolderButton.UseVisualStyleBackColor = true;
            this.ChooseFirstFolderButton.Click += new System.EventHandler(this.ChooseFirstFolderButton_Click);
            // 
            // MainGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 211);
            this.Controls.Add(this.ChoosenPathLabel);
            this.Controls.Add(this.FolderChosenPath);
            this.Controls.Add(this.StatusLabelText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ProcessFilesButton);
            this.Controls.Add(this.ChooseFirstFolderButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainGUI";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ChoosenPathLabel;
        private System.Windows.Forms.Label FolderChosenPath;
        private System.Windows.Forms.Label StatusLabelText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ProcessFilesButton;
        private System.Windows.Forms.Button ChooseFirstFolderButton;
    }
}

