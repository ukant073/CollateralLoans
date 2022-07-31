using CustomerManagementApi.DAL;
using CustomerManagementApi.DAL.DAO;
using CustomerManagementApi.Extentions;
using CustomerManagementApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CustomerManagementApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomerController : ControllerBase
	{
		private ICustomerDao _dao;

		public CustomerController(ICustomerDao dao)
		{
			_dao = dao;
		}

		[HttpGet("")]
		public IActionResult GetAll([FromQuery] Page page, [FromQuery] CustomerFilter filter, [FromServices] CustomerDb db)
		{
			if (page == null || page.PageNo < 1 || page.PageSize < 1)
				return StatusCode((int)HttpStatusCode.BadRequest, new { error = "invalid page details" });

			return Ok(_dao.GetAll(page, filter, db));
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id, [FromServices] CustomerDb db)
		{
			Customer customer = _dao.GetById(id, db);
			if (customer == null)
				return StatusCode((int)HttpStatusCode.NotFound, new { error = $"no entity found by id: {id}" });
			return Ok(customer);
		}

		[HttpPost("")]
		public IActionResult Save([FromBody] Customer customer, [FromServices] CustomerDb db)
		{
			if (customer == null)
				return StatusCode((int)HttpStatusCode.BadRequest, new { error = "cannot store null entity" });
			int rowId = _dao.Save(customer, db);
			if (rowId <= 0)
				return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "error occurred while saving customer" });
			return CreatedAtAction(nameof(CustomerController.GetById), nameof(CustomerController).RemoveSuffix("Controller"), new { id = rowId }, customer);
		}

		[HttpPut("{id}")]
		public IActionResult UpdateFull(int id, [FromBody] Customer customer, [FromServices] CustomerDb db)
		{
			if (customer == null)
				return StatusCode((int)HttpStatusCode.BadRequest, new { error = "cannot update with null entity" });

			int rowsAffected = _dao.UpdateFull(id, customer, db);
			if (rowsAffected <= 0)
				return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "error occurred while fully updating customer" });
			return Ok();
		}

		[HttpPatch("{id}")]
		public IActionResult UpdatePartial(int id, [FromBody] dynamic customer, [FromServices] CustomerDb db)
		{
			int rowsAffected = _dao.UpdatePartial(id, customer, db);
			if (rowsAffected <= 0)
				return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "error occurred while partially updating customer" });
			return Ok();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id, [FromServices] CustomerDb db)
		{
			int rowsAffected = _dao.Delete(id, db);
			if (rowsAffected <= 0)
				return StatusCode((int)HttpStatusCode.InternalServerError, new { error = "error occurred while deleting customer" });
			return Ok();
		}
	}
}
