﻿using System;
using System.Collections.Generic;

namespace ProjectPRN221_LIBManagement.Models
{
    public partial class Book
    {
        public Book()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public int? AuthorId { get; set; }
        public int? PublisherId { get; set; }
        public int? YearPublished { get; set; }
        public string? Isbn { get; set; }
        public int? CategoryId { get; set; }
        public int? Quantity { get; set; }

        public virtual Author? Author { get; set; }
        public virtual Category? Category { get; set; }
        public virtual Publisher? Publisher { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
