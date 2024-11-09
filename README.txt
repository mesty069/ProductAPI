1.印出所有 API 被呼叫以及呼叫外部 API 的 request and response body log
2.Error handling 處理 API response
3.swagger-ui
4.多語系設計
5.design pattern 實作
	(1)Repository Pattern：CurrencyService 作為資料存取層的一部分，
	負責封裝 ApplicationDbContext 的 CRUD 操作。
	(2)Dependency Injection：CurrencyService 使用建構子注入 ApplicationDbContext。