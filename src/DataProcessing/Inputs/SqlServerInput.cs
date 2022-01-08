using System;
using System.Data;
using System.Data.SqlClient;

namespace DataProcessing.Inputs
{
    public class SqlServerInput : IInput
    {
        private SqlDataReader _reader;

        private bool _header;

        public SqlServerInput(string connectionString, string query)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            
            SqlCommand command = connection.CreateCommand();
            _reader = command.ExecuteReader();
            _header = true;
        }

        public bool HasData()
        {
            if (_header)
                return true;

            return _reader.Read();
        }
        
        public string[] GetData()
        {
            int len = _reader.FieldCount;
            string[] data = new string[len];

            for (int i = 0; i < len; i++) 
            {
                if (_header)
                    data[i] = _reader.GetName(i);
                else
                    data[i] = _reader.GetString(i);
            }

            _header = false;
            return data;
        }

        public void Close()
        {
            _reader.Close();
        }
    }
}
