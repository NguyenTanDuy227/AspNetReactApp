using AspNetReactApp.Models;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AspNetReactApp.Data
{
    public class ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) : ApiAuthorizationDbContext<ApplicationUser>(options, operationalStoreOptions)
    {
        public required DbSet<Student> Students { get; set; }
    }
}