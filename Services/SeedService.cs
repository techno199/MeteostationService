using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MeteostationService
{
    public class SeedService
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private MyDbContext _context;

        public SeedService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            MyDbContext context
        )
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._context = context;
        }

        public void SeedDefaultUsers()
        {
            var defaultUserName = "ksan";
            var defaultPassword = "12345";
            
            
            var user = new ApplicationUser() { UserName = defaultUserName };
            var isUserAlreadyExists = _userManager.FindByNameAsync(defaultUserName).Result;

            if (isUserAlreadyExists != null)
            {
                System.Console.WriteLine("Default user already exists");
                return;
            }
            
            var result = _userManager.CreateAsync(user, defaultPassword).Result;
            if (result.Succeeded)
            {
                System.Console.WriteLine("Successfuly created default user");
                
            }
            else
                System.Console.WriteLine("Failed to create default user");
        }
    }
}