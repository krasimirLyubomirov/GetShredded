using System.ComponentModel.DataAnnotations;
using GetShredded.Common;

namespace GetShredded.ViewModel.Input.Users
{
    public class LoginInputModel
    {
        [Required]
        [StringLength(GlobalConstants.UserFirstNameMaxLength, MinimumLength = GlobalConstants.UserFirstNameMinLength)]
        [RegularExpression(GlobalConstants.ValidationForName)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
