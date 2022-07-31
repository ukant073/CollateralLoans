using CustomerManagementApi.DAL;
using CustomerManagementApi.DAL.DAO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace CustomerManagementApi
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

			services.AddDbContext<CustomerDb>(options =>
			{
				options.UseInMemoryDatabase("CustomerDb");
				//options.UseSqlServer("Server=DESKTOP-3CET6HL\\SQLEXPRESS;Database=CustomerDb;Trusted_Connection=True;");
			});
			services.AddScoped<ICustomerDao>(serviceProvider =>
				new CustomerEfDao(
					serviceProvider.GetService<ILogger<CustomerEfDao>>(),
					Configuration.GetSection("LogSensitiveData").Get<bool>()
				));
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				//app.UseSwagger();
                //app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomerManagementApi"));
			}

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}




