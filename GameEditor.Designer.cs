namespace Anime_Quiz
{
    partial class GameEditor
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
            this.saveBtn = new System.Windows.Forms.Button();
            this.loadBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.saveAsBtn = new System.Windows.Forms.Button();
            this.numQuestions = new System.Windows.Forms.TextBox();
            this.numQuestLabel = new System.Windows.Forms.Label();
            this.gameSave = new System.Windows.Forms.SaveFileDialog();
            this.gameLoad = new System.Windows.Forms.OpenFileDialog();
            this.genBtn = new System.Windows.Forms.Button();
            this.clrBtn = new System.Windows.Forms.Button();
            this.addBtn = new System.Windows.Forms.Button();
            this.removeBtn = new System.Windows.Forms.Button();
            this.uncheckBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // gameType
            // 
            this.gameType.FormattingEnabled = true;
            this.gameType.Items.AddRange(new object[] {
            "Question",
            "Music",
            "Screenshot"});
            this.gameType.Location = new System.Drawing.Point(12, 12);
            this.gameType.Name = "gameType";
            this.gameType.Size = new System.Drawing.Size(125, 21);
            this.gameType.TabIndex = 0;
            this.gameType.Text = "Choose a game type";
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(735, 10);
            this.saveBtn.Margin = new System.Windows.Forms.Padding(20);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(75, 23);
            this.saveBtn.TabIndex = 6;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // loadBtn
            // 
            this.loadBtn.Location = new System.Drawing.Point(825, 10);
            this.loadBtn.Margin = new System.Windows.Forms.Padding(20);
            this.loadBtn.Name = "loadBtn";
            this.loadBtn.Size = new System.Drawing.Size(75, 23);
            this.loadBtn.TabIndex = 7;
            this.loadBtn.Text = "Load";
            this.loadBtn.UseVisualStyleBackColor = true;
            this.loadBtn.Click += new System.EventHandler(this.loadBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(915, 10);
            this.cancelBtn.Margin = new System.Windows.Forms.Padding(20);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 8;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // saveAsBtn
            // 
            this.saveAsBtn.Location = new System.Drawing.Point(645, 10);
            this.saveAsBtn.Name = "saveAsBtn";
            this.saveAsBtn.Size = new System.Drawing.Size(75, 23);
            this.saveAsBtn.TabIndex = 5;
            this.saveAsBtn.Text = "Save As...";
            this.saveAsBtn.UseVisualStyleBackColor = true;
            this.saveAsBtn.Click += new System.EventHandler(this.saveAsBtn_Click);
            // 
            // numQuestions
            // 
            this.numQuestions.Location = new System.Drawing.Point(149, 12);
            this.numQuestions.MaxLength = 3;
            this.numQuestions.Name = "numQuestions";
            this.numQuestions.Size = new System.Drawing.Size(30, 20);
            this.numQuestions.TabIndex = 1;
            this.numQuestions.Text = "50";
            this.numQuestions.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.num_KeyPress);
            // 
            // numQuestLabel
            // 
            this.numQuestLabel.AutoSize = true;
            this.numQuestLabel.Location = new System.Drawing.Point(185, 15);
            this.numQuestLabel.Name = "numQuestLabel";
            this.numQuestLabel.Size = new System.Drawing.Size(52, 13);
            this.numQuestLabel.TabIndex = 6;
            this.numQuestLabel.Text = "questions";
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
            // genBtn
            // 
            this.genBtn.Location = new System.Drawing.Point(247, 10);
            this.genBtn.Name = "genBtn";
            this.genBtn.Size = new System.Drawing.Size(75, 23);
            this.genBtn.TabIndex = 2;
            this.genBtn.Text = "Generate";
            this.genBtn.UseVisualStyleBackColor = true;
            this.genBtn.Click += new System.EventHandler(this.genBtn_Click);
            // 
            // clrBtn
            // 
            this.clrBtn.Location = new System.Drawing.Point(337, 10);
            this.clrBtn.Name = "clrBtn";
            this.clrBtn.Size = new System.Drawing.Size(75, 23);
            this.clrBtn.TabIndex = 2;
            this.clrBtn.Text = "Clear";
            this.clrBtn.UseVisualStyleBackColor = true;
            this.clrBtn.Click += new System.EventHandler(this.clearBtn_Click);
            // 
            // addBtn
            // 
            this.addBtn.Enabled = false;
            this.addBtn.Location = new System.Drawing.Point(427, 10);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(25, 23);
            this.addBtn.TabIndex = 3;
            this.addBtn.Text = "+";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // removeBtn
            // 
            this.removeBtn.Enabled = false;
            this.removeBtn.Location = new System.Drawing.Point(463, 10);
            this.removeBtn.Name = "removeBtn";
            this.removeBtn.Size = new System.Drawing.Size(25, 23);
            this.removeBtn.TabIndex = 4;
            this.removeBtn.Text = "-";
            this.removeBtn.UseVisualStyleBackColor = true;
            this.removeBtn.Click += new System.EventHandler(this.removeBtn_Click);
            // 
            // uncheckBtn
            // 
            this.uncheckBtn.Location = new System.Drawing.Point(503, 10);
            this.uncheckBtn.Name = "uncheckBtn";
            this.uncheckBtn.Size = new System.Drawing.Size(75, 23);
            this.uncheckBtn.TabIndex = 9;
            this.uncheckBtn.Text = "Uncheck All";
            this.uncheckBtn.UseVisualStyleBackColor = true;
            this.uncheckBtn.Click += new System.EventHandler(this.uncheckBtn_Click);
            // 
            // GameEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.uncheckBtn);
            this.Controls.Add(this.removeBtn);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.clrBtn);
            this.Controls.Add(this.genBtn);
            this.Controls.Add(this.numQuestLabel);
            this.Controls.Add(this.numQuestions);
            this.Controls.Add(this.saveAsBtn);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.loadBtn);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.gameType);
            this.Name = "GameEditor";
            this.Text = "Game Editor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameEditor_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox gameType;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Button loadBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button saveAsBtn;
        private System.Windows.Forms.TextBox numQuestions;
        private System.Windows.Forms.Label numQuestLabel;
        private System.Windows.Forms.SaveFileDialog gameSave;
        private System.Windows.Forms.OpenFileDialog gameLoad;
        private System.Windows.Forms.Button genBtn;
        private System.Windows.Forms.Button clrBtn;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.Button removeBtn;
        private System.Windows.Forms.Button uncheckBtn;
    }
}