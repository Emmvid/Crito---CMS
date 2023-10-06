using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using Crito.Models;
using Crito.Services;

namespace Crito.Controllers
{
    public class ContactController : SurfaceController
    {
        public ContactController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
        }

        [HttpPost]
        public IActionResult Index(ContactForm contactForm)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            using var mail = new MailService("no-reply@crito.com", "smtp.crito.se", 587, "contactform@crito.com", "Bytmig123!"); ;

            //to sender
            mail.SendAsync(contactForm.Email, "Your Message was recieved", "Hi, thank you for your email. We will contact you soon").ConfigureAwait(false);
            // to reciever   
            mail.SendAsync("support@crito.com", $"{contactForm.Name} sent you a message", contactForm.Message).ConfigureAwait(false);


            return LocalRedirect(contactForm.RedirectUrl ?? "/");
            
        }
    }
}
 