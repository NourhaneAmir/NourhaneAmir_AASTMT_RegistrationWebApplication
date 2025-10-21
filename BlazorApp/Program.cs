using BlazorApp;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.NetworkInformation;
using static System.Formats.Asn1.AsnWriter;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7110")
});

builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<AuthState>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ToastService>();

var app = builder.Build();
using var scope = app.Services.CreateScope();
var authState = scope.ServiceProvider.GetRequiredService<AuthState>();

await builder.Build().RunAsync();
