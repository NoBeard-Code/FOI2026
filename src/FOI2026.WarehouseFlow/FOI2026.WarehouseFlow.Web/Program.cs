using FOI2026.WarehouseFlow.Web.Components;
using FOI2026.WarehouseFlow.Web.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services;
using FOI2026.WarehouseFlow.Services.Models;
using FOI2026.WarehouseFlow.Services.Repository_Interfaces;
using FOI2026.WarehouseFlow.Services.Services;
using FOI2026.WarehouseFlow.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
})
.AddRoles<ApplicationRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddSignInManager()
.AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// Repositories
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// Services
builder.Services.AddScoped<ArticleService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();


if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

    ctx.Database.Migrate();

    if (!ctx.Roles.Any())
    {
        await roleManager.CreateAsync(new ApplicationRole { Name = "Admin" });
        await roleManager.CreateAsync(new ApplicationRole { Name = "Worker" });
    }

    ApplicationUser admin;

    if (!ctx.Users.Any())
    {
        admin = new ApplicationUser
        {
            UserName = "admin",
            Email = "admin@admin.com",
            FirstName = "Admin",
            LastName = "Admin",
            IsActive = true,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(admin, "Admin123!");
        await userManager.AddToRoleAsync(admin, "Admin");
    }
    else
    {
        admin = ctx.Users.First();
    }

    if (!ctx.Categories.Any())
    {
        ctx.Categories.AddRange(
            new Category
            {
                Name = "Elektronika",
                Description = "Elektronièka oprema i dijelovi"
            },
            new Category
            {
                Name = "Ambalaža",
                Description = "Materijali za pakiranje"
            },
            new Category
            {
                Name = "Alat",
                Description = "Radni alati i oprema"
            }
        );

        await ctx.SaveChangesAsync();
    }

    if (!ctx.Partners.Any())
    {
        ctx.Partners.AddRange(
            new Partner
            {
                Name = "Dobavljaè d.o.o.",
                OIB = 123456789,
                Address = "Zagrebaèka 1, Varaždin",
                Contact = "0911234567",
                Email = "dobavljac@test.com",
                IsSupplier = true
            },
            new Partner
            {
                Name = "Kupac d.o.o.",
                OIB = 987654321,
                Address = "Industrijska 5, Zagreb",
                Contact = "0987654321",
                Email = "kupac@test.com",
                IsSupplier = false
            }
        );

        await ctx.SaveChangesAsync();
    }

    if (!ctx.Articles.Any())
    {
        var elektronika = ctx.Categories.First(c => c.Name == "Elektronika");
        var ambalaza = ctx.Categories.First(c => c.Name == "Ambalaža");
        var alat = ctx.Categories.First(c => c.Name == "Alat");

        ctx.Articles.AddRange(
            new Article
            {
                Name = "Barkod skener",
                Code = "ART-001",
                Unit = "kom",
                MinStock = 5,
                MaxStock = 50,
                Description = "Ruèni barkod skener",
                Status = "Aktivan",
                CategoryId = elektronika.CategoryId
            },
            new Article
            {
                Name = "Kartonska kutija",
                Code = "ART-002",
                Unit = "kom",
                MinStock = 100,
                MaxStock = 1000,
                Description = "Kutija za pakiranje robe",
                Status = "Aktivan",
                CategoryId = ambalaza.CategoryId
            },
            new Article
            {
                Name = "Ruèni vilièar",
                Code = "ART-003",
                Unit = "kom",
                MinStock = 1,
                MaxStock = 10,
                Description = "Vilièar za skladište",
                Status = "Aktivan",
                CategoryId = alat.CategoryId
            }
        );

        await ctx.SaveChangesAsync();
    }

    if (!ctx.Orders.Any())
    {
        var partner = ctx.Partners.First(p => !p.IsSupplier);
        var article = ctx.Articles.First();

        ctx.Orders.Add(
            new Order
            {
                Date = DateTime.Now,
                Code = "ORD-001",
                PartnerId = partner.PartnerId,
                ArticleId = article.ArticleId,
                Status = "Kreirano",
                Description = "Testna narudžba",
                UserId = admin.Id
            }
        );

        await ctx.SaveChangesAsync();
    }

    if (!ctx.OrderItems.Any())
    {
        var order = ctx.Orders.First();
        var article = ctx.Articles.First();

        ctx.OrderItems.Add(
            new OrderItem
            {
                OrderId = order.OrderId,
                ArticleId = article.ArticleId,
                Quantity = 10,
                Price = 25.50m
            }
        );

        await ctx.SaveChangesAsync();
    }

    if (!ctx.DeliveryNotes.Any())
    {
        var supplier = ctx.Partners.First(p => p.IsSupplier);
        var article = ctx.Articles.First();

        ctx.DeliveryNotes.Add(
            new DeliveryNote
            {
                Code = "DN-001",
                Date = DateTime.Now,
                PartnerId = supplier.PartnerId,
                ArticleId = article.ArticleId,
                Status = "Zaprimljeno",
                Description = "Testna primka",
                UserId = admin.Id
            }
        );

        await ctx.SaveChangesAsync();
    }

    if (!ctx.DeliveryNoteItems.Any())
    {
        var deliveryNote = ctx.DeliveryNotes.First();
        var article = ctx.Articles.First();

        ctx.DeliveryNoteItems.Add(
            new DeliveryNoteItem
            {
                DeliveryNoteId = deliveryNote.DeliveryNoteId,
                ArticleId = article.ArticleId,
                Quantity = 20,
                Price = 20.00m
            }
        );

        await ctx.SaveChangesAsync();
    }
}

app.Run();