using Auth0.AspNetCore.Authentication;
using VotingApp.Core.Services.Interfaces;
using VotingApp.Data.Repositories;
using VotingApp.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IElectionRepository, ElectionRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IEmailValidation, ExternalEmailVerificationService>();
builder.Services.AddControllersWithViews();
builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = "ludick.uk.auth0.com";
    options.ClientId = "BB8lZk14nXODsLlZYxANapoYjLaG5s7P";
    options.CallbackPath = "/account/callback";
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Election}/{action=Index}/{id?}");

app.Run();