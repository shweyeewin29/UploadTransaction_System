//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UploadTransaction_System.DBContext
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class UploadTransactionsDBEntities : DbContext
    {
        public UploadTransactionsDBEntities()
            : base("name=UploadTransactionsDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<FileRecord> FileRecords { get; set; }
        public virtual DbSet<TransactionData> TransactionDatas { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
    }
}
