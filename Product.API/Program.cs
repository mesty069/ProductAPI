using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Product.API.Middleware;
using Product.Infrastructure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ��ӱ��ػ�����
builder.Services.AddLocalization(options => options.ResourcesPath = "Resource");

// �O��֧Ԯ���Z���Ļ�
var supportedCultures = new[] { "en-US", "zh-CN" }; // ���ԔU������Z��

// ��ӿ������K�O���x�
builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

// ��� CORS ԭ�t
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", builder =>
    {
        builder.WithOrigins("http://localhost:3000") // �޸Ğ錍�H��Ҫ�ā�Դ
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// ��� Swagger ֧��
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Currency API", Version = "v1" });
});

// ��ӆ����هע������
builder.Services.InfraStructureConfigration(builder.Configuration);

var app = builder.Build();

// ȫ���e�`̎�����g����ʹ�� ProblemDetails �����e�`�YӍ
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

// ���ö��Zϵ���g���K�O��֧Ԯ���Z���Ļ�
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);

// ���Iӛ����g��
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

// ���� Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Currency API v1");
    c.RoutePrefix = string.Empty; // �O�Þ��·��
});

// ��B�a���
app.UseStatusCodePagesWithReExecute("/errors/{0}");

// �J�C���ڙ�
app.UseAuthentication();
app.UseAuthorization();

// �o�B�ļ�֧��
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
    RequestPath = "/static"
});

// �]�Կ�����
app.MapControllers();

// ��ӆ�����g������
InfraStructureRequistration.InfrastructureConfigMiddleware(app);

app.Run();
