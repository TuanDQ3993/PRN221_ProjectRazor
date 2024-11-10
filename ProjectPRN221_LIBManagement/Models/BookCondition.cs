using System;
using System.Collections.Generic;

namespace ProjectPRN221_LIBManagement.Models
{
    public partial class BookCondition
    {
        public BookCondition()
        {
            TransactionBookConditionOnBorrowNavigations = new HashSet<Transaction>();
            TransactionBookConditionOnReturnNavigations = new HashSet<Transaction>();
            UserBookLogs = new HashSet<UserBookLog>();
        }

        public int ConditionId { get; set; }
        public string ConditionName { get; set; } = null!;

        public virtual ICollection<Transaction> TransactionBookConditionOnBorrowNavigations { get; set; }
        public virtual ICollection<Transaction> TransactionBookConditionOnReturnNavigations { get; set; }
        public virtual ICollection<UserBookLog> UserBookLogs { get; set; }
    }
}
