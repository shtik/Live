using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ShtikLive.Identity
{
    public class ShtikClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public ShtikClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
        {
        }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            if (!string.IsNullOrWhiteSpace(user.Handle))
            {
                ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(ShtikClaimTypes.Handle, user.Handle));
            }
            return principal;
        }
    }
}