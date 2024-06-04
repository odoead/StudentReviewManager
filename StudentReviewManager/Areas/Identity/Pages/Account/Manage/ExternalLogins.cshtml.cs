// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable  disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentReviewManager.DAL.Models;

namespace StudentReviewManager.Areas.Identity.Pages.Account.Manage
{
    public class ExternalLoginsModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserStore<User> _userStore;

        public ExternalLoginsModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUserStore<User> userStore
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userStore = userStore;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not int ended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not int ended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> OtherLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not int ended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public bool ShowRemoveButton { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not int ended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetasync()
        {
            var user = await _userManager.GetUserasync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            CurrentLogins = await _userManager.GetLoginsasync(user);
            OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesasync())
                .Where(auth => CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
                .ToList();
            string passwordHash = null;
            if (_userStore is IUserPasswordStore<User> userPasswordStore)
            {
                passwordHash = await userPasswordStore.GetPasswordHashasync(
                    user,
                    HttpContext.RequestAborted
                );
            }
            ShowRemoveButton = passwordHash != null || CurrentLogins.Count > 1;
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveLoginasync(
            string loginProvider,
            string providerKey
        )
        {
            var user = await _userManager.GetUserasync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var result = await _userManager.RemoveLoginasync(user, loginProvider, providerKey);
            if (!result.Succeeded)
            {
                StatusMessage = "The external login was not removed.";
                return RedirectToPage();
            }
            await _signInManager.RefreshSignInasync(user);
            StatusMessage = "The external login was removed.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostLinkLoginasync(string provider)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutasync(IdentityConstants.ExternalScheme);
            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Page("./ExternalLogins", pageHandler: "LinkLoginCallback");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(
                provider,
                redirectUrl,
                _userManager.GetUserId(User)
            );
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetLinkLoginCallbackasync()
        {
            var user = await _userManager.GetUserasync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var userId = await _userManager.GetUserIdasync(user);
            var info = await _signInManager.GetExternalLoginInfoasync(userId);
            if (info == null)
            {
                throw new InvalidOperationException(
                    $"Unexpected error occurred loading external login info."
                );
            }
            var result = await _userManager.AddLoginasync(user, info);
            if (!result.Succeeded)
            {
                StatusMessage =
                    "The external login was not added. External logins can only be associated with one account.";
                return RedirectToPage();
            }
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutasync(IdentityConstants.ExternalScheme);
            StatusMessage = "The external login was added.";
            return RedirectToPage();
        }
    }
}
