using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManagementApi.DAL
{
	public class Filter
	{
		public int CustomerId { get; set; }
		public string Type { get; set; }
		public double MinPrincipal { get; set; }
		public double MaxPrincipal { get; set; }
		public double MinInterest { get; set; }
		public double MaxInterest { get; set; }
		public double MinEmi { get; set; }
		public double MaxEmi { get; set; }
		public DateTime SanctionAfter { get; set; }
		public DateTime SanctionBefore { get; set; }
		public DateTime MaturityAfter { get; set; }
		public DateTime MaturityBefore { get; set; }
	}
}
