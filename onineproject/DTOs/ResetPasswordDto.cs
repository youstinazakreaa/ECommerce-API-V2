using System.ComponentModel.DataAnnotations;

namespace onineproject.DTOs
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage="Email is Required")]
        [EmailAddress(ErrorMessage="Invalid Email Address")]
        [StringLength(256, ErrorMessage="Email cannot exceed 256 characters")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage=" Reset Token is Required")]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage="New Password is Required")]
        [StringLength(100, MinimumLength=6, ErrorMessage="Password must be between 6 and 100 characters")]
        public string NewPassword { get; set; } = string.Empty;
        [Required(ErrorMessage="Confirm Password is Required")]
        [Compare(nameof(NewPassword), ErrorMessage="Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
