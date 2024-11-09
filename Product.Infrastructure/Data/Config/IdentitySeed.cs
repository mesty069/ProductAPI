using Microsoft.AspNetCore.Identity;
using Product.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure.Data.Config
{
    public class IdentitySeed
    {
        public static async Task SeedUserAsync(UserManager<AppUsers> userManager)
        {
            //if (!userManager.Users.Any())
            //{
            //    var user = new AppUsers()
            //    {
            //        DisplayName = "鄭亦淳",
            //        Email = "mesty069@gmail.com",
            //        UserName = "mesty069@gmail.com",
            //        Address = new Address()
            //        {
            //            FirstName = "鄭",
            //            LastName = "亦淳",
            //            City = "台北市",
            //            State = "文山區",
            //            Street = "興隆路一段",
            //            ZipCode = "123"
            //        }
            //    };
            //    await userManager.CreateAsync(user, "P@$$w0rd");
            //}
        }
    }
}
