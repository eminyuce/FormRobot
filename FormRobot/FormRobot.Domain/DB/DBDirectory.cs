using EImece.Domain.DbContext;
using EImece.Domain.Helpers;
using FormRobot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormRobot.Domain.DB
{
    public class DBDirectory
    {
        public static string ConnectionStringKey = "TestEY6ConnectionString";


        public static int SaveOrUpdateAirDropLink(AirDropLink item)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
            String commandText = @"SaveOrUpdateAirDropLink";
            var parameterList = new List<SqlParameter>();
            var commandType = CommandType.StoredProcedure;
            parameterList.Add(DatabaseUtility.GetSqlParameter("airdroplinkid", item.AirDropLinkId, SqlDbType.Int));
            parameterList.Add(DatabaseUtility.GetSqlParameter("airdroplinkurl", item.AirDropLinkUrl.ToStr(), SqlDbType.NVarChar));
            int id = DatabaseUtility.ExecuteScalar(new SqlConnection(connectionString), commandText, commandType, parameterList.ToArray()).ToInt();
            return id;
        }

        public static AirDropLink GetAirDropLink(int airDropLinkId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
            String commandText = @"SELECT * FROM AirDropLink WHERE airDropLinkId=@airDropLinkId";
            var parameterList = new List<SqlParameter>();
            var commandType = CommandType.Text;
            parameterList.Add(DatabaseUtility.GetSqlParameter("airDropLinkId", airDropLinkId, SqlDbType.Int));
            DataSet dataSet = DatabaseUtility.ExecuteDataSet(new SqlConnection(connectionString), commandText, commandType, parameterList.ToArray());
            if (dataSet.Tables.Count > 0)
            {
                using (DataTable dt = dataSet.Tables[0])
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var e = GetAirDropLinkFromDataRow(dr);
                        return e;
                    }
                }
            }
            return new AirDropLink();
        }

        public static List<AirDropLink> GetAirDropLinks()
        {
            var list = new List<AirDropLink>();
            String commandText = @"SELECT * FROM AirDropLink ORDER BY AirDropLinkId DESC";
            var parameterList = new List<SqlParameter>();
            string connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
            var commandType = CommandType.Text;
            DataSet dataSet = DatabaseUtility.ExecuteDataSet(new SqlConnection(connectionString), commandText, commandType, parameterList.ToArray());
            if (dataSet.Tables.Count > 0)
            {
                using (DataTable dt = dataSet.Tables[0])
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var e = GetAirDropLinkFromDataRow(dr);
                        list.Add(e);
                    }
                }
            }
            return list;
        }
        public static void DeleteAirDropLink(int airDropLinkId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
            String commandText = @"DELETE FROM AirDropLink WHERE AirDropLinkId=@AirDropLinkId";
            var parameterList = new List<SqlParameter>();
            var commandType = CommandType.Text;
            parameterList.Add(DatabaseUtility.GetSqlParameter("AirDropLinkId", airDropLinkId, SqlDbType.Int));
            DatabaseUtility.ExecuteNonQuery(new SqlConnection(connectionString), commandText, commandType, parameterList.ToArray());
        }

        private static AirDropLink GetAirDropLinkFromDataRow(DataRow dr)
        {
            var item = new AirDropLink();

            item.AirDropLinkId = dr["AirDropLinkId"].ToInt();
            item.AirDropLinkUrl = dr["AirDropLinkUrl"].ToStr();
            return item;
        }



        public static int SaveOrUpdateFormMatch(FormMatch item)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
            String commandText = @"SaveOrUpdateFormMatch";
            var parameterList = new List<SqlParameter>();
            var commandType = CommandType.StoredProcedure;
            parameterList.Add(DatabaseUtility.GetSqlParameter("formmatchid", item.FormMatchId, SqlDbType.Int));
            parameterList.Add(DatabaseUtility.GetSqlParameter("formitemtext", item.FormItemText.ToStr(), SqlDbType.NVarChar));
            parameterList.Add(DatabaseUtility.GetSqlParameter("formitemkey", item.FormItemKey.ToStr(), SqlDbType.NVarChar));
            int id = DatabaseUtility.ExecuteScalar(new SqlConnection(connectionString), commandText, commandType, parameterList.ToArray()).ToInt();
            return id;
        }

        public static FormMatch GetFormMatch(int formMatchId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
            String commandText = @"SELECT * FROM FormMatch WHERE formMatchId=@formMatchId";
            var parameterList = new List<SqlParameter>();
            var commandType = CommandType.Text;
            parameterList.Add(DatabaseUtility.GetSqlParameter("formMatchId", formMatchId, SqlDbType.Int));
            DataSet dataSet = DatabaseUtility.ExecuteDataSet(new SqlConnection(connectionString), commandText, commandType, parameterList.ToArray());
            if (dataSet.Tables.Count > 0)
            {
                using (DataTable dt = dataSet.Tables[0])
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var e = GetFormMatchFromDataRow(dr);
                        return e;
                    }
                }
            }
            return null;
        }

        public static List<FormMatch> GetFormMatchs()
        {
            var list = new List<FormMatch>();
            String commandText = @"SELECT * FROM FormMatch ORDER BY FormMatchId DESC";
            var parameterList = new List<SqlParameter>();
            string connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
            var commandType = CommandType.Text;
            DataSet dataSet = DatabaseUtility.ExecuteDataSet(new SqlConnection(connectionString), commandText, commandType, parameterList.ToArray());
            if (dataSet.Tables.Count > 0)
            {
                using (DataTable dt = dataSet.Tables[0])
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var e = GetFormMatchFromDataRow(dr);
                        list.Add(e);
                    }
                }
            }
            return list;
        }
        public static void DeleteFormMatch(int formMatchId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
            String commandText = @"DELETE FROM FormMatch WHERE FormMatchId=@FormMatchId";
            var parameterList = new List<SqlParameter>();
            var commandType = CommandType.Text;
            parameterList.Add(DatabaseUtility.GetSqlParameter("FormMatchId", formMatchId, SqlDbType.Int));
            DatabaseUtility.ExecuteNonQuery(new SqlConnection(connectionString), commandText, commandType, parameterList.ToArray());
        }

        private static FormMatch GetFormMatchFromDataRow(DataRow dr)
        {
            var item = new FormMatch();

            item.FormMatchId = dr["FormMatchId"].ToInt();
            item.FormItemText = dr["FormItemText"].ToStr();
            item.FormItemKey = dr["FormItemKey"].ToStr();
            return item;
        }


    }
}
