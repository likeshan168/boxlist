using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace E_Speed.CommonLib
{
    public class DBAccess
    {
        private string connectionName = "ConnectionString";

        private int commandTimeout = 0;

        public DBAccess()
        {
        }

        public DBAccess(string connectionName)
        {
            ConnectionName = connectionName;
        }

        public string ConnectionName
        {
            get
            {
                return connectionName;
            }
            set
            {
                connectionName = value;
            }
        }

        public int CommandTimeout
        {
            get
            {
                if (commandTimeout == 0)
                {
                    //if (!int.TryParse(ConfigHelper.Instance.GetAppSettingsConfigValue("辉瑞易速.exe.Config", "CommandTimeout"), out commandTimeout))
                    //{
                    //    commandTimeout = 30;
                    //}
                    commandTimeout = 30;

                    return commandTimeout;
                }

                return commandTimeout;
            }
        }

        public string ConnectionString { get; set; }
        /// <summary>
        /// 获取连接字符串中的ProviderName
        /// </summary>
        /// <param name="connectionName">连接字符串名称</param>
        /// <returns></returns>
        public string GetProviderName(string connectionName)
        {
            if (connectionName == "ODBC通用数据库")
            {
                return "System.Data.Odbc";
            }

            ConnectionStringSettingsCollection ConfigStringCollention = ConfigurationManager.ConnectionStrings;
            if (ConfigStringCollention == null || ConfigStringCollention.Count <= 0)
            {
                throw new Exception("App.config 中无连接字符串!");
            }
            ConnectionStringSettings StringSettings = null;
            if (connectionName == string.Empty)
            {
                //StringSettings = ConfigHelper.Instance.GetConnectionStringsSettings("辉瑞易速.exe.Config", "ConnectionString");
                StringSettings = ConfigurationManager.ConnectionStrings["ConnectionString"];
            }
            else
            {
                StringSettings = ConfigurationManager.ConnectionStrings[connectionName];
               // StringSettings = ConfigHelper.Instance.GetConnectionStringsSettings("辉瑞易速.exe.Config", connectionName);
            }
            return StringSettings.ProviderName;
        }

        public string GetProviderName()
        {
            return GetProviderName(ConnectionName);
        }

        /// <summary>
        /// 根据连接字符串名称，获取连接字符串
        /// 如果名称为空，则用ConnectionString
        /// 作为默认的名称
        /// </summary>
        /// <param name="connectionName">连接字符串名称</param>
        /// <returns></returns>
        public string GetConnectionString(string connectionName)
        {
            ConnectionStringSettingsCollection ConfigStringCollention = ConfigurationManager.ConnectionStrings;
            if (ConfigStringCollention == null || ConfigStringCollention.Count <= 0)
            {
                throw new Exception("App.config 中无连接字符串!");
            }
            ConnectionStringSettings StringSettings = null;
            if (connectionName == string.Empty)
            {
                StringSettings = ConfigurationManager.ConnectionStrings["ConnectionString"];
            }
            else
            {
                StringSettings = ConfigurationManager.ConnectionStrings[connectionName];
            }
            return StringSettings.ConnectionString;

        }

        public string GetConnectionString()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
                return GetConnectionString(ConnectionName);
            return ConnectionString;
        }

        public DbProviderFactory GetDbProviderFactory()
        {
            DbProviderFactory f = null;
            string ProviderName = GetProviderName();
            switch (ProviderName)
            {
                case "System.Data.SqlClient"://sql server
                    f = GetDbProviderFactory("System.Data.SqlClient");
                    break;
                case "Oracle.ManagedDataAccess.Client"://oracle
                    f = GetDbProviderFactory("Oracle.ManagedDataAccess.Client");
                    break;
                case "System.Data.OleDb"://Access
                    f = GetDbProviderFactory("System.Data.OleDb");
                    break;
                case "System.Data.Odbc"://ODBC: sybase and some other db
                    f = GetDbProviderFactory("System.Data.Odbc");
                    break;
                case "MySql.Data.MySqlClient"://mysql
                    f = GetDbProviderFactory("MySql.Data.MySqlClient");
                    break;
                case "System.Data.SQLite"://Sqlite
                    f = GetDbProviderFactory("System.Data.SQLite");
                    break;
                default:
                    f = GetDbProviderFactory("System.Data.SqlClient");
                    break;
            }
            return f;
        }

        public DbProviderFactory GetDbProviderFactory(string providerName)
        {
            return DbProviderFactories.GetFactory(providerName);
        }

        public DbConnection CreateConnection()
        {
            DbConnection con = GetDbProviderFactory().CreateConnection();
            con.ConnectionString = GetConnectionString();
            return con;
        }

        public DbConnection CreateConnection(string provdername)
        {
            DbConnection con = GetDbProviderFactory(provdername).CreateConnection();
            con.ConnectionString = GetConnectionString();

            return con;
        }

        /// <summary>
        /// test the database connection 
        /// </summary>
        /// <param name="connectionName">the ConnectionString name in App.config</param>
        /// <returns></returns>
        public bool CheckConnection(string connectionName)
        {
            ConnectionName = connectionName;

            using (DbConnection con = GetDbProviderFactory().CreateConnection())
            {
                con.ConnectionString = GetConnectionString();
                con.Open();
                return true;
            }
        }

        public DbCommand CreateCommand(string sql, CommandType cmdType, DbParameter[] parameters)
        {
            DbCommand command = GetDbProviderFactory().CreateCommand();
            command.Connection = CreateConnection();
            command.CommandText = sql;
            command.CommandType = cmdType;
            command.CommandTimeout = CommandTimeout;

            if (parameters != null && parameters.Length > 0)
            {
                foreach (DbParameter param in parameters)
                {
                    command.Parameters.Add(param);
                }
            }
            return command;
        }


        public DbCommand CreateCommand(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            return CreateCommand(sql, CommandType.Text, parameters);
        }

        public DbCommand CreateCommand(string sql, CommandType cmdType)
        {
            DbParameter[] parameters = new DbParameter[0];
            return CreateCommand(sql, cmdType, parameters);
        }

        public DbCommand CreateCommand(string sql, DbParameter[] parameters)
        {
            return CreateCommand(sql, CommandType.Text, parameters);
        }

        public DbDataAdapter CreateAdapter(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            return CreateAdapter(sql, CommandType.Text, parameters);
        }


        public DbDataAdapter CreateAdapter(string sql, CommandType cmdType)
        {
            DbParameter[] parameters = new DbParameter[0];
            return CreateAdapter(sql, cmdType, parameters);
        }

        public DbDataAdapter CreateAdapter(string sql, CommandType cmdType, DbParameter[] parameters)
        {
            DbConnection connection = CreateConnection();
            DbCommand command = GetDbProviderFactory().CreateCommand();
            command.Connection = connection;
            command.CommandText = sql;
            command.CommandType = cmdType;
            command.CommandTimeout = CommandTimeout;
            if (parameters != null && parameters.Length > 0)
            {
                foreach (DbParameter param in parameters)
                {
                    command.Parameters.Add(param);
                }
            }
            DbDataAdapter da = GetDbProviderFactory().CreateDataAdapter();
            da.SelectCommand = command;

            return da;
        }


        public DbParameter CreateParameter(string field, string dbType, string value)
        {
            DbParameter p = GetDbProviderFactory().CreateParameter();
            p.ParameterName = field;
            p.Value = value;
            return p;
        }

        public DbParameter CreateParameter(string field, DbType dbType, object value)
        {
            DbParameter p = GetDbProviderFactory().CreateParameter();
            p.ParameterName = field;
            p.DbType = dbType;
            p.Value = value;
            return p;
        }

        public int ExecuteCommand(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            return ExecuteCommand(sql, CommandType.Text, parameters);
        }


        public int ExecuteCommand(string sql, CommandType cmdType)
        {
            DbParameter[] parameters = new DbParameter[0];
            return ExecuteCommand(sql, CommandType.Text, parameters);
        }


        public int ExecuteCommand(string sql, DbParameter[] parameters)
        {
            return ExecuteCommand(sql, CommandType.Text, parameters);
        }


        public bool ExecuteCommand(List<string> SqlList)
        {
            DbConnection con = CreateConnection();
            con.Open();
            bool iserror = false;
            string strerror = "";
            DbTransaction SqlTran = con.BeginTransaction();
            try
            {
                for (int i = 0; i < SqlList.Count; i++)
                {
                    DbCommand command = GetDbProviderFactory().CreateCommand();
                    command.Connection = con;
                    command.CommandText = SqlList[i].ToString();
                    command.CommandTimeout = CommandTimeout;
                    command.Transaction = SqlTran;
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                iserror = true;
                strerror = ex.Message;

            }
            finally
            {

                if (iserror)
                {
                    SqlTran.Rollback();
                    throw new Exception(strerror);
                }
                else
                {
                    SqlTran.Commit();
                }
                con.Close();
            }
            if (iserror)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int ExecuteCommand(string sql, CommandType cmdType, DbParameter[] parameters)
        {
            int result = 0;
            DbCommand command = CreateCommand(sql, cmdType, parameters);
            DbTransaction trans;
            command.Connection.Open();
            trans = command.Connection.BeginTransaction();
            command.Transaction = trans;
            try
            {
                result = command.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                command.Connection.Close();
            }
            return result;
        }

        public object ExecuteScalar(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            return ExecuteScalar(sql, CommandType.Text, parameters);
        }

        public object ExecuteScalar(string sql, CommandType cmdType)
        {
            DbParameter[] parameters = new DbParameter[0];
            return ExecuteScalar(sql, CommandType.Text, parameters);
        }

        public object ExecuteScalar(string sql, DbParameter[] parameters)
        {
            return ExecuteScalar(sql, CommandType.Text, parameters);
        }

        public object ExecuteScalar(string sql, CommandType cmdType, DbParameter[] parameters)
        {
            object result = null;
            DbCommand command = CreateCommand(sql, cmdType, parameters);
            try
            {
                command.Connection.Open();
                result = command.ExecuteScalar();
            }
            catch
            {
                throw;
            }
            finally
            {
                command.Connection.Close();
            }
            return result;
        }

        public DbDataReader ExecuteReader(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            return ExecuteReader(sql, CommandType.Text, parameters);
        }

        public DbDataReader ExecuteReader(string sql, CommandType cmdType)
        {
            DbParameter[] parameters = new DbParameter[0];
            return ExecuteReader(sql, CommandType.Text, parameters);
        }

        public DbDataReader ExecuteReader(string sql, DbParameter[] parameters)
        {
            return ExecuteReader(sql, CommandType.Text, parameters);
        }

        public DbDataReader ExecuteReader(string sql, CommandType cmdType, DbParameter[] parameters)
        {
            DbDataReader result;
            DbCommand command = CreateCommand(sql, cmdType, parameters);
            try
            {
                command.Connection.Open();
                result = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                throw;
            }
            finally
            {

            }
            return result;
        }

        public DataSet GetDataSet(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            return GetDataSet(sql, CommandType.Text, parameters);
        }

        public virtual DataSet GetDataSet(string sql, CommandType cmdType)
        {
            DbParameter[] parameters = new DbParameter[0];
            return GetDataSet(sql, CommandType.Text, parameters);
        }

        public virtual DataSet GetDataSet(string sql, DbParameter[] parameters)
        {
            return GetDataSet(sql, CommandType.Text, parameters);
        }

        public virtual DataSet GetDataSet(string sql, CommandType cmdType, DbParameter[] parameters)
        {
            DataSet result = new DataSet();
            IDataAdapter dataAdapter = CreateAdapter(sql, cmdType, parameters);
            try
            {
                dataAdapter.Fill(result);
            }
            catch
            {
                throw;
            }
            finally
            {
            }
            return result;
        }

        public virtual DataSet GetDataSet(string sql, int startIndex, int recordCount)
        {
            return GetDataSet(sql, startIndex, recordCount);
        }

        public DataView GetDataView(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            DataView dv = GetDataSet(sql, CommandType.Text, parameters).Tables[0].DefaultView;
            return dv;
        }

        public DataView GetDataView(string sql, CommandType cmdType)
        {
            DbParameter[] parameters = new DbParameter[0];
            DataView dv = GetDataSet(sql, cmdType, parameters).Tables[0].DefaultView;
            return dv;
        }

        public DataView GetDataView(string sql, DbParameter[] parameters)
        {

            DataView dv = GetDataSet(sql, CommandType.Text, parameters).Tables[0].DefaultView;
            return dv;
        }

        public DataView GetDataView(string sql, CommandType cmdType, DbParameter[] parameters)
        {
            DataView dv = GetDataSet(sql, cmdType, parameters).Tables[0].DefaultView;
            return dv;
        }

        public DataView GetDataView(string sql, int startIndex, int recordCount)
        {
            return GetDataSet(sql, startIndex, recordCount).Tables[0].DefaultView;
        }

        public DataTable GetDataTable(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            DataTable dt = GetDataSet(sql, CommandType.Text, parameters).Tables[0];
            return dt;
        }

        public DataTable GetDataTable(string sql, CommandType cmdType)
        {
            DbParameter[] parameters = new DbParameter[0];
            DataTable dt = GetDataSet(sql, cmdType, parameters).Tables[0];
            return dt;
        }

        public DataTable GetDataTable(string sql, DbParameter[] parameters)
        {

            DataTable dt = GetDataSet(sql, CommandType.Text, parameters).Tables[0];
            return dt;
        }

        public DataTable GetDataTable(string sql, CommandType cmdType, DbParameter[] parameters)
        {
            DataTable dt = GetDataSet(sql, cmdType, parameters).Tables[0];
            return dt;
        }

        public DataTable GetDataTable(string sql, int startIndex, int recordCount)
        {
            return GetDataSet(sql, startIndex, recordCount).Tables[0];
        }

        public DataTable GetDataTable(string sql, int sizeCount)
        {
            DataTable dt = GetDataSet(sql).Tables[0];
            int b = sizeCount - dt.Rows.Count;
            if (dt.Rows.Count < sizeCount)
            {
                for (int i = 0; i < b; i++)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
    }
}
