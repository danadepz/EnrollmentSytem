using System;
using System.Data;
using System.Data.OleDb;

namespace EnrollmentSytem
{
    public class DatabaseConnection : IDisposable
    {
        private static readonly string ConnectionString = @"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\Users\kimpc\source\repos\danadepz\final_EnrollmentSystem\Enrollment_System.accdb;";
        private OleDbConnection connection;
        private OleDbTransaction transaction;

        public DatabaseConnection()
        {
            connection = new OleDbConnection(ConnectionString);
        }

        public void Open()
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
        }

        public void Close()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }

        public void BeginTransaction()
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            transaction = connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            transaction?.Commit();
            transaction = null;
        }

        public void RollbackTransaction()
        {
            transaction?.Rollback();
            transaction = null;
        }

        public DataTable ExecuteQuery(string query, params OleDbParameter[] parameters)
        {
            using (var cmd = new OleDbCommand(query, connection, transaction))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                using (var adapter = new OleDbDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public object ExecuteScalar(string query, params OleDbParameter[] parameters)
        {
            using (var cmd = new OleDbCommand(query, connection, transaction))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                return cmd.ExecuteScalar();
            }
        }

        public int ExecuteNonQuery(string query, params OleDbParameter[] parameters)
        {
            using (var cmd = new OleDbCommand(query, connection, transaction))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                return cmd.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            if (transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }
            if (connection != null)
            {
                connection.Dispose();
                connection = null;
            }
        }
    }
}
