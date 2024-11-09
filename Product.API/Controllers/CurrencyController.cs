using Microsoft.AspNetCore.Mvc;
using Product.Core.Entities;
using Product.Core.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using i18n.Resource;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyRatesController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyRatesController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        /// <summary>
        /// 取得所有貨幣資訊
        /// </summary>
        /// <returns>貨幣列表</returns>
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<Currency>>> GetCurrencies()
        {
            var currencies = await _currencyService.GetAllCurrenciesAsync();
            return Ok(currencies);
        }

        /// <summary>
        /// 創建新的貨幣
        /// </summary>
        /// <param name="currency">貨幣資訊</param>
        /// <returns>創建的貨幣資訊</returns>
        [HttpPost("create")]
        public async Task<ActionResult<Currency>> CreateCurrency(Currency currency)
        {
            var createdCurrency = await _currencyService.CreateCurrencyAsync(currency);
            return CreatedAtAction(nameof(GetCurrencyByCode), new { currencyCode = createdCurrency.CurrencyCode }, createdCurrency);
        }

        /// <summary>
        /// 根據 CurrencyCode 取得特定貨幣資訊
        /// </summary>
        /// <param name="currencyCode">貨幣代碼</param>
        /// <returns>貨幣資訊</returns>
        [HttpGet("get-by-code/{currencyCode}")]
        public async Task<ActionResult<Currency>> GetCurrencyByCode(string currencyCode)
        {
            var currency = await _currencyService.GetCurrencyByCodeAsync(currencyCode);
            if (currency == null)
                return NotFound(new { message = string.Format(Message.NotFound, Label.CurrencyCode) });

            return Ok(currency);
        }

        /// <summary>
        /// 更新指定 CurrencyCode 的貨幣
        /// </summary>
        /// <param name="currencyCode">貨幣代碼</param>
        /// <param name="currency">更新的貨幣資訊</param>
        /// <returns>無內容</returns>
        [HttpPut("update/{currencyCode}")]
        public async Task<IActionResult> UpdateCurrency(string currencyCode, [FromBody] Currency currency)
        {
            await _currencyService.UpdateCurrencyAsync(currencyCode, currency);
            return NoContent();
        }

        /// <summary>
        /// 刪除指定 CurrencyCode 的貨幣
        /// </summary>
        /// <param name="currencyCode">貨幣代碼</param>
        /// <returns>無內容</returns>
        [HttpDelete("delete/{currencyCode}")]
        public async Task<IActionResult> DeleteCurrency(string currencyCode)
        {
            var deleted = await _currencyService.DeleteCurrencyAsync(currencyCode);
            if (!deleted)
                return NotFound(new { message = string.Format(Message.NotFound, Label.CurrencyCode) });

            return NoContent();
        }

        /// <summary>
        /// 獲取 Coindesk 數據
        /// </summary>
        /// <returns>Coindesk 數據</returns>
        [HttpGet("fetch-coindesk")]
        public async Task<IActionResult> FetchCoindeskData()
        {
            var currencyData = await _currencyService.FetchCoindeskDataAsync();
            return Ok(currencyData);
        }
    }
}
