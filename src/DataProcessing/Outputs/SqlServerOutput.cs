using System;
using System.Data;
using System.Data.SqlClient;

namespace DataProcessing.Outputs 
{
    public class SqlServerOutput : IOutput
    {
        private SqlConnection _connection;

        private SqlCommand _command;

        private SqlTransaction _transaction;

        private string[] _parameterIdentifier;

        private int _transactions;

        private int _lote;

        public SqlServerOutput(string connectionString, string query, string[] parameterIdentifier, int lote = 1000)
        {
            _parameterIdentifier = parameterIdentifier;
            _lote = lote;

            _connection = new SqlConnection(connectionString);
            _connection.Open();
            
            _command = _connection.CreateCommand();
            _command.CommandType = CommandType.Text;
            _command.CommandText = query;

            InitTransaction();
        }

        public void Set(string[] data)
        {
            SqlCommand command = _command.Clone();
            for (int i = 0; i < _parameterIdentifier.Length; i++)
                command.Parameters.AddWithValue(_parameterIdentifier[i], data[i]);

            command.ExecuteNonQuery();
            command.Dispose();
            _transactions++;

            if(_transactions >= _lote)
            {
                _transaction.Commit();
                InitTransaction();
            }
        }

        private void InitTransaction()
        {    
            _transaction = _connection.BeginTransaction();
            _command.Transaction = _transaction;
            _transactions = 0;
        }
        
        public void Close()
        {
            _transaction.Commit();
            _connection.Close();
            _command.Dispose();
        }

    }
}