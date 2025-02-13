using System.Text;
using Employee_Management_System.Model;
using Employee_Management_System.Repositories;
using Employee_Management_System.Services;
using Employee_Management_System.Services.EmployeeManagementAPI.Repositories;
using Employee_Management_System.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;


internal class Program
{
    private static void Main(string[] args)
    {
       

        var builder = WebApplication.CreateBuilder(args);

        // Add CORS service
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowBlazorClient", policy =>
            {
                policy.WithOrigins("https://localhost:7042") // Your Blazor UI origin
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        builder.Logging.ClearProviders();
        builder.Host.UseNLog();
        NLog.Common.InternalLogger.LogLevel = NLog.LogLevel.Debug;
        NLog.Common.InternalLogger.LogToConsole = true;

        // Register repository and service
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();

        // Register the Attendance repository and service
        builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        builder.Services.AddScoped<IAttendanceService, AttendanceService>();

        // Register repository and service for Missing Attendance Requests
        builder.Services.AddScoped<IMissingAttendanceRequestRepository, MissingAttendanceRequestRepository>();
        builder.Services.AddScoped<IMissingAttendanceRequestService, MissingAttendanceRequestService>();

        // Register Department repository and service
        builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        builder.Services.AddScoped<IDepartmentService, DepartmentService>();

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();



        string encryptedConnStr = builder.Configuration.GetConnectionString("DefaultConnection");
        string encryptionKey = builder.Configuration["Encryption:Key"];
        string connectionString;

        if (!string.IsNullOrEmpty(encryptedConnStr))
        {
            connectionString = EncryptionHelper.DecryptString(encryptedConnStr, encryptionKey);
        }
        else
        {
            connectionString = encryptedConnStr; // Fall back if not encrypted.
        }

       
        // Register the DbContext with SQL Server using a connection string from appsettings.json.
        builder.Services.AddDbContext<EmployeeContext>(options =>
            options.UseSqlServer(connectionString));

        //var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
        //Console.WriteLine("Key");
        //Console.WriteLine(EncryptionHelper.EncryptString(connStr, encryptionKey));
        //qsuFFNsUr137zQgvvlxeFTNjRrqRu7YC7+/iWr3cmLqI6i47TH8NZOMLKYfqne0LsrYLdryqsCGUXcms9VpZQyrKvuoggrT6sQ9mg3hHRQ7Gk8QSIl1wKJnW5F/hpgmYHXcCzNICQkRrNSkVVXYS5A==


        // Configure ASP.NET Identity
       builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<EmployeeContext>()
            .AddDefaultTokenProviders();

        // Configure JWT settings from appsettings.json
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        builder.Services.AddAuthorization();


        var app = builder.Build();

        // Call the seeder during startup
        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            IdentityDataSeeder.SeedDataAsync(userManager, roleManager).GetAwaiter().GetResult();

        }

        // Use CORS middleware before routing
        app.UseCors("AllowBlazorClient");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //app.UseSwagger();
            //app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }

        var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
        lifetime.ApplicationStopped.Register(() => {
            NLog.LogManager.Shutdown();
        });

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}