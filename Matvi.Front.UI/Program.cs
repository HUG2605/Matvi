using Matvi.Front.UI.Constants;
using Matvi.Front.UI.Filters;
using Matvi.Front.UI.Security.Middlewares;
using Matvi.Front.UI.Security.Services;
using Serilog;
using Matvi.Back.Application;
using Matvi.Back.Application.Common;

var builder = WebApplication.CreateBuilder(args);

#region Logging: Serilog (Windows / Linux)
var windowsRoot = builder.Configuration["AppSystem:WindowsRoot"] ?? @"C:\Matvi.Repository\Matvi.Front.UI";
var linuxRoot = builder.Configuration["AppSystem:LinuxRoot"];

var baseLogPath = ResolverRutaLogs();

var infoLogPath = Path.Combine(baseLogPath, "Serilogs", "Information", "AppLogs-.log");
var errorLogPath = Path.Combine(baseLogPath, "Serilogs", "Error", "AppLogs-.log");

builder.Host.UseSerilog((ctx, lc) =>
{
    lc.ReadFrom.Configuration(ctx.Configuration) 
      .WriteTo.File(
          infoLogPath,
          rollingInterval: RollingInterval.Day,
          restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
          outputTemplate: "{Timestamp:ddMMyyyy-HHmmssfff zzz} [{Level}] {Message}{NewLine}{Exception}")
      .WriteTo.File(
          errorLogPath,
          rollingInterval: RollingInterval.Day,
          restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error,
          outputTemplate: "{Timestamp:ddMMyyyy-HHmmssfff zzz} [{Level}] {Message}{NewLine}{Exception}");
});
#endregion

string ResolverRutaLogs()
{
    if (OperatingSystem.IsWindows())
        return windowsRoot;

    if (!string.IsNullOrWhiteSpace(linuxRoot))
        return linuxRoot;

    return Path.Combine(AppContext.BaseDirectory, "logs");
}

#region Servicios base
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUsuarioActualService, UsuarioActualService>();
#endregion

#region Servicios Matvi
builder.Services.AddMatviServices(
    builder.Configuration.GetConnectionString("DefaultConnection") ?? "",
    provider =>
    {
        var sesSer = provider.GetRequiredService<IUsuarioActualService>();
        var usuSes = sesSer.UsuarioSesion;
        return usuSes is null ? (short)0 : usuSes.Id;
    });
#endregion

#region Autenticación y sesión
var timeout = TimeSpan.FromMinutes(30);

builder.Services
    .AddAuthentication(AuthConstants.Scheme)
    .AddCookie(AuthConstants.Scheme, options =>
    {
        options.Cookie.Name = AuthConstants.CookieName;
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = timeout;
        options.SlidingExpiration = true;
        options.LoginPath = "/Security/Login";
        options.AccessDeniedPath = "/AccessDenied";
    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = timeout;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
#endregion

#region UI y filtros
builder.Services.AddRazorPages()
    .AddMvcOptions(options =>
    {
        options.Filters.Add<ExceptionFilter>();
    });
#endregion

#region Controllers
builder.Services.AddControllers()
    .AddMvcOptions(options =>
    {
        options.Filters.Add<ExceptionFilter>();
    });
#endregion

#region Cultura
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture("es-MX");
});
#endregion

var app = builder.Build();

#region Warm-up del DbContext

using (var scope = app.Services.CreateScope())
{
    await WarmUp.InitializeAsync(scope);
}

#endregion 

#region Error handling y seguridad

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

#endregion

#region Pipeline HTTP (orden semántico obligatorio)
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting(); // 1️⃣ Define rutas

app.UseSession(); // 2️⃣ Inicializa sesión
app.UseAuthentication(); // 3️⃣ Reconstruye ClaimsPrincipal desde cookie
app.UseAuthorization();  // 4️⃣ Aplica reglas como [Authorize]

app.UseMiddleware<UsuarioActualMiddleware>(); // 5️⃣ Usa sesión y context.User
#endregion

#region Middleware global adicional
app.UseSerilogRequestLogging();
app.UseRequestLocalization();
#endregion

#region Endpoints
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();
app.MapControllers();
#endregion

app.Run();