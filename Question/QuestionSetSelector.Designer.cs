namespace Anime_Quiz
{
    partial class QuestionSetSelector
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
            this.questionSetSelectLabel = new System.Windows.Forms.Label();
            this.questionSetList = new System.Windows.Forms.ComboBox();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.loadBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // questionSetSelectLabel
            // 
            this.questionSetSelectLabel.AutoSize = true;
            this.questionSetSelectLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.questionSetSelectLabel.Location = new System.Drawing.Point(12, 9);
            this.questionSetSelectLabel.Name = "questionSetSelectLabel";
            this.questionSetSelectLabel.Size = new System.Drawing.Size(151, 13);
            this.questionSetSelectLabel.TabIndex = 0;
            this.questionSetSelectLabel.Text = "Select a Question Set to load: ";
            // 
            // questionSetList
            // 
            this.questionSetList.FormattingEnabled = true;
            this.questionSetList.Location = new System.Drawing.Point(169, 6);
            this.questionSetList.Name = "questionSetList";
            this.questionSetList.Size = new System.Drawing.Size(175, 21);
            this.questionSetList.TabIndex = 1;
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(269, 38);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 2;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // loadBtn
            // 
            this.loadBtn.Location = new System.Drawing.Point(188, 38);
            this.loadBtn.Name = "loadBtn";
            this.loadBtn.Size = new System.Drawing.Size(75, 23);
            this.loadBtn.TabIndex = 3;
            this.loadBtn.Text = "Load";
            this.loadBtn.UseVisualStyleBackColor = true;
            this.loadBtn.Click += new System.EventHandler(this.loadBtn_Click);
            // 
            // QuestionSetSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 73);
            this.Controls.Add(this.loadBtn);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.questionSetList);
            this.Controls.Add(this.questionSetSelectLabel);
            this.Name = "QuestionSetSelector";
            this.Text = "Question Set Selector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label questionSetSelectLabel;
        private System.Windows.Forms.ComboBox questionSetList;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button loadBtn;
    }
}