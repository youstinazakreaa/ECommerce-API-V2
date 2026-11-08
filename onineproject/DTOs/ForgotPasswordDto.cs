using Microsoft.OpenApi.MicrosoftExtensions;
using System.ComponentModel.DataAnnotations;

namespace onineproject.DTOs
{
    public class ForgotPasswordDto
    {
        [Required (ErrorMessage ="Email is Required")]
        [EmailAddress(ErrorMessage ="Invalid Email Address")]
        [StringLength(256 ,ErrorMessage ="Email cannot exceed 256 characters")]
        public string Email { get; set; }=string.Empty;
    }
}
