using System;
using System.Collections.Generic;

namespace ProjectPRN221_LIBManagement.Models
{
    public partial class Transaction
    {
        public int TransactionId { get; set; }
        public int? UserId { get; set; }
        public int? BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int? Status { get; set; }

        public virtual Book? Book { get; set; }
        public virtual Status? StatusNavigation { get; set; }
        public virtual User? User { get; set; }

        // Thêm thuộc tính cho tháng
        public int Month { get; set; }

        // Thêm thuộc tính cho số lượng mượn và trả
        public int BorrowCount { get; set; }
        public int ReturnCount { get; set; }
    }
}
