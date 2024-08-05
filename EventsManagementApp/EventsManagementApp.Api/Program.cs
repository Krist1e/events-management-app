using EventsManagementApp.Application.Common;
using EventsManagementApp.Common;
using EventsManagementApp.Infrastructure.Common;

var builder = WebApplication.CreateBuilder();
builder.Services.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSwagger();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();