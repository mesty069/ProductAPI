using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Product.Core.Entities;
using System.Security.Claims;

namespace Product.API.Extensions
{
    public static class UserManagerExtensions
    {
        /// <summary>
        /// Email查詢帳號地址資訊
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static async Task<AppUsers> FindUserByClaimPrincipalWithAddress(this UserManager<AppUsers> userManager, ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            // 使用email查詢包含Address的用戶
            return await userManager.Users
                .Include(x => x.Address)
                .SingleOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Email查詢帳號資訊
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static async Task<AppUsers> FindEmailByClaimPirincipal(this UserManager<AppUsers> userManager,
            ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            // 使用email查詢用戶
            return await userManager.Users
                .SingleOrDefaultAsync(u => u.Email == email);
        }
    }
}
