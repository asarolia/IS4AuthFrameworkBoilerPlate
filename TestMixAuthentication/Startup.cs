using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestIdentityServerAuthentication.Services;

namespace TestIdentityServerAuthentication
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            const string connectionString =@"Data Source=(LocalDb)\MSSQLLocalDB;database=MinervaCloudIdentityServerDB;trusted_connection=yes;";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ApplicationDbContext>(builder =>
            builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders(); 

            services.AddControllersWithViews();

            //// to use in memory store 
            //services.AddIdentityServer()
            //.AddInMemoryClients(Config.GetClients())
            //.AddInMemoryIdentityResources(Config.GetIdentityResources())
            //.AddInMemoryApiResources(Config.GetApiResources())
            //.AddInMemoryApiScopes(Config.GetApiScopes())
            //.AddTestUsers(Config.GetUsers())
            //.AddDeveloperSigningCredential();

            // To use persistence store 
            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
            }
                )
                .AddDeveloperSigningCredential()
               // .AddInMemoryClients(Config.GetClients())
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                })
                // AddDot Net Core Identity User store 
                .AddAspNetIdentity<ApplicationUser>();

            // Add external authentication 
            services.AddAuthentication()
                .AddMicrosoftAccount(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                   
                    options.ClientId = "Azure registration Client ID";
                    options.ClientSecret = "Azure registration client secret";
                });

            // claims transformation is run after every Authenticate call
            //services.AddTransient<IClaimsTransformation, CustomClaimsTransformer>();

            //services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, ApplicationUserClaimsPrincipalFactory>();

            
            services.AddTransient<IEmailSender, EmailSender>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // this will do the initial DB population
           // InitializeDatabase(app);
           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            // app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var resource in Config.GetApiScopes())
                    {
                        context.ApiScopes.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

              
            }
        }

    }
}
