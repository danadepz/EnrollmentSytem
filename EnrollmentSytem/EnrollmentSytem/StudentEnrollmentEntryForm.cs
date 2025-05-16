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
                // Show loading state
                Cursor.Current = Cursors.WaitCursor;
                NameTextBox.Text = "Searching...";
                CourseTextBox.Text = "";
                YearTextBox.Text = "";

                string query = @"SELECT STFSTUDLNAME, STFSTUDFNAME, STFSTUDCOURSE, STFSTUDYEAR 
                        FROM StudentFile 
                        WHERE STFSTUDID = @id";

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
                    EdpCodeTextBox.Focus();
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
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion

        #region Subject Operations
        private void SearchSubjectByEdpCode(string edpCode)
        {
            string query = @"SELECT 
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
                    return;
                }

                // Check unit limit before adding
                double newUnits = Convert.ToDouble(row["SFSUBJUNITS"]);
                double currentTotal = GetCurrentTotalUnits();

                if (currentTotal + newUnits > 29)
                {
                    MessageBox.Show($"Cannot add subject. Total units would exceed 29.\n" +
                                  $"Current total: {currentTotal}\n" +
                                  $"This subject: {newUnits}",
                                  "Unit Limit Exceeded",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    return;
                }

                // Create the new subject to check
                var newSubject = new SubjectSchedule
                {
                    EDPCode = row["SSFEDPCODE"].ToString(),
                    SubjectCode = row["SSFSUBJCODE"].ToString(),
                    Days = row["SSFDAYS"].ToString(),
                    StartTime = Convert.ToDateTime(row["SSFSTARTTIME"]),
                    EndTime = Convert.ToDateTime(row["SSFENDTIME"])
                };

                // Check for conflicts with existing subjects
                bool hasConflict = false;
                DataGridViewRow conflictingRow = null;

                foreach (DataGridViewRow existingRow in StudentEnrollmentEntryDataGridView.Rows)
                {
                    if (existingRow.IsNewRow) continue;

                    var existingSubject = new SubjectSchedule
                    {
                        EDPCode = existingRow.Cells["EDPCodeColumn"].Value?.ToString(),
                        SubjectCode = existingRow.Cells["SubjectCodeColumn"].Value?.ToString(),
                        Days = existingRow.Cells["DaysColumn"].Value?.ToString(),
                        StartTime = Convert.ToDateTime(existingRow.Cells["StartTimeColumn"].Value),
                        EndTime = Convert.ToDateTime(existingRow.Cells["EndTimeColumn"].Value)
                    };

                    if (HasConflict(newSubject, existingSubject))
                    {
                        hasConflict = true;
                        conflictingRow = existingRow;
                        break;
                    }
                }

                if (hasConflict)
                {
                    // Highlight the conflicting row
                    conflictingRow.DefaultCellStyle.BackColor = Color.LightPink;

                    MessageBox.Show($"Cannot add subject: {newSubject.SubjectCode}\n\n" +
                                  $"Conflict with: {conflictingRow.Cells["SubjectCodeColumn"].Value}\n" +
                                  $"Day/Time: {newSubject.Days} {newSubject.StartTime:t}-{newSubject.EndTime:t}\n\n" +
                                  "Please remove the conflicting subject first.",
                                  "Schedule Conflict",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    return;
                }

                // If no conflicts, add to grid
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
        private double GetCurrentTotalUnits()
        {
            double total = 0;
            foreach (DataGridViewRow row in StudentEnrollmentEntryDataGridView.Rows)
            {
                if (!row.IsNewRow && row.Cells["UnitsColumn"].Value != null)
                {
                    if (double.TryParse(row.Cells["UnitsColumn"].Value.ToString(), out double units))
                    {
                        total += units;
                    }
                }
            }
            return total;
        }

        private void UpdateTotalUnits()
        {
            double totalUnits = GetCurrentTotalUnits();
            TotalUnitsTextBox.Text = totalUnits.ToString("0.0");

            // Show warning when approaching limit
            if (totalUnits >= 25)
            {
                TotalUnitsTextBox.BackColor = totalUnits > 29 ? Color.LightPink : Color.LightYellow;

                if (totalUnits > 29)
                {
                    MessageBox.Show("Warning: Total units exceed 29!",
                                  "Unit Limit Exceeded",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                }
            }
            else
            {
                TotalUnitsTextBox.BackColor = SystemColors.Window;
            }
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

        private bool HasScheduleConflicts()
        {
            try
            {
                List<SubjectSchedule> allSubjects = new List<SubjectSchedule>();

                // 1. Get existing enrolled subjects from database
                string studentId = IdNumberTextBox.Text.Trim();
                string schoolYear = YearTextBox.Text.Trim();

                string query = @"SELECT s.SSFSTARTTIME, s.SSFENDTIME, s.SSFDays
                        FROM ([EnrollmentDetailFile] AS ed
                        INNER JOIN [EnrollmentHeaderFile] AS eh ON ed.ENRDFSTUDID = eh.ENRHFSTUDID)
                        INNER JOIN [SubjectSchedFile] AS s ON ed.ENRDFSTUDEDPCODE = s.SSFEDPCODE
                        WHERE ed.ENRDFSTUDID = @studentId 
                        AND eh.ENRHFSTUDSCHLYR = @schoolYear
                        AND ed.ENRDFSTUDSTATUS = 'AC'";

                var existingSubjects = ExecuteQuery(query,
                    new OleDbParameter("@studentId", studentId),
                    new OleDbParameter("@schoolYear", schoolYear));

                foreach (DataRow row in existingSubjects.Rows)
                {
                    allSubjects.Add(new SubjectSchedule
                    {
                        StartTime = row["SSFSTARTTIME"] is DateTime ? (DateTime)row["SSFSTARTTIME"] : DateTime.MinValue,
                        EndTime = row["SSFENDTIME"] is DateTime ? (DateTime)row["SSFENDTIME"] : DateTime.MinValue,
                        Days = row["SSFDAYS"].ToString()
                    });
                }

                // 2. Add new subjects from grid
                foreach (DataGridViewRow row in StudentEnrollmentEntryDataGridView.Rows)
                {
                    if (row.IsNewRow) continue;

                    allSubjects.Add(new SubjectSchedule
                    {
                        StartTime = Convert.ToDateTime(row.Cells["StartTimeColumn"].Value),
                        EndTime = Convert.ToDateTime(row.Cells["EndTimeColumn"].Value),
                        Days = row.Cells["DaysColumn"].Value?.ToString()
                    });
                }

                // 3. Check all combinations for conflicts
                for (int i = 0; i < allSubjects.Count; i++)
                {
                    for (int j = i + 1; j < allSubjects.Count; j++)
                    {
                        if (HasConflict(allSubjects[i], allSubjects[j]))
                        {
                            // Highlight conflicts in grid (if they're new subjects)
                            foreach (DataGridViewRow row in StudentEnrollmentEntryDataGridView.Rows)
                            {
                                if (!row.IsNewRow &&
                                    row.Cells["StartTimeColumn"].Value != null &&
                                    Convert.ToDateTime(row.Cells["StartTimeColumn"].Value) == allSubjects[i].StartTime)
                                {
                                    row.DefaultCellStyle.BackColor = Color.LightPink;
                                }
                            }

                            MessageBox.Show($"Schedule conflict detected between:\n" +
                                          $"{allSubjects[i].Days} {allSubjects[i].StartTime:t}-{allSubjects[i].EndTime:t} and\n" +
                                          $"{allSubjects[j].Days} {allSubjects[j].StartTime:t}-{allSubjects[j].EndTime:t}\n\n" +
                                          "Please adjust your enrollment to resolve conflicts.",
                                          "Schedule Conflict",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning);
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking schedule conflicts: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true; // Return true to prevent enrollment if error occurs
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!ValidateEnrollment()) return;

            using (DatabaseConnection db = new DatabaseConnection())
            {
                db.Open();

                try
                {
                    string studentId = IdNumberTextBox.Text.Trim();
                    string schoolYear = YearTextBox.Text.Trim();
                    string encoder = EncodedByTextBox.Text.Trim();
                    double totalUnits = double.Parse(TotalUnitsTextBox.Text);
                    DateTime enrollmentDate = DateTime.Parse(StudentDateEnrollPicker.Text);

                    db.BeginTransaction();

                    // Check if enrollment header exists
                    string checkExistingQuery = @"SELECT COUNT(*) FROM EnrollmentHeaderFile 
                                  WHERE ENRHFSTUDID = @studentId 
                                  AND ENRHFSTUDSCHLYR = @schoolYear";

                    int existingCount = Convert.ToInt32(db.ExecuteScalar(checkExistingQuery,
                        new OleDbParameter("@studentId", studentId),
                        new OleDbParameter("@schoolYear", schoolYear)));

                    if (existingCount == 0)
                    {
                        // Create new enrollment header if doesn't exist
                        string headerQuery = @"INSERT INTO EnrollmentHeaderFile 
                            (ENRHFSTUDID, ENRHFSTUDDATEENROLL, ENRHFSTUDSCHLYR, 
                             ENRHFSTUDENCODER, ENRHFSTUDTOTALUNITS, ENRHFSTUDSTATUS)
                            VALUES (@studentId, @date, @schoolYear, @encoder, @totalUnits, @status)";

                        db.ExecuteNonQuery(headerQuery,
                            new OleDbParameter("@studentId", studentId),
                            new OleDbParameter("@date", enrollmentDate),
                            new OleDbParameter("@schoolYear", schoolYear),
                            new OleDbParameter("@encoder", encoder),
                            new OleDbParameter("@totalUnits", totalUnits),
                            new OleDbParameter("@status", "EN")
                        );
                    }
                    else
                    {
                        // Update existing enrollment header with new total units
                        string updateHeaderQuery = @"UPDATE EnrollmentHeaderFile 
                                   SET ENRHFSTUDTOTALUNITS = ENRHFSTUDTOTALUNITS + @addedUnits
                                   WHERE ENRHFSTUDID = @studentId 
                                   AND ENRHFSTUDSCHLYR = @schoolYear";

                        db.ExecuteNonQuery(updateHeaderQuery,
                            new OleDbParameter("@addedUnits", totalUnits),
                            new OleDbParameter("@studentId", studentId),
                            new OleDbParameter("@schoolYear", schoolYear)
                        );
                    }

                    // Save each subject to EnrollmentDetailFile (only if not already enrolled)
                    foreach (DataGridViewRow row in StudentEnrollmentEntryDataGridView.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string subjectCode = row.Cells["SubjectCodeColumn"].Value?.ToString() ?? "";
                        string edpCode = row.Cells["EDPCodeColumn"].Value?.ToString() ?? "";

                        // Check if already enrolled in this subject
                        string checkSubjectQuery = @"SELECT COUNT(*) FROM EnrollmentDetailFile 
                                     WHERE ENRDFSTUDID = @studentId 
                                     AND ENRDFSTUDSUBJCDE = @subjectCode
                                     AND ENRDFSTUDEDPCODE = @edpCode";

                        int subjectCount = Convert.ToInt32(db.ExecuteScalar(checkSubjectQuery,
                            new OleDbParameter("@studentId", studentId),
                            new OleDbParameter("@subjectCode", subjectCode),
                            new OleDbParameter("@edpCode", edpCode)));

                        if (subjectCount == 0)
                        {
                            string detailQuery = @"INSERT INTO EnrollmentDetailFile 
                                 (ENRDFSTUDID, ENRDFSTUDSUBJCDE, ENRDFSTUDEDPCODE, ENRDFSTUDSTATUS)
                                 VALUES (@studentId, @subjectCode, @edpCode, @status)";

                            db.ExecuteNonQuery(detailQuery,
                                new OleDbParameter("@studentId", studentId),
                                new OleDbParameter("@subjectCode", subjectCode),
                                new OleDbParameter("@edpCode", edpCode),
                                new OleDbParameter("@status", "AC")
                            );

                            // Update class size
                            string updateQuery = @"UPDATE SubjectSchedFile 
                                 SET SSFCLASSSIZE = SSFCLASSSIZE + 1 
                                 WHERE SSFEDPCODE = @edpCode";

                            db.ExecuteNonQuery(updateQuery,
                                new OleDbParameter("@edpCode", edpCode)
                            );
                        }
                    }

                    db.CommitTransaction();
                    MessageBox.Show("Enrollment saved successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                }
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    MessageBox.Show($"Error saving enrollment: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Cannot save enrollment due to schedule conflicts.",
                      "Schedule Conflict",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error);
                return false;
            }

            // Check for duplicate subjects in the grid
            if (HasDuplicateSubjects())
            {
                MessageBox.Show("Cannot save enrollment with duplicate subjects.", "Duplicate Subjects",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check unit limit
            double totalUnits = GetCurrentTotalUnits();
            if (totalUnits > 29)
            {
                MessageBox.Show($"Total units cannot exceed 29.\nCurrent total: {totalUnits}",
                              "Unit Limit Exceeded",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool HasDuplicateSubjects()
        {
            // Create a list to store subject codes
            HashSet<string> subjectCodes = new HashSet<string>();
            HashSet<string> edpCodes = new HashSet<string>();

            foreach (DataGridViewRow row in StudentEnrollmentEntryDataGridView.Rows)
            {
                if (row.IsNewRow) continue;

                string subjectCode = row.Cells["SubjectCodeColumn"].Value?.ToString();
                string edpCode = row.Cells["EDPCodeColumn"].Value?.ToString();

                // Check for duplicate subject codes
                if (!string.IsNullOrEmpty(subjectCode))
                {
                    if (subjectCodes.Contains(subjectCode))
                    {
                        // Highlight the duplicate row
                        row.DefaultCellStyle.BackColor = Color.LightPink;

                        MessageBox.Show($"Duplicate subject found: {subjectCode}",
                            "Duplicate Subject",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return true;
                    }
                    subjectCodes.Add(subjectCode);
                }

                // Check for duplicate EDP codes
                if (!string.IsNullOrEmpty(edpCode))
                {
                    if (edpCodes.Contains(edpCode))
                    {
                        // Highlight the duplicate row
                        row.DefaultCellStyle.BackColor = Color.LightPink;

                        MessageBox.Show($"Duplicate EDP code found: {edpCode}",
                            "Duplicate EDP Code",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return true;
                    }
                    edpCodes.Add(edpCode);
                }
            }

            return false;
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