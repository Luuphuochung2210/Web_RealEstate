using Microsoft.AspNetCore.Builder;
using Web_RealEstate.Reposistory;

namespace Web_RealEstate
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache(); // Use a distributed memory cache provider
            services.AddSession(options =>
            {
                options.Cookie.Name = "YourSessionCookieName"; // Set a unique name for your session cookie
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout duration
            });
            services.AddMvc();
        }
    }
}
