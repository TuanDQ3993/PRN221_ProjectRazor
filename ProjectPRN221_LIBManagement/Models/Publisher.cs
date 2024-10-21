using System;
using System.Collections.Generic;

namespace ProjectPRN221_LIBManagement.Models
{
    public partial class Publisher
    {
        public Publisher()
        {
            Books = new HashSet<Book>();
        }

        public int PublisherId { get; set; }
        public string PublisherName { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; }
    }
}
