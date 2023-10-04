using FolhaPontoDDD.Domain.Services;
using FolhaPontoDDD.Domain.Interfaces.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IFolhaPontoFuncionarioService, FolhaPontoFuncionarioService>();
builder.Services.AddScoped<IFolhaPontoDepartamentoService, FolhaPontoDepartamentoService>();
builder.Services.AddScoped<IArmazenaRelatorioService, ArmazenaRelatorioService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
