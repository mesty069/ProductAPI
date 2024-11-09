using Microsoft.AspNetCore.Mvc;
using Product.API.Errors;
using Product.Infrastructure.Data;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BugController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 假設沒找到幣別資訊業面
        /// </summary>
        /// <returns></returns>
        [HttpGet("not-found")]
        public ActionResult GetNotFound()
        {
            var product = _context.Currencies.Find(50);
            if (product is null)
            {
                return NotFound(new BaseCommonResponse(404));
            }
            return Ok(product);
        }

        /// <summary>
        /// 取得伺服器錯誤
        /// </summary>
        /// <returns></returns>
        [HttpGet("Server-error")]
        public ActionResult GetServerError()
        {
            var product = _context.Currencies.Find(100);
            product.CurrencyCode = "";
            return Ok();
        }

        [HttpGet("Bad-Request")]
        public ActionResult GetBadRequest()
        {
            return BadRequest();
        }
    }
}
