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
    public partial class StudentEntryForm : Form
    {
        public StudentEntryForm()
        {
            InitializeComponent();
            if (RemarksComboBox.Items.Count > 0)
            {
                RemarksComboBox.SelectedIndex = 0;
            }
        }


        private void StudentEntryLabel_Click(object sender, EventArgs e)
        {

        }

        private void IdNumberTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Connection string (ensure it's correct and points to your database)
            string connStr = @"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\Users\kimpc\Desktop\dePaz_EnrollmentSystem\Enrollment_System.accdb;";

            string sql = "SELECT * FROM StudentFile";

            try
            {
                using (OleDbConnection thisConnection = new OleDbConnection(connStr))
                {
                    // Create a data adapter and command builder
                    OleDbDataAdapter thisAdapter = new OleDbDataAdapter(sql, thisConnection);
                    OleDbCommandBuilder thisBuilder = new OleDbCommandBuilder(thisAdapter);

                    // Create a DataSet and fill it with data from the StudentFile table
                    DataSet thisDataSet = new DataSet();
                    thisAdapter.Fill(thisDataSet, "StudentFile");

                    // Create a new row in the StudentFile table
                    DataRow thisRow = thisDataSet.Tables["StudentFile"].NewRow();
                    thisRow["STFSTUDID"] = Convert.ToInt32(IdNumberTextBox.Text); 
                    thisRow["STFSTUDLNAME"] = LastNameTextBox.Text; 
                    thisRow["STFSTUDFNAME"] = FirstNameTextBox.Text;
                    thisRow["STFSTUDMNAME"] = MiddleNameTextBox.Text;
                    thisRow["STFSTUDCOURSE"] = CourseTextBox.Text;
                    thisRow["STFSTUDYEAR"] = Convert.ToInt16(YearTextBox.Text);
                    thisRow["STFSTUDREMARKS"] = RemarksComboBox.Text;
                    thisRow["STFSTUDSTATUS"] = "AC";

                    // Add the new row to the DataSet
                    thisDataSet.Tables["StudentFile"].Rows.Add(thisRow);

                    // Update the database with the changes in the DataSet
                    thisAdapter.Update(thisDataSet, "StudentFile");

                    // Notify the user of success
                    MessageBox.Show("Entries Recorded!");

                    // Verify if the data was saved
                    string verifyQuery = "SELECT COUNT(*) FROM StudentFile WHERE STFSTUDID = @STFSTUDID";
                    using (OleDbCommand verifyCmd = new OleDbCommand(verifyQuery, thisConnection))
                    {
                        verifyCmd.Parameters.AddWithValue("@STFSTUDID", Convert.ToInt32(IdNumberTextBox.Text));
                        thisConnection.Open();
                        int count = (int)verifyCmd.ExecuteScalar();
                        thisConnection.Close();

                        if (count > 0)
                        {
                            MessageBox.Show("Entries Recorded and Verified!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to verify the saved data.");
                        }
                    }
                    SubjectEntryForm form2 = new SubjectEntryForm();
                    form2.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
            {
                // Iterate through all controls on the form
                foreach (Control control in this.Controls)
                {
                    if (control is TextBox textBox)
                    {
                        textBox.Text = string.Empty;
                    }
                }
            }

    }
}
