namespace UserManagment.Models
{
    public class User
    {
        private const int DAYS_IN_YEAR = 365;

        public Guid Guid { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!; // better to store hashed but ok
        public string Name { get; set; } = null!;
        public int Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public bool Admin { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; } = null!;
        public DateTime? RevokedOn { get; set; }
        public string RevokedBy { get; set; } = null!;
        public bool IsActive() => RevokedOn != null;
        public bool IsOlderThan(int age) => Birthday != null && (DateTime.UtcNow - Birthday.Value).TotalDays >= age * DAYS_IN_YEAR;
    }
}
