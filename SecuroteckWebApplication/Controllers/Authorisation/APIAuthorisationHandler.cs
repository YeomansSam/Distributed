using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using SecuroteckWebApplication.Models;

namespace SecuroteckWebApplication.Controllers
{
    public class APIAuthorisationHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync (HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //Task5
            // TODO:  Find if a header ‘ApiKey’ exists, and if it does, check the database to determine if the given API Key is valid
            //        Then authorise the principle on the current thread using a claim, claimidentity and claimsprinciple
            if (request.Headers.Contains("ApiKey"))
            {
                string Key = request.Headers.GetValues("ApiKey").First();
                var dbaccess = new UserDatabaseAccess();
                if (dbaccess.CheckUserKey(Key))
                {
                    User user = dbaccess.UserCheckKey(Key);

                    ClaimsIdentity identity = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, user.Role)
                    }, Key);

                    Thread.CurrentPrincipal = new ClaimsPrincipal(identity);

                    return await base.SendAsync(request, cancellationToken);
                }
            }
            
            return await base.SendAsync(request, cancellationToken);
        }
    }
}