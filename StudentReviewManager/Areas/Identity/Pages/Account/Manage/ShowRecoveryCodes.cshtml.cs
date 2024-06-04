//Licensedtothe.NETFoundationunderoneormoreagreements.
//The.NETFoundationlicensesthisfiletoyouundertheMITlicense.
#nullable disable
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace StudentReviewManager.Areas.Identity.Pages.Account.Manage
{
    ///<summary>
    ///ThisAPIsupportstheASP.NETCoreIdentitydefaultUIinfrastructureandisnotint endedtobeused
    ///directlyfromyourcode.ThisAPImaychangeorberemovedinfuturereleases.
    ///</summary>
    public class ShowRecoveryCodesModel : PageModel
    {
        ///<summary>
        ///ThisAPIsupportstheASP.NETCoreIdentitydefaultUIinfrastructureandisnotint endedtobeused
        ///directlyfromyourcode.ThisAPImaychangeorberemovedinfuturereleases.
        ///</summary>
        [TempData]
        public string[] RecoveryCodes { get; set; }

        ///<summary>
        ///ThisAPIsupportstheASP.NETCoreIdentitydefaultUIinfrastructureandisnotint endedtobeused
        ///directlyfromyourcode.ThisAPImaychangeorberemovedinfuturereleases.
        ///</summary>
        [TempData]
        public string StatusMessage { get; set; }

        ///<summary>
        ///ThisAPIsupportstheASP.NETCoreIdentitydefaultUIinfrastructureandisnotint endedtobeused
        ///directlyfromyourcode.ThisAPImaychangeorberemovedinfuturereleases.
        ///</summary>
        public IActionResult OnGet()
        {
            if (RecoveryCodes == null || RecoveryCodes.Length == 0)
            {
                return RedirectToPage("./TwoFactorAuthentication");
            }
            return Page();
        }
    }
}
