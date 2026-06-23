using AutoMapper;
using GymSystem.BLL.Services.Classes;
using GymSystem.BLL.Services.Intrterfaces;
using GymSystem.BLL.Utilities;
using GymSystem.DAL.Data.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Classes;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace GymSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
   

           //builder.Services.AddScoped<IPlanRepository, DAL.Repositories.Classes.PlanRepository>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(DAL.Repositories.Classes.GenericRepository<>));
            builder.Services.AddScoped<IMemberServices , MemberServices>();
            builder.Services.AddScoped<ISessionServices, SessionServices>();
            builder.Services.AddScoped<ITrainerServices, TrainerServices>();
            builder.Services.AddScoped<IPlanServices, PlanServices>();
            builder.Services.AddScoped<IAnalyticsServices ,AnalyticsServices>();
            builder.Services.AddScoped<IAttachmentServices , AttachmentServices>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork >();      
            builder.Services.AddIdentity<ApplicationUser , IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(2);
            
            }).AddEntityFrameworkStores<GymDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            }); 
            builder.Services.AddAutoMapper(m=>m.AddProfile(new MappingProfile()));
            var app = builder.Build();
            await app.MigarateAndSeedAsync();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }   

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
