using System.Security.Claims;
using Authorization.RazorPages.Data;
using Authorization.RazorPages.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Authorization.RazorPages
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>(config =>
            {
                config.UseInMemoryDatabase("MEMORY");
                config.EnableDetailedErrors();
                config.EnableSensitiveDataLogging();
            }
            ).AddIdentity<ApplicationUser, ApplicationRole>(config =>
            {
                config.Password.RequireDigit = false;
                config.Password.RequireLowercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequiredLength = 6;

            }).AddEntityFrameworkStores<ApplicationDbContext>();

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/Login"; //Путь, по которому будет перенаправляться анонимный пользователь при доступе к ресурсам, для которых нужна аутентификация
                config.AccessDeniedPath = "/Account/AccessDenied";//Страница на которую будет перенаправлен пользователь при нехватке прав
            });

            services.AddAuthorization(options =>
            {
                //Доступ к контроллерам с [Authorize(Policy = "Manager")]
                options.AddPolicy("Модератор", builder =>
                {
                    builder.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, "Модератор") ||
                                             x.User.HasClaim(ClaimTypes.Role, "Администратор")
                    );//Необходима роль администратор или менеджер
                });

                //Доступ к контроллерам с [Authorize(Policy = "Administrator")]
                options.AddPolicy("Администратор", builder =>
                {
                    builder.RequireClaim(ClaimTypes.Role, "Администратор");//Необходима роль администратор
                });
                options.AddPolicy("Пользователь", builder =>
                {
                    builder.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, "Модератор") ||
                                             x.User.HasClaim(ClaimTypes.Role, "Администратор") ||
                                             x.User.HasClaim(ClaimTypes.Role, "Пользователь")
                    );//Необходима роль администратор или менеджер
                });
            });//Добавляем авторизацию


            services.AddRazorPages();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();//Используем аунтентификацию
            app.UseAuthorization();//Используем авторизацию

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
