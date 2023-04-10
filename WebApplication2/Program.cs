using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;
using WebApplication2;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureWebHostDefaults(webBuilder =>
{
    webBuilder.ConfigureKestrel(options =>
    {
        var port = 5001;
        var pfxFilePath = @"D:\workshop.ursatile.com\certificate.pfx";
        // The password you specified when exporting the PFX file using OpenSSL.
        // This would normally be stored in configuration or an environment variable;
        // I've hard-coded it here just to make it easier to see what's going on.
        var pfxPassword = "green cairo angle piano";

        options.Listen(IPAddress.Any, port, listenOptions =>
        {
            // Enable support for HTTP1 and HTTP2 (required if you want to host gRPC endpoints)
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
            // Configure Kestrel to use a certificate from a local .PFX file for hosting HTTPS
            listenOptions.UseHttps(pfxFilePath, pfxPassword);
        });
    });
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<RazorViewEngineOptions>(o =>
{
    o.ViewLocationExpanders.Add(new LocationExpanderService());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
