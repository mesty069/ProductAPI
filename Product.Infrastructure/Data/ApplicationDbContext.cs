using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Product.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Currency> Currencies { get; set; }
        /// <summary>
        /// 配置模型的建構。
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(e => e.CurrencyCode); // 設置 CurrencyCode 為主鍵
                entity.Property(e => e.CurrencyCode).HasMaxLength(3).IsRequired(); // 設置長度限制
                entity.Property(e => e.CurrencyName_en).IsRequired();
                entity.Property(e => e.CurrencyName_zh).IsRequired();
                entity.Property(e => e.ExchangeRate).HasColumnType("decimal(18, 4)"); // 配置匯率的精度
            });
            // 應用當前組件中的所有模型配置
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
