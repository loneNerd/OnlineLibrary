using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using LibraryDAL.Models;

namespace LibraryDAL
{
    /// <summary>
    /// This class create and manage user system for this application.
    /// </summary>
    public class ApplicationUserManager : UserManager<DBUser, int>
    {
        public ApplicationUserManager(IUserStore<DBUser, int> store) : base(store) { }

        /// <summary>
        /// This method create new user.
        /// </summary>
        /// <param name="options">Configuration options for application user manager.</param>
        /// <param name="context">Strongly typed accessor.</param>
        /// <returns>Return new user manager.</returns>
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<DBUser, DBRole, int, DBLogin, DBUserRole, DBClaim>(context.Get<ApplicationDbContext>()));
            
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<DBUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            var dataProtectionProvider = options.DataProtectionProvider;

            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<DBUser, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }
    }

    /// <summary>
    /// Configure the application sign-in manager which is used in this application.
    /// </summary>
    public class ApplicationSignInManager : SignInManager<DBUser, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager) { }


        /// <summary>
        /// This method generate new user idenetity in user manager.
        /// </summary>
        /// <param name="user">Parametr present new user.</param>
        /// <returns>Parametre return new user.</returns>
        public override Task<ClaimsIdentity> CreateUserIdentityAsync(DBUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        /// <summary>
        /// This method create sign-in manager for this application.
        /// </summary>
        /// <param name="options">This parametr present options which needed in  statndart implemintation but not used.</param>
        /// <param name="context">This parametr provides stronglt typed accessors.</param>
        /// <returns>Returns new sign-in manager.</returns>
        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
