using GymSystem.BLL.Services.Intrterfaces;
using GymSystem.BLL.Services.Classes;
using GymSystem.DAL.Contexts;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using GymSystem.DAL.Repositories.Classes;

namespace GymSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            
           //builder.Services.AddScoped<IPlanRepository, DAL.Repositories.Classes.PlanRepository>();
           //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(DAL.Repositories.Classes.GenericRepository<>));
           builder.Services.AddScoped<IMemberServices , MemberServices>();
           builder.Services.AddScoped<IUnitOfWork, UnitOfWork >();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }   

            app.UseHttpsRedirection();
            app.UseRouting();

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
