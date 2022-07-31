using CustomerManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementApi.DAL.DAO
{
	public interface ICustomerDao
	{
		List<Customer> GetAll(Page page, CustomerFilter filter, CustomerDb db);
		Customer GetById(int id, CustomerDb db);
		int Save(Customer customer, CustomerDb db);
		int UpdateFull(int id, Customer customer, CustomerDb db);
		int UpdatePartial(int id, dynamic customer, CustomerDb db);
		int Delete(int id, CustomerDb db);
	}
}
