using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using UploadTransaction_System.Models;

namespace UploadTransaction_System.Services
{
    public class LoggingService
    {
        private static string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static void SaveInvalidTrasaction(List<TransactionDataInfo> invtdlist)
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                foreach(var each in invtdlist)
                {
                    //Build the insertion query
                    SqlCommand insertCommand = new SqlCommand(@"Insert into InvalidTransaction_Log (TransactionId,Amount,Currency,TransactionDate,Status,CreatedDate,CreatedIPAddress) values(@transactionId,@amount,@currency,@transactiondate,@status,@createddate,@clientIp)", conn);

                    insertCommand.Parameters.Add(new SqlParameter("@transactionId", each.TransactionId));
                    if(each.Amount != null)
                    {
                        insertCommand.Parameters.Add(new SqlParameter("@amount", each.Amount));
                    }
                    else
                        insertCommand.Parameters.Add(new SqlParameter("@amount", ""));

                    insertCommand.Parameters.Add(new SqlParameter("@currency", each.CurrencyCode));
                    if(each.TransactionDate != null)
                    {
                        insertCommand.Parameters.Add(new SqlParameter("@transactiondate", each.TransactionDate));
                    }
                    else
                        insertCommand.Parameters.Add(new SqlParameter("@transactiondate", ""));

                    insertCommand.Parameters.Add(new SqlParameter("@status", each.Status));
                    insertCommand.Parameters.Add(new SqlParameter("@createddate", DateTime.Now));
                    insertCommand.Parameters.Add(new SqlParameter("@clientIp", HttpContext.Current.Request.UserHostAddress));

                    int row = insertCommand.ExecuteNonQuery();

                }

                conn.Close();
            }
        }

    }
}