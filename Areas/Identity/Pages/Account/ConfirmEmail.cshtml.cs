using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Vet.Data;
using Vet.Models;

namespace Vet.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<VetUser> _userManager;
        private readonly VetDbContext _context;

        public ConfirmEmailModel(UserManager<VetUser> userManager, VetDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "E-mail címed megerősítése megtörtént." : "Hiba történt az e-mail cím megerősítése során. Kérjük próbáld újra!";
            if (result.Succeeded)
            {
                user.AuthLevel = 1;
                var medicalRecords = await _context.MedicalRecords.Where(m => m.OwnerEmail == user.Email).ToListAsync();
                foreach (var record in medicalRecords)
                {
                    record.OwnerId = userId;
                }
                await _context.SaveChangesAsync();
            }
            return Page();
        }
    }
}
