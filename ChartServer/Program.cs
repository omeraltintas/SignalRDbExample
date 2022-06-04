using ChartServer.Hubs;
using ChartServer.Models;
using ChartServer.Subscription;
using ChartServer.Subscription.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<DatabaseSubscription<Satislar>>();
builder.Services.AddSingleton<DatabaseSubscription<Personeller>>();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
policy.AllowCredentials().AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(x => true)));
builder.Services.AddSignalR();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCors();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseDatabaseSubscription<DatabaseSubscription<Personeller>>("Personeller");
app.UseDatabaseSubscription<DatabaseSubscription<Satislar>>("Satislar");

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<SatisHub>("/satishub");
});

app.MapRazorPages();

app.Run();
