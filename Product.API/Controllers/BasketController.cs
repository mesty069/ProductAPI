using Microsoft.AspNetCore.Mvc;
using Product.Core.Entities;
using Product.Core.Interface;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IUnitOfWork _uOW;


        public BasketController(IUnitOfWork UOW)
        {
            _uOW = UOW;
        }

        /// <summary>
        /// 取得購物車資訊
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("get-basket-item/{Id}")]
        public async Task<IActionResult> GetBasketById(string Id)
        {
            var _basket = await _uOW.BasketRepository.GetBasketAsync(Id);
            return Ok(_basket ?? new CustomerBasket(Id));
        }

        /// <summary>
        /// 更新購物車
        /// </summary>
        /// <param name="customerBasket"></param>
        /// <returns></returns>
        [HttpPost("update-basket")]
        public async Task<IActionResult> UpdateBasket(CustomerBasket customerBasket)
        {
            var _basket = await _uOW.BasketRepository.UpdateBasketAsync(customerBasket);

            return Ok(_basket);
        }

        /// <summary>
        /// 刪除購物車項目
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("delete-basket-item/{Id}")]
        public async Task<IActionResult> DeleteBasket(string Id)
        {
            return Ok(await _uOW.BasketRepository.DeleteBasketAsync(Id));
        }
    }
}
