using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Product.Core.Entities;
using Product.Infrastructure.Repository;
using Product.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq.Protected;

namespace Product.Tests
{
    [TestClass]
    public class CurrencyServiceTests
    {
        private CurrencyService _currencyService;
        private Mock<ApplicationDbContext> _contextMock;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;

        /// <summary>
        /// 設置測試環境，初始化 InMemoryDatabase、HttpClient 模擬及 CurrencyService
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationDbContext(options);
            _currencyService = new CurrencyService(context);
        }

        /// <summary>
        /// 測試查詢所有貨幣的方法，檢查返回的貨幣列表是否包含新增的貨幣項目
        /// </summary>
        [TestMethod]
        public async Task GetAllCurrenciesAsync_ShouldReturnAllCurrencies()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Currencies.AddRange(new List<Currency>
                {
                    new Currency { CurrencyCode = "USD", CurrencyName_en = "Dollar", CurrencyName_zh = "美元", ExchangeRate = 1.0M },
                    new Currency { CurrencyCode = "EUR", CurrencyName_en = "Euro", CurrencyName_zh = "歐元", ExchangeRate = 0.9M }
                });
                await context.SaveChangesAsync();

                var currencyService = new CurrencyService(context);

                // Act
                var result = await currencyService.GetAllCurrenciesAsync();

                // Assert
                Assert.AreEqual(2, result.Count(), $"Expected 2 currencies but found {result.Count()}.");
                Assert.IsTrue(result.Any(c => c.CurrencyCode == "USD"));
                Assert.IsTrue(result.Any(c => c.CurrencyCode == "EUR"));
            }
        }

        /// <summary>
        /// 測試新增貨幣的方法，確認返回的貨幣資訊是否正確
        /// </summary>
        [TestMethod]
        public async Task CreateCurrencyAsync_ShouldAddCurrency()
        {
            var currency = new Currency
            {
                CurrencyCode = "GBP",
                CurrencyName_en = "British Pound Sterling",
                CurrencyName_zh = "英鎊",
                ExchangeRate = 0.75M
            };

            var result = await _currencyService.CreateCurrencyAsync(currency);

            Assert.IsNotNull(result);
            Assert.AreEqual("GBP", result.CurrencyCode);
            Assert.AreEqual("British Pound Sterling", result.CurrencyName_en);
            Assert.AreEqual("英鎊", result.CurrencyName_zh);
        }

        /// <summary>
        /// 測試依照貨幣代碼查詢貨幣的方法，確認返回的貨幣是否符合要求
        /// </summary>
        [TestMethod]
        public async Task GetCurrencyByCodeAsync_ShouldReturnCurrency_WhenExists()
        {
            // 建立 In-Memory 資料庫配置，使用唯一的資料庫名稱來避免測試間的資料衝突
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // 為每個測試使用獨立的資料庫
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                // 初始化資料庫並插入測試資料
                var currency = new Currency
                {
                    CurrencyCode = "JPY",
                    CurrencyName_en = "Japanese Yen",
                    CurrencyName_zh = "日元",
                    ExchangeRate = 110.0M
                };
                context.Currencies.Add(currency);
                await context.SaveChangesAsync(); // 儲存變更

                // 建立 CurrencyService 服務
                var currencyService = new CurrencyService(context);

                // 執行查詢動作
                var result = await currencyService.GetCurrencyByCodeAsync("JPY");

                // 驗證查詢結果
                Assert.IsNotNull(result); // 檢查結果不為 null
                Assert.AreEqual("JPY", result.CurrencyCode); // 檢查幣別代碼
                Assert.AreEqual("Japanese Yen", result.CurrencyName_en); // 檢查幣別英文名稱
            }
        }



        /// <summary>
        /// 測試更新貨幣的方法，確認更新後的資料是否符合預期
        /// </summary>
        [TestMethod]
        public async Task UpdateCurrencyAsync_ShouldUpdateCurrency_WhenExists()
        {
            // 建立 In-Memory 資料庫配置，使用唯一的資料庫名稱來避免測試間的資料衝突
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // 每次使用新的資料庫
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                // 初始化資料庫並插入要更新的貨幣資料
                var currency = new Currency
                {
                    CurrencyCode = "USD",
                    CurrencyName_en = "Dollar",
                    CurrencyName_zh = "美元",
                    ExchangeRate = 1.0M
                };
                context.Currencies.Add(currency);
                await context.SaveChangesAsync(); // 儲存初始資料

                // 建立 CurrencyService 實例
                var currencyService = new CurrencyService(context);

                // 準備更新的資料
                var updatedCurrency = new Currency
                {
                    CurrencyCode = "USD",
                    CurrencyName_en = "US Dollar",
                    CurrencyName_zh = "美元",
                    ExchangeRate = 1.1M
                };

                // 執行更新操作
                var result = await currencyService.UpdateCurrencyAsync("USD", updatedCurrency);

                // 驗證更新後的結果
                Assert.AreEqual("US Dollar", result.CurrencyName_en); // 檢查英文名稱是否更新
                Assert.AreEqual(1.1M, result.ExchangeRate); // 檢查匯率是否更新
            }
        }


        /// <summary>
        /// 測試刪除貨幣的方法，確認成功刪除後返回的結果
        /// </summary>
        /// <summary>
        /// 測試刪除貨幣的方法，確認成功刪除後返回的結果
        /// </summary>
        [TestMethod]
        public async Task DeleteCurrencyAsync_ShouldReturnTrue_WhenCurrencyExists()
        {
            // 使用 InMemoryDatabase 的唯一实例，确保测试隔离
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // 每次创建新的数据库
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var currencyService = new CurrencyService(context);

                var currency = new Currency
                {
                    CurrencyCode = "AUD",
                    CurrencyName_en = "Australian Dollar",
                    CurrencyName_zh = "澳元",
                    ExchangeRate = 1.4M
                };

                // 在数据库中添加该货币
                context.Currencies.Add(currency);
                await context.SaveChangesAsync();

                // 调用服务的删除方法
                var result = await currencyService.DeleteCurrencyAsync("AUD");

                // 断言返回结果为 true
                Assert.IsTrue(result);
            }
        }


        /// <summary>
        /// 測試刪除不存在的貨幣，確認返回的結果為 false
        /// </summary>
        [TestMethod]
        public async Task DeleteCurrencyAsync_ShouldReturnFalse_WhenCurrencyDoesNotExist()
        {
            var result = await _currencyService.DeleteCurrencyAsync("NonExistingCode");

            Assert.IsFalse(result);
        }

        /// <summary>
        /// 測試從 Coindesk API 獲取匯率的功能，這裡模擬 HttpClient 來測試 FetchCoindeskDataAsync 是否能正確解析並返回匯率資料
        /// </summary>
        [TestMethod]
        public async Task FetchCoindeskDataAsync_ShouldReturnCurrencies_WhenDataIsAvailable()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // 每次使用新的資料庫
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                // 清空並初始化資料庫
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                // 模擬 HttpClient 回應
                var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
                var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(@"
                    {
                        ""bpi"": {
                            ""USD"": { ""code"": ""USD"", ""rate_float"": 1.0, ""description"": ""United States Dollar"" },
                            ""EUR"": { ""code"": ""EUR"", ""rate_float"": 0.9, ""description"": ""Euro"" },
                            ""GBP"": { ""code"": ""GBP"", ""rate_float"": 0.8, ""description"": ""British Pound Sterling"" }
                        }
                    }")
                };

                mockHttpMessageHandler
                    .Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>()
                    )
                    .ReturnsAsync(mockResponse);

                var httpClient = new HttpClient(mockHttpMessageHandler.Object);
                var currencyService = new CurrencyService(context);

                // Act
                var result = await currencyService.FetchCoindeskDataAsync();

                // Assert
                // 驗證特定幣別是否存在且資料正確，而非驗證總筆數
                Assert.IsTrue(result.Any(c => c.CurrencyCode == "USD" && c.CurrencyName_en == "United States Dollar"));
                Assert.IsTrue(result.Any(c => c.CurrencyCode == "EUR" && c.CurrencyName_en == "Euro"));
                Assert.IsTrue(result.Any(c => c.CurrencyCode == "GBP" && c.CurrencyName_en == "British Pound Sterling"));

                // 驗證資料庫中確實包含這些特定幣別資料
                var storedCurrencies = await context.Currencies.ToListAsync();
                Assert.IsTrue(storedCurrencies.Any(c => c.CurrencyCode == "USD" && c.CurrencyName_en == "United States Dollar"));
                Assert.IsTrue(storedCurrencies.Any(c => c.CurrencyCode == "EUR" && c.CurrencyName_en == "Euro"));
                Assert.IsTrue(storedCurrencies.Any(c => c.CurrencyCode == "GBP" && c.CurrencyName_en == "British Pound Sterling"));
            }
        }

    }
}
