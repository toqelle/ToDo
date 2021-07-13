using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Data;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    [Route("api/[controller]")] // api/todo
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ToDoController : ControllerBase
    {
        private readonly ApiDBContext _context;
        public ToDoController(ApiDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var items = await _context.Items.ToListAsync();
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(ItemData data)
        {
            if (ModelState.IsValid)
            {
                await _context.Items.AddAsync(data);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetItem", new { data.Id }, (data));
            }

            return new JsonResult("Something went wrong") { StatusCode = 500};

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, ItemData item)
        {
            if (id != item.Id)
                return BadRequest();
            
            var exitstItem = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (exitstItem == null)
                return NotFound();

            exitstItem.Name = item.Name;
            exitstItem.Description = item.Description;
            exitstItem.IsDone = item.IsDone;

            //Implement the changes on the database level
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var exitstItem = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (exitstItem == null)
                return NotFound();
            _context.Items.Remove(exitstItem);
            await _context.SaveChangesAsync();

            return Ok(exitstItem);
        }

    }
}
