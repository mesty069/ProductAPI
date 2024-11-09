using i18n.Resource;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly IHostEnvironment _environment;
        private readonly List<string> _supportedCultures;

        public LanguageController(IHostEnvironment environment)
        {
            _environment = environment;
            _supportedCultures = GetAvailableCultures();
        }

        /// <summary>
        /// 切換語言
        /// </summary>
        /// <param name="culture">語言代碼，例如 "en-US" 或 "zh-CN"</param>
        /// <returns>成功訊息</returns>
        [HttpPost("set-language")]
        public IActionResult SetLanguage(string culture)
        {
            if (string.IsNullOrEmpty(culture))
            {
                string errorMessage = string.Format(Message.IsRequired, Label.Language);
                return BadRequest(new { message = errorMessage });
            }

            if (!_supportedCultures.Contains(culture))
            {
                string errorMessage = string.Format(Message.NotFound, culture);
                return BadRequest(new { message = errorMessage });
            }

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            string successMessage = string.Format(Message.LanguageSwitched, culture);
            return Ok(new { message = successMessage });
        }

        /// <summary>
        /// 測試語言切換的顯示
        /// </summary>
        /// <returns>顯示當前語言的測試訊息</returns>
        [HttpGet("get-message")]
        public IActionResult GetMessage()
        {
            string testMessage = Label.TestMessage;
            return Ok(new { message = testMessage });
        }

        private List<string> GetAvailableCultures()
        {
            var cultures = new List<string>();

            // 取得 ContentRootPath 的上一層資料夾
            var rootPath = Directory.GetParent(_environment.ContentRootPath)?.FullName;
            if (rootPath == null) return cultures;

            var resourcePath = Path.Combine(rootPath, "i18n", "Resource");

            if (Directory.Exists(resourcePath))
            {
                var files = Directory.GetFiles(resourcePath, "*.resx");
                foreach (var file in files)
                {
                    var cultureName = Path.GetFileNameWithoutExtension(file)
                        .Replace("Label.", "")
                        .Replace("Message.", "");

                    if (!string.IsNullOrEmpty(cultureName) && cultureName != "Label" && cultureName != "Message")
                    {
                        cultures.Add(cultureName);
                    }
                }
            }

            return cultures;
        }
    }
}
