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
                config.LoginPath = "/Account/Login"; //����, �� �������� ����� ���������������� ��������� ������������ ��� ������� � ��������, ��� ������� ����� ��������������
                config.AccessDeniedPath = "/Account/AccessDenied";//�������� �� ������� ����� ������������� ������������ ��� �������� ����
            });

            services.AddAuthorization(options =>
            {
                //������ � ������������ � [Authorize(Policy = "Manager")]
                options.AddPolicy("���������", builder =>
                {
                    builder.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, "���������") ||
                                             x.User.HasClaim(ClaimTypes.Role, "�������������")
                    );//���������� ���� ������������� ��� ��������
                });

                //������ � ������������ � [Authorize(Policy = "Administrator")]
                options.AddPolicy("�������������", builder =>
                {
                    builder.RequireClaim(ClaimTypes.Role, "�������������");//���������� ���� �������������
                });
                options.AddPolicy("������������", builder =>
                {
                    builder.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, "���������") ||
                                             x.User.HasClaim(ClaimTypes.Role, "�������������") ||
                                             x.User.HasClaim(ClaimTypes.Role, "������������")
                    );//���������� ���� ������������� ��� ��������
                });
            });//��������� �����������


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

            app.UseAuthentication();//���������� ���������������
            app.UseAuthorization();//���������� �����������

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
