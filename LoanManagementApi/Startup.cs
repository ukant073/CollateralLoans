using LoanManagementApi.DAL;
using LoanManagementApi.DAL.DAO;
using LoanManagementApi.DAL.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using System.Net.Http;

namespace LoanManagementApi
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddHttpClient();

			services.AddDbContext<LoanDb>(options =>
			{
				options.UseInMemoryDatabase("LoanDb");
				//options.UseSqlServer("Server=NITISH;Database=nks;Trusted_Connection=True;");
			});

			services.AddScoped<ILoanDao>(serviceProvider =>
				new LoanEfDao(
					serviceProvider.GetService<ILogger<LoanEfDao>>(),
					Configuration.GetSection("LogSensitiveData").Get<bool>()
				));

			services.AddScoped<ICollateralManagement>(serviceProvider =>
				new CollateralManagement(
					serviceProvider.GetService<IHttpClientFactory>(),
					Configuration.GetValue<string>("CollateralSaveEndpoint"),
					serviceProvider.GetService<ILogger<CollateralManagement>>())
				);

			//services.AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo() { Title = "LoanManagementApi", Version = "v1" }));
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				   .AddJwtBearer(options =>
				   {
					   options.TokenValidationParameters = new TokenValidationParameters
					   {
						   ValidateIssuer = true,
						   ValidateAudience = true,
						   ValidateLifetime = true,
						   ValidateIssuerSigningKey = true,
						   ValidIssuer = Configuration["Jwt:Issuer"],
						   ValidAudience = Configuration["Jwt:Issuer"],
						   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
					   };
				   });
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "LoanManagementApi", Version = "v1" });
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please insert token",
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					Scheme = "bearer"
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement{
					{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					new string[] { }
				}

				});
			}
			);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "LoanManagementApi"));

				app.UseAuthentication();

				app.UseRouting();

				app.UseAuthorization();

				app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
			}
		}
	}
}
