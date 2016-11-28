using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Caracal.Common;
using Caracal.Net;
namespace Caracal.Data
{
    public class DatabaseConnection
    {
        private Server _server;
        private string _databaseName;
        private SqlConnection _connection;
        private LoginInfo _li;
        private IpAddress _ip;
        public DatabaseConnection(Server server, string databaseName)
        {
            _server = server;
            _databaseName = databaseName;
            ConnectNow();
        }
        public DatabaseConnection(LoginInfo li, IpAddress ip, string databaseName)
        {
            _li = li;
            _ip = ip;
            _databaseName = databaseName;
            ConnectNow();
        }
        public string GetConnectionString()
        {
            SqlConnectionStringBuilder csBulider = new SqlConnectionStringBuilder();
            csBulider.DataSource = _ip.ToString(",");
            csBulider.UserID = _li.UserName;
            csBulider.Password = _li.Password.ToString();
            //csBulider.DataSource = _server.ConnectionAddress.ToString(",");
            //csBulider.UserID = _server.LoginInfo.UserName;
            //csBulider.Password = _server.LoginInfo.Password.ToString();
            csBulider.InitialCatalog = _databaseName;
            return csBulider.ConnectionString;
        }
        public bool ConnectNow()
        {
            _connection = new SqlConnection(GetConnectionString());
            switch (_connection.State)
            {
                case System.Data.ConnectionState.Broken:
                case System.Data.ConnectionState.Closed: _connection.Open(); return true;
                default: break;
            }
            return false;
        }
        public bool ConnectNow(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            switch (_connection.State)
            {
                case System.Data.ConnectionState.Broken:
                case System.Data.ConnectionState.Closed: _connection.Open(); return true;
                default: break;
            }
            return false;
        }
        public int ExecuteCommand(string command)
        {
            SqlCommand sCommand = new SqlCommand(command, _connection);
            return sCommand.ExecuteNonQuery();
        }

        public int ExecuteCommand(string command, List<xParameter> parameters)
        {
            SqlCommand sCommand = new SqlCommand(command, _connection);
            sCommand.Parameters.Clear();
            sCommand.Parameters.AddRange(parameters.Select(x => new SqlParameter { ParameterName = x.ParameterName, Value = x.Value, SqlDbType = (SqlDbType)x.DbType }).ToArray());
            return sCommand.ExecuteNonQuery();
        }
        public DataTable GetData(string command)
        {
            SqlDataAdapter sAdapter = new SqlDataAdapter(command, _connection);
            DataSet dSet = new DataSet();
            sAdapter.Fill(dSet);
            if (dSet.Tables.Count > 0)
                return dSet.Tables[0];
            return new DataTable();
        }
        public DataSet GetMultipleData(string command)
        {
            SqlDataAdapter sAdapter = new SqlDataAdapter(command, _connection);
            DataSet dSet = new DataSet();
            sAdapter.Fill(dSet);
            return dSet;
        }
        public bool Disconnect()
        {
            try
            {
                switch (_connection.State)
                {
                    case System.Data.ConnectionState.Open:
                    case System.Data.ConnectionState.Connecting:
                    case System.Data.ConnectionState.Executing:
                    case System.Data.ConnectionState.Fetching: _connection.Close(); return true;
                    default: break;
                }
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
        ~DatabaseConnection()
        {
            Disconnect();
        }
    }
}
