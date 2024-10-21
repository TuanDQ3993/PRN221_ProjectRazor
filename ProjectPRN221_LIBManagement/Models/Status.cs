using System;
using System.Collections.Generic;

namespace ProjectPRN221_LIBManagement.Models
{
    public partial class Status
    {
        public Status()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int StatusId { get; set; }
        public string StatusName { get; set; } = null!;

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
