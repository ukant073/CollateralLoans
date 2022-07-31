using CustomerManagementApi.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerManagementApi.DAL.DAO
{
	public class CustomerEfDao : ICustomerDao
	{
		private readonly ILogger<CustomerEfDao> _logger;
		private readonly bool _logSensitiveData;

		public CustomerEfDao(ILogger<CustomerEfDao> logger, bool logSensitiveData)
		{
			_logger = logger;
			_logSensitiveData = logSensitiveData;
		}

		public List<Customer> GetAll(Page page, CustomerFilter filter, CustomerDb db)
		{
			if (IsDbContextNull(db))
				return null;

			if (page == null || page.PageNo < 1 || page.PageSize < 1)
			{
				_logger.LogDebug(new ArgumentException($"Given page{JsonConvert.SerializeObject(page, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore })} details are invalid"), "Invalid Page Details");
				return null;
			}

			IQueryable<Customer> query = db.Customers.AsQueryable();

			if (filter != null)
			{
				if (filter.Name != null)
					query = query.Where(c => c.Name.ToLower().Trim().Contains(filter.Name.ToLower().Trim()));
				if (filter.Phone != 0)
					query = query.Where(c => c.Phone == filter.Phone);
				if (filter.Email != null)
					query = query.Where(c => c.Email.ToLower().Trim() == filter.Email.ToLower().Trim());
				if (filter.Dob != DateTime.MinValue)
					query = query.Where(c => c.Dob == filter.Dob);
				if (filter.Country != null)
					query = query.Where(c => c.Country.ToLower().Trim() == filter.Country.ToLower().Trim());
				if (filter.State != null)
					query = query.Where(c => c.State.ToLower().Trim() == filter.State.ToLower().Trim());
				if (filter.City != null)
					query = query.Where(c => c.City.ToLower().Trim() == filter.City.ToLower().Trim());
			}

			query = query.Skip((page.PageNo - 1) * page.PageSize).Take(page.PageSize);

			return query.ToList();
		}

		public Customer GetById(int id, CustomerDb db)
		{
			if (IsDbContextNull(db))
				return null;

			try
			{
				Customer customer = db.Customers.SingleOrDefault(c => c.Id == id);
				if (customer == null)
					_logger.LogDebug($"No customer found for id: {id}");
				return customer;
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Error occurred while getting customer by id{(_logSensitiveData ? "{" + id + "}" : "")}");
				return null;
			}
		}

		public int Save(Customer customer, CustomerDb db)
		{
			if (IsDbContextNull(db) || IsCustomerNull(customer))
				return 0;

			db.Customers.Add(customer);
			try
			{
				db.SaveChanges();
				return customer.Id;
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Error occurred while saving customer{(_logSensitiveData ? JsonConvert.SerializeObject(customer) : "")}");
				return 0;
			}
		}

		public int UpdateFull(int id, Customer customer, CustomerDb db)
		{
			if (IsDbContextNull(db) || IsCustomerNull(customer))
				return 0;

			Customer trackedCustomer = null;
			try
			{
				trackedCustomer = db.Customers.SingleOrDefault(c => c.Id == id);
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Error occurred while getting customer by id{(_logSensitiveData ? "{" + id + "}" : "")} for full update");
				return 0;
			}

			if (trackedCustomer == null)
			{
				_logger.LogDebug($"No customer found for given id{(_logSensitiveData ? "{" + id + "}" : "")}");
				return 0;
			}

			customer.Id = id;   //avoid updating the id
			try
			{
				db.Entry(trackedCustomer).CurrentValues.SetValues(customer);
				return db.SaveChanges();
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Error occurred while fully updating customer{(_logSensitiveData ? JsonConvert.SerializeObject(customer) : "")}");
				return 0;
			}
		}

		public int UpdatePartial(int id, dynamic customer, CustomerDb db)
		{
			if (IsDbContextNull(db))
				return 0;

			Customer trackedCustomer = null;
			try
			{
				trackedCustomer = db.Customers.SingleOrDefault(c => c.Id == id);
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Error occurred while getting customer by id{(_logSensitiveData ? "{" + id + "}" : "")} for partial update");
			}

			if (trackedCustomer == null)
			{
				_logger.LogDebug($"No customer found for given id{(_logSensitiveData ? "{" + id + "}" : "")}");
				return 0;
			}

			if (customer.GetType().GetProperty("Id") != null)
				customer.Id = id; //cannot update the id

			try
			{
				db.Entry(trackedCustomer).CurrentValues.SetValues(customer);
				return db.SaveChanges();
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Error occurred while partially updating customer{(_logSensitiveData ? JsonConvert.SerializeObject(customer, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }) : "")}");
				return 0;
			}
		}

		public int Delete(int id, CustomerDb db)
		{
			if (IsDbContextNull(db))
				return 0;

			Customer trackedCustomer = null;
			try
			{
				trackedCustomer = db.Customers.SingleOrDefault(c => c.Id == id);
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Error occurred while getting customer by id{(_logSensitiveData ? "{" + id + "}" : "")} for delete");
			}

			if (trackedCustomer == null)
			{
				_logger.LogDebug($"No customer found for given id{(_logSensitiveData ? "{" + id + "}" : "")}");
				return 0;
			}

			try
			{
				db.Customers.Remove(trackedCustomer);
				return db.SaveChanges();
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Error occurred while deleting customer by id{(_logSensitiveData ? "{" + id + "}" : "")}");
				return 0;
			}
		}

		private bool IsDbContextNull(CustomerDb db)
		{
			if (db == null)
			{
				_logger.LogError(new ArgumentNullException($"{nameof(CustomerDb)} instance is required to perform database operations"), $"{nameof(CustomerDb)} is null");
				return true;
			}
			return false;
		}

		private bool IsCustomerNull(Customer customer)
		{
			if (customer == null)
			{
				_logger.LogError(new ArgumentNullException($"{nameof(Customer)} is null, so it cannot be used for database operation"), $"{nameof(Customer)} is null");
				return true;
			}
			return false;
		}
	}
}