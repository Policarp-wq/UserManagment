using System.ComponentModel.DataAnnotations;
using UserManagment.Utility;

namespace UserManagment.ApiContracts.User
{
    public class UserUpdateInfo : IValidatableObject
    {
        public string? Name { get; set; }
        public int? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name != null && !ModelValidator.IsNameValid(Name))
                yield return new ValidationResult("Name must contain only latin or russian letters");
            if (Gender != null && !ModelValidator.IsGenderValid(Gender.Value))
                yield return new ValidationResult("Gender must be in range [0, 2]");
        }
    }
}
