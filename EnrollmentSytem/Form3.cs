using System;
using System.Data.OleDb;
using System.Windows.Forms;

string connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\kimpc\Desktop\dePaz_EnrollmentSystem\EnrollentSystem.accdb;";
try
{
    using (OleDbConnection conn = new OleDbConnection(connStr))
    {
        conn.Open();
        MessageBox.Show("Connection successful! 🎉");
    }
}
catch (Exception ex)
{
    MessageBox.Show($"Error: {ex.Message}");
}