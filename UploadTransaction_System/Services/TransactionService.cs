using log4net;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using UploadTransaction_System.DBContext;
using UploadTransaction_System.Models;

namespace UploadTransaction_System.Services
{
    public class TransactionService
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string ConStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        
        public static DateTime _RangonTime()
        {
            DateTime serverTime = DateTime.Now;
            DateTime _YangonTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(serverTime, TimeZoneInfo.Local.Id, "Myanmar Standard Time");
            return _YangonTime;
        }

        #region master 23Jan
        public static Task<List<CurrencyList>> GetCurrencyList()
        {
            return Task.Run(() =>
            {
                DataTable dt = new DataTable();
                string sql = @"select * from Currency";
                SqlDataAdapter adpt = new SqlDataAdapter(sql, ConStr);
                adpt.Fill(dt);
                List<CurrencyList> cList = new List<CurrencyList>();
                foreach (DataRow r in dt.Rows)
                {
                    cList.Add(new CurrencyList { CurrencyCode = r["CurrencyCode"].ToString()});
                }
                return cList;
            });

        }
        #endregion

        #region Save Transaction 23Jan
        public static async Task<ResponseMessageInfo> SaveTransaction(FileRecordInfo info)
        {
            ResponseMessageInfo response = new ResponseMessageInfo();
            string ext = string.Empty;
            try
            {
                using (UploadTransactionsDBEntities db = new UploadTransactionsDBEntities())
                {
                    ext = System.IO.Path.GetExtension(info.TransactionFile.FileName);

                    FileRecord newfile = new FileRecord();
                    string currentDate = _RangonTime().ToString("ddMMyyyy");
                    string FileID = currentDate + "01";
                    IList<FileRecord> existtransaction = await (db.FileRecords.SqlQuery("select * from FileRecord Where FileId like '"+currentDate+"%'").ToListAsync());
                    if (existtransaction.Count > 0)
                    {
                        FileID = (long.Parse(existtransaction.Max(m => m.FileId)) + 1).ToString("D10");
                    }

                    //file record tbl
                    newfile.FileId = FileID;
                    newfile.FileName = info.TransactionFile.FileName;
                    newfile.CreatedDate = _RangonTime();
                    //newtransaction.CreatedUser= System.Web.HttpContext.Current.User.Identity.Name;
                    newfile.CreatedIPAddress = HttpContext.Current.Request.UserHostAddress;
                    db.FileRecords.Add(newfile);

                    List<TransactionData> tdlist = new List<TransactionData>();
                    List<TransactionDataInfo> invalidtdlist = new List<TransactionDataInfo>();

                    DataTable dt = new DataTable();
                    DataTable transTable = getTransactionTable();
                    if (ext == ".xml")
                    {
                        transTable.Clear();
                        string xmlpath = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + info.TransactionFile.FileName;
                        info.TransactionFile.SaveAs(xmlpath);
                        XmlDocument doc = new XmlDocument();
                        doc.Load(xmlpath);

                        XmlNode node = doc.DocumentElement.SelectSingleNode("/Transactions");
                        foreach (XmlNode NodeXml in node)
                        {
                            DataRow dtrow = transTable.NewRow();
                            XmlElement root = (XmlElement)NodeXml;
                            if (root.HasAttribute("id"))
                            {
                                string transactionid = root.GetAttribute("id");
                                dtrow["TransactionId"] = transactionid;
                            }
                            
                            dtrow["TransactionDate"] = root.GetElementsByTagName("TransactionDate")[0].InnerText;
                            XmlNode AddrNode = node.SelectSingleNode("/Transactions/Transaction/PaymentDetails");
                            for (int i = 0; i < AddrNode.ChildNodes.Count; i++)
                            {
                                if (AddrNode.ChildNodes[i].Name == "Amount")
                                {
                                    dtrow["Amount"] = AddrNode["Amount"].InnerText;
                                }
                                else
                                {
                                    dtrow["CurrencyCode"] = AddrNode["CurrencyCode"].InnerText;
                                }
                            }
                            dtrow["Status"] = root.GetElementsByTagName("Status")[0].InnerText;

                            transTable.Rows.Add(dtrow);
                        }
                        if (xmlpath != null)
                        {
                            System.IO.File.Delete(xmlpath);
                        }
                    }
                    else
                    {
                        transTable.Clear();
                        Stream stream = info.TransactionFile.InputStream;
                        using (CsvReader csvReader = new CsvReader(new StreamReader(stream), true))
                        {
                            dt.Load(csvReader);
                            
                        }
                        if (dt.Rows.Count > 0 && (dt.Columns[0].ColumnName.ToString() == "TransactionId" && dt.Columns[1].ColumnName.ToString() == "Amount" && dt.Columns[2].ColumnName.ToString() == "Currency" && dt.Columns[3].ColumnName.ToString() == "Transaction Date" && dt.Columns[4].ColumnName.ToString() == "Status"))
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                transTable.Rows.Add(dr.ItemArray);
                            }
                        }
                        else
                        {
                            response.status = false;
                            response.MessageContent = "File should not be imported.File Format is wrong!";
                            return response;
                        }
                        
                    }
                    
                    
                    //Transaction Data
                    foreach (DataRow dr in transTable.Rows)
                    {
                        int invalidcount = 0;
                        foreach (DataColumn column in transTable.Columns)
                        {
                            if (column.ColumnName == "Status" && dr[column] != null && dr[column].ToString() != "")
                            {
                                string[] statuslist = { "Approved", "Rejected", "Failed", "Finished", "Done" };
                                bool has = statuslist.Contains(dr[column].ToString());
                                if (!has)
                                {
                                    invalidcount++;
                                    break;
                                }
                            }
                            else if (dr[column] == null || dr[column].ToString() == "")
                            {
                                invalidcount++;
                                break;
                            }
                        }
                        if (invalidcount > 0)
                        {
                            //invalid data to collect in log
                            TransactionDataInfo invaliddata = new TransactionDataInfo();
                            invaliddata.TransactionId = Convert.ToString(dr["TransactionId"]);
                            if(dr["Amount"] != null && dr["Amount"].ToString() != "")
                            {
                                invaliddata.Amount = Convert.ToDouble(dr["Amount"]);
                            }
                            invaliddata.CurrencyCode = Convert.ToString(dr["CurrencyCode"]);
                            if (dr["TransactionDate"] != null && dr["TransactionDate"].ToString() != "")
                            {
                                invaliddata.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                            }
                            invaliddata.Status = Convert.ToString(dr["Status"]);
                            invalidtdlist.Add(invaliddata);
                        }
                        else
                        {
                            //add valid data to list
                            TransactionData newdata = new TransactionData();
                            newdata.FileId = newfile.FileId;
                            newdata.TransactionId = Convert.ToString(dr["TransactionId"]);
                            newdata.Amount = Convert.ToDouble(dr["Amount"]);
                            newdata.CurrencyCode = Convert.ToString(dr["CurrencyCode"]);
                            if (dr["TransactionDate"] != null && dr["TransactionDate"].ToString() != "")
                            {
                                newdata.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                            }
                            newdata.Status = Convert.ToString(dr["Status"]);

                            tdlist.Add(newdata);
                        }
                    }
                    if(invalidtdlist.Count>0)
                    {
                        //collect log into db
                        LoggingService.SaveInvalidTrasaction(invalidtdlist);
                    }
                    if (invalidtdlist.Count == 0 && tdlist.Count > 0)
                    {
                        //submit valid data to db
                        db.TransactionDatas.AddRange(tdlist);
                        await db.SaveChangesAsync();
                        response.status = true;
                    }
                    else
                    {
                        response.status = false;
                        response.MessageContent = "File should not be imported.Any Records is invalid!";
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                #region
                foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                {
                    // Get entry

                    DbEntityEntry entry = item.Entry;
                    string entityTypeName = entry.Entity.GetType().Name;

                    // Display or log error messages

                    foreach (DbValidationError subItem in item.ValidationErrors)
                    {
                        string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                 subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                        response.MessageContent += message;
                    }
                }
                response.status = false;
                #endregion
            }
            catch (DbUpdateException e)
            {
                foreach (var error in e.Entries)
                {
                    response.MessageContent += error.Entity.GetType().Name;
                }
                response.status = false;
            }
            catch (Exception e)
            {
                response.MessageContent += e.Message;
                response.status = false;
            }
            log4net.ThreadContext.Properties["User"] = HttpContext.Current.User.Identity.Name;
            if (response.status == false)
                log.Error("SaveTransaction: " + response.MessageContent);
            else
                log.Info("SaveTransaction: " + "upload file");
            return response;
        }

        public static DataTable getTransactionTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[5] {
                    new DataColumn("TransactionId",typeof(string)),
                    new DataColumn("Amount", typeof(double)),
                    new DataColumn("CurrencyCode", typeof(string)),
                    new DataColumn("TransactionDate", typeof(DateTime)),
                    new DataColumn("Status", typeof(string)),
                    });
            return dt;
        }
        #endregion

        #region get transaction data 24Jan
        public static Task<DataTable> GetTransactionData(string fromdate,string todate,string status,string currency)
        {
            return Task.Run(() =>
            {
                DataTable dt = new DataTable();
                string cond = string.Empty;
                if(status != null)
                {
                    cond += @" and td.Status=@status ";
                }
                if(currency != null)
                {
                    cond += @" and td.CurrencyCode=@currency ";
                }
                string sql = @"select ROW_NUMBER() OVER(
                               ORDER BY td.TransactionId) AS RowNo,td.TransactionId,td.Amount,td.CurrencyCode,td2.Status from TransactionData td inner join
                               (select FileID,TransactionId,'A' as Status from TransactionData where Status in ('Approved')
                               union all
                               select FileID,TransactionId,'R' as Status from TransactionData where Status in ('Failed','Rejected')
                               union all
                               select FileID,TransactionId,'D' as Status from TransactionData where Status in ('Done','Finished')) 
                               as td2 on (td.TransactionId=td2.TransactionId and td.FileId=td2.FileId) where CAST(td.TransactionDate as date) between @fromdate and @todate " + cond;
                SqlDataAdapter adpt = new SqlDataAdapter(sql, ConStr);
                adpt.SelectCommand.Parameters.AddWithValue("@fromdate", fromdate);
                adpt.SelectCommand.Parameters.AddWithValue("@todate", todate);
                if(status != null)
                {
                    adpt.SelectCommand.Parameters.AddWithValue("@status", status);
                }
                if(currency != null)
                {
                    adpt.SelectCommand.Parameters.AddWithValue("@currency", currency);
                }
                adpt.Fill(dt);
                return dt;
            });
           
        }
        #endregion

        #region get record list 25Jan
        public static Task<DataTable> GetFileRecordList(string fromdate, string todate)
        {
            return Task.Run(() =>
            {
                DataTable dt = new DataTable();
                
                string sql = @"select FileId,FileName,CreatedDate,CreatedIPAddress from FileRecord where CAST(CreatedDate as date) between @fromdate and @todate ";
                SqlDataAdapter adpt = new SqlDataAdapter(sql, ConStr);
                adpt.SelectCommand.Parameters.AddWithValue("@fromdate", fromdate);
                adpt.SelectCommand.Parameters.AddWithValue("@todate", todate);
                
                adpt.Fill(dt);
                return dt;
            });

        }
        #endregion
    }
}