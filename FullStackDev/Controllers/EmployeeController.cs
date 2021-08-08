using FullStackDev.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FullStackDev.Controllers
{
	[Route("api/[controller]")]
	public class EmployeeController : Controller
	{
		FullStackDevContext db;
		IWebHostEnvironment env;
		public EmployeeController(IWebHostEnvironment _env)
		{
			env = _env;
			db = new FullStackDevContext();
		}

		[HttpGet]
		public async Task<JsonResult> Get()
		{
			List <Employee> allRecordsOfEmployee = await db.Employees.ToListAsync();
			return Json(allRecordsOfEmployee);
		}

		[HttpPost]
		public async Task<JsonResult> Post(Employee newEmployee)
		{
			await db.Employees.AddAsync(newEmployee);
			await db.SaveChangesAsync();
			return Json(true);
		}

		[HttpDelete("{id}")]
		public async Task<JsonResult> Delete(int id)
		{
			db.Employees.Remove(await db.Employees.FindAsync(id));
			await db.SaveChangesAsync();
			return Json(true);
		}

		[HttpPut]
		public async Task<JsonResult> Put(Employee newEmployee)
		{
			var oldEmployee = await db.Employees.FindAsync(newEmployee.EmployeeId);
			oldEmployee.EmployeeName = newEmployee.EmployeeName;
			return Json(true);
		}

		[HttpPost]
		[Route("SaveFile")]
		public async Task<JsonResult> SaveFile()
		{
			try
			{
				var httpRequest = Request.Form;
				var postFile = httpRequest.Files[0];
				string fileName = postFile.FileName;
				var physicalPath = env.ContentRootPath + "/Photos/" + fileName;

				using(var stream = new FileStream(physicalPath, FileMode.Create))
				{
					await postFile.CopyToAsync(stream);
				}
				return Json(fileName);
			}
			catch (Exception ex)
			{
				return Json("Error: " + ex.Message);
			}
		}
	}
}
