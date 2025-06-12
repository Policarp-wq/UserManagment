using System.ComponentModel.DataAnnotations;
using UserManagment.Utility;

namespace UserManagment.ApiContracts.User
{
    public class UserCreateInfo : IValidatableObject
    {
        public string Login { get; set; } = null!;
        public string Pasword { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public bool IsAdmin { get; set; }

        public UserCreateInfo(string login, string pasword, string name, int gender, DateTime? birthday, bool isAdmin)
        {
            Login = login;
            Pasword = pasword;
            Name = name;
            Gender = gender;
            Birthday = birthday;
            IsAdmin = isAdmin;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ModelValidator.IsLoginValid(Login))
                yield return new ValidationResult("Login must contain only latin letters and digits");
            if (!ModelValidator.IsPasswordValid(Pasword))
                yield return new ValidationResult("Password must contain only latin letters and digits");
            if(!ModelValidator.IsNameValid(Name))
                yield return new ValidationResult("Name must contain only latin or russian letters");
            if(!ModelValidator.IsGenderValid(Gender))
                yield return new ValidationResult("Gender must be in range [0, 2]");
        }
    }
}
