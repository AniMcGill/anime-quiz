namespace Anime_Quiz
{
    partial class SettingsForm
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
            this.defaultFolderBox = new System.Windows.Forms.TextBox();
            this.defaultFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.okBtn = new System.Windows.Forms.Button();
            this.browseBtn = new System.Windows.Forms.Button();
            this.reloadCheckBox = new System.Windows.Forms.CheckBox();
            this.autostartMusicBtn = new System.Windows.Forms.CheckBox();
            this.volumeGroupBox = new System.Windows.Forms.GroupBox();
            this.volumeBar = new System.Windows.Forms.TrackBar();
            this.folderGroupBox = new System.Windows.Forms.GroupBox();
            this.screenshotFolderLabel = new System.Windows.Forms.Label();
            this.screenshotFolderBrowseBtn = new System.Windows.Forms.Button();
            this.musicFolderBrowseBtn = new System.Windows.Forms.Button();
            this.screenshotFolderTextBox = new System.Windows.Forms.TextBox();
            this.musicFolderTextBox = new System.Windows.Forms.TextBox();
            this.musicFolderLabel = new System.Windows.Forms.Label();
            this.gameFolderLabel = new System.Windows.Forms.Label();
            this.settingsTab = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.secondsLabel = new System.Windows.Forms.Label();
            this.songDuration = new System.Windows.Forms.TextBox();
            this.songDurationLabel = new System.Windows.Forms.Label();
            this.volumeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.volumeBar)).BeginInit();
            this.folderGroupBox.SuspendLayout();
            this.settingsTab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // defaultFolderBox
            // 
            this.defaultFolderBox.Enabled = false;
            this.defaultFolderBox.Location = new System.Drawing.Point(109, 14);
            this.defaultFolderBox.Name = "defaultFolderBox";
            this.defaultFolderBox.Size = new System.Drawing.Size(345, 20);
            this.defaultFolderBox.TabIndex = 1;
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(497, 260);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 3;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // okBtn
            // 
            this.okBtn.Location = new System.Drawing.Point(416, 260);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 4;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // browseBtn
            // 
            this.browseBtn.Location = new System.Drawing.Point(458, 12);
            this.browseBtn.Name = "browseBtn";
            this.browseBtn.Size = new System.Drawing.Size(75, 23);
            this.browseBtn.TabIndex = 5;
            this.browseBtn.Text = "Browse";
            this.browseBtn.UseVisualStyleBackColor = true;
            this.browseBtn.Click += new System.EventHandler(this.browseBtn_Click);
            // 
            // reloadCheckBox
            // 
            this.reloadCheckBox.AutoSize = true;
            this.reloadCheckBox.Checked = true;
            this.reloadCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.reloadCheckBox.Location = new System.Drawing.Point(6, 122);
            this.reloadCheckBox.Name = "reloadCheckBox";
            this.reloadCheckBox.Size = new System.Drawing.Size(170, 17);
            this.reloadCheckBox.TabIndex = 6;
            this.reloadCheckBox.Text = "Reload previous game on start";
            this.reloadCheckBox.UseVisualStyleBackColor = true;
            // 
            // autostartMusicBtn
            // 
            this.autostartMusicBtn.AutoSize = true;
            this.autostartMusicBtn.Location = new System.Drawing.Point(6, 6);
            this.autostartMusicBtn.Name = "autostartMusicBtn";
            this.autostartMusicBtn.Size = new System.Drawing.Size(141, 17);
            this.autostartMusicBtn.TabIndex = 7;
            this.autostartMusicBtn.Text = "Automatically start music";
            this.autostartMusicBtn.UseVisualStyleBackColor = true;
            // 
            // volumeGroupBox
            // 
            this.volumeGroupBox.Controls.Add(this.volumeBar);
            this.volumeGroupBox.Location = new System.Drawing.Point(6, 142);
            this.volumeGroupBox.Name = "volumeGroupBox";
            this.volumeGroupBox.Size = new System.Drawing.Size(280, 68);
            this.volumeGroupBox.TabIndex = 8;
            this.volumeGroupBox.TabStop = false;
            this.volumeGroupBox.Text = "Volume";
            // 
            // volumeBar
            // 
            this.volumeBar.Location = new System.Drawing.Point(6, 19);
            this.volumeBar.Maximum = 100;
            this.volumeBar.Name = "volumeBar";
            this.volumeBar.Size = new System.Drawing.Size(268, 45);
            this.volumeBar.TabIndex = 0;
            this.volumeBar.TickFrequency = 5;
            this.volumeBar.Value = 20;
            // 
            // folderGroupBox
            // 
            this.folderGroupBox.Controls.Add(this.screenshotFolderLabel);
            this.folderGroupBox.Controls.Add(this.screenshotFolderBrowseBtn);
            this.folderGroupBox.Controls.Add(this.musicFolderBrowseBtn);
            this.folderGroupBox.Controls.Add(this.screenshotFolderTextBox);
            this.folderGroupBox.Controls.Add(this.musicFolderTextBox);
            this.folderGroupBox.Controls.Add(this.musicFolderLabel);
            this.folderGroupBox.Controls.Add(this.gameFolderLabel);
            this.folderGroupBox.Controls.Add(this.defaultFolderBox);
            this.folderGroupBox.Controls.Add(this.browseBtn);
            this.folderGroupBox.Location = new System.Drawing.Point(6, 12);
            this.folderGroupBox.Name = "folderGroupBox";
            this.folderGroupBox.Size = new System.Drawing.Size(563, 104);
            this.folderGroupBox.TabIndex = 9;
            this.folderGroupBox.TabStop = false;
            this.folderGroupBox.Text = "Default Folders";
            // 
            // screenshotFolderLabel
            // 
            this.screenshotFolderLabel.AutoSize = true;
            this.screenshotFolderLabel.Location = new System.Drawing.Point(6, 69);
            this.screenshotFolderLabel.Name = "screenshotFolderLabel";
            this.screenshotFolderLabel.Size = new System.Drawing.Size(93, 13);
            this.screenshotFolderLabel.TabIndex = 15;
            this.screenshotFolderLabel.Text = "Screenshot Folder";
            // 
            // screenshotFolderBrowseBtn
            // 
            this.screenshotFolderBrowseBtn.Location = new System.Drawing.Point(458, 64);
            this.screenshotFolderBrowseBtn.Name = "screenshotFolderBrowseBtn";
            this.screenshotFolderBrowseBtn.Size = new System.Drawing.Size(75, 23);
            this.screenshotFolderBrowseBtn.TabIndex = 14;
            this.screenshotFolderBrowseBtn.Text = "Browse";
            this.screenshotFolderBrowseBtn.UseVisualStyleBackColor = true;
            this.screenshotFolderBrowseBtn.Click += new System.EventHandler(this.screenshotFolderBrowseBtn_Click);
            // 
            // musicFolderBrowseBtn
            // 
            this.musicFolderBrowseBtn.Location = new System.Drawing.Point(458, 38);
            this.musicFolderBrowseBtn.Name = "musicFolderBrowseBtn";
            this.musicFolderBrowseBtn.Size = new System.Drawing.Size(75, 23);
            this.musicFolderBrowseBtn.TabIndex = 13;
            this.musicFolderBrowseBtn.Text = "Browse";
            this.musicFolderBrowseBtn.UseVisualStyleBackColor = true;
            this.musicFolderBrowseBtn.Click += new System.EventHandler(this.musicFolderBrowseBtn_Click);
            // 
            // screenshotFolderTextBox
            // 
            this.screenshotFolderTextBox.Enabled = false;
            this.screenshotFolderTextBox.Location = new System.Drawing.Point(109, 66);
            this.screenshotFolderTextBox.Name = "screenshotFolderTextBox";
            this.screenshotFolderTextBox.Size = new System.Drawing.Size(345, 20);
            this.screenshotFolderTextBox.TabIndex = 12;
            // 
            // musicFolderTextBox
            // 
            this.musicFolderTextBox.Enabled = false;
            this.musicFolderTextBox.Location = new System.Drawing.Point(109, 40);
            this.musicFolderTextBox.Name = "musicFolderTextBox";
            this.musicFolderTextBox.Size = new System.Drawing.Size(345, 20);
            this.musicFolderTextBox.TabIndex = 11;
            // 
            // musicFolderLabel
            // 
            this.musicFolderLabel.AutoSize = true;
            this.musicFolderLabel.Location = new System.Drawing.Point(6, 43);
            this.musicFolderLabel.Name = "musicFolderLabel";
            this.musicFolderLabel.Size = new System.Drawing.Size(67, 13);
            this.musicFolderLabel.TabIndex = 10;
            this.musicFolderLabel.Text = "Music Folder";
            // 
            // gameFolderLabel
            // 
            this.gameFolderLabel.AutoSize = true;
            this.gameFolderLabel.Location = new System.Drawing.Point(6, 17);
            this.gameFolderLabel.Name = "gameFolderLabel";
            this.gameFolderLabel.Size = new System.Drawing.Size(67, 13);
            this.gameFolderLabel.TabIndex = 6;
            this.gameFolderLabel.Text = "Game Folder";
            // 
            // settingsTab
            // 
            this.settingsTab.Controls.Add(this.tabPage1);
            this.settingsTab.Controls.Add(this.tabPage2);
            this.settingsTab.Location = new System.Drawing.Point(12, 12);
            this.settingsTab.Name = "settingsTab";
            this.settingsTab.SelectedIndex = 0;
            this.settingsTab.Size = new System.Drawing.Size(560, 242);
            this.settingsTab.TabIndex = 16;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.folderGroupBox);
            this.tabPage1.Controls.Add(this.reloadCheckBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(552, 216);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Game Settings";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.secondsLabel);
            this.tabPage2.Controls.Add(this.songDuration);
            this.tabPage2.Controls.Add(this.songDurationLabel);
            this.tabPage2.Controls.Add(this.autostartMusicBtn);
            this.tabPage2.Controls.Add(this.volumeGroupBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(552, 216);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Music Settings";
            // 
            // secondsLabel
            // 
            this.secondsLabel.AutoSize = true;
            this.secondsLabel.Location = new System.Drawing.Point(118, 28);
            this.secondsLabel.Name = "secondsLabel";
            this.secondsLabel.Size = new System.Drawing.Size(47, 13);
            this.secondsLabel.TabIndex = 11;
            this.secondsLabel.Text = "seconds";
            // 
            // songDuration
            // 
            this.songDuration.Location = new System.Drawing.Point(82, 25);
            this.songDuration.Name = "songDuration";
            this.songDuration.Size = new System.Drawing.Size(30, 20);
            this.songDuration.TabIndex = 10;
            this.songDuration.Text = "30";
            this.songDuration.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.num_KeyPress);
            // 
            // songDurationLabel
            // 
            this.songDurationLabel.AutoSize = true;
            this.songDurationLabel.Location = new System.Drawing.Point(3, 28);
            this.songDurationLabel.Name = "songDurationLabel";
            this.songDurationLabel.Size = new System.Drawing.Size(73, 13);
            this.songDurationLabel.TabIndex = 9;
            this.songDurationLabel.Text = "Play songs for";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 295);
            this.Controls.Add(this.settingsTab);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.cancelBtn);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.volumeGroupBox.ResumeLayout(false);
            this.volumeGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.volumeBar)).EndInit();
            this.folderGroupBox.ResumeLayout(false);
            this.folderGroupBox.PerformLayout();
            this.settingsTab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox defaultFolderBox;
        private System.Windows.Forms.FolderBrowserDialog defaultFolderBrowserDialog;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button browseBtn;
        private System.Windows.Forms.CheckBox reloadCheckBox;
        private System.Windows.Forms.CheckBox autostartMusicBtn;
        private System.Windows.Forms.GroupBox volumeGroupBox;
        private System.Windows.Forms.TrackBar volumeBar;
        private System.Windows.Forms.GroupBox folderGroupBox;
        private System.Windows.Forms.Label screenshotFolderLabel;
        private System.Windows.Forms.Button screenshotFolderBrowseBtn;
        private System.Windows.Forms.Button musicFolderBrowseBtn;
        private System.Windows.Forms.TextBox screenshotFolderTextBox;
        private System.Windows.Forms.TextBox musicFolderTextBox;
        private System.Windows.Forms.Label musicFolderLabel;
        private System.Windows.Forms.Label gameFolderLabel;
        private System.Windows.Forms.TabControl settingsTab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label secondsLabel;
        private System.Windows.Forms.TextBox songDuration;
        private System.Windows.Forms.Label songDurationLabel;
    }
}