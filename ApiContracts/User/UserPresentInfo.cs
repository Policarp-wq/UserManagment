using System.ComponentModel.DataAnnotations;
using UserManagment.Utility;

namespace UserManagment.ApiContracts.User
{
    public class UserPresentInfo : IValidatableObject
    {
        public string Name { get; set; }
        public int Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public bool IsActive { get; set; }

        public UserPresentInfo(string name, int gender, DateTime? birthday, bool isActive)
        {
            Name = name;
            Gender = gender;
            Birthday = birthday;
            IsActive = isActive;
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ModelValidator.IsNameValid(Name))
                yield return new ValidationResult("Name must contain only latin or russian letters");
            if (!ModelValidator.IsGenderValid(Gender))
                yield return new ValidationResult("Gender must be in range [0, 2]");
        }
    }
}
