using System;
using System.Collections.Generic;

namespace ProjectPRN221_LIBManagement.Models
{
    public partial class UserBookLog
    {
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public int? TotalBorrows { get; set; }
        public int? TotalReturns { get; set; }
        public DateTime? LastBorrowDate { get; set; }
        public DateTime? LastReturnDate { get; set; }
        public int? LastBookConditionOnReturn { get; set; }
        public int? ViolationCount { get; set; }
        public int? LastTransactionId { get; set; }
        public DateTime? LastUpdated { get; set; }

        public virtual BookCondition? LastBookConditionOnReturnNavigation { get; set; }
        public virtual Transaction? LastTransaction { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
