using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using PoWeather.Components;
using PoWeather.Components.Account;
using PoWeather.Data;
using PoWeather.Services;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddHttpClient<WeatherService>();


builder.Services.AddHttpClient();
builder.Services.AddMudServices();

// builder.Services.AddSingleton<BlobStorageService>(provider =>
// {
//    // var connectionString = builder.Configuration.GetConnectionString("AzureStorage:ConnectionString");
//     return new BlobStorageService(connectionString2);
// });


var connectionString2 = builder.Configuration.GetConnectionString("StorageConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddSingleton<BlobServiceClient>(provider => new BlobServiceClient(connectionString2));

builder.Services.AddScoped<BlobStorageService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new BlobStorageService(connectionString2);
});

// builder.Services.AddScoped<BlobStorageService>();

//builder.Services.AddAzureClients(clientBuilder =>
//{
//    clientBuilder.AddBlobServiceClient(builder.Configuration["ConnectionStrings:StorageConnection:blob"], preferMsi: true);
//    clientBuilder.AddQueueServiceClient(builder.Configuration["ConnectionStrings:StorageConnection:queue"], preferMsi: true);
//});

//builder.Services.AddScoped<BlobStorageService>();

//services.AddScoped<BlobStorageService>(provider => new BlobStorageService(connectionString));



builder.Services.AddAzureClients(clientBuilder =>
{
    string storageConnectionString = builder.Configuration.GetConnectionString("StorageConnection");

    clientBuilder.AddBlobServiceClient(storageConnectionString, preferMsi: true);
    clientBuilder.AddQueueServiceClient(storageConnectionString, preferMsi: true);
});





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
