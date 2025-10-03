using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Repositories.Implementations;
using WebApplication1.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LearningCenterContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ICourseStudentRepository, CourseStudentRepository>();

// Session
builder.Services.AddSession();

// ----------------------
// Add Authentication & Authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";       // Redirect here if not logged in
        options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect here if not authorized
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;           // Extend session if active
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireHR", policy => policy.RequireRole("HR"));
    options.AddPolicy("RequireInstructor", policy => policy.RequireRole("Instructor"));
    options.AddPolicy("RequireStudent", policy => policy.RequireRole("Student"));
});
// ----------------------

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseMiddleware<RequestLoggingMiddleware>();

// ----------------------
// Add Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();
// ----------------------

app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();