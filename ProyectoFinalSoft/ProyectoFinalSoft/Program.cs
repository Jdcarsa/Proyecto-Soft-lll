using Microsoft.EntityFrameworkCore;
using ProyectoFinalSoft.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

/*
builder.Services.AddDbContext<AppDbContext>(options =>
{
	var connectionString = builder.Configuration.GetConnectionString("DefaultConnnection");
	options.UseSqlServer(connectionString);
});
*/

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();


var connectionString = builder.Configuration.GetConnectionString("MySql");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString,
    ServerVersion.AutoDetect(connectionString)));
builder.Services.AddScoped<DocenteServicio>();
builder.Services.AddScoped<CompetenciaServicio>();
builder.Services.AddScoped<PeriodoAcademicoServicio>();
builder.Services.AddScoped<AmbienteServicio>();

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

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
