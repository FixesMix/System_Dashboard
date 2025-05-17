using SystemStatusDashboard;
using SystemStatusDashboard.Provider.Hubs;
using SystemStatusDashboard.Provider.Interfaces;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddSingleton<ISystemMonitorService, SystemMonitorService>();
builder.Services.AddHostedService<SystemMonitorService>();
builder.Services.AddLogging();
var app = builder.Build();
app.UseStaticFiles();




    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapHub<SystemStatusHub>("/monitorHub");



app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
