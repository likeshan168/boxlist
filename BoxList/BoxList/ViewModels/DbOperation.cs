using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using BoxList.Models;

namespace BoxList.ViewModels
{
    public class DbOperation
    {
        public static readonly string ConnectionStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;

        public IDbConnection GetSqlConnection()
        {
            return new OleDbConnection(ConnectionStr);
        }

        public PrintLabelEnhanced GetPrintLabel(bool isCheckIn, string code)
        {
            string sql = string.Empty;
            if (isCheckIn)
            {
                 sql = "select * from [Sheet1$]";
                //sql = "select 货号,英文名称,英文颜色,鞋图名称,条形码,EU,AUS,USA,CM,UK from [Sheet1$] where 条形码=@条形码";
            }
            else
            {
                sql = "select 货号,中文名称,颜色,鞋图名称,京东码,EU,AUS,USA,CM,UK from [Sheet1$] where 条形码=@barCode";
            }

            using (var conn = GetSqlConnection())
            {
                var list = conn.Query<PrintLabelEnhanced>(sql, new
                {
                    条形码 = code
                }).ToList();

                return list.FirstOrDefault();
            }
        }
    }
}
