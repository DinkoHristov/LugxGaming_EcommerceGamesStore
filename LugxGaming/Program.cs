using LugxGaming.BusinessLogic.Interfaces;
using LugxGaming.BusinessLogic.Models.Payment;
using LugxGaming.BusinessLogic.Services;
using LugxGaming.Data.Data;
using LugxGaming.Data.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICurrencyInterface, CurrencyService>();
builder.Services.AddScoped<IAccountInterface, AccountService>();
builder.Services.AddScoped<IContactUsInterface, ContactUsService>();
builder.Services.AddScoped<IHomeInterface, HomeService>();
builder.Services.AddScoped<IShopInterface, ShopService>();
builder.Services.AddScoped<ICartInterface, CartService>();
builder.Services.AddScoped<ICreateGameInterface, CreateGameService>();
builder.Services.AddScoped<IPaymentInterface, PaymentService>();
builder.Services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => 
{ 
    options.SignIn.RequireConfirmedAccount = true; 
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
    .AddRoles<IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(3);
    options.Cookie.IsEssential = true;
});

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/SignIn";
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ApplicationDbContext>();

    await LugxGaming.Data.DataSeed.AddAdmin(scope.ServiceProvider);
    await LugxGaming.Data.DataSeed.AddGenres(dbContext);
    await LugxGaming.Data.DataSeed.AddGames(dbContext);

    await dbContext.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseStatusCodePagesWithReExecute("/Home/Page404");
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/Home/Page404");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
