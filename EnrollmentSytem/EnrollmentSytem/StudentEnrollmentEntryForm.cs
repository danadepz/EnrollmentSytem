using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnrollmentSytem
{
    public partial class StudentEnrollmentEntryForm : Form
    {
        public StudentEnrollmentEntryForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            
            IdNumberTextBox.Focus();
        }

        #region Database Operations
        private DataTable ExecuteQuery(string query, params OleDbParameter[] parameters)
        {
            using (DatabaseConnection db = new DatabaseConnection())
            {
                db.Open();
                return db.ExecuteQuery(query, parameters);
            }
        }

        private int ExecuteNonQuery(string query, params OleDbParameter[] parameters)
        {
            using (DatabaseConnection db = new DatabaseConnection())
            {
                db.Open();
                return db.ExecuteNonQuery(query, parameters);
            }
        }
        #endregion

        #region Student Operations
        private void SearchStudent()
        {
            string idNumber = IdNumberTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(idNumber))
            {
                ShowValidationError("Please enter a valid ID number.", IdNumberTextBox);
                return;
            }

            try
            {
                // Show loading state (optional)
                Cursor.Current = Cursors.WaitCursor;
                NameTextBox.Text = "Searching...";
                CourseTextBox.Text = "";
                YearTextBox.Text = "";

                string query = @"SELECT STFSTUDLNAME, STFSTUDFNAME, STFSTUDCOURSE, STFSTUDYEAR 
                        FROM StudentFile 
                        WHERE STFSTUDID = @id"; // Ensure column name matches DB

                using (DatabaseConnection db = new DatabaseConnection())
                {
                    db.Open();
                    DataTable result = db.ExecuteQuery(query, new OleDbParameter("@id", idNumber));

                    if (result.Rows.Count == 0)
                    {
                        MessageBox.Show("Student not found. Check the ID.", "Not Found",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ClearForm();
                        return;
                    }

                    // Safely assign values (handles NULLs)
                    DataRow row = result.Rows[0];
                    NameTextBox.Text = $"{row["STFSTUDFNAME"]?.ToString()} {row["STFSTUDLNAME"]?.ToString()}".Trim();
                    CourseTextBox.Text = row["STFSTUDCOURSE"]?.ToString() ?? "N/A";
                    YearTextBox.Text = row["STFSTUDYEAR"]?.ToString() ?? "N/A";

                    ClearValidationError(IdNumberTextBox);
                    EdpCodeTextBox.Focus(); // Move focus to EDP Code field
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching student details:\n\n{ex.Message}", "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearForm();
            }
            finally
            {
                Cursor.Current = Cursors.Default; // Reset cursor
            }
        }
        #endregion

        #region Subject Operations
        private void SearchSubjectByEdpCode(string edpCode)
        {
            string query = @"
                SELECT 
                    s.SSFEDPCODE,
                    s.SSFSUBJCODE, 
                    sub.SFSUBJDESC, 
                    s.SSFSTARTTIME, 
                    s.SSFENDTIME, 
                    s.SSFDays, 
                    s.SSFROOM, 
                    sub.SFSUBJUNITS
                FROM SubjectSchedFile s
                INNER JOIN SubjectFile sub ON s.SSFSUBJCODE = sub.SFSUBJCODE
                WHERE s.SSFEDPCODE = @edpCode AND s.SSFSTATUS = 'AC'";

            try
            {
                DataTable result = ExecuteQuery(query, new OleDbParameter("@edpCode", edpCode));

                if (result.Rows.Count == 0)
                {
                    MessageBox.Show("Active subject not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataRow row = result.Rows[0];
                string subjectCode = row["SSFSUBJCODE"].ToString();
                string studentId = IdNumberTextBox.Text.Trim();

                // Check prerequisites before adding subject
                if (!CheckPrerequisites(subjectCode, studentId))
                {
                    return; // Stop if prerequisites aren't met
                }
                // Add to DataGridView (columns must match!)
                StudentEnrollmentEntryDataGridView.Rows.Add(
                    row["SSFEDPCODE"],       
                    row["SSFSUBJCODE"],      
                    row["SSFSTARTTIME"],     
                    row["SSFENDTIME"],      
                    row["SSFDAYS"],         
                    row["SSFROOM"],        
                    row["SFSUBJUNITS"]      
                );
                UpdateTotalUnits();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching subject: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckPrerequisites(string subjectCode, string studentId)
        {
            string query = @"
                SELECT 
                    SP.SUBJPRECODE, 
                    SP.SUBJCATEGORY, 
                    S.SFSUBJDESC, 
                    IIF(EXISTS (
                        SELECT 1 FROM StudentGradeFile SG 
                        WHERE SG.SGFSTUDID = @studentId 
                        AND SG.SGFSTUDSUBJCODE = SP.SUBJPRECODE
                        AND SG.SGFSTUDSUBJGRADE <= 3.0
                    ), 1, 0) AS IsCompleted
                FROM SubjectPreqFile SP
                INNER JOIN SubjectFile S ON SP.SUBJPRECODE = S.SFSUBJCODE
                WHERE SP.SUBJCODE = @subjectCode";

            try
            {
                DataTable result = ExecuteQuery(query,
                    new OleDbParameter("@studentId", studentId),
                    new OleDbParameter("@subjectCode", subjectCode));

                if (result.Rows.Count == 0)
                {
                    return true; // No prerequisites
                }

                foreach (DataRow row in result.Rows)
                {
                    string category = row["SUBJCATEGORY"].ToString();
                    bool isCompleted = Convert.ToInt32(row["IsCompleted"]) == 1;
                    string subjectDesc = row["SFSUBJDESC"].ToString();

                    if (category == "PR" && !isCompleted)
                    {
                        MessageBox.Show($"Prerequisite not met: {subjectDesc}", "Prerequisite Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking prerequisites: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void UpdateTotalUnits()
        {
            double totalUnits = 0;

            foreach (DataGridViewRow row in StudentEnrollmentEntryDataGridView.Rows)
            {
                // Skip empty rows
                if (row.IsNewRow || row.Cells["UnitsColumn"].Value == null)
                    continue;

                // Parse units (handle errors)
                if (double.TryParse(row.Cells["UnitsColumn"].Value.ToString(), out double units))
                {
                    totalUnits += units;
                }
            }

            TotalUnitsTextBox.Text = totalUnits.ToString("0.0"); 
        }

        private bool HasScheduleConflicts()
        {
            List<SubjectSchedule> enrolledSubjects = new List<SubjectSchedule>();

            foreach (DataGridViewRow row in StudentEnrollmentEntryDataGridView.Rows)
            {
                if (row.IsNewRow) continue;

                var subject = new SubjectSchedule
                {
                    EDPCode = row.Cells["EDPCodeColumn"].Value?.ToString(),
                    SubjectCode = row.Cells["SubjectCodeColumn"].Value?.ToString(),
                    Days = row.Cells["DaysColumn"].Value?.ToString(),
                    StartTime = row.Cells["StartTimeColumn"].Value is DateTime ?
                               (DateTime)row.Cells["StartTimeColumn"].Value : DateTime.MinValue,
                    EndTime = row.Cells["EndTimeColumn"].Value is DateTime ?
                             (DateTime)row.Cells["EndTimeColumn"].Value : DateTime.MinValue
                };

                enrolledSubjects.Add(subject);
            }

            for (int i = 0; i < enrolledSubjects.Count; i++)
            {
                for (int j = i + 1; j < enrolledSubjects.Count; j++)
                {
                    if (HasConflict(enrolledSubjects[i], enrolledSubjects[j]))
                    {
                        // Highlight conflicts
                        StudentEnrollmentEntryDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;
                        StudentEnrollmentEntryDataGridView.Rows[j].DefaultCellStyle.BackColor = Color.LightPink;

                        MessageBox.Show($"Schedule conflict between:\n" +
                                     $"{enrolledSubjects[i].SubjectCode} ({enrolledSubjects[i].Days} {enrolledSubjects[i].StartTime:t}-{enrolledSubjects[i].EndTime:t})\n" +
                                     $"and\n" +
                                     $"{enrolledSubjects[j].SubjectCode} ({enrolledSubjects[j].Days} {enrolledSubjects[j].StartTime:t}-{enrolledSubjects[j].EndTime:t})",
                                     "Schedule Conflict",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Warning);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool HasConflict(SubjectSchedule subj1, SubjectSchedule subj2)
        {
            // Check common days
            bool dayConflict = false;
            foreach (char day1 in subj1.Days ?? "")
            {
                foreach (char day2 in subj2.Days ?? "")
                {
                    if (char.ToUpper(day1) == char.ToUpper(day2))
                    {
                        dayConflict = true;
                        break;
                    }
                }
                if (dayConflict) break;
            }

            if (!dayConflict) return false;

            // Check time overlap
            return (subj1.StartTime < subj2.EndTime && subj1.EndTime > subj2.StartTime);
        }
        #endregion

        #region Event Handlers
        private void IdNumberTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchStudent(); // Trigger search on Enter
                e.SuppressKeyPress = true; // Prevent the 'ding' sound
            }
        }

        private void EdpCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string edpCode = EdpCodeTextBox.Text.Trim();

                if (string.IsNullOrWhiteSpace(edpCode))
                {
                    ShowValidationError("Please enter a valid EDP code.", EdpCodeTextBox);
                    return;
                }

                SearchSubjectByEdpCode(edpCode);
                e.SuppressKeyPress = true;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!ValidateEnrollment()) return;

            using (DatabaseConnection db = new DatabaseConnection())
            {
                db.Open();
                db.BeginTransaction();

                try
                {
                    string studentId = IdNumberTextBox.Text.Trim();
                    string encoder = EncodedByTextBox.Text.Trim();
                    string schoolYear = YearTextBox.Text.Trim();
                    double totalUnits = double.Parse(TotalUnitsTextBox.Text);

                    DateTime enrollmentDate = DateTime.Parse(StudentDateEnrollPicker.Text);

                    // Save to EnrollmentHeaderFile
                    string headerQuery = @"INSERT INTO EnrollmentHeaderFile 
                                        (ENRHFSTUDID, ENRHFSTUDDATEENROLL, ENRHFSTUDSCHLYR, 
                                         ENRHFSTUDENCODER, ENRHFSTUDTOTALUNITS, ENRHFSTUDSTATUS)
                                        VALUES (@studentId, @date, @schoolYear, @encoder, @totalUnits, @status)";

                    OleDbParameter dateParam = new OleDbParameter("@date", OleDbType.Date);
                    dateParam.Value = enrollmentDate;

                    OleDbParameter totalUnitsParam = new OleDbParameter("@totalUnits", OleDbType.Double);
                    totalUnitsParam.Value = totalUnits;

                    db.ExecuteNonQuery(headerQuery,
                        new OleDbParameter("@studentId", studentId),
                        new OleDbParameter("@date", enrollmentDate),
                        new OleDbParameter("@schoolYear", schoolYear),
                        new OleDbParameter("@encoder", encoder),
                        new OleDbParameter("@totalUnits", totalUnits),
                        new OleDbParameter("@status", "EN")
                    );

                    // Save each subject to EnrollmentDetailFile
                    foreach (DataGridViewRow row in StudentEnrollmentEntryDataGridView.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string detailQuery = @"INSERT INTO EnrollmentDetailFile 
                                             (ENRDFSTUDID, ENRDFSTUDSUBJCDE, ENRDFSTUDEDPCODE, ENRDFSTUDSTATUS)
                                             VALUES (@studentId, @subjectCode, @edpCode, @status)";

                        db.ExecuteNonQuery(detailQuery,
                            new OleDbParameter("@studentId", studentId),
                            new OleDbParameter("@subjectCode", row.Cells["SubjectCodeColumn"].Value?.ToString() ?? ""),
                            new OleDbParameter("@edpCode", row.Cells["EDPCodeColumn"].Value?.ToString() ?? ""),
                            new OleDbParameter("@status", "AC")
                        );

                        // Update class size in SubjectSchedFile
                        string updateQuery = @"UPDATE SubjectSchedFile 
                                             SET SSFCLASSSIZE = SSFCLASSSIZE + 1 
                                             WHERE SSFEDPCODE = @edpCode";

                        db.ExecuteNonQuery(updateQuery,
                            new OleDbParameter("@edpCode", row.Cells["EDPCodeColumn"].Value?.ToString() ?? "")
                        );
                    }

                    db.CommitTransaction();
                    MessageBox.Show("Enrollment saved successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                }
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    MessageBox.Show($"Error saving enrollment: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DeleteSubjButton_Click_1(object sender, EventArgs e)
        {
            if (StudentEnrollmentEntryDataGridView.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Delete selected subjects?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in StudentEnrollmentEntryDataGridView.SelectedRows)
                    {
                        StudentEnrollmentEntryDataGridView.Rows.Remove(row);
                    }
                    UpdateTotalUnits();
                }
            }
            else
            {
                MessageBox.Show("Please select subjects to delete.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        #endregion

        #region Helper Methods
        private bool ValidateEnrollment()
        {
            // Check if student ID is empty
            if (string.IsNullOrWhiteSpace(IdNumberTextBox.Text))
            {
                ShowValidationError("Please enter student ID first.", IdNumberTextBox);
                return false;
            }

            // Check if grid is empty or only contains the new row placeholder
            if (StudentEnrollmentEntryDataGridView.Rows.Count == 0 ||
                (StudentEnrollmentEntryDataGridView.Rows.Count == 1 &&
                 StudentEnrollmentEntryDataGridView.Rows[0].IsNewRow))
            {
                MessageBox.Show("No subjects added for enrollment.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Check for schedule conflicts
            if (HasScheduleConflicts())
            {
                return false;
            }

            return true;
        }

        private void ShowValidationError(string message, Control control)
        {
            MessageBox.Show(message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.BackColor = Color.LightPink;
            control.Focus();
        }

        private void ClearValidationError(Control control)
        {
            control.BackColor = SystemColors.Window;
        }

        private void ClearForm()
        {
            IdNumberTextBox.Clear();
            NameTextBox.Clear();
            CourseTextBox.Clear();
            YearTextBox.Clear();
            StudentEnrollmentEntryDataGridView.Rows.Clear();
            TotalUnitsTextBox.Clear();
            EdpCodeTextBox.Clear();
            IdNumberTextBox.Focus();
        }
        #endregion

        private class SubjectSchedule
        {
            public string EDPCode { get; set; }
            public string SubjectCode { get; set; }
            public string Days { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
        }

        private void StudentEnrollmentEntryForm_Load(object sender, EventArgs e) { }

        private void BackButton_Click(object sender, EventArgs e)
        {
            StudentEntryForm studentEntryForm = new StudentEntryForm();
            studentEntryForm.Show();
            this.Hide();
        }
    }
}