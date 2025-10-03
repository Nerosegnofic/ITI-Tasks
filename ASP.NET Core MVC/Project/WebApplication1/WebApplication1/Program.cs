using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Repositories.Implementations;
using WebApplication1.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// ----------------------
// Add services to the container
builder.Services.AddControllersWithViews();

// ----------------------
// Database Context
builder.Services.AddDbContext<LearningCenterContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ----------------------
// Add ASP.NET Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<LearningCenterContext>()
.AddDefaultTokenProviders();

// ----------------------
// Repositories
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ICourseStudentRepository, CourseStudentRepository>();

// ----------------------
// Session
builder.Services.AddSession();

// ----------------------
// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireHR", policy => policy.RequireRole("HR"));
    options.AddPolicy("RequireInstructor", policy => policy.RequireRole("Instructor"));
    options.AddPolicy("RequireStudent", policy => policy.RequireRole("Student"));
});

var app = builder.Build();

// ----------------------
// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Custom middleware
app.UseMiddleware<RequestLoggingMiddleware>();

// Session must come before Authentication/Authorization
app.UseSession();

// Identity Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// ----------------------
// Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ----------------------
// Seed roles and hardcoded admin user
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    await SeedRoles.InitializeAsync(roleManager, userManager);
}

// ----------------------
// Run app
app.Run();
