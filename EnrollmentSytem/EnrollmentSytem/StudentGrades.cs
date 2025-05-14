using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Windows.Forms;

namespace EnrollmentSytem
{
    public partial class StudentGrades : Form
    {
        private string currentStudentId = string.Empty;
        private DataTable enrolledSubjects = new DataTable();

        public StudentGrades()
        {
            InitializeComponent();
            SetupDataGridView();
            StudentIdTextBox.KeyDown += StudentIdTextBox_KeyDown;
        }

        private void SetupDataGridView()
        {
            GradesDataGridView.AutoGenerateColumns = false;
            GradesDataGridView.AllowUserToAddRows = false;

            // Configure columns with exact data property names
            if (GradesDataGridView.Columns["EdpCodeColumn"] != null)
                GradesDataGridView.Columns["EdpCodeColumn"].DataPropertyName = "EdpCodeColumn";

            if (GradesDataGridView.Columns["SubjectCodeColumn"] != null)
                GradesDataGridView.Columns["SubjectCodeColumn"].DataPropertyName = "SubjectCodeColumn";

            if (GradesDataGridView.Columns["DescriptionColumn"] != null)
                GradesDataGridView.Columns["DescriptionColumn"].DataPropertyName = "DescriptionColumn";

            if (GradesDataGridView.Columns["GradeColumn"] != null)
            {
                GradesDataGridView.Columns["GradeColumn"].DataPropertyName = "GradeColumn";
                GradesDataGridView.Columns["GradeColumn"].DefaultCellStyle.Format = "0.00";
                GradesDataGridView.Columns["GradeColumn"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (GradesDataGridView.Columns["RemarksColumn"] != null)
            {
                GradesDataGridView.Columns["RemarksColumn"].DataPropertyName = "RemarksColumn";
                GradesDataGridView.Columns["RemarksColumn"].ReadOnly = true;
            }

            // Handle grade entry and automatic remarks update
            GradesDataGridView.CellEndEdit += (sender, e) =>
            {
                if (GradesDataGridView.Columns["GradeColumn"] != null &&
                    e.ColumnIndex == GradesDataGridView.Columns["GradeColumn"].Index)
                {
                    UpdateRemarksForRow(e.RowIndex);
                }
            };
        }

        private void UpdateRemarksForRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= GradesDataGridView.Rows.Count) return;

            var row = GradesDataGridView.Rows[rowIndex];
            if (row.IsNewRow) return;

            if (row.Cells["GradeColumn"].Value != null &&
                double.TryParse(row.Cells["GradeColumn"].Value.ToString(), out double grade))
            {
                row.Cells["RemarksColumn"].Value = grade > 3.0 ? "FAIL" : "PASS";
            }
            else
            {
                row.Cells["GradeColumn"].Value = 0;
                row.Cells["RemarksColumn"].Value = "PASS";
            }
        }

        private void StudentIdTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                LoadStudentDataAndSubjects();
            }
        }

        private void LoadStudentDataAndSubjects()
        {
            if (string.IsNullOrWhiteSpace(StudentIdTextBox.Text))
            {
                MessageBox.Show("Please enter a student ID", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            currentStudentId = StudentIdTextBox.Text.Trim();

            try
            {
                using (var db = new DatabaseConnection())
                {
                    db.Open();

                    // 1. Load and display student information
                    string studentQuery = @"SELECT STFSTUDFNAME, STFSTUDLNAME, STFSTUDCOURSE, STFSTUDYEAR 
                                         FROM StudentFile 
                                         WHERE STFSTUDID = ?";

                    DataTable studentData = db.ExecuteQuery(studentQuery,
                        new OleDbParameter("", currentStudentId));

                    if (studentData.Rows.Count == 0)
                    {
                        MessageBox.Show("Student ID not found", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ClearStudentDetails();
                        return;
                    }

                    // Display student info in textboxes
                    DataRow student = studentData.Rows[0];
                    StudentNameTextBox.Text = $"{student["STFSTUDFNAME"]} {student["STFSTUDLNAME"]}";
                    CourseTextBox.Text = student["STFSTUDCOURSE"].ToString();
                    YearTextBox.Text = student["STFSTUDYEAR"].ToString();

                    // 2. Load enrolled subjects with descriptions and grades
                    string subjectsQuery = @"SELECT 
                        ed.ENRDFSTUDEDPCODE AS EdpCodeColumn,
                        ed.ENRDFSTUDSUBJCDE AS SubjectCodeColumn,
                        sub.SFSUBJDESC AS DescriptionColumn,
                        IIF(sg.SGFSTUDSUBJGRADE IS NULL, 0, sg.SGFSTUDSUBJGRADE) AS GradeColumn,
                        IIF(sg.SGFSTUDREMARKS IS NULL, 'PASS', sg.SGFSTUDREMARKS) AS RemarksColumn
                    FROM (EnrollmentDetailFile AS ed 
                    INNER JOIN SubjectFile AS sub ON ed.ENRDFSTUDSUBJCDE = sub.SFSUBJCODE)
                    LEFT JOIN StudentGradeFile AS sg ON sg.SGFSTUDID = ed.ENRDFSTUDID 
                        AND sg.SGFSTUDEDPCODE = ed.ENRDFSTUDEDPCODE 
                        AND sg.SGFSTUDSUBJCODE = ed.ENRDFSTUDSUBJCDE
                    WHERE ed.ENRDFSTUDID = ?
                    AND ed.ENRDFSTUDSTATUS <> 'CA'";

                    enrolledSubjects = db.ExecuteQuery(subjectsQuery,
                        new OleDbParameter("", currentStudentId));

                    // Bind to DataGridView
                    GradesDataGridView.DataSource = null;
                    GradesDataGridView.DataSource = enrolledSubjects;

                    // Update remarks for all loaded rows
                    for (int i = 0; i < GradesDataGridView.Rows.Count; i++)
                    {
                        if (!GradesDataGridView.Rows[i].IsNewRow)
                            UpdateRemarksForRow(i);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearStudentDetails();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentStudentId))
            {
                MessageBox.Show("Please load student data first", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (GradesDataGridView.Rows.Count == 0)
            {
                MessageBox.Show("No subjects to save", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Validate all grades before saving
            foreach (DataGridViewRow row in GradesDataGridView.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells["GradeColumn"].Value == null ||
                    !double.TryParse(row.Cells["GradeColumn"].Value.ToString(), out double grade))
                {
                    MessageBox.Show("Please enter valid numeric grades for all subjects",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (grade < 0 || grade > 5.0)
                {
                    MessageBox.Show("Grades must be between 0 and 5.0",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            try
            {
                using (var db = new DatabaseConnection())
                {
                    db.Open();
                    db.BeginTransaction();

                    foreach (DataGridViewRow row in GradesDataGridView.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string edpCode = row.Cells["EdpCodeColumn"].Value?.ToString() ?? "";
                        string subjectCode = row.Cells["SubjectCodeColumn"].Value?.ToString() ?? "";
                        double grade = row.Cells["GradeColumn"].Value != null ?
                            Convert.ToDouble(row.Cells["GradeColumn"].Value) : 0;
                        string remarks = row.Cells["RemarksColumn"].Value?.ToString() ?? "PASS";

                        // Check if grade record exists
                        string checkQuery = @"SELECT COUNT(*) FROM StudentGradeFile 
                                           WHERE SGFSTUDID = ?
                                           AND SGFSTUDEDPCODE = ?
                                           AND SGFSTUDSUBJCODE = ?";

                        int count = Convert.ToInt32(db.ExecuteScalar(checkQuery,
                            new OleDbParameter("", currentStudentId),
                            new OleDbParameter("", edpCode),
                            new OleDbParameter("", subjectCode)));

                        if (count > 0)
                        {
                            // Update existing record
                            string updateQuery = @"UPDATE StudentGradeFile 
                                                SET SGFSTUDSUBJGRADE = ?,
                                                    SGFSTUDREMARKS = ?
                                                WHERE SGFSTUDID = ?
                                                AND SGFSTUDEDPCODE = ?
                                                AND SGFSTUDSUBJCODE = ?";
                            db.ExecuteNonQuery(updateQuery,
                                new OleDbParameter("", grade),
                                new OleDbParameter("", remarks),
                                new OleDbParameter("", currentStudentId),
                                new OleDbParameter("", edpCode),
                                new OleDbParameter("", subjectCode));
                        }
                        else
                        {
                            // Insert new record
                            string insertQuery = @"INSERT INTO StudentGradeFile 
                                                (SGFSTUDID, SGFSTUDSUBJCODE, SGFSTUDSUBJGRADE, 
                                                 SGFSTUDEDPCODE, SGFSTUDREMARKS)
                                                VALUES (?, ?, ?, ?, ?)";
                            db.ExecuteNonQuery(insertQuery,
                                new OleDbParameter("", currentStudentId),
                                new OleDbParameter("", subjectCode),
                                new OleDbParameter("", grade),
                                new OleDbParameter("", edpCode),
                                new OleDbParameter("", remarks));
                        }
                    }

                    db.CommitTransaction();
                    MessageBox.Show("Grades saved successfully", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving grades: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearButton_Click_1(object sender, EventArgs e)
        {
            currentStudentId = string.Empty;
            enrolledSubjects.Clear();
            StudentIdTextBox.Clear();
            StudentNameTextBox.Clear();
            CourseTextBox.Clear();
            YearTextBox.Clear();
            GradesDataGridView.DataSource = null;
            StudentIdTextBox.Focus();
        }

        private void ClearStudentDetails()
        {
            StudentNameTextBox.Clear();
            CourseTextBox.Clear();
            YearTextBox.Clear();
            GradesDataGridView.DataSource = null;
        }

        
    }
}