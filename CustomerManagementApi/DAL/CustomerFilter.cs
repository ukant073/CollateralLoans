using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementApi.DAL
{
	public class CustomerFilter
	{
		public string Name { get; set; }
		public long Phone { get; set; }
		public string Email { get; set; }
		public DateTime Dob { get; set; }
		public string Country { get; set; }
		public string State { get; set; }
		public string City { get; set; }
	}
}
