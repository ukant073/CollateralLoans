using CollateralManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollateralManagementApi.DAL.DAO
{
	public class RealEstateEfDao : ISubCollateralDao<RealEstate>
	{
		public int Save(RealEstate collateral, CollateralDb db)
		{
			if (collateral == null)
				return 0;

			//verify collateral details

			db.Collaterals.Add(collateral);
			db.SaveChanges();
			return collateral.Id;
		}
	}
}
