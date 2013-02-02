namespace Anime_Quiz
{
    partial class GameSetEditor
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
            this.gameType = new System.Windows.Forms.ComboBox();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.gameSave = new System.Windows.Forms.SaveFileDialog();
            this.gameLoad = new System.Windows.Forms.OpenFileDialog();
            this.clrBtn = new System.Windows.Forms.Button();
            this.uncheckBtn = new System.Windows.Forms.Button();
            this.newSetLabel = new System.Windows.Forms.Label();
            this.newSetTextbox = new System.Windows.Forms.TextBox();
            this.addBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // gameType
            // 
            this.gameType.FormattingEnabled = true;
            this.gameType.Items.AddRange(new object[] {
            "Question",
            "Music",
            "Screenshot"});
            this.gameType.Location = new System.Drawing.Point(597, 12);
            this.gameType.Name = "gameType";
            this.gameType.Size = new System.Drawing.Size(121, 21);
            this.gameType.TabIndex = 3;
            this.gameType.Text = "Choose a game type";
            this.gameType.SelectedIndexChanged += new System.EventHandler(this.gameType_SelectedIndexChanged);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(915, 10);
            this.cancelBtn.Margin = new System.Windows.Forms.Padding(20);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 9;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // gameSave
            // 
            this.gameSave.DefaultExt = "xml";
            this.gameSave.Filter = "XML files|*xml";
            this.gameSave.SupportMultiDottedExtensions = true;
            this.gameSave.Title = "Game Save";
            // 
            // gameLoad
            // 
            this.gameLoad.DefaultExt = "xml";
            this.gameLoad.Filter = "XML files|*xml";
            this.gameLoad.Title = "Game Load";
            // 
            // clrBtn
            // 
            this.clrBtn.Enabled = false;
            this.clrBtn.Location = new System.Drawing.Point(200, 10);
            this.clrBtn.Name = "clrBtn";
            this.clrBtn.Size = new System.Drawing.Size(75, 23);
            this.clrBtn.TabIndex = 3;
            this.clrBtn.Text = "Clear";
            this.clrBtn.UseVisualStyleBackColor = true;
            this.clrBtn.Click += new System.EventHandler(this.clearBtn_Click);
            // 
            // uncheckBtn
            // 
            this.uncheckBtn.Location = new System.Drawing.Point(815, 10);
            this.uncheckBtn.Name = "uncheckBtn";
            this.uncheckBtn.Size = new System.Drawing.Size(75, 23);
            this.uncheckBtn.TabIndex = 10;
            this.uncheckBtn.Text = "Uncheck All";
            this.uncheckBtn.UseVisualStyleBackColor = true;
            this.uncheckBtn.Click += new System.EventHandler(this.uncheckBtn_Click);
            // 
            // newSetLabel
            // 
            this.newSetLabel.AutoSize = true;
            this.newSetLabel.Location = new System.Drawing.Point(293, 15);
            this.newSetLabel.Name = "newSetLabel";
            this.newSetLabel.Size = new System.Drawing.Size(140, 13);
            this.newSetLabel.TabIndex = 0;
            this.newSetLabel.Text = "Create a new Question Set: ";
            // 
            // newSetTextbox
            // 
            this.newSetTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newSetTextbox.Location = new System.Drawing.Point(431, 12);
            this.newSetTextbox.Name = "newSetTextbox";
            this.newSetTextbox.Size = new System.Drawing.Size(160, 21);
            this.newSetTextbox.TabIndex = 2;
            // 
            // addBtn
            // 
            this.addBtn.Enabled = false;
            this.addBtn.Location = new System.Drawing.Point(724, 10);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(53, 23);
            this.addBtn.TabIndex = 4;
            this.addBtn.Text = "Add";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // GameSetEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.newSetTextbox);
            this.Controls.Add(this.newSetLabel);
            this.Controls.Add(this.uncheckBtn);
            this.Controls.Add(this.clrBtn);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.gameType);
            this.Name = "GameSetEditor";
            this.Text = "Game Editor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameEditor_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox gameType;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.SaveFileDialog gameSave;
        private System.Windows.Forms.OpenFileDialog gameLoad;
        private System.Windows.Forms.Button clrBtn;
        private System.Windows.Forms.Button uncheckBtn;
        private System.Windows.Forms.Label newSetLabel;
        private System.Windows.Forms.TextBox newSetTextbox;
        private System.Windows.Forms.Button addBtn;
    }
}