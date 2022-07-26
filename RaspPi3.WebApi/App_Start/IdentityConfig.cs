using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using RaspPi3.WebApi.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace RaspPi3.WebApi
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(
            IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
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

            manager.MaxFailedAccessAttemptsBeforeLockout = 3;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.UserLockoutEnabledByDefault = true;

            manager.EmailService = new EmailService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(
                    dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    public class EmailService : IIdentityMessageService
    {
        public async System.Threading.Tasks.Task SendAsync(IdentityMessage message)
        {
            var userName = WebConfigurationManager.AppSettings["MailUser"];
            var domain = WebConfigurationManager.AppSettings["MailDomain"];
            var password = WebConfigurationManager.AppSettings["MailPass"];
            var sentFrom = WebConfigurationManager.AppSettings["MailFrom"];

            var service = new ExchangeService(ExchangeVersion.Exchange2010_SP2)
            {
                Credentials = new WebCredentials(userName, password, domain)
            };
            service.AutodiscoverUrl(sentFrom);

            var email = new EmailMessage(service);
            email.ToRecipients.Add(message.Destination);
            email.Subject = message.Subject;
            var body = new MessageBody(message.Body)
            {
                BodyType = BodyType.HTML
            };
            email.Body = body;
            email.Send();
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) 
            : base(userManager, authenticationManager)
        { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
