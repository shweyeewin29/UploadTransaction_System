using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using UploadTransaction_System.Services;

namespace UploadTransaction_System.Controllers
{
    public class TransactionAPIController : ApiController
    {
        //transaction data
        [HttpGet]
        [ActionName("GetTransactionData")]
        public async Task<IHttpActionResult> GetTransactionData(string fromdate, string todate,string status,string currency)
        {
            DataTable dt = await TransactionService.GetTransactionData(fromdate,todate,status,currency);
            return Json(dt);
        }

        //file rec list
        [HttpGet]
        [ActionName("GetFileRecordList")]
        public async Task<IHttpActionResult> GetFileRecordList(string fromdate, string todate)
        {
            DataTable dt = await TransactionService.GetFileRecordList(fromdate, todate);
            return Json(dt);
        }
    }
}
