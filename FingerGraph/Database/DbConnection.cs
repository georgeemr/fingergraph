using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace FingerGraph.Database
{

    internal class DbConnection : IDisposable
    {
        string MyConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

        private readonly MySqlConnection connection;
        private bool IsConnected { get; set; }
        public DbConnection()
        {
            try
            {
                connection = new MySqlConnection(MyConnectionString);

            }
            catch (Exception e)
            {
                Console.WriteLine("Can't create connection! " + e.Message);

            }
        }

        public bool Connect()
        {
            try
            {
                connection.Open();
                IsConnected = true;
                Console.WriteLine("Connection established.");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Close()
        {
            if (!IsConnected) return;
            connection.Close();
            Console.WriteLine("Connection closed.");
            IsConnected = false;
        }


        public void SendQueryAndClose(string query)
        {
            SendQuery(query).Close();
        }

        public void SendQueryAndClose(string query, MySqlParameter param)
        {
            try
            {
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.Add(param);
                command.ExecuteReader().Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public MySqlDataReader SendQuery(string query)
        {
            try
            {
                if (!IsConnected) return null;
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                return reader;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }


        }

        public void Dispose()
        {
            Close();
        }
    }
}
