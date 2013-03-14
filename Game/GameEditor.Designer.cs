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
            this.delBtn = new System.Windows.Forms.Button();
            this.renameBtn = new System.Windows.Forms.Button();
            this.createGameLabel = new System.Windows.Forms.Label();
            this.gameNameTextbox = new System.Windows.Forms.TextBox();
            this.addBtn = new System.Windows.Forms.Button();
            this.closeBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // delBtn
            // 
            this.delBtn.Location = new System.Drawing.Point(200, 10);
            this.delBtn.Name = "delBtn";
            this.delBtn.Size = new System.Drawing.Size(75, 23);
            this.delBtn.TabIndex = 2;
            this.delBtn.Text = "Delete";
            this.delBtn.UseVisualStyleBackColor = true;
            this.delBtn.Click += new System.EventHandler(this.delBtn_Click);
            // 
            // renameBtn
            // 
            this.renameBtn.Location = new System.Drawing.Point(281, 10);
            this.renameBtn.Name = "renameBtn";
            this.renameBtn.Size = new System.Drawing.Size(75, 23);
            this.renameBtn.TabIndex = 3;
            this.renameBtn.Text = "Rename";
            this.renameBtn.UseVisualStyleBackColor = true;
            this.renameBtn.Click += new System.EventHandler(this.renameBtn_Click);
            // 
            // createGameLabel
            // 
            this.createGameLabel.AutoSize = true;
            this.createGameLabel.Location = new System.Drawing.Point(12, 46);
            this.createGameLabel.Name = "createGameLabel";
            this.createGameLabel.Size = new System.Drawing.Size(105, 13);
            this.createGameLabel.TabIndex = 4;
            this.createGameLabel.Text = "Create a new game: ";
            // 
            // gameNameTextbox
            // 
            this.gameNameTextbox.Location = new System.Drawing.Point(123, 43);
            this.gameNameTextbox.Name = "gameNameTextbox";
            this.gameNameTextbox.Size = new System.Drawing.Size(198, 20);
            this.gameNameTextbox.TabIndex = 5;
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(327, 41);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(75, 23);
            this.addBtn.TabIndex = 6;
            this.addBtn.Text = "Add";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.Location = new System.Drawing.Point(570, 10);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(75, 23);
            this.closeBtn.TabIndex = 7;
            this.closeBtn.Text = "Close";
            this.closeBtn.UseVisualStyleBackColor = true;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // GameEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 383);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.gameNameTextbox);
            this.Controls.Add(this.createGameLabel);
            this.Controls.Add(this.renameBtn);
            this.Controls.Add(this.delBtn);
            this.Name = "GameEditor";
            this.Text = "GameEditor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button delBtn;
        private System.Windows.Forms.Button renameBtn;
        private System.Windows.Forms.Label createGameLabel;
        private System.Windows.Forms.TextBox gameNameTextbox;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.Button closeBtn;
    }
}