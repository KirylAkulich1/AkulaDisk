using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Threading.Tasks;
using Domain.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using System.Text.Encodings.Web;

namespace AkulaDisk.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _sender;
        private readonly IMailService _mailService;

        public RegisterConfirmationModel(UserManager<ApplicationUser> userManager, IEmailSender sender, IMailService mailService)
        {
            _userManager = userManager;
            _sender = sender;
            _mailService = mailService;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;
            // Once you add a real email sender, you should remove this code that lets you confirm the account
            DisplayConfirmAccountLink = true;
            if (DisplayConfirmAccountLink)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);
                MailRequest mailRequest = new MailRequest
                {
                    Subject = "Subject",
                    Attachments = new List<IFormFile>(),
                    ToEmail = "AkulaDiskSender@yandex.by",
                    Body =
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(EmailConfirmationUrl)}'>clicking here</a>." //String.Format(
                                         // "User {0} offer you request on {1}:\n" +
                                         //"To accept follow link: {2}/Request/AddRequest/?fileid={3} username={4}} \n" +
                                         //"To deny follow link:  {2}/Request/AddRequest/?fileid={3} username={4}}")
                };
                await _mailService.SendEmailAsync(mailRequest);

            }

            return Page();
        }
    }
}
