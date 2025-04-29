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
            string connStr = @"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\Users\kimpc\Desktop\dePaz_EnrollmentSystem\Enrollment_System.accdb;";
            string checkQuery = "SELECT COUNT(*) FROM StudentFile WHERE STFSTUDID = @STFSTUDID";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    conn.Open();
                    using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@STFSTUDID", Convert.ToInt32(IdNumberTextBox.Text));
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("A student with this ID already exists. Please enter a unique ID.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Insert new record if no duplicate is found
                    string sql = "SELECT * FROM StudentFile";
                    using (OleDbDataAdapter thisAdapter = new OleDbDataAdapter(sql, conn))
                    {
                        OleDbCommandBuilder thisBuilder = new OleDbCommandBuilder(thisAdapter);

                        DataSet thisDataSet = new DataSet();
                        thisAdapter.Fill(thisDataSet, "StudentFile");

                        DataRow thisRow = thisDataSet.Tables["StudentFile"].NewRow();
                        thisRow["STFSTUDID"] = Convert.ToInt32(IdNumberTextBox.Text);
                        thisRow["STFSTUDLNAME"] = LastNameTextBox.Text;
                        thisRow["STFSTUDFNAME"] = FirstNameTextBox.Text;
                        thisRow["STFSTUDMNAME"] = MiddleNameTextBox.Text;
                        thisRow["STFSTUDCOURSE"] = CourseTextBox.Text;
                        thisRow["STFSTUDYEAR"] = Convert.ToInt16(YearTextBox.Text);
                        thisRow["STFSTUDREMARKS"] = RemarksComboBox.Text;
                        thisRow["STFSTUDSTATUS"] = "AC";

                        thisDataSet.Tables["StudentFile"].Rows.Add(thisRow);
                        thisAdapter.Update(thisDataSet, "StudentFile");

                        MessageBox.Show("Entries Recorded!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private void CancelButton_Click(object sender, EventArgs e)
        {
            ClearControls(this);
            IdNumberTextBox.Focus();
        }

        private void ClearControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.Text = string.Empty;
                }
                else if (control is ComboBox comboBox)
                {
                    if (comboBox.Items.Count > 0)
                    {
                        comboBox.SelectedIndex = 0;
                    }
                }
                else if (control is Panel || control is GroupBox || control is TabPage)
                {
                    ClearControls(control);
                }
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
