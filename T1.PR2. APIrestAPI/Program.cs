
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using T1.PR2._APIrestAPI.Context;
using T1.PR2._APIrestAPI.Models;

namespace T1.PR2._APIrestAPI
{
    public class Program
    {
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			// Context and identity configuration
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
			builder.Services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlServer(connectionString);
			});

			builder.Services.AddIdentity<User, IdentityRole>(options =>
			{
				// Password configurations
				options.Password.RequireDigit = true;
				options.Password.RequiredLength = 6;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireLowercase = true;

				// Email configurations
				options.User.RequireUniqueEmail = true;

				// Lock configurations
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.AllowedForNewUsers = true;

				// Login configurations
				options.SignIn.RequireConfirmedEmail = false;
			})
				.AddEntityFrameworkStores<AppDbContext>()
				.AddDefaultTokenProviders();

			//Token and validations configuration
			var jwtSettings = builder.Configuration.GetSection("JwtSettings");

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidIssuer = jwtSettings["Issuer"],

						ValidateAudience = true,
						ValidAudience = jwtSettings["Audience"],

						ValidateLifetime = true,
						ClockSkew = TimeSpan.Zero,

						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
					};
				});

			builder.Services.AddAuthorization();
			builder.Services.AddControllers();

			builder.Services.AddEndpointsApiExplorer();

			builder.Services.AddSwaggerGen(opt =>
			{
				opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please enter token",
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					Scheme = "bearer"
				});
				opt.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type=ReferenceType.SecurityScheme,
								Id="Bearer"
							}
						},
						Array.Empty<string>()
					}
				});
			});

			// SignalR configuration
			builder.Services.AddCors(options =>
			{
				options.AddDefaultPolicy(policy =>
				{
					policy.WithOrigins("https://localhost:7269")
						  .AllowAnyHeader()
						  .AllowAnyMethod()
						  .AllowCredentials();
				});
			});

			builder.Services.AddSignalR();

			var app = builder.Build();
			
			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					var context = services.GetRequiredService<AppDbContext>();
					context.Database.Migrate();
				}
				catch (Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error ocurred while migrating the database.");
				}
			}

			// App pipeline
			// Create initials roles: Admin and User
			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				await Tools.RoleTools.CreateInitialsRoles(services);
			}

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.UseCors();

			app.Run();
		}
	}
}
