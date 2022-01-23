using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace UploadTransaction_System.Models
{
    public class ResponseMessageInfo
    {
        public bool status { get; set; }
        public string MessageContent { get; set; }
    }
    public class FileRecordInfo
    {
        [Required(ErrorMessage ="File is required.")]
        public HttpPostedFileBase TransactionFile { get; set; }
    }

    public class TransactionDataInfo
    {
        public string FileId { get; set; }
        public string TransactionId { get; set; }
        public Nullable<double> Amount { get; set; }
        public string CurrencyCode { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public string Status { get; set; }
    }

    public class CurrencyList
    {
        public string CurrencyCode { get; set; }
    }
   
}