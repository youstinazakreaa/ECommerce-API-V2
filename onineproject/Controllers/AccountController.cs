using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using onine.core.Constants;
using onine.core.DTOs;
using onine.core.Entites.Identity;
using onine.services;
using onineproject.DTOs;
using onineproject.Errors;

namespace onineproject.Controllers
{
    public class AccountController : ApiBaseController
    {

       private readonly UserManager<AppUser> _userManager;
       private readonly SignInManager<AppUser> _signInManager;
       private readonly ITokenService _tokenService;
       private readonly IMapper _mapper;
       private readonly IEmailService _emailService;

    public AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        IMapper mapper,
        IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _mapper = mapper;
        _emailService = emailService;
    }

    //Register

    [HttpPost("register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {

            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.EmailAddress,
                UserName = model.EmailAddress.Split('@')[0],
                PhoneNumber = model.phoneNumber
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            //   اضافة role تلقائي بعد تسجيل المستخدم user
            var roleResult = await _userManager.AddToRoleAsync(user, AppRoles.User);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return BadRequest(roleResult.Errors);
            }
            //  انشاء token للمستخدم بعد تسجيله


            var returneduser = new UserDto()
            {
                DisplayName = user.DisplayName,
                EmailAddress = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };

            return Ok(returneduser);


        }


        //login
        [HttpPost("Login")]

       
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.EmailAddress);

            if (user == null || string.IsNullOrWhiteSpace(user.Email))
                return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(
                user, model.Password, false);

            if (!result.Succeeded)
                return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                EmailAddress = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }




        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);
            if (user == null)
                return Unauthorized();
            if (user.Address == null)
                return NotFound("Address not found");
            var addressDto = _mapper.Map<AddressDto>(user.Address);
            return Ok(addressDto);

        }
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress([FromBody] AddressDto addressDto)
        {
            var user = await _userManager.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);
            if (user == null)
                return Unauthorized();
            if (user.Address == null)
                return NotFound("Address not found");
            _mapper.Map(addressDto, user.Address);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest("Problem updating the user");
            var updatedAddressDto = _mapper.Map<AddressDto>(user.Address);
            return Ok(updatedAddressDto);
        }


        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(
            [FromBody] ForgotPasswordDto dto)
        {
            const string responseMessage =
                "If the email exists, password reset instructions will be sent.";

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return Ok(new { Message = responseMessage });

            var token = await _userManager
                .GeneratePasswordResetTokenAsync(user);

            var safeToken = System.Net.WebUtility.HtmlEncode(token);

            var emailBody = $"""
        <h2>Reset Your Password</h2>
        <p>Use the following token to reset your password:</p>
        <p><code>{safeToken}</code></p>
        <p>This token is confidential. Do not share it.</p>
        <p>If you did not request a password reset, ignore this email.</p>
        """;

            await _emailService.SendEmailAsync(
                user.Email!,
                "Reset Your Password",
                emailBody);

            return Ok(new { Message = responseMessage });
        }




        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(
            [FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return BadRequest("Invalid or expired reset token.");

            var result = await _userManager.ResetPasswordAsync(
                user,
                dto.Token,
                dto.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Message = "Password reset failed.",
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            return Ok(new
            {
                Message = "Password has been reset successfully."
            });
        }



    }
}

