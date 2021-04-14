using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Vet.Models;
using Vet.Data;
using Microsoft.EntityFrameworkCore;

namespace Vet.Areas.Identity.Pages.Account.Manage
{
    public partial class EmailModel : PageModel
    {
        private readonly UserManager<VetUser> _userManager;
        private readonly SignInManager<VetUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly VetDbContext _context;

        public EmailModel(
            UserManager<VetUser> userManager,
            SignInManager<VetUser> signInManager,
            IEmailSender emailSender,
            VetDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _context = context;
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Új e-mail")]
            public string NewEmail { get; set; }
        }

        private async Task LoadAsync(VetUser user)
        {
            var email = await _userManager.GetEmailAsync(user);
            Email = email;

            Input = new InputModel
            {
                NewEmail = email,
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (await _context.Users.AnyAsync(u => u.Email == Input.NewEmail)) 
            {
                ModelState.AddModelError("Email", "A megadott e-mail cím már használatban van.");
                return Page();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail != email)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { userId = userId, email = Input.NewEmail, code = code },
                    protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(
                    user.Email,
                    "Új e-mail megerősítése",
                    $"Kedves Felhasználónk!<br>Az alábbi emailt azért küldtük, mert megváltoztattad az e-mail címed rendszerünkben.<br>Az új email cím: {Input.NewEmail}<br><br>A véglegesítéséhez kérlek erősítsd meg az e-mail címed a következő link segítségével.<br>Az e-mail cím megerősítéséhez: <a href = '{HtmlEncoder.Default.Encode(callbackUrl)}' > kattints ide </a >.<br><br>További szép napot kívánunk! <br>Vet csapata");

                StatusMessage = "Az új e-mail cím beállításával kapcsolatban e-mailt küldtünk neked a régi e-mail címedre.";
                return RedirectToPage();
            }
            
            StatusMessage = "Az e-mail címed nem változott meg.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "E-mail cím megerősítése",
                $"Kedves Felhasználónk!<br>Az alábbi emailt azért küldtük, mert megváltoztattad az e-mail címed rendszerünkben.<br><br>A véglegesítéséhez kérlek erősítsd meg az e-mail címed a következő link segítségével.<br>Az e-mail cím megerősítéséhez: <a href = '{HtmlEncoder.Default.Encode(callbackUrl)}' > kattints ide </a >.<br><br>További szép napot kívánunk! <br>Vet csapata");

            StatusMessage = "A megadott e-mail címre megerősítő üzenetet küldtünk. A megadott link segítségével erősítsd meg az e-mail címed.";
            return RedirectToPage();
        }
    }
}
