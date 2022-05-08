using Microsoft.EntityFrameworkCore;
using StudyControlWeb.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<DataContext>(o => o.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapDefaultControllerRoute();

app.UseAuthorization();

app.MapRazorPages();

app.Run();