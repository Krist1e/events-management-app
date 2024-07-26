using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Commands.CreateEvent;
using EventsManagementApp.Common.Constants;
using EventsManagementApp.Infrastructure.Common.Persistence;
using EventsManagementApp.Infrastructure.Events.Persistence;
using EventsManagementApp.Infrastructure.Images.Persistence;
using EventsManagementApp.Infrastructure.Users.Persistence;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder();

#region Database Configuration

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

#region Identity Configuration

builder.Services.AddIdentityApiEndpoints<User>()
    .AddRoles<Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole(Roles.Admin));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole(Roles.User));
});

#endregion

#region Swagger Configuration

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

#endregion

#region Controllers Configuration

builder.Services.AddControllers();
builder.Services.AddApiVersioning();

#endregion

#region Services Configuration

builder.Services.AddScoped<IUnitOfWork, ApplicationDbContext>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();

builder.Services.AddScoped<IOptionsMonitor<BearerTokenOptions>, OptionsMonitor<BearerTokenOptions>>();
builder.Services.AddScoped<IRoleStore<Role>, RoleStore<Role, ApplicationDbContext, Guid>>(serviceProvider =>
{
    var roleStore = serviceProvider.GetRequiredService<RoleStore<Role, ApplicationDbContext, Guid>>();
    roleStore.AutoSaveChanges = false;
    return roleStore;
});
builder.Services.AddScoped<IUserStore<User>, UserStore<User, Role, ApplicationDbContext, Guid>>(serviceProvider =>
{
    var userStore = serviceProvider.GetRequiredService<UserStore<User, Role, ApplicationDbContext, Guid>>();
    userStore.AutoSaveChanges = false;
    return userStore;
});

// mediatr with EventsManagementApp.Application assembly
builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblyContaining<CreateEventCommandHandler>();
});

#endregion

var app = builder.Build();

#region Application Configuration

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

#endregion

app.Run();