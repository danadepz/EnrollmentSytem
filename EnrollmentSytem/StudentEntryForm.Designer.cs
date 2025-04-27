namespace EnrollmentSytem
{
    partial class StudentEntryForm
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
            this.IdNumberLabel = new System.Windows.Forms.Label();
            this.FirstNameLabel = new System.Windows.Forms.Label();
            this.MiddleNameLabel = new System.Windows.Forms.Label();
            this.LastNameLabel = new System.Windows.Forms.Label();
            this.CourseLabel = new System.Windows.Forms.Label();
            this.YearLabel = new System.Windows.Forms.Label();
            this.RemarksLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.IdNumberTextBox = new System.Windows.Forms.TextBox();
            this.FirstNameTextBox = new System.Windows.Forms.TextBox();
            this.MiddleNameTextBox = new System.Windows.Forms.TextBox();
            this.LastNameTextBox = new System.Windows.Forms.TextBox();
            this.YearTextBox = new System.Windows.Forms.TextBox();
            this.CourseTextBox = new System.Windows.Forms.TextBox();
            this.RemarksComboBox = new System.Windows.Forms.ComboBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ClearButton1 = new System.Windows.Forms.Button();
            this.StudentEntryLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // IdNumberLabel
            // 
            this.IdNumberLabel.AutoSize = true;
            this.IdNumberLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IdNumberLabel.Location = new System.Drawing.Point(60, 96);
            this.IdNumberLabel.Name = "IdNumberLabel";
            this.IdNumberLabel.Size = new System.Drawing.Size(70, 15);
            this.IdNumberLabel.TabIndex = 0;
            this.IdNumberLabel.Text = "ID Number";
            // 
            // FirstNameLabel
            // 
            this.FirstNameLabel.AutoSize = true;
            this.FirstNameLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FirstNameLabel.Location = new System.Drawing.Point(67, 124);
            this.FirstNameLabel.Name = "FirstNameLabel";
            this.FirstNameLabel.Size = new System.Drawing.Size(63, 15);
            this.FirstNameLabel.TabIndex = 1;
            this.FirstNameLabel.Text = "Firstname";
            // 
            // MiddleNameLabel
            // 
            this.MiddleNameLabel.AutoSize = true;
            this.MiddleNameLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MiddleNameLabel.Location = new System.Drawing.Point(51, 154);
            this.MiddleNameLabel.Name = "MiddleNameLabel";
            this.MiddleNameLabel.Size = new System.Drawing.Size(79, 15);
            this.MiddleNameLabel.TabIndex = 2;
            this.MiddleNameLabel.Text = "Middle Initial";
            // 
            // LastNameLabel
            // 
            this.LastNameLabel.AutoSize = true;
            this.LastNameLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LastNameLabel.Location = new System.Drawing.Point(68, 185);
            this.LastNameLabel.Name = "LastNameLabel";
            this.LastNameLabel.Size = new System.Drawing.Size(62, 15);
            this.LastNameLabel.TabIndex = 3;
            this.LastNameLabel.Text = "Lastname";
            // 
            // CourseLabel
            // 
            this.CourseLabel.AutoSize = true;
            this.CourseLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CourseLabel.Location = new System.Drawing.Point(82, 217);
            this.CourseLabel.Name = "CourseLabel";
            this.CourseLabel.Size = new System.Drawing.Size(48, 15);
            this.CourseLabel.TabIndex = 4;
            this.CourseLabel.Text = "Course";
            // 
            // YearLabel
            // 
            this.YearLabel.AutoSize = true;
            this.YearLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YearLabel.Location = new System.Drawing.Point(97, 242);
            this.YearLabel.Name = "YearLabel";
            this.YearLabel.Size = new System.Drawing.Size(33, 15);
            this.YearLabel.TabIndex = 5;
            this.YearLabel.Text = "Year";
            // 
            // RemarksLabel
            // 
            this.RemarksLabel.AutoSize = true;
            this.RemarksLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RemarksLabel.Location = new System.Drawing.Point(72, 270);
            this.RemarksLabel.Name = "RemarksLabel";
            this.RemarksLabel.Size = new System.Drawing.Size(58, 15);
            this.RemarksLabel.TabIndex = 6;
            this.RemarksLabel.Text = "Remarks";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(307, 344);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 13);
            this.label8.TabIndex = 7;
            // 
            // IdNumberTextBox
            // 
            this.IdNumberTextBox.Location = new System.Drawing.Point(136, 94);
            this.IdNumberTextBox.Name = "IdNumberTextBox";
            this.IdNumberTextBox.Size = new System.Drawing.Size(100, 20);
            this.IdNumberTextBox.TabIndex = 8;
            this.IdNumberTextBox.TextChanged += new System.EventHandler(this.IdNumberTextBox_TextChanged);
            // 
            // FirstNameTextBox
            // 
            this.FirstNameTextBox.Location = new System.Drawing.Point(136, 122);
            this.FirstNameTextBox.Name = "FirstNameTextBox";
            this.FirstNameTextBox.Size = new System.Drawing.Size(156, 20);
            this.FirstNameTextBox.TabIndex = 9;
            // 
            // MiddleNameTextBox
            // 
            this.MiddleNameTextBox.Location = new System.Drawing.Point(136, 152);
            this.MiddleNameTextBox.Name = "MiddleNameTextBox";
            this.MiddleNameTextBox.Size = new System.Drawing.Size(156, 20);
            this.MiddleNameTextBox.TabIndex = 10;
            // 
            // LastNameTextBox
            // 
            this.LastNameTextBox.Location = new System.Drawing.Point(136, 183);
            this.LastNameTextBox.Name = "LastNameTextBox";
            this.LastNameTextBox.Size = new System.Drawing.Size(156, 20);
            this.LastNameTextBox.TabIndex = 11;
            // 
            // YearTextBox
            // 
            this.YearTextBox.Location = new System.Drawing.Point(136, 240);
            this.YearTextBox.Name = "YearTextBox";
            this.YearTextBox.Size = new System.Drawing.Size(100, 20);
            this.YearTextBox.TabIndex = 12;
            // 
            // CourseTextBox
            // 
            this.CourseTextBox.Location = new System.Drawing.Point(136, 212);
            this.CourseTextBox.Name = "CourseTextBox";
            this.CourseTextBox.Size = new System.Drawing.Size(156, 20);
            this.CourseTextBox.TabIndex = 13;
            // 
            // RemarksComboBox
            // 
            this.RemarksComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RemarksComboBox.FormattingEnabled = true;
            this.RemarksComboBox.Items.AddRange(new object[] {
            "--Choose--",
            "Shiftee",
            "Transferee",
            "New",
            "Old",
            "Cross-Enrollee",
            "Returnee"});
            this.RemarksComboBox.Location = new System.Drawing.Point(136, 268);
            this.RemarksComboBox.Name = "RemarksComboBox";
            this.RemarksComboBox.Size = new System.Drawing.Size(100, 21);
            this.RemarksComboBox.TabIndex = 14;
            // 
            // SaveButton
            // 
            this.SaveButton.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveButton.Location = new System.Drawing.Point(98, 308);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 15;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ClearButton1
            // 
            this.ClearButton1.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClearButton1.Location = new System.Drawing.Point(179, 308);
            this.ClearButton1.Name = "ClearButton1";
            this.ClearButton1.Size = new System.Drawing.Size(75, 23);
            this.ClearButton1.TabIndex = 16;
            this.ClearButton1.Text = "Clear";
            this.ClearButton1.UseVisualStyleBackColor = true;
            this.ClearButton1.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // StudentEntryLabel
            // 
            this.StudentEntryLabel.AutoSize = true;
            this.StudentEntryLabel.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StudentEntryLabel.Location = new System.Drawing.Point(109, 47);
            this.StudentEntryLabel.Name = "StudentEntryLabel";
            this.StudentEntryLabel.Size = new System.Drawing.Size(145, 18);
            this.StudentEntryLabel.TabIndex = 17;
            this.StudentEntryLabel.Text = "STUDENT ENTRY";
            this.StudentEntryLabel.Click += new System.EventHandler(this.StudentEntryLabel_Click);
            // 
            // StudentEntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 410);
            this.Controls.Add(this.StudentEntryLabel);
            this.Controls.Add(this.ClearButton1);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.RemarksComboBox);
            this.Controls.Add(this.CourseTextBox);
            this.Controls.Add(this.YearTextBox);
            this.Controls.Add(this.LastNameTextBox);
            this.Controls.Add(this.MiddleNameTextBox);
            this.Controls.Add(this.FirstNameTextBox);
            this.Controls.Add(this.IdNumberTextBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.RemarksLabel);
            this.Controls.Add(this.YearLabel);
            this.Controls.Add(this.CourseLabel);
            this.Controls.Add(this.LastNameLabel);
            this.Controls.Add(this.MiddleNameLabel);
            this.Controls.Add(this.FirstNameLabel);
            this.Controls.Add(this.IdNumberLabel);
            this.Name = "StudentEntryForm";
            this.Text = "Student Entry";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label IdNumberLabel;
        private System.Windows.Forms.Label FirstNameLabel;
        private System.Windows.Forms.Label MiddleNameLabel;
        private System.Windows.Forms.Label LastNameLabel;
        private System.Windows.Forms.Label CourseLabel;
        private System.Windows.Forms.Label YearLabel;
        private System.Windows.Forms.Label RemarksLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox IdNumberTextBox;
        private System.Windows.Forms.TextBox FirstNameTextBox;
        private System.Windows.Forms.TextBox MiddleNameTextBox;
        private System.Windows.Forms.TextBox LastNameTextBox;
        private System.Windows.Forms.TextBox YearTextBox;
        private System.Windows.Forms.TextBox CourseTextBox;
        private System.Windows.Forms.ComboBox RemarksComboBox;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button ClearButton1;
        private System.Windows.Forms.Label StudentEntryLabel;
    }
}

