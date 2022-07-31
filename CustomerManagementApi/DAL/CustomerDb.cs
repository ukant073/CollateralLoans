using CustomerManagementApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementApi.DAL
{
	public class CustomerDb : DbContext
	{
		public CustomerDb(DbContextOptions<CustomerDb> options) : base(options)
		{ }

		public DbSet<Customer> Customers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			ConfigureCustomerEntity(modelBuilder.Entity<Customer>());
			base.OnModelCreating(modelBuilder);
		}

		private void ConfigureCustomerEntity(EntityTypeBuilder<Customer> customerEntity)
		{
			customerEntity.ToTable("Customer");
			customerEntity.HasKey(c => c.Id);
			customerEntity.Property(c => c.Id).ValueGeneratedNever();
			customerEntity.Property(c => c.Name).IsRequired();
			customerEntity.Property(c => c.Address).IsRequired();
			customerEntity.Property(c => c.Dob).IsRequired();
			customerEntity.Property(c => c.Phone).IsRequired();
			customerEntity.Property(c => c.Email).IsRequired();
			customerEntity.Property(c => c.Country).IsRequired();
			customerEntity.Property(c => c.State).IsRequired();
			customerEntity.Property(c => c.City).IsRequired();
		}
	}
}
