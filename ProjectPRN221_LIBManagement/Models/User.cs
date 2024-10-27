using System;
using System.Collections.Generic;

namespace ProjectPRN221_LIBManagement.Models
{
    public partial class User
    {
        public User()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Role { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? IsBan { get; set; }

        public virtual Role RoleNavigation { get; set; } = null!;
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
