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
        }

        public int ConditionId { get; set; }
        public string ConditionName { get; set; } = null!;

        public virtual ICollection<Transaction> TransactionBookConditionOnBorrowNavigations { get; set; }
        public virtual ICollection<Transaction> TransactionBookConditionOnReturnNavigations { get; set; }
    }
}
