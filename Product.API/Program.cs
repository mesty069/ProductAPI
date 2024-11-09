using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Product.API.Middleware;
using Product.Infrastructure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// 添加本地化服務
builder.Services.AddLocalization(options => options.ResourcesPath = "Resource");

// 設定支援的語言文化
var supportedCultures = new[] { "en-US", "zh-CN" }; // 可以擴充更多語言

// 添加控制器並設置選項
builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

// 添加 CORS 原則
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", builder =>
    {
        builder.WithOrigins("http://localhost:3000") // 修改為實際需要的來源
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// 添加 Swagger 支持
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Currency API", Version = "v1" });
});

// 自訂的依賴注入配置
builder.Services.InfraStructureConfigration(builder.Configuration);

var app = builder.Build();

// 全局錯誤處理中間件，使用 ProblemDetails 來返回錯誤資訊
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
        if (exception != null)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred.",
                Detail = exception.Message
            };
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    });
});

// 啟用多語系中間件並設置支援的語言文化
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);

// 日誌記錄中間件
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

// 啟用 Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Currency API v1");
    c.RoutePrefix = string.Empty; // 設置為根路徑
});

// 狀態碼頁面
app.UseStatusCodePagesWithReExecute("/errors/{0}");

// 認證和授權
app.UseAuthentication();
app.UseAuthorization();

// 靜態文件支持
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
    RequestPath = "/static"
});

// 註冊控制器
app.MapControllers();

// 自訂的中間件配置
InfraStructureRequistration.InfrastructureConfigMiddleware(app);

app.Run();
