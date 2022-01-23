using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UploadTransaction_System.Models;
using UploadTransaction_System.Services;

namespace UploadTransaction_System.Controllers
{
    public class TransactionController : Controller
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: Transaction
        public ActionResult UploadTransactionFile()
        {
            log.Info("UploadTransactionFile");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UploadTransactionFile(FileRecordInfo info)
        {

            ResponseMessageInfo response = await TransactionService.SaveTransaction(info);
            if (response.status == true)
            {
                log.Info(response.MessageContent);
                return Json(HttpStatusCode.OK, "success");
            }
            else
                log.Error(response.MessageContent);
                return Json(response);
        }

        public async Task<ActionResult> TransactionList()
        {
            log.Info("TransactionList");
            ViewBag.StatusList = new String[] { "Approved", "Failed", "Rejected", "Done", "Finished" };
            ViewBag.CurrencyList = await TransactionService.GetCurrencyList();
            return View();
        }
    }
}