using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Product.Core.Entities;
using Product.Core.Interface;
using Product.Infrastructure.Data;
using Product.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure
{
    public static class InfraStructureRequistration
    {
        /// <summary>
        /// 設定應用程式的基礎架構配置，包括服務註冊和身份驗證設置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection InfraStructureConfigration(this IServiceCollection services, IConfiguration configuration)
        {
            // 註冊單元工作模式
            services.AddScoped<ICurrencyService, CurrencyService>(); // 确保添加这一行
            // 配置資料庫上下文
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );
            // 註冊內存快取
            services.AddMemoryCache();

            // 設置 JWT 身份驗證
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"])),
                    ValidIssuer = configuration["Token:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = false
                };
            });
            return services;
        }

        /// <summary>
        /// 設置基礎架構中間件，並進行用戶種子數據的初始化。
        /// </summary>
        /// <param name="app"></param>
        public static async void InfrastructureConfigMiddleware(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                //var userManger = scope.ServiceProvider.GetRequiredService<UserManager<AppUsers>>();
                //await IdentitySeed.SeedUserAsync(userManger);
            }
        }
    }
}
