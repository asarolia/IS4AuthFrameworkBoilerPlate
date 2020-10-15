using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TestIdentityServerAuthentication
{
    public class Users
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(connectionString));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    //var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                    var userMgr = serviceProvider.GetService<UserManager<IdentityUser>>();

                    var alice = userMgr.FindByNameAsync("abhishek.sarolia@anacapfp.com").Result;

                    if(alice != null)
                    {
                        var result = userMgr.AddClaimsAsync(alice, new Claim[]{
                        new Claim(JwtClaimTypes.Name, "Abhishek Sarolia"),
                        new Claim(JwtClaimTypes.GivenName, "Abhishek"),
                        new Claim(JwtClaimTypes.FamilyName, "Sarolia"),
                        new Claim(JwtClaimTypes.Email, "abhishek.sarolia@anacapfp.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://anacapfp.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'Flat': 'B 502', 'Apartment': 'JM Aroma', 'postal_code': 201301, 'country': 'India' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                    }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Console.WriteLine("custom claim created");

                    }
                    //if (alice == null)
                    //{
                    //    alice = new ApplicationUser
                    //    {
                    //        UserName = "alice",
                    //        Email = "AliceSmith@email.com",
                    //        EmailConfirmed = true
                    //    };
                    //    var result = userMgr.CreateAsync(alice, "My long 123$ password").Result;
                    //    if (!result.Succeeded)
                    //    {
                    //        throw new Exception(result.Errors.First().Description);
                    //    }

                    //    result = userMgr.AddClaimsAsync(alice, new Claim[]{
                    //    new Claim(JwtClaimTypes.Name, "Alice Smith"),
                    //    new Claim(JwtClaimTypes.GivenName, "Alice"),
                    //    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    //    new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                    //    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    //    new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                    //    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                    //}).Result;
                    //    if (!result.Succeeded)
                    //    {
                    //        throw new Exception(result.Errors.First().Description);
                    //    }
                    //    Console.WriteLine("alice created");
                    //}
                    //else
                    //{
                    //    Console.WriteLine("alice already exists");
                    //}

                    //var bob = userMgr.FindByNameAsync("bob").Result;
                    //if (bob == null)
                    //{
                    //    bob = new ApplicationUser
                    //    {
                    //        UserName = "bob",
                    //        Email = "BobSmith@email.com",
                    //        EmailConfirmed = true
                    //    };
                    //    var result = userMgr.CreateAsync(bob, "My long 123$ password").Result;
                    //    if (!result.Succeeded)
                    //    {
                    //        throw new Exception(result.Errors.First().Description);
                    //    }

                    //    result = userMgr.AddClaimsAsync(bob, new Claim[]{
                    //    new Claim(JwtClaimTypes.Name, "Bob Smith"),
                    //    new Claim(JwtClaimTypes.GivenName, "Bob"),
                    //    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    //    new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                    //    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    //    new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                    //    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                    //    new Claim("location", "somewhere")
                    //}).Result;
                    //    if (!result.Succeeded)
                    //    {
                    //        throw new Exception(result.Errors.First().Description);
                    //    }
                    //    Console.WriteLine("bob created");
                    //}
                    //else
                    //{
                    //    Console.WriteLine("bob already exists");
                    //}
                }
            }
        }
    }
}
