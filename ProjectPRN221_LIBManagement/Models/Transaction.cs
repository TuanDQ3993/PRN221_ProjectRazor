using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPRN221_LIBManagement.Models
{
    public partial class Transaction
    {
        public Transaction()
        {
            UserBookLogs = new HashSet<UserBookLog>();
        }

        public int TransactionId { get; set; }
        public int? UserId { get; set; }
        public int? BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int? Status { get; set; }
        public int BookConditionOnBorrow { get; set; }
        public int? BookConditionOnReturn { get; set; }
        // Thêm thuộc tính cho tháng
        [NotMapped]
        public int Month { get; set; }

        // Thêm thuộc tính cho số lượng mượn và trả
        [NotMapped]

        public int BorrowCount { get; set; }
        [NotMapped]

        public int ReturnCount { get; set; }

        [NotMapped]
        public int StatusCount { get; set; }
        public virtual Book? Book { get; set; }
        public virtual BookCondition BookConditionOnBorrowNavigation { get; set; } = null!;
        public virtual BookCondition? BookConditionOnReturnNavigation { get; set; }
        public virtual Status? StatusNavigation { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<UserBookLog> UserBookLogs { get; set; }
    }
}
