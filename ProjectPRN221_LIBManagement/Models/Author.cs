using System;
using System.Collections.Generic;

namespace ProjectPRN221_LIBManagement.Models
{
    public partial class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; }
    }
}
