using HomeAutomationBlazor.Components;
using HomeAutomationBlazor.Services;

namespace HomeAutomationBlazor;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddHttpClient("Api", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress"] ?? "https://localhost:7119/");
        });

        builder.Services.AddScoped<ApiService>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            var js = sp.GetRequiredService<IJSRuntime>();
            return new ApiService(factory.CreateClient("Api"), js);
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
