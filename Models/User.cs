namespace UserManagment.Models
{
    public class User
    {
        public Guid Guid { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!; // better to store hashed but ok
        public string Name { get; set; } = null!;
        public int Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public bool Admin { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; } = null!;
        public DateTime RevokedOn { get; set; }
        public string RevokedBy { get; set; } = null!;
    }
}
