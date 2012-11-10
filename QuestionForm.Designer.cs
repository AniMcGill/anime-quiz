namespace Anime_Quiz
{
    partial class QuestionForm
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
            this.answerBtn = new System.Windows.Forms.Button();
            this.closeBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // answerBtn
            // 
            this.answerBtn.Location = new System.Drawing.Point(12, 12);
            this.answerBtn.Name = "answerBtn";
            this.answerBtn.Size = new System.Drawing.Size(75, 23);
            this.answerBtn.TabIndex = 0;
            this.answerBtn.Text = "Answer";
            this.answerBtn.UseVisualStyleBackColor = true;
            this.answerBtn.Click += new System.EventHandler(this.answerBtn_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeBtn.Location = new System.Drawing.Point(93, 12);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(75, 23);
            this.closeBtn.TabIndex = 1;
            this.closeBtn.Text = "Close";
            this.closeBtn.UseVisualStyleBackColor = true;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // QuestionForm
            // 
            this.AcceptButton = this.answerBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeBtn;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.answerBtn);
            this.Name = "QuestionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Question";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QuestionForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button answerBtn;
        private System.Windows.Forms.Button closeBtn;
    }
}