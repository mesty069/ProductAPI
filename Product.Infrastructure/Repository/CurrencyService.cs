using i18n.Resource;
using Microsoft.EntityFrameworkCore;
using Product.Core.Entities;
using Product.Core.Interface;
using Product.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Product.Infrastructure.Repository
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;

        public CurrencyService(ApplicationDbContext context)
        {
            _context = context;
            _httpClient = new HttpClient(); // 用於呼叫外部 API
        }

        // 查詢所有貨幣並依幣別代碼排序
        public async Task<IEnumerable<Currency>> GetAllCurrenciesAsync()
        {
            return await _context.Currencies.OrderBy(c => c.CurrencyCode).ToListAsync();
        }

        // 新增貨幣
        public async Task<Currency> CreateCurrencyAsync(Currency currency)
        {
            // 設定初始建立時間
            currency.CreateDatetime = DateTime.UtcNow;

            _context.Currencies.Add(currency);
            await _context.SaveChangesAsync();
            return currency;
        }

        // 根據 CurrencyCode 查詢單一貨幣
        public async Task<Currency> GetCurrencyByCodeAsync(string currencyCode)
        {
            return await _context.Currencies.FindAsync(currencyCode);
        }

        // 更新貨幣
        public async Task<Currency> UpdateCurrencyAsync(string currencyCode, Currency currency)
        {
            var existingCurrency = await _context.Currencies.FindAsync(currencyCode);
            if (existingCurrency == null)
                throw new KeyNotFoundException(string.Format(Message.NotFound, Label.CurrencyCode));

            existingCurrency.CurrencyName_en = currency.CurrencyName_en;
            existingCurrency.CurrencyName_zh = currency.CurrencyName_zh;
            existingCurrency.ExchangeRate = currency.ExchangeRate;

            // 更新更新時間
            existingCurrency.UpdatedDatetime = DateTime.UtcNow;

            _context.Currencies.Update(existingCurrency);
            await _context.SaveChangesAsync();
            return existingCurrency;
        }

        // 刪除貨幣
        public async Task<bool> DeleteCurrencyAsync(string currencyCode)
        {
            var currency = await _context.Currencies.FindAsync(currencyCode);
            if (currency == null)
                return false;

            _context.Currencies.Remove(currency);
            await _context.SaveChangesAsync();
            return true;
        }

        // 從 Coindesk API 獲取最新的匯率資訊
        public async Task<List<Currency>> FetchCoindeskDataAsync()
        {
            var response = await _httpClient.GetStringAsync("https://api.coindesk.com/v1/bpi/currentprice.json");

            var data = JsonDocument.Parse(response).RootElement;
            var currencies = new List<Currency>();

            // 獲取 "bpi" 中所有幣別
            var bpi = data.GetProperty("bpi");

            foreach (var currencyElement in bpi.EnumerateObject())
            {
                var currencyCode = currencyElement.Name;
                var rate = currencyElement.Value.GetProperty("rate_float").GetDecimal();
                var currencyNameEn = currencyElement.Value.GetProperty("description").GetString();
                var currencyNameZh = TranslateCurrencyToChinese(currencyNameEn); // 添加翻譯方法

                var currency = new Currency
                {
                    CurrencyCode = currencyCode,
                    CurrencyName_en = currencyNameEn,
                    CurrencyName_zh = currencyNameZh,
                    ExchangeRate = rate,
                    CreateDatetime = DateTime.UtcNow // 每次 API 資料新增的幣別都設置建立時間
                };

                // 檢查是否已存在該幣別，若存在則更新，不存在則新增
                var existingCurrency = await _context.Currencies
                    .FirstOrDefaultAsync(c => c.CurrencyCode == currency.CurrencyCode);

                if (existingCurrency != null)
                {
                    existingCurrency.ExchangeRate = currency.ExchangeRate;
                    existingCurrency.CurrencyName_en = currency.CurrencyName_en;
                    existingCurrency.CurrencyName_zh = currency.CurrencyName_zh;
                    existingCurrency.UpdatedDatetime = DateTime.UtcNow; // 更新更新時間
                    _context.Currencies.Update(existingCurrency);
                }
                else
                {
                    _context.Currencies.Add(currency);
                }

                currencies.Add(currency);
            }

            await _context.SaveChangesAsync();
            return currencies;
        }

        // 假設的翻譯方法，將英文轉換為中文。
        private string TranslateCurrencyToChinese(string englishName)
        {
             // 定義英文到中文的對應字典
             var ChineseTranslate = new Dictionary<string, string>
             {
                 { "Euro", "歐元" },
                 { "British Pound Sterling", "英鎊" },
                 { "United States Dollar", "美元" }
             };
            // 若字典中包含該英文名稱，則返回對應的中文名稱，否則返回原始名稱
            return ChineseTranslate.TryGetValue(englishName, out var chineseName) ? chineseName : englishName;
        }

    }
}
