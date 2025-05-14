namespace EnrollmentSytem
{
    partial class StudentGrades
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
            this.NameLabel = new System.Windows.Forms.Label();
            this.CourseLabel = new System.Windows.Forms.Label();
            this.YearLabel = new System.Windows.Forms.Label();
            this.TableTitleLabel = new System.Windows.Forms.Label();
            this.GradesDataGridView = new System.Windows.Forms.DataGridView();
            this.EdpCodeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubjectCodeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GradeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RemarksColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClearButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.StudentIdLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.StudentIdTextBox = new System.Windows.Forms.TextBox();
            this.YearTextBox = new System.Windows.Forms.TextBox();
            this.CourseTextBox = new System.Windows.Forms.TextBox();
            this.StudentNameTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.GradesDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameLabel.Location = new System.Drawing.Point(64, 100);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(68, 15);
            this.NameLabel.TabIndex = 1;
            this.NameLabel.Text = "Full Name:";
            // 
            // CourseLabel
            // 
            this.CourseLabel.AutoSize = true;
            this.CourseLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CourseLabel.Location = new System.Drawing.Point(64, 129);
            this.CourseLabel.Name = "CourseLabel";
            this.CourseLabel.Size = new System.Drawing.Size(53, 15);
            this.CourseLabel.TabIndex = 2;
            this.CourseLabel.Text = "Course:";
            // 
            // YearLabel
            // 
            this.YearLabel.AutoSize = true;
            this.YearLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YearLabel.Location = new System.Drawing.Point(64, 156);
            this.YearLabel.Name = "YearLabel";
            this.YearLabel.Size = new System.Drawing.Size(42, 15);
            this.YearLabel.TabIndex = 3;
            this.YearLabel.Text = "Year: ";
            // 
            // TableTitleLabel
            // 
            this.TableTitleLabel.AutoSize = true;
            this.TableTitleLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TableTitleLabel.Location = new System.Drawing.Point(235, 182);
            this.TableTitleLabel.Name = "TableTitleLabel";
            this.TableTitleLabel.Size = new System.Drawing.Size(106, 15);
            this.TableTitleLabel.TabIndex = 4;
            this.TableTitleLabel.Text = "Enrolled Subjects";
            // 
            // GradesDataGridView
            // 
            this.GradesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GradesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EdpCodeColumn,
            this.SubjectCodeColumn,
            this.DescriptionColumn,
            this.GradeColumn,
            this.RemarksColumn});
            this.GradesDataGridView.Location = new System.Drawing.Point(45, 210);
            this.GradesDataGridView.Name = "GradesDataGridView";
            this.GradesDataGridView.Size = new System.Drawing.Size(541, 150);
            this.GradesDataGridView.TabIndex = 5;
            // 
            // EdpCodeColumn
            // 
            this.EdpCodeColumn.HeaderText = "EDP Code";
            this.EdpCodeColumn.Name = "EdpCodeColumn";
            // 
            // SubjectCodeColumn
            // 
            this.SubjectCodeColumn.HeaderText = "Subject Code";
            this.SubjectCodeColumn.Name = "SubjectCodeColumn";
            // 
            // DescriptionColumn
            // 
            this.DescriptionColumn.HeaderText = "Description";
            this.DescriptionColumn.Name = "DescriptionColumn";
            // 
            // GradeColumn
            // 
            this.GradeColumn.HeaderText = "Grade";
            this.GradeColumn.Name = "GradeColumn";
            // 
            // RemarksColumn
            // 
            this.RemarksColumn.HeaderText = "Remarks";
            this.RemarksColumn.Name = "RemarksColumn";
            // 
            // ClearButton
            // 
            this.ClearButton.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClearButton.Location = new System.Drawing.Point(330, 380);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(75, 23);
            this.ClearButton.TabIndex = 6;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click_1);
            // 
            // SaveButton
            // 
            this.SaveButton.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveButton.Location = new System.Drawing.Point(238, 380);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 7;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // StudentIdLabel
            // 
            this.StudentIdLabel.AutoSize = true;
            this.StudentIdLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StudentIdLabel.Location = new System.Drawing.Point(60, 70);
            this.StudentIdLabel.Name = "StudentIdLabel";
            this.StudentIdLabel.Size = new System.Drawing.Size(74, 15);
            this.StudentIdLabel.TabIndex = 0;
            this.StudentIdLabel.Text = "Student ID:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(200, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "Student Grade Entry Form";
            // 
            // StudentIdTextBox
            // 
            this.StudentIdTextBox.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StudentIdTextBox.Location = new System.Drawing.Point(143, 66);
            this.StudentIdTextBox.Name = "StudentIdTextBox";
            this.StudentIdTextBox.Size = new System.Drawing.Size(100, 21);
            this.StudentIdTextBox.TabIndex = 9;
            // 
            // YearTextBox
            // 
            this.YearTextBox.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YearTextBox.Location = new System.Drawing.Point(143, 145);
            this.YearTextBox.Name = "YearTextBox";
            this.YearTextBox.ReadOnly = true;
            this.YearTextBox.Size = new System.Drawing.Size(100, 21);
            this.YearTextBox.TabIndex = 10;
            // 
            // CourseTextBox
            // 
            this.CourseTextBox.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CourseTextBox.Location = new System.Drawing.Point(143, 119);
            this.CourseTextBox.Name = "CourseTextBox";
            this.CourseTextBox.ReadOnly = true;
            this.CourseTextBox.Size = new System.Drawing.Size(100, 21);
            this.CourseTextBox.TabIndex = 11;
            // 
            // StudentNameTextBox
            // 
            this.StudentNameTextBox.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StudentNameTextBox.Location = new System.Drawing.Point(143, 93);
            this.StudentNameTextBox.Name = "StudentNameTextBox";
            this.StudentNameTextBox.ReadOnly = true;
            this.StudentNameTextBox.Size = new System.Drawing.Size(214, 21);
            this.StudentNameTextBox.TabIndex = 12;
            // 
            // StudentGrades
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 450);
            this.Controls.Add(this.StudentNameTextBox);
            this.Controls.Add(this.CourseTextBox);
            this.Controls.Add(this.YearTextBox);
            this.Controls.Add(this.StudentIdTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.GradesDataGridView);
            this.Controls.Add(this.TableTitleLabel);
            this.Controls.Add(this.YearLabel);
            this.Controls.Add(this.CourseLabel);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.StudentIdLabel);
            this.Name = "StudentGrades";
            this.Text = "StudentGrades";
            ((System.ComponentModel.ISupportInitialize)(this.GradesDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Label CourseLabel;
        private System.Windows.Forms.Label YearLabel;
        private System.Windows.Forms.Label TableTitleLabel;
        private System.Windows.Forms.DataGridView GradesDataGridView;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label StudentIdLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox StudentIdTextBox;
        private System.Windows.Forms.TextBox YearTextBox;
        private System.Windows.Forms.TextBox CourseTextBox;
        private System.Windows.Forms.TextBox StudentNameTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn EdpCodeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubjectCodeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DescriptionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn GradeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RemarksColumn;
    }
}