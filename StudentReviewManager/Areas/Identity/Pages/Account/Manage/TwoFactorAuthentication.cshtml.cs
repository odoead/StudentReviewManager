//Licensedtothe.NETFoundationunderoneormoreagreements.
//The.NETFoundationlicensesthisfiletoyouundertheMITlicense.
#nullable  disable
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using StudentReviewManager.DAL.Models;

namespace StudentReviewManager.Areas.Identity.Pages.Account.Manage
{
    public class TwoFactorAuthenticationModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<TwoFactorAuthenticationModel> _logger;

        public TwoFactorAuthenticationModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<TwoFactorAuthenticationModel> logger
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        ///<summary>
        ///ThisAPIsupportstheASP.NETCoreIdentitydefaultUIinfrastructureandisnotint endedtobeused
        ///directlyfromyourcode.ThisAPImaychangeorberemovedinfuturereleases.
        ///</summary>
        public bool HasAuthenticator { get; set; }

        ///<summary>
        ///ThisAPIsupportstheASP.NETCoreIdentitydefaultUIinfrastructureandisnotint endedtobeused
        ///directlyfromyourcode.ThisAPImaychangeorberemovedinfuturereleases.
        ///</summary>
        public int RecoveryCodesLeft { get; set; }

        ///<summary>
        ///ThisAPIsupportstheASP.NETCoreIdentitydefaultUIinfrastructureandisnotint endedtobeused
        ///directlyfromyourcode.ThisAPImaychangeorberemovedinfuturereleases.
        ///</summary>
        [BindProperty]
        public bool Is2faEnabled { get; set; }

        ///<summary>
        ///ThisAPIsupportstheASP.NETCoreIdentitydefaultUIinfrastructureandisnotint endedtobeused
        ///directlyfromyourcode.ThisAPImaychangeorberemovedinfuturereleases.
        ///</summary>
        public bool IsMachineRemembered { get; set; }

        ///<summary>
        ///ThisAPIsupportstheASP.NETCoreIdentitydefaultUIinfrastructureandisnotint endedtobeused
        ///directlyfromyourcode.ThisAPImaychangeorberemovedinfuturereleases.
        ///</summary>
        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetasync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"UnabletoloaduserwithID'{_userManager.GetUserId(User)}'.");
            }
            HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null;
            Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user);
            RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostasync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"UnabletoloaduserwithID'{_userManager.GetUserId(User)}'.");
            }
            await _signInManager.ForgetTwoFactorClientAsync();
            StatusMessage =
                "Thecurrentbrowserhasbeenforgotten.Whenyouloginagainfromthisbrowseryouwillbepromptedforyour2facode.";
            return RedirectToPage();
        }
    }
}
