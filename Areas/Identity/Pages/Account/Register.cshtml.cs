// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using PusulaSu.Models;
using PusulaSu.Data;
using Microsoft.EntityFrameworkCore;

namespace PusulaSu.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _emailSender = emailSender;
_context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
          [Required(ErrorMessage = "Abone numarası zorunludur.")]
[Display(Name = "Abone Numarası")]
public string AboneNo { get; set; } = "";

[Required(ErrorMessage = "Sayaç numarası zorunludur.")]
[Display(Name = "Sayaç Numarası")]
public string SayacNo { get; set; } = "";
            
            [Required(ErrorMessage = "Şifre zorunludur.")]
[StringLength(
    100,
    ErrorMessage = "Şifre en az {2}, en fazla {1} karakter olmalıdır.",
    MinimumLength = 6)]
[DataType(DataType.Password)]
[Display(Name = "Şifre")]
public string Password { get; set; } = "";

[Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
[DataType(DataType.Password)]
[Display(Name = "Şifre Tekrarı")]
[Compare(
    "Password",
    ErrorMessage = "Şifre ve şifre tekrarı eşleşmiyor.")]
public string ConfirmPassword { get; set; } = "";
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/Dashboard");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var aboneKaydi = await _context.AboneKayitlari
    .FirstOrDefaultAsync(a =>
        a.AboneNo == Input.AboneNo &&
        a.SayacNo == Input.SayacNo);
 if (aboneKaydi == null)
    {
        ModelState.AddModelError(
            string.Empty,
            "Abone No veya Sayaç No hatalı.");

        return Page();
    }
if (aboneKaydi.KullaniciId != null)
{
    ModelState.AddModelError(
        string.Empty,
        "Bu abonelik için daha önce hesap oluşturulmuş.");

    return Page();
}

                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.AboneNo, CancellationToken.None);
                
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    aboneKaydi.KullaniciId = userId;
await _context.SaveChangesAsync();
                   await _signInManager.SignInAsync(user, isPersistent: false);
return LocalRedirect(returnUrl);
                
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
