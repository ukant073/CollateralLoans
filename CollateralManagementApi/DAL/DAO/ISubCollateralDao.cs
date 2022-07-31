using CollateralManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollateralManagementApi.DAL.DAO
{
	public interface ISubCollateralDao<T> where T : Collateral
	{
		int Save(T collateral, CollateralDb db);
	}
}
