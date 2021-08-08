using FullStackDev.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullStackDev.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DepartmentController : Controller
	{
		FullStackDevContext db;
		public DepartmentController()
		{
			db = new FullStackDevContext();
		}

		[HttpGet]
		public async Task<JsonResult> Get()
		{
			List<Department> allRecordsOfDepartment = await db.Departments.ToListAsync();
			return Json(allRecordsOfDepartment);
		}

		[HttpPost]
		public async Task<JsonResult> Post(Department newDepartment)
		{
			await db.Departments.AddAsync(newDepartment);
			await db.SaveChangesAsync();
			return Json(true);
		}

		[HttpDelete("{id}")]
		public async Task<JsonResult> Delete(int id)
		{
			db.Departments.Remove(await db.Departments.FindAsync(id));
			await db.SaveChangesAsync();
			return Json(true);
		}

		[HttpPut]
		public async Task<JsonResult> Put(Department department)
		{
			var oldDepartment = await db.Departments.FindAsync(department.DepartmentId);
			oldDepartment.DeparmentName = department.DeparmentName;
			return Json(true);
		}
	}
}
