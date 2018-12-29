using System.ComponentModel.DataAnnotations;
using GetShredded.Common;

namespace GetShredded.ViewModel.Input.Users
{
    public class RegisterInputModel
    {
        [Required]
        [StringLength(GlobalConstants.UserFirstNameMaxLength, MinimumLength = GlobalConstants.UserFirstNameMinLength)]
        [RegularExpression(GlobalConstants.ValidationForName, 
            ErrorMessage = GlobalConstants.ErrorMessageInRegisterModel)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(GlobalConstants.UserLastNameMaxLength, MinimumLength = GlobalConstants.UserLastNameMinLength)]
        [RegularExpression(GlobalConstants.ValidationForName, 
            ErrorMessage = GlobalConstants.ErrorMessageInRegisterModel)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(GlobalConstants.ValidationForName, 
            ErrorMessage = GlobalConstants.ErrorMessageInRegisterModel)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        [Display(Name = GlobalConstants.ConfirmPassword)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
