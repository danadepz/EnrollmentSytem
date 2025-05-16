namespace EnrollmentSytem
{
    partial class AdminMenu
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
            this.CreateButton = new System.Windows.Forms.Button();
            this.ReturnButton = new System.Windows.Forms.Button();
            this.UsernameTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.StudentRadioButton = new System.Windows.Forms.RadioButton();
            this.TeacherRadioButton = new System.Windows.Forms.RadioButton();
            this.SubjectsScheduleLinkLabel = new System.Windows.Forms.LinkLabel();
            this.AccManDataGridView = new System.Windows.Forms.DataGridView();
            this.AccManagementLabel = new System.Windows.Forms.Label();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.AccManDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // CreateButton
            // 
            this.CreateButton.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreateButton.Location = new System.Drawing.Point(170, 237);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(62, 27);
            this.CreateButton.TabIndex = 1;
            this.CreateButton.Text = "Create Account";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // ReturnButton
            // 
            this.ReturnButton.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReturnButton.Location = new System.Drawing.Point(12, 11);
            this.ReturnButton.Name = "ReturnButton";
            this.ReturnButton.Size = new System.Drawing.Size(74, 23);
            this.ReturnButton.TabIndex = 2;
            this.ReturnButton.Text = "Return";
            this.ReturnButton.UseVisualStyleBackColor = true;
            this.ReturnButton.Click += new System.EventHandler(this.ReturnButton_Click);
            // 
            // UsernameTextBox
            // 
            this.UsernameTextBox.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameTextBox.Location = new System.Drawing.Point(111, 132);
            this.UsernameTextBox.Name = "UsernameTextBox";
            this.UsernameTextBox.Size = new System.Drawing.Size(194, 21);
            this.UsernameTextBox.TabIndex = 3;
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordTextBox.Location = new System.Drawing.Point(111, 171);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(194, 21);
            this.PasswordTextBox.TabIndex = 4;
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameLabel.Location = new System.Drawing.Point(89, 116);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(70, 15);
            this.UsernameLabel.TabIndex = 5;
            this.UsernameLabel.Text = "Username:";
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordLabel.Location = new System.Drawing.Point(89, 155);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(66, 15);
            this.PasswordLabel.TabIndex = 6;
            this.PasswordLabel.Text = "Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(131, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 22);
            this.label1.TabIndex = 7;
            this.label1.Text = "Add an Account";
            // 
            // StudentRadioButton
            // 
            this.StudentRadioButton.AutoSize = true;
            this.StudentRadioButton.Location = new System.Drawing.Point(135, 203);
            this.StudentRadioButton.Name = "StudentRadioButton";
            this.StudentRadioButton.Size = new System.Drawing.Size(62, 17);
            this.StudentRadioButton.TabIndex = 8;
            this.StudentRadioButton.TabStop = true;
            this.StudentRadioButton.Text = "Student";
            this.StudentRadioButton.UseVisualStyleBackColor = true;
            // 
            // TeacherRadioButton
            // 
            this.TeacherRadioButton.AutoSize = true;
            this.TeacherRadioButton.Location = new System.Drawing.Point(220, 203);
            this.TeacherRadioButton.Name = "TeacherRadioButton";
            this.TeacherRadioButton.Size = new System.Drawing.Size(65, 17);
            this.TeacherRadioButton.TabIndex = 9;
            this.TeacherRadioButton.TabStop = true;
            this.TeacherRadioButton.Text = "Teacher";
            this.TeacherRadioButton.UseVisualStyleBackColor = true;
            // 
            // SubjectsScheduleLinkLabel
            // 
            this.SubjectsScheduleLinkLabel.AutoSize = true;
            this.SubjectsScheduleLinkLabel.Location = new System.Drawing.Point(177, 16);
            this.SubjectsScheduleLinkLabel.Name = "SubjectsScheduleLinkLabel";
            this.SubjectsScheduleLinkLabel.Size = new System.Drawing.Size(98, 13);
            this.SubjectsScheduleLinkLabel.TabIndex = 10;
            this.SubjectsScheduleLinkLabel.TabStop = true;
            this.SubjectsScheduleLinkLabel.Text = "Subjects/Schedule";
            this.SubjectsScheduleLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SubjectsScheduleLinkLabel_LinkClicked);
            // 
            // AccManDataGridView
            // 
            this.AccManDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AccManDataGridView.Location = new System.Drawing.Point(35, 334);
            this.AccManDataGridView.Name = "AccManDataGridView";
            this.AccManDataGridView.Size = new System.Drawing.Size(346, 320);
            this.AccManDataGridView.TabIndex = 12;
            // 
            // AccManagementLabel
            // 
            this.AccManagementLabel.AutoSize = true;
            this.AccManagementLabel.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AccManagementLabel.Location = new System.Drawing.Point(94, 291);
            this.AccManagementLabel.Name = "AccManagementLabel";
            this.AccManagementLabel.Size = new System.Drawing.Size(209, 22);
            this.AccManagementLabel.TabIndex = 13;
            this.AccManagementLabel.Text = "Account Management";
            // 
            // DeleteButton
            // 
            this.DeleteButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.DeleteButton.Location = new System.Drawing.Point(332, 305);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(53, 23);
            this.DeleteButton.TabIndex = 15;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = false;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click_1);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(362, 16);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(82, 13);
            this.linkLabel1.TabIndex = 16;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Assign Subjects";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // AssSubjLinkLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 718);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.AccManagementLabel);
            this.Controls.Add(this.AccManDataGridView);
            this.Controls.Add(this.SubjectsScheduleLinkLabel);
            this.Controls.Add(this.TeacherRadioButton);
            this.Controls.Add(this.StudentRadioButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.UsernameLabel);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.UsernameTextBox);
            this.Controls.Add(this.ReturnButton);
            this.Controls.Add(this.CreateButton);
            this.Name = "AssSubjLinkLabel";
            this.Text = "Admin_Menu";
            ((System.ComponentModel.ISupportInitialize)(this.AccManDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.Button ReturnButton;
        private System.Windows.Forms.TextBox UsernameTextBox;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton StudentRadioButton;
        private System.Windows.Forms.RadioButton TeacherRadioButton;
        private System.Windows.Forms.LinkLabel SubjectsScheduleLinkLabel;
        private System.Windows.Forms.DataGridView AccManDataGridView;
        private System.Windows.Forms.Label AccManagementLabel;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}