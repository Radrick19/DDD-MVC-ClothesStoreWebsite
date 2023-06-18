using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Store.Application.AutoMapper;
using Store.Application.Services.ArticleGeneratorSertvice;
using Store.Application.Services.CaptchaValidatorService;
using Store.Application.Services.DatabaseCleanerService;
using Store.Application.Services.ProductPopularityService;
using Store.Domain;
using Store.Domain.DatabaseRepositories.Postgre;
using Store.Domain.Infrastructure;
using Store.Domain.Interfaces;
using Store.Domain.Models;
using Store.Domain.Models.ManyToManyProductEntities;
using Store.Domain.Models.ProductEntities;
using Store.Mvc.Services.CartService;
using Store.Mvc.Services.EmailService;
using Store.Mvc.Services.PicturesControlService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc().AddRazorRuntimeCompilation();

builder.Services.AddAutoMapper(typeof(AppMappingProfile));

builder.Logging.ClearProviders();
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

builder.Services.AddHangfire(h => h.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

#region DIServices

builder.Services.AddScoped<StoreContext>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IArticleGeneratorService, ArticleGeneratorService>();

builder.Services.AddScoped<IPicturesControlService, PicturesControlService>();

builder.Services.AddScoped<IDatabaseCleanerService, DatabaseCleanerService>();

builder.Services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));

builder.Services.AddTransient<IRepository<Product>, ProductRepository>();

builder.Services.AddTransient<IRepository<CollectionProduct>, CollectionProductRepository>();

builder.Services.AddTransient<IRepository<Subcategory>, SubcategoryRepository>();

builder.Services.AddTransient<IRepository<UserEmailConfirmationHash>, UserEmailConfirmationHashRepository>();

builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddTransient<ICartService, CartService>();

builder.Services.AddTransient<ICaptchaValidatorService, CaptchaValidatorService>();

builder.Services.AddTransient<IProductPopularityService, ProductPopularityService>();

builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddHttpClient<CaptchaValidatorService>();

#endregion

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();

try
{
    logger.Debug("Application started");

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseHangfireDashboard("/dashboard");
    }
    else
    {
        app.UseExceptionHandler("/error/errormessage?message={0}");
        app.UseStatusCodePagesWithReExecute("/error/errormessage", "?message={0}");
    }

    app.UseAuthentication();

    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endponints =>
    {
        endponints.MapControllerRoute("Default", "{controller=home}/{action=index}/{id?}");
    });

    app.UseStaticFiles();

    RecurringJob.AddOrUpdate<IDatabaseCleanerService>(service => service.DeleteUnactiveConfirmHashes(), Cron.Daily);
    RecurringJob.AddOrUpdate<IDatabaseCleanerService>(service => service.DeleteUnactivatedUsers(), Cron.Daily);
    RecurringJob.AddOrUpdate<IDatabaseCleanerService>(service => service.DeleteUnusedMainProductPictures(), Cron.Daily);
    RecurringJob.AddOrUpdate<IDatabaseCleanerService>(service => service.DeleteUnusedAdditionalProductPictures(), Cron.Daily);
    RecurringJob.AddOrUpdate<IDatabaseCleanerService>(service => service.DeleteUnusedPromoBgPictures(), Cron.Daily);

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Program stopped because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}