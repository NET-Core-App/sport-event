using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;
using SportEvent.Data;
using SportEvent.Data.Implementations;
using SportEvent.Data.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
/*Log.Logger = new LoggerConfiguration()
*//*    .WriteTo.Console()*/
/*    .WriteTo.File("wwwroot/logger.txt", rollingInterval: RollingInterval.Day)*//*
    .WriteTo.File(new CompactJsonFormatter(), "wwwroot/logger.json", rollingInterval: RollingInterval.Day)
*//*    .MinimumLevel.Information()*//*
    .CreateLogger();
*/
var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "logger.json");
var logEvents = new List<LogEvent>();
Log.Logger = new LoggerConfiguration()
    .WriteTo.Sink(new CollectionSink(logEvents, jsonFilePath))
    .CreateLogger();


// Add services to the container.
builder.Services.AddSerilog();
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddLogging();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // the session timeout.
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IOrganizerRepository, OrganizerRepository>();
builder.Services.AddTransient<ISportEventRepository, SportEventRepository>();
builder.Services.AddTransient<AuthorizationService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

/*app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();

        context.Response.Redirect("/Home/Error");
    });
});*/


app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.UseSerilogRequestLogging(opt =>
{
    opt.GetLevel = (httpContext, elapsed, ex) =>
    {
        if (httpContext.Request.Method == "GET" ||
            httpContext.Request.Method == "POST" ||
            httpContext.Request.Method == "PUT" ||
            httpContext.Request.Method == "DELETE")
        {
            return LogEventLevel.Information;
        }

        return LogEventLevel.Verbose;
    };
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
