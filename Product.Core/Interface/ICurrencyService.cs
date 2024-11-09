using Product.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.Core.Interface
{
    public interface ICurrencyService
    {
        // 查詢所有貨幣並依幣別代碼排序
        Task<IEnumerable<Currency>> GetAllCurrenciesAsync();

        // 根據 CurrencyCode 查詢單一貨幣
        Task<Currency> GetCurrencyByCodeAsync(string currencyCode);

        // 新增貨幣
        Task<Currency> CreateCurrencyAsync(Currency currency);

        // 更新貨幣
        Task<Currency> UpdateCurrencyAsync(string currencyCode, Currency currency);

        // 刪除貨幣
        Task<bool> DeleteCurrencyAsync(string currencyCode);

        // 從 Coindesk API 獲取最新的匯率資訊
        Task<List<Currency>> FetchCoindeskDataAsync();
    }
}
