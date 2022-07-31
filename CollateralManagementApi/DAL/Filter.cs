using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollateralManagementApi.DAL
{
	public class Filter
	{
		public string Type { get; set; }
		public string SortBy { get; set; }
		public int LoanId { get; set; }
		public int CustomerId { get; set; }
	}
}
