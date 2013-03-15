namespace Anime_Quiz.Team
{
    partial class TeamSelector
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
            this.numTeamLabel = new System.Windows.Forms.Label();
            this.teamNumberBox = new System.Windows.Forms.NumericUpDown();
            this.teamNumberBtn = new System.Windows.Forms.Button();
            this.helpTextLabel = new System.Windows.Forms.Label();
            this.registerBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.teamNumberBox)).BeginInit();
            this.SuspendLayout();
            // 
            // numTeamLabel
            // 
            this.numTeamLabel.AutoSize = true;
            this.numTeamLabel.Location = new System.Drawing.Point(12, 9);
            this.numTeamLabel.Name = "numTeamLabel";
            this.numTeamLabel.Size = new System.Drawing.Size(93, 13);
            this.numTeamLabel.TabIndex = 0;
            this.numTeamLabel.Text = "Number of teams: ";
            // 
            // teamNumberBox
            // 
            this.teamNumberBox.Location = new System.Drawing.Point(111, 7);
            this.teamNumberBox.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.teamNumberBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.teamNumberBox.Name = "teamNumberBox";
            this.teamNumberBox.Size = new System.Drawing.Size(28, 20);
            this.teamNumberBox.TabIndex = 1;
            this.teamNumberBox.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // teamNumberBtn
            // 
            this.teamNumberBtn.Location = new System.Drawing.Point(145, 4);
            this.teamNumberBtn.Name = "teamNumberBtn";
            this.teamNumberBtn.Size = new System.Drawing.Size(39, 23);
            this.teamNumberBtn.TabIndex = 2;
            this.teamNumberBtn.Text = "Go";
            this.teamNumberBtn.UseVisualStyleBackColor = true;
            this.teamNumberBtn.Click += new System.EventHandler(this.teamNumberBtn_Click);
            // 
            // helpTextLabel
            // 
            this.helpTextLabel.AutoSize = true;
            this.helpTextLabel.Location = new System.Drawing.Point(12, 30);
            this.helpTextLabel.Name = "helpTextLabel";
            this.helpTextLabel.Size = new System.Drawing.Size(224, 13);
            this.helpTextLabel.TabIndex = 3;
            this.helpTextLabel.Text = "Choose an existing Team or create a new one";
            // 
            // registerBtn
            // 
            this.registerBtn.Location = new System.Drawing.Point(377, 227);
            this.registerBtn.Name = "registerBtn";
            this.registerBtn.Size = new System.Drawing.Size(75, 23);
            this.registerBtn.TabIndex = 4;
            this.registerBtn.Text = "Register";
            this.registerBtn.UseVisualStyleBackColor = true;
            this.registerBtn.Click += new System.EventHandler(this.registerBtn_Click);
            // 
            // TeamSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 262);
            this.Controls.Add(this.registerBtn);
            this.Controls.Add(this.helpTextLabel);
            this.Controls.Add(this.teamNumberBtn);
            this.Controls.Add(this.teamNumberBox);
            this.Controls.Add(this.numTeamLabel);
            this.Name = "TeamSelector";
            this.Text = "Team Registration";
            ((System.ComponentModel.ISupportInitialize)(this.teamNumberBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label numTeamLabel;
        private System.Windows.Forms.NumericUpDown teamNumberBox;
        private System.Windows.Forms.Button teamNumberBtn;
        private System.Windows.Forms.Label helpTextLabel;
        private System.Windows.Forms.Button registerBtn;
    }
}