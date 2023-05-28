using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Store.Api.Interfaces;
using Store.API.Infrastructure;
using Store.API.Middlewares;
using Store.Application.AutoMapper;
using Store.Application.Infrastructure;
using Store.Application.Interfaces;
using Store.Domain;
using Store.Domain.DatabaseRepositories.Postgre;
using Store.Domain.Infrastructure;
using Store.Domain.Interfaces;
using Store.Domain.Models.ManyToManyProductEntities;
using Store.Domain.Models.ProductEntities;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddMvc().AddRazorRuntimeCompilation();

    builder.Services.AddAutoMapper(typeof(AppMappingProfile));

    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
    builder.Host.UseNLog();

    builder.Services.AddDbContext<StoreContext>(
        options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddAuthentication("Cookies")
        .AddCookie(option =>
        {
            option.LoginPath = "/account/login";
            option.AccessDeniedPath = "/accessdenied";
        });

    builder.Services.AddAuthorization();

    #region DIServices

    builder.Services.AddScoped<StoreContext>();

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    builder.Services.AddScoped<IArticleGenerator, ArticleGenerator>();

    builder.Services.AddScoped<IPicturesControl, PicturesControl>();

    builder.Services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));

    builder.Services.AddTransient<IRepository<Product>, ProductRepository>();

    builder.Services.AddTransient<IRepository<CollectionProduct>, CollectionProductRepository>();

    builder.Services.AddTransient<IRepository<Subcategory>, SubcategoryRepository>();

    builder.Services.AddTransient<ICartService, CartService>();

    builder.Services.AddTransient<IProductPopularityService, ProductPopularityService>();

    builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

    #endregion

    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    var app = builder.Build();

    app.UseAuthentication();

    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endponints =>
    {
        endponints.MapControllerRoute("Default", "{controller=home}/{action=index}/{id?}");
    });

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseExceptionHandler("/Error/ErrorMessage");
    }

    app.UseStaticFiles();

    app.Run();
}
catch(Exception ex)
{
    logger.Error(ex, "Program stopped because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}