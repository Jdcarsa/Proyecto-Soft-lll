using Microsoft.EntityFrameworkCore;
using ProyectoFinalSoft.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
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


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
	AddCookie(options =>
	{
		options.LoginPath = "/AccesoControlador/Login";
		options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
		options.AccessDeniedPath = "/Home/IndexDocente";
    });


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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=AccesoControlador}/{action=Login}/{id?}");

app.Run();
