using Maib.Sdk;
using Maib.Sdk.Extensions;
using Maib.Sdk.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddMaibClient(new MaibClientConfiguration
{
    BaseUrl = MaibConstants.MAIB_TEST_BASE_URI,
    RedirectBaseUrl = MaibConstants.MAIB_TEST_REDIRECT_URL,
    CertificatePath = "Assets\\0149583.pfx",
    CertificatePassword = MaibConstants.MAIB_TEST_CERT_PASS
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
